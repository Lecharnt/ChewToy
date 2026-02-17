using UnityEngine;

public class ButtionScript : MonoBehaviour
{
    private ArtSelectionGameModeManager ArtSelectionGameModeManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ArtSelectionGameModeManager = ArtSelectionGameModeManager.artSelectManager;
    }
    public void DoButtionClickStuff()
    {
        ArtSelectionGameModeManager.Inspect(transform.GetComponent<ArtStats>().ArtObject, transform.gameObject);
    }

}
