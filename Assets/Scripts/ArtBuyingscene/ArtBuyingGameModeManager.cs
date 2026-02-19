using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtBuyingGameModeManager : GameModeManagerObject
{
    public int amountOfIncounters;
    public List<ArtObject> boughtArt;
    public ArtSpawningManager artSpawningManager;
    public GameObject currentArtWork;
    public GameObject currentArtWorkPrefab;
    public ArtObject currentArtWorkStats;

    public Transform spawnParent;

    [Header("Text")]
    public TextMeshProUGUI pantingTitle;
    public TextMeshProUGUI Price;
    public TextMeshProUGUI Quality;
    public TextMeshProUGUI AiChance;
    public TextMeshProUGUI ArtistInfo;

    [Header("Artist Image (Unused)")]
    public Image Artist;

    [Header("Bars")]
    public Image PriceFill;
    public Image QualityFill;
    public Image AiChanceFill;

    [Header("Frame")]
    public Image ArtFrame;
    public Sprite frame1Sprite;
    public Sprite frame2Sprite;

    [Header("Buttons")]
    public Button acceptButton;
    public Button rejectButton;
    public Button backButton;
    public bool transitioning;


    public override void StartThisGameMode()
    {
        transitioning = false;
        acceptButton.interactable = true;
        rejectButton.interactable = true;
        backButton.interactable = true;
    }
    private void SetUi()
    {
        //pantingTitle.text = "Art Name: \"" + currentArtWorkStats.artName+"\"";
        pantingTitle.text = $"\"{currentArtWorkStats.artName}\"";
        Price.text = $"<#00CC00>Price: ${currentArtWorkStats.price}";
        Quality.text = $"<#0055CC>Quality: {currentArtWorkStats.quality}";
        AiChance.text = $"<#CC1111>AI Chance: {currentArtWorkStats.aiSuspicion}%";
        ArtistInfo.text = $"<b>Artist:</b> " +
            $"<#DDDDDD>{currentArtWorkStats.artistName}</color>\n" + 
            $"<b>Artistic Process:</b>\n " +
            $"<#DDDDDD>\"{currentArtWorkStats.artisticProcess}\"</color>\n" +
            $"<b>Medium Methods:</b> " +
            $"<#DDDDDD>\"{currentArtWorkStats.mediumMethods}\"</color>\n" +
            $"<b>Inspiration:</b>\n " +
            $"<#DDDDDD>\"{currentArtWorkStats.inspiration}\"</color>";


        float ratio1 = Mathf.InverseLerp(0, 1000, (float)currentArtWorkStats.price);
        PriceFill.fillAmount = ratio1;

        float ratio2 = Mathf.InverseLerp(0, 650, currentArtWorkStats.quality);
        QualityFill.fillAmount = ratio2;

        float ratio3 = Mathf.InverseLerp(0, 100, currentArtWorkStats.aiSuspicion);
        AiChanceFill.fillAmount = ratio3;
    }

    public override void StartThisGameModeAnimation()
    {
        artSpawningManager.InitiliseArtSpawningManager("Level1");
        CreateSesificChoice(currentArtWorkStats);

        gameModeAnimator.SetTrigger("Start");
        RevealArt();
        SetUi();
    }

    public override void UpdateThisGameMode()
    {

    }

    public override void ExitThisGameMode()
    {
        gameModeAnimator.SetTrigger("Exit");

    }

    private void CreateChoice()
    {
        if(amountOfIncounters <= 0)
        {
            GoToNextGameMode(GameManager.GameModeType.ART_BUY);
            return;
        }
        amountOfIncounters -= 1;
        currentArtWorkStats = artSpawningManager.CreateChoiceRand(false);
        InstantiateArt(currentArtWorkStats);
    }
    private void InstantiateArt(ArtObject artObject)
    {

        currentArtWork = Instantiate(currentArtWorkPrefab, spawnParent);
        currentArtWork.SetActive(false);
        currentArtWork.GetComponent<Image>().sprite = artObject.sprite;
        currentArtWork.GetComponent<Image>().SetNativeSize();
        RectTransform rt = spawnParent.GetComponent<RectTransform>();
        rt.sizeDelta = currentArtWork.GetComponent<RectTransform>().sizeDelta + new Vector2(16f, 16f);
        currentArtWork.AddComponent<ArtStats>().ArtObject = artObject;
        if ((currentArtWorkStats.artName + currentArtWorkStats.artistName).Length % 2 == 0)
        {
            ArtFrame.sprite = frame1Sprite;
        }
        else
        {
            ArtFrame.sprite = frame2Sprite;
        }
    }
    private void RevealArt()
    {
        currentArtWork.SetActive(true);
    }
    public void Accepted()
    {
        if (transitioning)
        {
            return;
        }
        transitioning = true;
        acceptButton.interactable = false;
        rejectButton.interactable = false;
        backButton.interactable = false;
        ArtSelectionGameModeManager.artSelectManager.UpdateTrackers(currentArtWorkStats.price);
        boughtArt.Add(currentArtWorkStats);
        Destroy(currentArtWork);
        GoToNextGameMode(GameManager.GameModeType.ART_BUY);
    }

    public void Denied()
    {
        if (transitioning)
        {
            return;
        }
        transitioning = true;
        acceptButton.interactable = false;
        rejectButton.interactable = false;
        backButton.interactable = false;
        Destroy(currentArtWork);
        GoToNextGameMode(GameManager.GameModeType.ART_BUY);
    }
    public void Back()
    {
        if (transitioning)
        {
            return;
        }
        transitioning = true;
        acceptButton.interactable = false;
        rejectButton.interactable = false;
        backButton.interactable = false;
        Destroy(currentArtWork);
        GoToNextGameMode(GameManager.GameModeType.ART_BUY);
    }
    public void CreateSesificChoice(ArtObject artObject)
    {
        InstantiateArt(artObject);
    }
}
