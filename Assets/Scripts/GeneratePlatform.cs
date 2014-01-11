using UnityEngine;
using System.Collections;
public class GeneratePlatform : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		GameObject GroundPlatformPrefab = Resources.Load("Platform") as GameObject;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				GameObject createdPlatform = Instantiate(GroundPlatformPrefab) as GameObject;
				createdPlatform.transform.position = new Vector2(i * createdPlatform.transform.lossyScale.x + createdPlatform.transform.lossyScale.x / 2, j * createdPlatform.transform.lossyScale.y + createdPlatform.transform.lossyScale.y / 2);
				createdPlatform.renderer.material.mainTextureScale = new Vector2(createdPlatform.renderer.material.mainTexture.width / 48 * 15, createdPlatform.renderer.material.mainTexture.height / 48 * 15);
				if (Random.Range(0, 2) == 1)
				{
					createdPlatform.transform.FindChild("Light").light.color = Color.red;
				}
				else
				{
					createdPlatform.transform.FindChild("Light").light.color = Color.green;
				}
			}
		}
	}
}