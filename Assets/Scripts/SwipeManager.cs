using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.U2D;
using UnityEditor.iOS.Xcode;

// for run time creation of assets(dina)


public delegate void CardSwipedDelegate(bool liked);

public class SwipeManager : MonoBehaviour {
    
    [SerializeField] bibiCards bibiCards;
    List<Card> allCards = new List<Card>();// loaded on start via excel list (dina)
    //public List<Card> allCards;// before - dragged
    
    
    
    public Swipeable currentCard;

    private int currentCardIndex = 0;
    
    
	void Start()
    {

        // reading the excel structure (dina)
        Debug.Log(bibiCards.UnitySheet.Count);
        foreach (bibiCard c in bibiCards.UnitySheet)
        {
            Card tmp = ScriptableObject.CreateInstance<Card>();
            tmp.title = c.Character;
            tmp.myText = c.Text;
            tmp.opt01 = c.Left;
            tmp.opt02 = c.Right;
            Debug.Log("next allCards: " + tmp.title + " " + tmp.myText);

            tmp.image = GetCardImage(tmp.title);
            //tmp.image = Resources.Load<Sprite>("Characters/Yeled");


            //AssetDatabase.CreateAsset(tmp, "Assets/Cards/tmp.asset");// if we needed actual asset (dina)
            allCards.Add(tmp);
        }
        
        
        for (int i = 0; i < allCards.Count; i++)
        {
            Card temp = allCards[i];
            int randomIndex = Random.Range(i, allCards.Count);
            allCards[i] = allCards[randomIndex];
            allCards[randomIndex] = temp;
        }
        

        // currentCard.SetCardIcon(allCards[currentCardIndex].image);
        Debug.Log("CARD TITLE " + allCards[currentCardIndex].title);
        currentCard.SetCardData(allCards[currentCardIndex]);

        currentCard.CardSwipedDelegate += CardSwiped;
	}



    void CardSwiped(bool liked)
    {
        Debug.Log("Image: " + allCards[currentCardIndex].image.name + " liked? " + liked);
        currentCardIndex += 1;

        if (currentCardIndex < allCards.Count)
        {
           // currentCard.cardImage.sprite = allCards[currentCardIndex].image;
            currentCard.SetCardData(allCards[currentCardIndex]);
        }
         
        if (currentCardIndex == allCards.Count - 1) 
        {
            currentCard.SetLastCard();
        } 

        if (currentCardIndex >= allCards.Count)
        {
            Debug.Log("finished!");
        }
    }



    // all sprites should be somewhere under path "Assets/Resources/"
    Sprite GetCardImage(string character)
    {
        Sprite image;
        switch (character)
        {
            
            case string a when a.Contains("רמטכ"):
                image = Resources.Load<Sprite>("Characters/chief");
                break;
            case string a when a.Contains("יונתן"):
                image = Resources.Load<Sprite>("Characters/Urich");
                break;
            case string a when a.Contains("דרעי"):
                image = Resources.Load<Sprite>("Characters/deree");
                break;
            case string a when a.Contains("שרה"):
                image = Resources.Load<Sprite>("Characters/sara");
                break;
            case string a when a.Contains("עמיר פרץ"):
                image = Resources.Load<Sprite>("Characters/peretz");
                break;
            case string a when a.Contains("ביתן"):
                image = Resources.Load<Sprite>("Characters/bitan");
                break;
            case string a when a.Contains("ישראל"):
                image = Resources.Load<Sprite>("Characters/bitan");
                break;
            case string a when a.Contains("טראמפ"):
                image = Resources.Load<Sprite>("Characters/tramp");
                break;
            case string a when a.Contains("רגב"):
                 image = Resources.Load<Sprite>("Characters/regev");
                 break;
            case string a when a.Contains("לפיד"):
                 image = Resources.Load<Sprite>("Characters/lapid");
                 break;
            case string a when a.Contains("מיכאלי"):
                 image = Resources.Load<Sprite>("Characters/michaeli");
                 break;
            case string a when a.Contains("גנץ"):
                image = Resources.Load<Sprite>("Characters/gantz");
                break;
            case string a when a.Contains("יאיר"):
                image = Resources.Load<Sprite>("Characters/yeled");
                break;
            case string a when a.Contains("שקד"):
                image = Resources.Load<Sprite>("Characters/michaeli");
                break;
            case string a when a.Contains("כלב"):
                image = Resources.Load<Sprite>("Characters/dog");
                break;

            default:
                image = Resources.Load<Sprite>("Characters/Peretz");// fallback
                Debug.Log("default image for character: "+ character);
                break;
        }

        if (image == null)
        {
            image = Resources.Load<Sprite>("Characters/sara"); //fallback
            Debug.Log("Image do not exist: " + character);
        }
        return image;
    }
  

}
