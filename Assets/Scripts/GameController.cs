using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : Singleton<GameController>
{
    public UnityEvent GameStartEvent;
    public UnityEvent GameStopEvent;
    [HideInInspector] public bool GameRunning = false;

    [SerializeField] private SpawnerController spawner;
    [SerializeField] private PlayerController player;
    [SerializeField] private CanvasController UI;

    private void Start()
    {
        player.OnPlayerDead += StopGame;
        UI.SetGUIState(GameRunning);
    }

    public void RestartGame()
    {
        GameRunning = true;
        player.Restart();
        spawner.Restart();
        UI.SetGUIState(GameRunning);
        GameStartEvent?.Invoke();
    }

    public void StopGame()
    {
        GameRunning = false;
        spawner.StopSpawner();
        UI.SetGUIState(GameRunning);
        GameStopEvent?.Invoke();
    }
}
