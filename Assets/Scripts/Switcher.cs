using UnityEngine;
using System.Collections;

public class Switcher : MonoBehaviour
{

    private Game _game;
    private Light _light;
    private Platform _platform;

    void Awake() {
        _game = Game.Instance;
        _light = transform.parent.FindChild("Light").light;
        _platform = transform.parent.GetComponent<Platform>();
    }

	void OnTriggerEnter2D (Collider2D col)
	{
        if (col.gameObject.name == "Player")
		{
            _game.TriggerWinPattern(true, _platform.PositionX, _platform.PositionY);
			renderer.material.mainTextureOffset = new Vector2(0.5f, 0);
			if (_light.color == Color.green)
			{
                _light.color = Color.red;
                _platform.ChangeMyType(Platform.PlatformColorType.Type2);
			}
			else
			{
				_light.color = Color.green;
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
		if (col.gameObject.name == "Player")
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