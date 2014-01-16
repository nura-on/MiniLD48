using UnityEngine;
using System.Collections;

public class Switcher : MonoBehaviour
{

    private Game _game;
    private Color _lightColor;
    private Platform _platform;

    void Awake() {
        _game = Game.Instance;
        _lightColor = transform.parent.FindChild("Light").light.color;
        _platform = transform.parent.GetComponent<Platform>();
    }

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.name == "Player")
		{
            _game.TriggerWinPattern(true, _platform.PositionX, _platform.PositionY);
			renderer.material.mainTextureOffset = new Vector2(0.5f, 0);
			if (_lightColor == Color.green)
			{
				_lightColor = Color.red;
                _platform.ChangeMyType(Platform.PlatformColorType.Type2);
			}
			else
			{
				_lightColor = Color.green;
                _platform.ChangeMyType(Platform.PlatformColorType.Type1);
			}
            if (_game.CheckIfWinPatternIsReached()) {
                // TODO reload and load next wave
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
			StartCoroutine(DisableWinPatternAfterXSeconds());
		}
	}
	IEnumerator DisableWinPatternAfterXSeconds ()
	{
		yield return new WaitForSeconds(1f);
        _game.TriggerWinPattern(false, _platform.PositionX, _platform.PositionY);
	}
}