using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public enum GameState
    {
        INGAME,
        INMENU
    }

    public delegate void DOnGameStateChanged(GameState oldState, GameState newState);
    private static void NullStateChange(GameState o, GameState n) { }

    private DOnGameStateChanged onGameStateChanged = new DOnGameStateChanged(NullStateChange);

    public GameState state;

	// Use this for initialization
	void Start () {
        state = GameState.INGAME;
	}
	
	// Update is called once per frame
	void Update () {
        CheckForGameStateChange();

        if (state == GameState.INMENU)
        {

        }
	}

    private void CheckForGameStateChange()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (state == GameState.INGAME)
            {
                state = GameState.INMENU;
                Time.timeScale = 0.0f;
                onGameStateChanged(GameState.INGAME, GameState.INMENU);
            }
            else
            {
                state = GameState.INGAME;
                Time.timeScale = 1.0f;
                onGameStateChanged(GameState.INMENU, GameState.INGAME);
            }
        }
    }
}
