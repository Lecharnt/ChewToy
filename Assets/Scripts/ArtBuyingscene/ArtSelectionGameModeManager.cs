using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtSelectionGameModeManager : GameModeManagerObject
{
    public int amountOfIncounters;
    public List<GameObject> boughtArt;
    public ArtSpawningManager artSpawningManager;
    public GameObject currentArtWork;
    public GameObject currentArtWorkPrefab;
    public ArtObject currentArtObject;
    public Transform spawnParent;

    public ArtObject SelcectedArtObject;
    public List<ArtObject> Art = new List<ArtObject>();
    private bool doOnce = true;


    public override void StartThisGameMode()
    {

    }

    public override void StartThisGameModeAnimation()
    {
            InitiliseArtSpawningManager("Level1");

        gameModeAnimator.SetTrigger("Start");
        if (doOnce)
        {
            foreach (ArtObject artObject in Art)
            {
                CreateChoice(artObject);
            }
            doOnce = false;
        }
    }

    public void InitiliseArtSpawningManager(string level)
    {
        Art.Clear();
        Art.AddRange(Resources.LoadAll<ArtObject>(level));

    }
    public override void UpdateThisGameMode()
    {

    }

    public override void ExitThisGameMode()
    {
        gameModeAnimator.SetTrigger("Exit");

    }

    private void CreateChoice(ArtObject artObject)
    {
        InstantiateArtPeace(artObject);
    }
    private GameObject InstantiateArtPeace(ArtObject artObject)
    {
        GameObject artGameObject = Instantiate(currentArtWorkPrefab, spawnParent);
        artGameObject.GetComponent<Image>().sprite = artObject.sprite;
        artGameObject.GetComponent<RectTransform>().sizeDelta = artObject.widthAndHeight;

        artGameObject.AddComponent<ArtStats>().ArtObject = artObject;
        return artGameObject;
    }

    public void Inspect(ArtObject artObject, GameObject gameObject)
    {
        currentArtWork = gameObject;
        currentArtObject = artObject;

    }
    public void conferm() {

        SelcectedArtObject = currentArtObject;
        GoToNextGameMode(GameManager.GameModeType.ART_SHOW);

    }

    public void RemoveElement()
    {
        ArtObject removeArt = currentArtWork.GetComponent<ArtStats>().ArtObject;
        Art.Remove(removeArt);
        Destroy(currentArtWork);
    }
    public void GoToFinnish()
    {
        GoToNextGameMode(GameManager.GameModeType.ART_FINNISH);
    }
}
