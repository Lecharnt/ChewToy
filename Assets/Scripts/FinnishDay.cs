using TMPro;
using UnityEngine;

public class FinnishDay : GameModeManagerObject
{
    public System.Collections.Generic.List<ArtObject> boughtArt;

    public TextMeshProUGUI TotalScore;
    public TextMeshProUGUI AiOwned;
    public TextMeshProUGUI RealOwned;

    public int totalScore;
    public int aiCount;
    public int realCount;

    public override void StartThisGameMode()
    {
        CalculateResults();
        SetUi();
    }

    public override void StartThisGameModeAnimation()
    {
        gameModeAnimator.SetTrigger("Start");
    }

    public override void UpdateThisGameMode()
    {

    }

    public override void ExitThisGameMode()
    {
        gameModeAnimator.SetTrigger("Exit");
    }

    private void CalculateResults()
    {
        totalScore = 0;
        aiCount = 0;
        realCount = 0;

        foreach (ArtObject art in boughtArt)
        {
            totalScore += art.quality;

            if (art.isAi)
            {
                aiCount += 1;
            }
            else
            {
                realCount += 1;
            }
        }
    }

    private void SetUi()
    {
        TotalScore.text = "Total Score: " + totalScore;
        AiOwned.text = "AI Paintings Owned: " + aiCount;
        RealOwned.text = "Real Paintings Owned: " + realCount;
    }

    public void Continue()
    {
        GoToNextGameMode(GameManager.GameModeType.ART_BUY);
    }
}
