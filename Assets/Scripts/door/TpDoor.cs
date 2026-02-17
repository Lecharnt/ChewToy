using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpDoor : MonoBehaviour
{
    public GameObject door;
    public GameObject exit;
    public bool locked;
    public GameManager.Room room;
    public GameManager.Room roomExit;
    public string def;
    public Vector3 GetEnterPlace()
    {
        return door.transform.position;
    }
    public Vector3 GetExitPlace()
    {
        return exit.transform.position;
    }

}