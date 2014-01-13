using UnityEngine;
using System.Collections;
public class MonsterBasic : MonoBehaviour
{
	private GameObject Player;
	private Transform Model;
	protected int currentHP = 100;

	// Use this for initialization
	protected void BaiscStart ()
	{
		Player = GameObject.Find("Player");
		Model = transform.FindChild("Model");
	}

	private int xDiff, yDiff;
	// Update is called once per frame
	protected void BaiscUpdate ()
	{
		xDiff = 0;
		yDiff = 0;

		xDiff = Mathf.RoundToInt(transform.position.x - Player.transform.position.x);
		yDiff = Mathf.RoundToInt(transform.position.y - Player.transform.position.y);

		if 		(xDiff <= 0 && yDiff < 0)
		{
			Model.eulerAngles = new Vector3(0, 0, 270 + Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
		}
		else if (xDiff < 0 && yDiff >= 0)
		{
			Model.eulerAngles = new Vector3(0, 0, 270 - Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
		}
		else if (xDiff >= 0 && yDiff > 0)
		{
			Model.eulerAngles = new Vector3(0, 0, 90 + Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
		}
		else if (xDiff > 0 && yDiff <= 0)
		{
			Model.eulerAngles = new Vector3(0, 0, 90 - Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
		}
		Model.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
	}

	public void ReceiveDamage (int dmg)
	{
		currentHP -= dmg;
		if (CheckIfDead())
		{
			Destroy(gameObject);
		}
	}

	bool CheckIfDead()
	{
		if (currentHP <= 0)
		{
			return true;
		}
		return false;
	}
}