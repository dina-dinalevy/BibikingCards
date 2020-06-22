using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Swipeable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    //to drag
    [SerializeField] private ParamsManager paramsManager;
    [SerializeField] private GameObject [] effectSmallMarkers; 
    [SerializeField] private GameObject [] effectLargeMarkers; 

    private Vector2 offset;
    private Vector2 currentPosition;
    private Vector2 startPosition;

    private RectTransform rectTransform;

    private float leftRightBufferPercentage = 30f;
    private float leftBufferPosition;
    private float rightBufferPosition;

    private bool lastCard = false;

    public Image cardImage;
    public TMP_Text cardTitle;
    public TMP_Text cardText;
    public TMP_Text optionText;

    public Animator staticPanelAnimator;
    public TMP_Text staticRightText;
    public TMP_Text staticLeftText;
    
    public GameObject rearCard;

    public GameObject yesNoBanner;


    public CardSwipedDelegate CardSwipedDelegate;


    private int cardId;
    string opt01;
    string opt02;
    int [] paramPossibleAffect;


    
    void Start()
    {
        Debug.Log("Swipeable.Start()");
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position;

        var totalScreenWidth = startPosition.x * 2;
        leftBufferPosition = -60f;//totalScreenWidth * (leftRightBufferPercentage / 100f);
        rightBufferPosition = 60f;//totalScreenWidth * ((100f - leftRightBufferPercentage) / 100f);
        ResetDragInfo();
    }

    public void SetLastCard() {
        lastCard = true;
    }

	public void OnBeginDrag(PointerEventData eventData)
    {
        if (!lastCard)
        {
            rearCard.SetActive(true);//....... 
        }
        GetComponent<Animator>().enabled = false;
        offset = rectTransform.position - new Vector3(eventData.position.x, eventData.position.y, 0);
    }

    public void OnDrag(PointerEventData data)
    {
        currentPosition = data.position + offset;

        transform.position = currentPosition; 
        transform.rotation = Quaternion.Euler(0, 0, -transform.localPosition.x*0.05f);
        //Debug.Log("-transform.position.x*0.05f " + -transform.localPosition.x);
        /*
        rectTransform.position = new Vector2(currentPosition.x, startPosition.y);
        if (rectTransform.position.x > startPosition.x)
        {
            var positionOffset = 20 * ((startPosition.x - rectTransform.position.x) / startPosition.x);
           // gameObject.transform.localRotation = Quaternion.Euler(0, 0, positionOffset);
        }
        else
        {
            var positionOffset = 20 * ((startPosition.x - rectTransform.position.x) / startPosition.x);
        //    gameObject.transform.localRotation = Quaternion.Euler(0, 0, positionOffset);
        }

        if (rectTransform.position.x > rightBufferPosition)
        {
            ShowYesBanner();
        }
        else if (rectTransform.position.x < leftBufferPosition)
        {
            ShowNoBanner();
        }
        else
        {
            yesNoBanner.GetComponent<Animator>().SetBool("showBanner", false);
        }
        */
        
        
        if (rectTransform.localPosition.x > rightBufferPosition)
        {
            staticPanelAnimator.SetBool("showBanner", true);
            staticRightText.text = opt01;
            staticLeftText.text = "";
            ShowExpectedEffect(false);
        }
        else if (rectTransform.localPosition.x < leftBufferPosition)
        {
            staticPanelAnimator.SetBool("showBanner", true);
            staticRightText.text = "";
            staticLeftText.text = opt02;
            ShowExpectedEffect(true);

        }
        else
        {
            staticPanelAnimator.SetBool("showBanner", false);
            ResetDragInfo();
        }

    }


    void ResetDragInfo()
    {
        staticRightText.text = "";
        staticLeftText.text = "";
        for (int i = 0; i < 4; i++)
        {
            effectSmallMarkers[i].SetActive(false);
            effectLargeMarkers[i].SetActive(false);
        }
    }
    


    void ShowExpectedEffect(bool isRight)
    {
        int[] possibleEffects = paramsManager.MarkPossibleEffects(cardId, isRight);

        for (int i = 0; i < 4; i++)
        {
            if (possibleEffects[i] == 1)
            {
                // signaling that this option will cause small effect
                effectSmallMarkers[i].SetActive(true);
                effectLargeMarkers[i].SetActive(false);

            }
            else if (possibleEffects[i] == 2)
            {
                // signaling that this option will cause large effect
                effectSmallMarkers[i].SetActive(false);
                effectLargeMarkers[i].SetActive(true);
            }
            else
            {
                effectSmallMarkers[i].SetActive(false);
                effectLargeMarkers[i].SetActive(false);
            }
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform.localPosition.x > rightBufferPosition)
        {
            // when choosing right
            StartCoroutine(MoveObject(rectTransform.localPosition, new Vector3(1500f, -300, 0), 0.2f));
            StartCoroutine(ShowNextCard(true));
        }
        else if (rectTransform.localPosition.x < leftBufferPosition)
        {
            // when choosing left
            StartCoroutine(MoveObject(rectTransform.localPosition, new Vector3(-1500f, -300, 0), 0.2f));
            StartCoroutine(ShowNextCard(false));
        }
        else
        {
            StartCoroutine(MoveObject(rectTransform.localPosition, Vector3.zero, 0.1f));
        }
        
        ResetDragInfo();
    }

    public void SetCardIcon(Sprite image)
    {
        cardImage.sprite = image;
        for (int i = 0; i < 4; i++)
        {
            effectSmallMarkers[i].SetActive(false);
            effectLargeMarkers[i].SetActive(false);
        }
    }

    public void SetCardData(Card card)
    {
        cardId = int.Parse(card.name);
        cardText.text = card.myText;
        cardTitle.text = card.title;
        opt01 = card.opt01;
        opt02 = card.opt02;
        Debug.Log(card.title +" "+card.image);
        cardImage.sprite = card.image;
    }

    IEnumerator MoveObject(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            rectTransform.localPosition = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
         //   var positionOffset = 20 * ((startPosition.x - rectTransform.loca.x) / startPosition.x);
         //   gameObject.transform.localRotation = Quaternion.Euler(0, 0, positionOffset);
            yield return null;
        }
        rectTransform.localPosition = target;
        gameObject.transform.localRotation = Quaternion.identity;
    }

    IEnumerator ShowNextCard(bool liked)
    {
        yesNoBanner.GetComponent<Animator>().SetBool("showBanner", false);
        yield return new WaitForSeconds(0.2f);
        if (!lastCard)
        {
            CardSwipedDelegate(liked);
            GetComponent<Animator>().enabled = true;
            ResetCard();
            rearCard.SetActive(false);
            GetComponent<Animator>().Play("FlipCard");
        } 
        else 
        {
            CardSwipedDelegate(liked);
        }
    }

    void ResetCard() 
    {
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        GetComponent<Image>().color = new Color(89.0f / 255.0f, 104.0f / 255.0f, 226.0f / 255.0f, 1.0f);
    }

    void ShowYesBanner()
    {

        yesNoBanner.GetComponent<Animator>().SetBool("showBanner", true);
        optionText.text = opt01;
    }

    void ShowNoBanner()
    {

        yesNoBanner.GetComponent<Animator>().SetBool("showBanner", true);
        optionText.text = opt02;
    }
    
    
}
