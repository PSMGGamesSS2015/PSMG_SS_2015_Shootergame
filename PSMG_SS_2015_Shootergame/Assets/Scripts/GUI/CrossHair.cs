// Draw a crosshair in the center of the screen. Might want to implement this in a different way together with other GUI implementations

using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour {

    // The 2D texture for the crosshair
    public Texture2D crosshairTexture;

    // Scale factor for the texture to increase or decrease its size
    public float crosshairScale = 0.1f;

    // Should the default mouse cursor be hidden?
    public bool hideCursor = true;

    void Start()
    {
        // Upon initialisation, set the (mouse) cursor's visibility to false
        if (hideCursor)
        {
            Cursor.visible = false;
        }
    }

    // Called once everytime the GUI is updated
    void OnGUI()
    {
        // If the crosshairTexture is not null, draw it on the UI
        if (crosshairTexture != null)
        {
            // On the GUI, draw the assigned texture with the given scale in the center of the screen
            GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale) / 2, crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
        }
    }

}
