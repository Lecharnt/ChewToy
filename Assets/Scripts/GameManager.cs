using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ArtBuyingGameModeManager artBuyingSceneManager;
    public ArtSelectionGameModeManager artSelectioSceneManager;
    public FinnishDay finnishDay;
    public GameModeType currentGameMode;
    public int amountOfIncounters;

    public List<ArtObject> OwnedArt;
    public PlayerInventory playerInventory;
    public DiologeScript DiologeScript;

    public ArtObject SelcectedArtObject;

    public AudioClip clickSound;
    public AudioClip musicTrack1;
    public AudioClip musicTrack2;

    private int totalScore;
    private int aiCount;
    private int realCount;
    public enum GameModeType
    {
        ART_SHOW,
        ART_BUY,
        ART_FINNISH
    }
    public enum Room
    {
        gallary,
        bathroom,
        outSide
    }
    public static GameManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        artBuyingSceneManager.ExitGameMode += switchGameMode;
        artSelectioSceneManager.ExitGameMode += switchGameMode;

        StartCoroutine(StartCurrentGameMode());
    }
    private IEnumerator StartCurrentGameMode()
    {
        switch (currentGameMode)
        {
            case GameModeType.ART_SHOW:
                ArtSelectionGameModeManager.artSelectManager.transitioning = false;
                ArtSelectionGameModeManager.artSelectManager.inspectButton.interactable = true;
                AudioManager.Instance.PlayMusic(musicTrack1);
                artSelectioSceneManager.transform.gameObject.SetActive(false);
                artBuyingSceneManager.transform.gameObject.SetActive(true);
                finnishDay.transform.gameObject.SetActive(false);

                artBuyingSceneManager.currentArtWorkStats = artSelectioSceneManager.SelcectedArtObject;
                artBuyingSceneManager.StartThisGameModeAnimation();
                artBuyingSceneManager.amountOfIncounters = amountOfIncounters;
                // Wait until the animation finishes
                yield return new WaitUntil(() =>
                    artBuyingSceneManager.gameModeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f &&
                    !artBuyingSceneManager.gameModeAnimator.IsInTransition(0)
                );
                artBuyingSceneManager.StartThisGameMode();

                // Code to execute if variableToCheck equals constantValue1
                break; // Exits the switch statement
            case GameModeType.ART_BUY:
                AudioManager.Instance.PlayMusic(musicTrack2);

                artBuyingSceneManager.transform.gameObject.SetActive(false);
                artSelectioSceneManager.transform.gameObject.SetActive(true);
                finnishDay.transform.gameObject.SetActive(false);

                // Code to execute if variableToCheck equals constantValue2
                artSelectioSceneManager.StartThisGameModeAnimation();
                artSelectioSceneManager.amountOfIncounters = amountOfIncounters;
                //// Wait until the animation finishes
                //yield return new WaitUntil(() =>
                //    artSelectioSceneManager.gameModeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                //    !artSelectioSceneManager.gameModeAnimator.IsInTransition(0)
                //);
                artSelectioSceneManager.StartThisGameMode();
                break;
            case GameModeType.ART_FINNISH:
                //finnishDay.totalScore = totalScore;
                //finnishDay.aiCount = aiCount;
                //finnishDay.realCount = realCount;

                AudioManager.Instance.PlayMusic(musicTrack1);

                artBuyingSceneManager.transform.gameObject.SetActive(false);
                artSelectioSceneManager.transform.gameObject.SetActive(false);
                finnishDay.transform.gameObject.SetActive(true);
                // Code to execute if variableToCheck equals constantValue2
                finnishDay.StartThisGameModeAnimation();
                finnishDay.boughtArt = OwnedArt;
                //// Wait until the animation finishes
                //yield return new WaitUntil(() =>
                //    finnishDay.gameModeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                //    !finnishDay.gameModeAnimator.IsInTransition(0)
                //);
                finnishDay.StartThisGameMode();
                break;
            // ... more cases ...
            default:
                // Code to execute if none of the cases match
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        UpdateCurrentGameMode();
    }
    private void UpdateCurrentGameMode()
    {
        switch (currentGameMode)
        {
            case GameModeType.ART_SHOW:
                artBuyingSceneManager.UpdateThisGameMode();
                break; // Exits the switch statement
            case GameModeType.ART_BUY:
                artSelectioSceneManager.UpdateThisGameMode();

                // Code to execute if variableToCheck equals constantValue2
                break;
            case GameModeType.ART_FINNISH:
                finnishDay.UpdateThisGameMode();

                // Code to execute if variableToCheck equals constantValue2
                break;
            // ... more cases ...
            default:
                // Code to execute if none of the cases match
                break;
        }
    }
    private void switchGameMode(GameModeType nextGameMode)
    {
        StartCoroutine(SwitchGameModeRoutine(nextGameMode));
    }
    private IEnumerator SwitchGameModeRoutine(GameModeType nextGameMode)
    {
        switch (currentGameMode)
        {
            case GameModeType.ART_SHOW:
                if (artBuyingSceneManager.currentArtWorkStats.isAi) {
                    aiCount++;
                }
                else
                {
                    realCount++;
                }
                if (artBuyingSceneManager.currentArtWorkStats.price > 0)
                {
                    totalScore = (int)artBuyingSceneManager.currentArtWorkStats.price / 2;
                }

                artSelectioSceneManager.transform.gameObject.SetActive(false);
                artBuyingSceneManager.transform.gameObject.SetActive(true);
                finnishDay.transform.gameObject.SetActive(false);

                OwnedArt.Clear();
                OwnedArt.AddRange(artBuyingSceneManager.boughtArt);
                artBuyingSceneManager.EndThisGameModeAnimation();
                // Wait until the animation finishes
                yield return new WaitUntil(() =>
                    artBuyingSceneManager.gameModeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f &&
                    !artBuyingSceneManager.gameModeAnimator.IsInTransition(0)
                );
                artBuyingSceneManager.ExitThisGameMode();
                //Debug.Log("cool");
                break; // Exits the switch statement
            case GameModeType.ART_BUY:

                artBuyingSceneManager.transform.gameObject.SetActive(false);
                artSelectioSceneManager.transform.gameObject.SetActive(true);
                finnishDay.transform.gameObject.SetActive(false);

                artSelectioSceneManager.EndThisGameModeAnimation();
                //// Wait until the animation finishes
                //yield return new WaitUntil(() =>
                //    artSelectioSceneManager.gameModeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                //    !artSelectioSceneManager.gameModeAnimator.IsInTransition(0)
                //);
                artSelectioSceneManager.ExitThisGameMode();
                // Code to execute if variableToCheck equals constantValue2
                //Debug.Log("nolll");

                break;
            case GameModeType.ART_FINNISH:
                artBuyingSceneManager.transform.gameObject.SetActive(false);
                artSelectioSceneManager.transform.gameObject.SetActive(false);
                finnishDay.transform.gameObject.SetActive(true);
                finnishDay.EndThisGameModeAnimation();
                //// Wait until the animation finishes
                //yield return new WaitUntil(() =>
                //    finnishDay.gameModeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                //    !finnishDay.gameModeAnimator.IsInTransition(0)
                //);
                finnishDay.ExitThisGameMode();
                // Code to execute if variableToCheck equals constantValue2
                //Debug.Log("pollll");

                break;
            // ... more cases ...
            default:
                // Code to execute if none of the cases match
                break;
        }

        currentGameMode = nextGameMode;
        StartCoroutine(StartCurrentGameMode());
    }
    void OnDestroy()
    {
        artBuyingSceneManager.ExitGameMode -= switchGameMode;
        artSelectioSceneManager.ExitGameMode -= switchGameMode;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}