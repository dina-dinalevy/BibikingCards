using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class bibiCard
{
    public int id;
    public string character;
    public string text;
    public string right;
    public string left;
    public int affectOnPR1;    
    public int affectOnPR2;
    public int affectOnPR3;
    public int affectOnPR4;
    public int affectOnPL1;    
    public int affectOnPL2;
    public int affectOnPL3;
    public int affectOnPL4;
    public string category;    
    public float weight;



    public int Id
    {
        get
        {
            return id;
        }
    }
    
    public string Character
    {
        get
        {
            return character;
        }
    }
    public string Text
    {
        get
        {
            return text;
        }
    }
    public string Right
    {
        get
        {
            return right;
        }
    }
    public string Left
    {
        get
        {
            return left;
        }
    }
    public int AffectOnPR1
    {
        get
        {
            return affectOnPR1;
        }
    }
    public int AffectOnPR2
    {
        get
        {
            return affectOnPR2;
        }
    }
    public int AffectOnPR3
    {
        get
        {
            return affectOnPR3;
        }
    }
    public int AffectOnPR4
    {
        get
        {
            return affectOnPR4;
        }
    }
    public int AffectOnPL1
    {
        get
        {
            return affectOnPL1;
        }
    }
    public int AffectOnPL2
    {
        get
        {
            return affectOnPL2;
        }
    }
    public int AffectOnPL3
    {
        get
        {
            return affectOnPL3;
        }
    }
    public int AffectOnPL4
    {
        get
        {
            return affectOnPL4;
        }
    }
    public string Category
    {
        get
        {
            return category;
        }
    }
    public float Weight
    {
        get
        {
            return weight;
        }
    }

    public enum CardCategory
    {
        MILITARY,
        FAMILY,
        ADVISORS,
        MINISTERS,
        COALITION,
        OPOSITION,
        OTHER
    }

    public enum CharacterId
    {
        AVIVI,
        SARA,
        YAIR,
        ORICH,
        BENET,
        DEREE,
        OTHER
    }

    /*
    public string GetNameNative()
    {
        return itemName;
    }

    public ItemColor cl;

    
    */

}

