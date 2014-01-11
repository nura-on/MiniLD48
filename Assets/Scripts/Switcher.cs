using UnityEngine;
using System.Collections;

public class Switcher : MonoBehaviour
{
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.name == "Player")
		{
			renderer.material.mainTextureOffset = new Vector2(0.5f, 0);
			if (transform.parent.FindChild("Light").light.color == Color.green)
			{
				transform.parent.FindChild("Light").light.color = Color.red;
				
			}
			else
			{
				transform.parent.FindChild("Light").light.color = Color.green;
			}
		}
	}
	void OnTriggerExit2D (Collider2D col)
	{
		if (col.name == "Player")
		{
			renderer.material.mainTextureOffset = new Vector2(0, 0);
		}
	}
}