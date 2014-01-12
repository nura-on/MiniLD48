using UnityEngine;
using System.Collections;

public class WinPatternPositionBlinker : MonoBehaviour
{
	private int counter = 0;
	void Update ()
	{
		if (counter == 20)
		{
			if (renderer.enabled == true)
				renderer.enabled = false;
			else
				renderer.enabled = true;
			counter = 0;
		}
		counter++;
	}
}