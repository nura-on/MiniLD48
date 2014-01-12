﻿using UnityEngine;
using System.Collections;
public class HitEnemy : MonoBehaviour
{
	public float WaitForFrameTime = 0.1f;
	public float TileWidth = 16;
    Transform Model;
	void Start ()
	{
		StartCoroutine(AnimateEffect());
	}
	IEnumerator AnimateEffect ()
	{
        Model = transform.FindChild("Model");
		int Counter1 = 0;
		while(Counter1 < (Model.renderer.material.mainTexture.width / TileWidth) - 1)
		{
			Model.renderer.material.mainTextureOffset	= new Vector2(1f / (Model.renderer.material.mainTexture.width / TileWidth) * Counter1, 0);
			Counter1++;
			yield return new WaitForSeconds(WaitForFrameTime);
		}
		Destroy(gameObject);
        //gameObject.SetActive(false);
	}
}