using TMPro;
using UnityEngine;

public class FinnishDay : GameModeManagerObject
{
    public System.Collections.Generic.List<ArtObject> boughtArt;

    public TextMeshProUGUI TotalScore;
    public TextMeshProUGUI Notes;

    public override void StartThisGameMode()
    {
        CalculateResults();
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
        float exhibitRating = 0; 
        float averageQuality = 0;
        float scrutiny = 0;
        int aiCount = 0;
        int realCount = 0;
        float expenses = 0;

        if(boughtArt.Count > 0)
        {
            foreach (ArtObject art in boughtArt)
            {
                averageQuality += art.quality;
                if (art.isAi)
                {
                    scrutiny += Mathf.Max((art.aiSuspicion - 5) / 50f, 0) * Mathf.Max((650 - art.quality) / 100f, 0);
                }
                expenses += art.price;

                if (art.isAi)
                {
                    aiCount += 1;
                }
                else
                {
                    realCount += 1;
                }
            }
            averageQuality /= boughtArt.Count;
        }
        // exhibit rating calculation 
        float qualityScore = Mathf.Clamp01(averageQuality / 650f) * 5; // quality factor: 5 points
        float budgetScore = Mathf.Clamp01((2000 - expenses) / 1000) * 4f; // price factor: 4 points
        float scrutinyScore = Mathf.Clamp01(1 - (Mathf.Max(scrutiny - 5, 0) / 95)) * 1f; //scrutiny factor:  0.5 points
        float quantityPenalty = -Mathf.Max(10-boughtArt.Count,0); // quantity penality: up to -10 points
        exhibitRating = Mathf.Clamp(qualityScore+budgetScore+scrutinyScore+quantityPenalty, 0, 10); // ensure rating is between 0 and 10
        Debug.Log($"Quality Score:{qualityScore}\n" +
            $"Budget Score: {budgetScore}\n" +
            $"Scrutiny Score: {scrutinyScore}\n" +
            $"Quantity Penalty: -{quantityPenalty}");
        TotalScore.text =
            $"<#ff38fc><b>Exhibit Rating</b>: {exhibitRating.ToString("F1")}/10\n" +
            $"<#0055CC><b>Average Quality</b>: {averageQuality}/650\n" +
            $"<#00CC00><b>Expenses</b>: ${expenses}/$1000\n" +
            $"<#CC1111><b>Scrutiny</b>: {scrutiny.ToString("F1")}%\n" +
            $"<#e8861e><b>Paintings</b>: {boughtArt.Count}/10";
        Notes.text = $"<#000000><b><size=150%>Notes:</size></b>\n" +
            GenerateNotes(averageQuality, budgetScore / 4f, scrutinyScore / 1f, quantityPenalty);

    }
    //$"<#e8861e><b>AI Paintings</b>: {aiCount}\n" +
    //$"<#aa44cf><b>Human Paintings</b>: {realCount}";
    private string GenerateNotes(float averageQuality, float budgetScoreRatio, float scrutinyScoreRatio, float quantityPenalty)
    {
        string notes = "";
        switch (averageQuality)
        {
            case < 100:
                notes += "<#b00300>- Quality was terrible</color>\n";
                break;
            case < 200:
                notes += "<#b05800>- Quality was lacking</color>\n";
                break;
            case < 300:
                notes += "<#b09b00>- Quality was okay</color>\n";
                break;
            case < 400:
                notes += "<#87b000>- Quality was good</color>\n";
                break;
            case < 500:
                notes += "<#46b000>- Quality was excellent</color>\n";
                break;
            default:
                notes += "<#00b03e>- Quality was exquisite</color>\n";
                break;
        }
        switch (budgetScoreRatio*100)
        {
            case 0:
                notes += "<#b00300>- Expenses were extremely overbudget</color>\n";
                break;
            case < 33:
                notes += "<#b05800>- Expenses were way overbudget</color>\n";
                break;
            case < 66:
                notes += "<#b09b00>- Expenses were overbudget</color>\n";
                break;
            case < 100:
                notes += "<#87b000>- Expenses were slightly overbudget</color>\n";
                break;
            case 100:
                notes += "<#46b000>- Expenses were within the budget</color>\n";
                break;
            default:
                notes += "<#00b03e>- We profitted from the expenses themselves</color>\n";
                break;
        }
        switch (scrutinyScoreRatio * 100)
        {
            case 0:
                notes += "<#b00300>- None of the art pieces seem human-made</color>\n";
                break;
            case < 33:
                notes += "<#b05800>- Some art pieces seem artificial</color>\n";
                break;
            case < 66:
                notes += "<#b09b00>- Some art pieces looked suspicious</color>\n";
                break;
            case < 100:
                notes += "<#87b000>- Some art pierces looked off</color>\n";
                break;
            default:
                break;
        }
        switch (quantityPenalty)
        {
            case -10:
                notes = "<#b00300>- I didn't hire you to make a statement, please purchase art.</color>\n";
                break;
            case -9:
                notes += "<#b05800>- One art piece is not sufficient, please purchase more art.</color>\n";
                break;
            case < -5:
                notes += "<#b09b00>- Very small exhibit, please purchase more art.</color>\n";
                break;
            case < -3:
                notes += "<#87b000>- Small exhibit, please purchase more art.</color>\n";
                break;
            case < 0:
                notes += "<#46b000>- A few more pieces would've made the exhibit feel complete.</color>\n";
                break;
            default:
                break;
        }
        return notes;
    }
    private void SetUi()
    {
        
    }

    public void Continue()
    {
        GoToNextGameMode(GameManager.GameModeType.ART_BUY);
    }
}
