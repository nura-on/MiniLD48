using UnityEngine;
using System.Collections;

public class Bullet_HitEnemy : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(AnimateWalk());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	IEnumerator AnimateWalk ()
	{
		GameObject Model = gameObject;
		int Counter1 = 0;
		while(Counter1 < (renderer.material.mainTexture.width / 16) - 1)
		{
			Model.renderer.material.mainTextureOffset	= new Vector2(1f / (renderer.material.mainTexture.width / 16f) * Counter1, 0);
			Counter1++;
			yield return new WaitForSeconds(0.1f);
		}
		Destroy(gameObject);
	}
}
