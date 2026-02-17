using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    
    private Dictionary<ArtObject, bool> itemCollectionStatus;
    

    void Start()
    {
        itemCollectionStatus = new Dictionary<ArtObject, bool>();
        //foreach (ArtObject item in System.Enum.GetValues(typeof(ArtObject)))
        //{
        //    itemCollectionStatus[item] = false;
        //}
    }
    public void CollectArt(ArtObject item)
    {
        if (itemCollectionStatus.ContainsKey(item))
        {
            itemCollectionStatus[item] = true;
        }
    }
    public bool IsArtCollected(ArtObject item)
    {
        if (itemCollectionStatus.ContainsKey(item))
        {
            return itemCollectionStatus[item];
        }
        else
        {
            return false;
        }
    }
    public void RemoveArt(ArtObject item)
    {
        if (itemCollectionStatus.ContainsKey(item))
        {
            itemCollectionStatus[item] = false;
        }
    }
}