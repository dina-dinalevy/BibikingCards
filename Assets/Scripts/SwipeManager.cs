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


    public ParamsManager paramsManager;
    public Swipeable currentCard;

    private int currentCardIndex = 0;
    
    
	void Start()
    {
        bool isLastExists = false;
        bibiCard tmpLastCard = new bibiCard();
        
        // reading the excel structure (dina)
        Debug.Log(bibiCards.UnitySheet.Count);
        foreach (bibiCard c in bibiCards.UnitySheet)
        {
            if (c.weight == 10)
            {
                // this card should be last in the game - adding it after the shuffle
                tmpLastCard = c;
                isLastExists = true;
            }
            else
                CreateCard(c);
        }
        
        for (int i = 0; i < allCards.Count; i++)
        {
            Card temp = allCards[i];
            int randomIndex = Random.Range(i, allCards.Count);
            allCards[i] = allCards[randomIndex];
            allCards[randomIndex] = temp;
        }
        
        if (isLastExists)
        {
            CreateCard(tmpLastCard);
        }
        
        // currentCard.SetCardIcon(allCards[currentCardIndex].image);
        Debug.Log("CARD TITLE " + allCards[currentCardIndex].title);
        currentCard.SetCardData(allCards[currentCardIndex]);

        currentCard.CardSwipedDelegate += CardSwiped;
	}


    void CreateCard(bibiCard c)
    {
        Card tmp = ScriptableObject.CreateInstance<Card>();
            
        tmp.name = c.id.ToString();// "id" field added to cards using empty field "name"
        tmp.title = c.Character;
        tmp.myText = c.Text;
        tmp.opt01 = c.Left;
        tmp.opt02 = c.Right;
        Debug.Log("next allCards: " + tmp.title + " " + tmp.myText);
        tmp.image = GetCardImage(tmp.title);

        allCards.Add(tmp);
    }
    

    void CardSwiped(bool liked)
    {
        if (currentCardIndex < allCards.Count)
            SwipeEffect(currentCardIndex, liked); // swiping right or left affects the game status parameters
        
        Debug.Log("Image: " + allCards[currentCardIndex].image.name + " liked? " + liked);
        currentCardIndex += 1;

        if (currentCardIndex < allCards.Count)
        {
           // currentCard.cardImage.sprite = allCards[currentCardIndex].image;
            currentCard.SetCardData(allCards[currentCardIndex]);
        }
         
        if (currentCardIndex == allCards.Count - 1) 
        {
            currentCard.SetLastCard();  //..... wrong position of last card
        } 

        if (currentCardIndex >= allCards.Count)
        {
            Debug.Log("finished!");
            // stop the game....
        }
    }



    // all sprites should be somewhere under path "Assets/Resources/"
    Sprite GetCardImage(string character)
    {
        Sprite image;
        switch (character)
        {
            case string a when a.Contains("יאיר"):
                image = Resources.Load<Sprite>("Characters/yeled");
                break;
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
            case string a when a.Contains("שקד"):
                image = Resources.Load<Sprite>("Characters/ayelet");
                break;
            case string a when a.Contains("פוטין"):
                image = Resources.Load<Sprite>("Characters/putin");
                break;
            case string a when a.Contains("יניב"):
                image = Resources.Load<Sprite>("Characters/sapar");
                break;
            case string a when a.Contains("אוחנה"):
                image = Resources.Load<Sprite>("Characters/ohana");
                break;
            case string a when a.Contains("זאב"):
                image = Resources.Load<Sprite>("Characters/zeev");
                break;
            case string a when a.Contains("מפכ"):
                image = Resources.Load<Sprite>("Characters/mafkal");
                break;
            case string a when a.Contains("כלב"):
                image = Resources.Load<Sprite>("Characters/dog");
                break;
            default:
                image = Resources.Load<Sprite>("Characters/dog");// fallback
                Debug.Log("default image for character: "+ character);
                break;
        }

        if (image == null)
        {
            image = Resources.Load<Sprite>("Characters/dog"); //fallback
            Debug.Log("Image do not exist: " + character);
        }
        return image;
    }


    #region BibiLogic

    void SwipeEffect(int currentCardIndex, bool isRight)
    {
        if (allCards[currentCardIndex].name == null)
            return;

        int id = int.Parse(allCards[currentCardIndex].name);
        bibiCard currBibiCard = bibiCards.UnitySheet.Find(x => x.id == id);
        int WishCardId = paramsManager.SwipeEffectOnParams(currBibiCard, isRight);
        if (WishCardId == 0)
            EndGameDueParam();
    }


    void EndGameDueParam()
    {
        Debug.Log(" End game due to Parameter full or empty");
        //.........
    }

    #endregion
    

}
