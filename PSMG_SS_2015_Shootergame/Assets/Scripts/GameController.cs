using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class GameController : MonoBehaviour {

    public enum GameState
    {
        INGAME,
        INMENU
    }

    public delegate void DOnGameStateChanged(GameState oldState, GameState newState);
    private static void NullStateChange(GameState o, GameState n) { }

    public DOnGameStateChanged onGameStateChanged = new DOnGameStateChanged(NullStateChange);

    BlurOptimized menuBackgroundBlur = null;


    public GameState state;

	// Use this for initialization
	void Start () {
        state = GameState.INGAME;
        menuBackgroundBlur = Camera.main.GetComponent<BlurOptimized>();
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

                menuBackgroundBlur.enabled = true;
            }
            else
            {
                state = GameState.INGAME;
                Time.timeScale = 1.0f;
                onGameStateChanged(GameState.INMENU, GameState.INGAME);

                menuBackgroundBlur.enabled = false;


            }
        }
    }
}
