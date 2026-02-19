using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtSelectionGameModeManager : GameModeManagerObject
{
    public static ArtSelectionGameModeManager artSelectManager;
    public int amountOfIncounters;
    public List<GameObject> boughtArt;
    public ArtSpawningManager artSpawningManager;
    public GameObject currentArtWork;
    public GameObject currentArtWorkPrefab;
    public ArtObject currentArtObject;
    public Transform spawnParent;
    public Button inspectButton;

    public ArtObject SelcectedArtObject;
    public List<ArtObject> Art = new List<ArtObject>();
    private bool doOnce = true;
    public bool transitioning = false;

    public float expenses;
    public TMP_Text budgetTracker;
    public int paintings;
    public TMP_Text paintingTracker;

    private void Awake()
    {
        if (artSelectManager == null)
        {
            artSelectManager = this;
            expenses = 0;
        }
        else
        {
            Destroy(artSelectManager);
            artSelectManager = this;
        }
    }
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
        //gameModeAnimator.SetTrigger("Exit");

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
        inspectButton.interactable = currentArtObject != null && currentArtObject != null;
        if (currentArtWork != null && currentArtObject != null && !transitioning)
        {
            transitioning = true;
            inspectButton.interactable = false;
            SelcectedArtObject = currentArtObject;
            GoToNextGameMode(GameManager.GameModeType.ART_SHOW);
        }
        else
        {
            Debug.Log($"Current Artwork: {currentArtWork}, Current ArtObject: {currentArtObject}, Transitioning:{transitioning}");
        }
    }
    public void Confirm() {
        if (currentArtWork != null && currentArtObject != null && !transitioning)
        {
            transitioning = true;
            inspectButton.interactable = false;
            SelcectedArtObject = currentArtObject;
            GoToNextGameMode(GameManager.GameModeType.ART_SHOW);
        } 
    }

    public void RemoveElement()
    {
        ArtObject removeArt = currentArtWork.GetComponent<ArtStats>().ArtObject;
        Art.Remove(removeArt);
        Destroy(currentArtWork);
    }
    public void GoToFinish()
    {
        GoToNextGameMode(GameManager.GameModeType.ART_FINNISH);
    }
    public void UpdateTrackers(float price)
    {
        expenses += price;
        paintings++;
        paintingTracker.text = $"<#002ee6>Painting Goal: {paintings}/10";
        budgetTracker.text = $"<#00e600>Budget: ${expenses}/$1000";
        //Debug.Log($"Trackers updated: {price}, {expenses}, {paintings}");
    }
}
