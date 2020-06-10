﻿using System;
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
    [SerializeField] private float topThreshold = 0.9f;
    [SerializeField] private float bottomThreshold = 0.2f; 
    [SerializeField] private Color normalColor = Color.yellow; 
    [SerializeField] private Color topColor = Color.blue; 
    [SerializeField] private Color bottomColor = Color.red; 



    private bool [] isTopThresholds = new bool [] {false, false, false, false}; 
    private bool [] isBottomThresholds = new bool [] {false, false, false, false};
    private float [] valParams = new float [] {0.5f, 0.5f, 0.5f, 0.5f}; 
    private float unitEffect;


    private void Start()
    {
        // fill range is  0-1, starting with middle value
        for (int i = 0; i < 4; i++)
        {
            valParams[i] = 0.5f;
            gameParams[i].fillAmount = 0.5f;
            gameParams[i].color = normalColor;
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

            CheckThreshold(i);
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