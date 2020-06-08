using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/New Card", order = 1)]
public class Card : ScriptableObject
{
    public Sprite image;
    public string title;
    public string myText;
    public string opt01;
    public string opt02;
    public int[] valuesRight;
    public int[] valuesLeft;
}
