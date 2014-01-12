using UnityEngine;
using System.Collections;
public class Game : MonoBehaviour
{
	private int PlatformRows = 3;
	private int PlatformColumns = 3;

	private Platform[,] Platforms;
	private Platform.PlatformColorType[,] PlattformPattern = new Platform.PlatformColorType[4, 4];

	private GameObject WinPattern;
	private GameObject[,] WinPatternBlocks;
	private GameObject WinPatternPositionBlinker;
	void Start ()
	{
		Platforms = new Platform[PlatformRows, PlatformColumns];

		WinPattern = new GameObject("WinPattern");
		WinPatternPositionBlinker = Instantiate(Resources.Load("WinPatternPositionBlinker") as GameObject) as GameObject;
		WinPatternPositionBlinker.transform.parent = WinPattern.transform;
		WinPatternBlocks = new GameObject[PlatformRows,PlatformColumns];

		GeneratePlatform();
		GeneratePlatformWinPattern();
	}            
	void GeneratePlatform ()
	{
		GameObject GroundPlatformPrefab = Resources.Load("Platform") as GameObject;
		for (int i = 0; i < PlatformRows; i++)
		{
			for (int j = 0; j < PlatformColumns; j++)
			{
				GameObject createdPlatform = Instantiate(GroundPlatformPrefab) as GameObject;
				Platforms[i, j] = createdPlatform.GetComponent<Platform>();

				createdPlatform.transform.position = new Vector2(i * createdPlatform.transform.lossyScale.x + createdPlatform.transform.lossyScale.x / 2, j * createdPlatform.transform.lossyScale.y + createdPlatform.transform.lossyScale.y / 2);
				createdPlatform.GetComponent<Platform>().PositionX = i;
				createdPlatform.GetComponent<Platform>().PositionY = j;
				createdPlatform.renderer.material.mainTextureScale = new Vector2(createdPlatform.renderer.material.mainTexture.width / 48 * 15, createdPlatform.renderer.material.mainTexture.height / 48 * 15);
				if (Random.Range(0, 2) == 1)
				{
					createdPlatform.transform.FindChild("Light").light.color = Color.red;
					createdPlatform.GetComponent<Platform>().ChangeMyType(Platform.PlatformColorType.Type1);
				}
				else
				{
					createdPlatform.transform.FindChild("Light").light.color = Color.green;
					createdPlatform.GetComponent<Platform>().ChangeMyType(Platform.PlatformColorType.Type2);
				}
			}
		}
	}

	void GeneratePlatformWinPattern ()
	{
		WinPattern.SetActive(false);
		WinPattern.transform.parent = GameObject.Find("Player/Camera").transform;
		WinPattern.transform.localPosition = new Vector3(-35, -35, 1);
		for (int i = 0; i < PlatformRows; i++)
		{
			for (int j = 0; j < PlatformColumns; j++)
            {
				GameObject createdBlock = Instantiate(Resources.Load("WinPatternBlock") as GameObject) as GameObject;
				WinPatternBlocks[i, j] = createdBlock;
				createdBlock.transform.parent = WinPattern.transform;
				createdBlock.transform.localPosition =  new Vector3(8 + i * 16 + i * 2, 8 + j * 16 + j * 2, 0);
				if (Random.Range(0, 2) == 1)
				{
					PlattformPattern[i, j] = Platform.PlatformColorType.Type1;
					createdBlock.renderer.material.color = Color.green;
				}
				else
				{
					PlattformPattern[i, j] = Platform.PlatformColorType.Type2;
					createdBlock.renderer.material.color = Color.red;
				}
			}
		}
	}
	public void AbleDisableWinPattern (bool State, int xPos, int yPos)
	{
		WinPattern.SetActive(State);
		WinPatternPositionBlinker.transform.position = new Vector3(WinPatternBlocks[xPos, yPos].transform.position.x, WinPatternBlocks[xPos, yPos].transform.position.y, WinPatternBlocks[xPos, yPos].transform.position.z - 0.1f);
	}
}