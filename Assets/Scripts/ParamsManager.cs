using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamsManager : MonoBehaviour
{
    //to drag
    public bibiCard bibicard;
    public Image [] gameParams;
        
    
    [SerializeField] private int numOfCards=21;
    [SerializeField] private float midParamEffect = 2.5f; //middle level
    
    
    private float [] valParams = new float [] {0.5f, 0.5f, 0.5f, 0.5f}; 
    private float unitEffect;


    private void Start()
    {
        // fill range is  0-1, starting with middle value
        for (int i = 0; i < 4; i++)
        {
            valParams[i] = 0.5f;
            gameParams[i].fillAmount = 0.5f;
        }
        unitEffect = 0.5f / (midParamEffect * numOfCards);
    }
    
    public int SwipeEffectOnParams(bibiCard currBibiCard, bool isRight)
    {

        int[] vals = GetEffectVals(currBibiCard, isRight);
        Debug.Log("SwipeEffectOnParams cardId: "+ currBibiCard.Id+" " +vals[0]+" " +vals[1]+" " +vals[2]+" " +vals[3]);

        for (int i = 0; i < 4; i++)
        {
            float rowEffect = vals[i] - midParamEffect;
            float effect = rowEffect * unitEffect;
            valParams[i] += effect;
            Debug.Log("val:" + vals[i] + " rowEffect:" + rowEffect + " unitEffect:"+unitEffect+" effect:" + effect + " valParam:" + valParams[i]);
            
            gameParams[i].fillAmount += effect;
        }

        int wishCardId = -1;
        return wishCardId;
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
    
}
