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
                    scrutiny += Mathf.Clamp01(Mathf.Max((art.aiSuspicion - 5) / 95f, 0) * (0.5f+Mathf.Clamp01(250/art.quality)))*100;
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
            scrutiny /= boughtArt.Count;
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
            $"<#0055CC><b>Average Quality</b>: {averageQuality}/650\n" +
            $"<#00CC00><b>Expenses</b>: ${expenses}/$1000\n" +
            $"<#CC1111><b>Scrutiny</b>: {scrutiny.ToString("F1")}%\n" +
            $"<#e8861e><b>Paintings</b>: {boughtArt.Count}/10\n<size=50%>\n</size>" +
            $"<size=125%><#ff38fc><b>Exhibit Rating</b>: {exhibitRating.ToString("F1")}/10</size>";
        Notes.text = $"<#000000><b><size=150%>Notes:</size></b>\n" +
            GenerateNotes(averageQuality, budgetScore / 4f, scrutinyScore / 1f, quantityPenalty);

    }
    //$"<#e8861e><b>AI Paintings</b>: {aiCount}\n" +
    //$"<#aa44cf><b>Human Paintings</b>: {realCount}";
    static readonly string[] RATING_COLORS = { "cc0300", "cc6600", "a69300", "7fa600", "46b000", "00b07b" };
    private string GenerateNoteString(string text, int rating)
    {
        return $"<#{RATING_COLORS[rating]}>- {text}</color>\n";
    }
    private string GenerateNotes(float averageQuality, float budgetScoreRatio, float scrutinyScoreRatio, float quantityPenalty, bool debug = false)
    {
        string notes = "";
        if (debug)
        {
            for (int i = 0; i < RATING_COLORS.Length; i++)
            {
                notes += GenerateNoteString($"Rating {i}", i);
            }
            return notes;
        }
        switch (averageQuality)
        {
            case < 100:
                notes += GenerateNoteString("Quality was terrible",0);
                break;
            case < 200:
                notes += GenerateNoteString("Quality was lacking", 1);
                break;
            case < 300:
                notes += GenerateNoteString("Quality was okay", 2);
                break;
            case < 400:
                notes += GenerateNoteString("Quality was good", 3);
                break;
            case < 500:
                notes += GenerateNoteString("Quality was excellent", 4);
                break;
            default:
                notes += GenerateNoteString("Quality was exquisite", 5);
                break;
        }
        switch (budgetScoreRatio*100)
        {
            case 0:
                notes += GenerateNoteString("Expenses were extremely overbudget", 0);
                break;
            case < 33:
                notes += GenerateNoteString("Expenses were way overbudget", 1);
                break;
            case < 66:
                notes += GenerateNoteString("Expenses were overbudget", 2);
                break;
            case < 100:
                notes += GenerateNoteString("Expenses were slightly overbudget", 3);
                break;
            case 100:
                notes += GenerateNoteString("Expenses were within the budget", 4);
                break;
            default:
                notes += GenerateNoteString("We profitted from the expenses themselves", 5);
                break;
        }
        switch (scrutinyScoreRatio * 100)
        {
            case 0:
                notes += GenerateNoteString("None of the art pieces seem human-made", 0);
                break;
            case < 33:
                notes += GenerateNoteString("Some art pieces seem artificial", 1);
                break;
            case < 66:
                notes += GenerateNoteString("Some art pieces looked suspicious", 2);
                break;
            case < 100:
                notes += GenerateNoteString("Some art pieces looked off", 3);
                break;
            default:
                break;
        }
        switch (quantityPenalty)
        {
            case -10:
                notes = GenerateNoteString("I didn't hire you to make a statement, please purchase art", 0);
                break;
            case -9:
                notes += GenerateNoteString("One art piece is not sufficient, please purchase more art", 0);
                break;
            case < -5:
                notes += GenerateNoteString("Very small exhibit, please purchase more art", 1);
                break;
            case < -3:
                notes += GenerateNoteString("Small exhibit, please purchase more art", 1);
                break;
            case < 0:
                notes += GenerateNoteString("Some art pieces looked suspicious", 2);
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
