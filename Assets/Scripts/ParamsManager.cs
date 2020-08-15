using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamsManager : MonoBehaviour
{
    //to drag
    [SerializeField] bibiCards bibiCards;
    public Image [] gameParams;
        
    
    [SerializeField] private int maxCardsDeltaEffect=8;// influencing delta effect. how many full cards up or down
    [SerializeField] private float midParamEffect = 5f; //middle level of param value
    [SerializeField] private float topThreshold = 0.9f;
    [SerializeField] private float bottomThreshold = 0.2f; 
    [SerializeField] private Color normalColor = Color.yellow; 
    [SerializeField] private Color topColor = Color.blue; 
    [SerializeField] private Color bottomColor = Color.red;
    [SerializeField] private float fillSignalTime = 2f;
    [SerializeField] private float thresholdEmpty = 0.08f;
    [SerializeField] private float thresholdFull = 0.92f;



    private bool [] isTopThresholds = new bool [] {false, false, false, false}; 
    private bool [] isBottomThresholds = new bool [] {false, false, false, false};
    private float [] valParams = new float [] {0.5f, 0.5f, 0.5f, 0.5f}; 
    private float unitEffect;


    /*void OnEnable()
    {
        Start();
    }*/
    
    void Start()
    {
        Debug.Log("ParamsManager.Start()");
        
        // fill range is  0-1, starting with middle value
        for (int i = 0; i < 4; i++)
        {
            valParams[i] = 0.5f;
            gameParams[i].fillAmount = 0.5f;
            gameParams[i].color = normalColor;
        }
        unitEffect = 0.5f / (midParamEffect * maxCardsDeltaEffect);
    }
    
    public int SwipeEffectOnParams(bibiCard currBibiCard, bool isRight)
    {
        // on swipe left - the text is shown on the right
        // therefore swipe left chooses the right option
        int[] vals = GetEffectVals(currBibiCard, !isRight);
        Debug.Log("SwipeEffectOnParams cardId: "+ currBibiCard.Id+" " +vals[0]+" " +vals[1]+" " +vals[2]+" " +vals[3]);

        for (int i = 0; i < 4; i++)
        {
            float rowEffect = vals[i] - midParamEffect;
            float effect = rowEffect * unitEffect;
            valParams[i] += effect;
            if (valParams[i] <= thresholdEmpty)
                valParams[i] = 0;
            else if (valParams[i] >= thresholdFull)
                valParams[i] = 1;
            Debug.Log("val:" + vals[i] + " rowEffect:" + rowEffect + " unitEffect:"+unitEffect+" effect:" + effect + " valParam:" + valParams[i]);
            
            gameParams[i].fillAmount += effect;
            
            CheckThreshold(i);
        }
        
        int wishCardId = CheckEndGame();

        StartCoroutine(SignalUpDown(vals));
        
        return wishCardId;
    }


    int CheckEndGame()
    {
        int wishCardId = -1;
        
        for (int i = 0; i < 4; i++)
        {
            // priority to empty over Full
            if (valParams[i] <= 0)
            {
                // formula for coding the cause for ending the game - decoded in SwipeManager.SwipeEffect
                //==================================================
                wishCardId = 1000 + i*10 + (int)valParams[i]; // next move fulfills parameter full/empty => end game
                break;
            }
        }
        if (wishCardId < 0)
            for (int i = 0; i < 4; i++)
            {
                if (valParams[i] >= 1)
                {
                    // formula for coding the cause for ending the game - decoded in SwipeManager.SwipeEffect
                    //==================================================
                    wishCardId = 1000 + i*10 + (int)valParams[i];// next move fulfills parameter full/empty => end game
                    break;
                }
            }

        return wishCardId;
    }
    
    
    
     IEnumerator SignalUpDown( int[] values)
    {
        for (int i = 0; i < 4; i++)
        {
            float rowEffect = values[i] - midParamEffect;

            if (rowEffect > 0)
                gameParams[i].color = topColor;
            else if (rowEffect < 0)
                gameParams[i].color = bottomColor;
        }
        Debug.Log("SignalUpDown "+values[0]+" "+values[1]+" "+values[2]+" "+values[3]);
        yield return new WaitForSeconds(fillSignalTime);
        Debug.Log("SignalUpDown "+values[0]+" "+values[1]+" "+values[2]+" "+values[3]);

        for (int i = 0; i < 4; i++)
        {
            gameParams[i].color = normalColor;
        }
    }
    
    
    // for marking the parameters that will be afftected - when swiping to each direction
    public int [] MarkPossibleEffects(int currentCardId, bool isRight)
    {

        bibiCard currBibiCard = bibiCards.UnitySheet.Find(x => x.id == currentCardId);

        int[] vals = GetEffectVals(currBibiCard, isRight);
        
        Debug.Log("MarkPossibleEffects cardId: "+ currBibiCard.Id+" " +vals[0]+" " +vals[1]+" " +vals[2]+" " +vals[3]);

        
        int [] possibleEffects = new int[] {0,0,0,0};
        for (int i = 0; i < 4; i++)
        {
            if (vals[i] == midParamEffect)// no effect
                possibleEffects[i] = 0;
            else if (vals[i] == midParamEffect*2 || vals[i] == 0)// extreme effect
                possibleEffects[i] = 2;
            else 
                possibleEffects[i] = 1;
        }
        
        Debug.Log("MarkPossibleEffects cardId: "+ currBibiCard.Id+" " +possibleEffects[0]+" " +possibleEffects[1]+" " +possibleEffects[2]+" " +possibleEffects[3]);


        return possibleEffects;
    }


    int[] GetEffectVals(bibiCard currBibiCard, bool isRight)
    {
        int [] vals = new int [] {-1,-1,-1,-1};
        if (isRight)
        {
            vals[0] = currBibiCard.affectOnPR1;
            vals[1] = currBibiCard.affectOnPR2;
            vals[2] = currBibiCard.affectOnPR3;
            vals[3] = currBibiCard.affectOnPR4;
        }
        else
        {
            vals[0] = currBibiCard.affectOnPL1;
            vals[1] = currBibiCard.affectOnPL2;
            vals[2] = currBibiCard.affectOnPL3;
            vals[3] = currBibiCard.affectOnPL4;
        }
        
        return vals;
    }


    void CheckThreshold(int paramInd)
    {
        // bottom Thresholds
        if (gameParams[paramInd].fillAmount < bottomThreshold && !isBottomThresholds[paramInd])
        {
            // crossing the threshold down
            isBottomThresholds[paramInd] = true;
            gameParams[paramInd].color = bottomColor;
            
            OnBottomThresholdExcceded(paramInd);// message?
        }
        else if (gameParams[paramInd].fillAmount >= bottomThreshold && isBottomThresholds[paramInd])
        {
            // crossing the threshold back up
            isBottomThresholds[paramInd] = false;
            gameParams[paramInd].color = normalColor;
        }
        // Top thresholds
        else if (gameParams[paramInd].fillAmount > topThreshold && !isTopThresholds[paramInd])
        {
            // crossing the threshold up
            isTopThresholds[paramInd] = true;
            gameParams[paramInd].color = topColor;
            
            OnTopThresholdExcceded(paramInd);// message?
        }
        else if (gameParams[paramInd].fillAmount <= topThreshold && isTopThresholds[paramInd])
        {
            // crossing the threshold back up
            isTopThresholds[paramInd] = false;
            gameParams[paramInd].color = normalColor;
        }
    }

    void OnBottomThresholdExcceded(int paramInd)
    {
        // encoragment text card can be displayed
        Debug.Log("param "+paramInd+ " just crossed the bottom threshold ");
    }

    void OnTopThresholdExcceded(int paramInd)
    {
        // warning text card can be displayed
        Debug.Log("param "+paramInd+ " just crossed the Top threshold");
    }



}
