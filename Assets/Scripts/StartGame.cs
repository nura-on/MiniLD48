using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour
{
    private GUIStyle centeredStyle;
    public Font font;
    private bool visible = true;

    void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            if (visible)
            {
                yield return new WaitForSeconds(1.2f);
                visible = false;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                visible = true;
            }
        }
    }
    void OnGUI()
    {

        centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.font = font;
        camera.backgroundColor = new Color32(0, 0, 0, 0);

        GUI.Label(new Rect((Screen.width / 2) - 400, Screen.height / 2 - 250, 800, 500), "<color=white><size=40>RON JAMBO</size></color>", centeredStyle);
        GUI.Label(new Rect((Screen.width / 2) - 400, Screen.height / 2 - 180, 800, 500), "<color=red><size=24>(early alpha version)</size></color>", centeredStyle);

        centeredStyle.alignment = TextAnchor.UpperLeft;
        GUI.Label(new Rect((Screen.width / 2) - 200, Screen.height / 2 - 100, 512, 500), "<color=white><size=15>There are 9 colored areas. \nTry to match the colors of the \nscheme shown to you at the beginning \nof each wave.</size></color>", centeredStyle);
        centeredStyle.alignment = TextAnchor.UpperCenter;
        if (visible) GUI.Label(new Rect((Screen.width / 2) - 400, Screen.height / 2 + 140, 800, 500), "<color=white><size=15>Press any key to start the game</size></color>", centeredStyle);

        GUI.Label(new Rect((Screen.width / 2) - 400, Screen.height / 2 + 180, 800, 500), "<color=white><size=10>a game made for #MiniLD48 by</size></color>", centeredStyle);
        GUI.Label(new Rect((Screen.width / 2) - 400, Screen.height / 2 + 200, 800, 500), "<color=white><size=10>Zerano, nyrrrr & Butan</size></color>", centeredStyle);

        // GUI.Label(new Rect((Screen.width / 2) - 400, Screen.height / 2 + 110, 800, 500), "<color=white><size=10>Music by: Tommy Bulpa</size></color>", centeredStyle);



        //There are 9 colored areas. Try to match the colors of the scheme shown to you at the beginning of each wave.
        if (Event.current.type == EventType.KeyDown)
        {

            Application.LoadLevel(1);
        }

        // TODO press any key
        // TODO title screen
    }
}