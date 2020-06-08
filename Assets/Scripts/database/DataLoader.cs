using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DataLoader : MonoBehaviour
{
    [SerializeField] bibiCards cards;

    void Awake()
    {
        ShowItems();
    }

    void ShowItems()
    {
        // right to left correction is done in the card display module
        string str = "";
        cards.UnitySheet
            .ForEach(entity => str += DescribeItemEntity(entity) + "\n");
        Debug.Log(str);
    }

    string DescribeItemEntity(bibiCard entity)
    {
        return string.Format(
            "{0} : {1}, {2}, {3}, {4}, {5}",
            entity.id,
            entity.character,
            entity.text,
            entity.right,
            entity.left,
            entity.category
        );
    }
    
    
    
    
    
    /*
    public static T[] GetAllInstances<T>() where T : bibiCards
    {
        string[] guids = cardsData.FindAssets("t:"+ typeof(T).Name);  //FindAssets uses tags check documentation for more info
        T[] a = new T[guids.Length];
        for(int i =0;i<guids.Length;i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
 
        return a;
    }
    */
}