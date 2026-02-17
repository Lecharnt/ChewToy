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

    public TextMeshProUGUI pantingTitle;
    public TextMeshProUGUI Price;
    public TextMeshProUGUI Quality;
    public TextMeshProUGUI AiChance;
    public TextMeshProUGUI ArtistInfo;

    public Image Artist;

    public Image PriceFill;
    public Image QualityFill;
    public Image AiChanceFill;



    public override void StartThisGameMode()
    {

    }
    private void SetUi()
    {
        pantingTitle.text = "Art Name: \"" + currentArtWorkStats.artName+"\"";
        Price.text = "Price: $" + currentArtWorkStats.price;
        Quality.text = "Quality: " + currentArtWorkStats.quality;
        AiChance.text = "Ai Chance: " + currentArtWorkStats.aiSuspicion+"%";
        ArtistInfo.text = "ArtisticProcess: \"" + currentArtWorkStats.artisticProcess+ "\""+ "\nMedium Methods: \"" + currentArtWorkStats.mediumMethods + "\""+ "\nInspiration: \"" + currentArtWorkStats.inspiration + "\"";


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

    }
    private void RevealArt()
    {
        currentArtWork.SetActive(true);
    }
    public void Accepted()
    {
        boughtArt.Add(currentArtWorkStats);
        Destroy(currentArtWork);
        GoToNextGameMode(GameManager.GameModeType.ART_BUY);
    }

    public void Denied()
    {
        Destroy(currentArtWork);
        GoToNextGameMode(GameManager.GameModeType.ART_BUY);
    }
    public void Back()
    {
        Destroy(currentArtWork);
        GoToNextGameMode(GameManager.GameModeType.ART_BUY);
    }
    public void CreateSesificChoice(ArtObject artObject)
    {
        InstantiateArt(artObject);
    }
}
