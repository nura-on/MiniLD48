using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour
{
	void OnGUI ()
	{
		if (GUI.Button(new Rect(Screen.width / 2 - Screen.width * 0.2f / 2, Screen.height / 2 - Screen.height * 0.05f / 2, Screen.width * 0.2f, Screen.height * 0.05f), "New Game"))
		{
			Application.LoadLevel(1);
		}
	}
}