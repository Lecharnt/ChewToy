using UnityEngine;

[CreateAssetMenu(fileName = "ArtObjectStats", menuName = "ScriptableObjects/ArtObjectStats", order = 1)]
public class ArtObject : ScriptableObject
{
    public string artistName;
    public Sprite artistImage;


    public int quality;
    public double price;
    public int aiSuspicion;
    public bool isAi;


    public string artName;
    public Sprite sprite;
    public string artisticProcess;
    public string mediumMethods;
    public string inspiration;


    public Vector2 widthAndHeight;
}
