using UnityEngine;
using System.Collections;

public class Switcher : MonoBehaviour
{

    private Game _game;

    void Awake() {
        _game = Game.Instance;
    }

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.name == "Player")
		{
			_game.AbleDisableWinPattern(true, transform.parent.GetComponent<Platform>().PositionX, transform.parent.GetComponent<Platform>().PositionY);
			renderer.material.mainTextureOffset = new Vector2(0.5f, 0);
			if (transform.parent.FindChild("Light").light.color == Color.green)
			{
				transform.parent.FindChild("Light").light.color = Color.red;
				transform.parent.GetComponent<Platform>().ChangeMyType(Platform.PlatformColorType.Type2);
			}
			else
			{
				transform.parent.FindChild("Light").light.color = Color.green;
				transform.parent.GetComponent<Platform>().ChangeMyType(Platform.PlatformColorType.Type1);
			}
            if (_game.CheckIfWinPatternIsReached()) {

                Debug.Log("PatternMatch!!!");
                Application.LoadLevel(1);
                _game.CurrentWave++;
            }
         
		}
	}
	void OnTriggerExit2D (Collider2D col)
	{
		if (col.name == "Player")
		{
			renderer.material.mainTextureOffset = new Vector2(0, 0);
			_game.AbleDisableWinPattern(false, transform.parent.GetComponent<Platform>().PositionX, transform.parent.GetComponent<Platform>().PositionY);
		}
	}
}