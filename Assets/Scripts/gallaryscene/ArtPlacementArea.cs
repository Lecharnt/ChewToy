using UnityEngine;

public class ArtPlacementArea : MonoBehaviour
{
    public ArtObject art;

    public void PlaceArt(ArtObject artObject)
    {
        art = artObject;
        
    }
    public void SellArt()
    {
        //GameManager.Instance.art
    }
}
