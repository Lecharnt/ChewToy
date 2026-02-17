using UnityEngine;

public class GallaryGameModeManager : GameModeManagerObject
{
    

    public override void StartThisGameMode()
    {
        CreateChoice();
    }

    public override void StartThisGameModeAnimation()
    {
        //artSpawningManager.InitiliseArtSpawningManager();
        gameModeAnimator.SetTrigger("Start");
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
        //if (amountOfIncounters <= 0)
        //{
        //    GoToNextGameMode(GameManager.GameModeType.ART_DEAL);
        //    return;
        //}
        //amountOfIncounters -= 1;
        //currentArtWorkPrefab = artSpawningManager.CreateChoiceRand(false);
        //currentArtWork = Instantiate(artSpawningManager.CreateChoiceRand(false), transform);
    }

    public void PlaceArt(GameObject art)
    {
        //boughtArt.Add(art);
        Destroy(art);
        CreateChoice();
    }

    public void SellArt(GameObject art)
    {
        Destroy(art);
        CreateChoice();
    }

}
