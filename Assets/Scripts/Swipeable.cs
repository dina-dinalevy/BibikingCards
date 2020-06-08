using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Swipeable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

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


    public GameObject rearCard;

    public GameObject yesNoBanner;


    public CardSwipedDelegate CardSwipedDelegate;



    string opt01;
    string opt02;



    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position;

        var totalScreenWidth = startPosition.x * 2;
        leftBufferPosition = totalScreenWidth * (leftRightBufferPercentage / 100f);
        rightBufferPosition = totalScreenWidth * ((100f - leftRightBufferPercentage) / 100f);
    }

    public void SetLastCard() {
        lastCard = true;
    }

	public void OnBeginDrag(PointerEventData eventData)
    {
        if (!lastCard)
        {
            rearCard.SetActive(true);
        }
        GetComponent<Animator>().enabled = false;
        offset = rectTransform.position - new Vector3(eventData.position.x, eventData.position.y, 0);
    }

    public void OnDrag(PointerEventData data)
    {
        currentPosition = data.position + offset;
        rectTransform.position = new Vector2(currentPosition.x, startPosition.y);
        if (rectTransform.position.x > startPosition.x)
        {
            var positionOffset = 20 * ((startPosition.x - rectTransform.position.x) / startPosition.x);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, positionOffset);
        }
        else
        {
            var positionOffset = 20 * ((startPosition.x - rectTransform.position.x) / startPosition.x);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, positionOffset);
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform.position.x > rightBufferPosition)
        {
            // when choosing right
            StartCoroutine(MoveObject(rectTransform.localPosition, new Vector3((startPosition.x * 2) + 200f, 0, 0), 0.2f));
            StartCoroutine(ShowNextCard(true));
        }
        else if (rectTransform.position.x < leftBufferPosition)
        {
            // when choosing left
            StartCoroutine(MoveObject(rectTransform.localPosition, new Vector3(-(startPosition.x * 2), 0, 0), 0.2f));
            StartCoroutine(ShowNextCard(false));
        }
        else
        {
            StartCoroutine(MoveObject(rectTransform.localPosition, Vector3.zero, 0.2f));
        }
    }

    public void SetCardIcon(Sprite image)
    {
        cardImage.sprite = image;
    }

    public void SetCardData(Card card)
    {
        
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
            var positionOffset = 20 * ((startPosition.x - rectTransform.position.x) / startPosition.x);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, positionOffset);
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
