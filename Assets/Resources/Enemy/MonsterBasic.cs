using UnityEngine;
using System.Collections;
public class MonsterBasic : MonoBehaviour
{
	public float DetectDistance;
	private bool InRange;
	public GameObject Player;

	// Use this for initialization
	void Start ()
	{
		Player = GameObject.Find("Player");
	}

	private int xDiff, yDiff;
	// Update is called once per frame
	void Update ()
	{
		if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(Player.transform.position.x, Player.transform.position.y)) < 30 * 16)
		{
			InRange = true;
		}
		else
		{
			InRange = false;
		}

		if (InRange)
		{
			xDiff = 0;
			yDiff = 0;

			xDiff = Mathf.RoundToInt(transform.position.x - Player.transform.position.x);
			yDiff = Mathf.RoundToInt(transform.position.y - Player.transform.position.y);

			if 		(xDiff <= 0 && yDiff < 0)
			{
				transform.eulerAngles = new Vector3(0, 0, 270 + Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
			}
			else if (xDiff < 0 && yDiff >= 0)
			{
				transform.eulerAngles = new Vector3(0, 0, 270 - Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
			}
			else if (xDiff >= 0 && yDiff > 0)
			{
				transform.eulerAngles = new Vector3(0, 0, 90 + Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
			}
			else if (xDiff > 0 && yDiff <= 0)
			{
				transform.eulerAngles = new Vector3(0, 0, 90 - Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(yDiff * 1f / xDiff * 1f)));
			}
		}
	}
}