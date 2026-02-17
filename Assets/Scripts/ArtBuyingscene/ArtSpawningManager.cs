using System.Collections.Generic;
using UnityEngine;

public class ArtSpawningManager : MonoBehaviour
{
    public List<ArtObject> Art = new List<ArtObject>();


    public void InitiliseArtSpawningManager(string level)
    {
        Art.Clear();
        Art.AddRange(Resources.LoadAll<ArtObject>(level));

    }

    public ArtObject CreateChoiceRand(bool IsAI)
    {
        ArtObject artObject;
        int randChocePlace;
        randChocePlace = Random.Range(0, Art.Count);
        artObject = Art[randChocePlace];
        Art.RemoveAt(randChocePlace);
        
        StartDiologe(artObject);
        return artObject;
    }
    private void StartDiologe(ArtObject artObject)
    {

    }

}
