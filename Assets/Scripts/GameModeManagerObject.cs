using System;
using UnityEngine;

public class GameModeManagerObject : MonoBehaviour
{
    public Animator gameModeAnimator;
    public Action<GameManager.GameModeType> ExitGameMode;
    public virtual void StartThisGameMode()
    {

    }
    public virtual void StartThisGameModeAnimation()
    {

    }
    public virtual void UpdateThisGameMode()
    {

    }
    public virtual void ExitThisGameMode()
    {

    }
    public virtual void EndThisGameModeAnimation()
    {

    }
    public virtual void GoToNextGameMode(GameManager.GameModeType WhatModeToGoTo)
    {
        ExitGameMode?.Invoke(WhatModeToGoTo);
        Debug.Log("object go next");
    }
}
