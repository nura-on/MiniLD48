using UnityEngine;
using System.Collections;
public class Game : MonoBehaviour
{
	private int PlatformRows = 3;
	private int PlatformColumns = 3;

	private int PlatformWidth = 512, PlatformHeight = 512;
	private GameObject Platforms;
	private Platform[,] PlatformFields;

	public Platform.PlatformColorType[,] WinPatternTypes = new Platform.PlatformColorType[4, 4];
	private GameObject WinPattern;
	private GameObject[,] WinPatternBlocks;
	private GameObject WinPatternPositionBlinker;

    public enum GameState {
        NotRunning,
        InWave,
        WaceClear
    }
    public GameState state = GameState.NotRunning;

    private int currentWave = 0;

    public int CurrentWave
    {
        get { return currentWave; }
        set { currentWave = value; }
    }

    #region singleton
    private Game() { }
    private static Game _singleton;
    private Object _block;
    public static Game Instance {
        get { return (_singleton == null ? new Game() : _singleton); }
    }
    #endregion

    void Awake () {
        _singleton = this;
        _block = Resources.Load("WinPatternBlock") as GameObject;
    }
	void Start ()
	{
		DontDestroyOnLoad(gameObject);
	}
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			Application.LoadLevel(1);
		}
	}
	void OnLevelWasLoaded (int Level)
	{
		if (Level == 1)
		{
			InitiateGame();
		}
	}
	void InitiateGame ()
	{
		PlatformFields = new Platform[PlatformRows, PlatformColumns];
		WinPattern = new GameObject("WinPattern");
		WinPatternPositionBlinker = Instantiate(Resources.Load("WinPatternPositionBlinker") as GameObject) as GameObject;
		WinPatternPositionBlinker.transform.parent = WinPattern.transform;
		WinPatternBlocks = new GameObject[PlatformRows,PlatformColumns];

		GeneratePlatform();
		GenerateWinPattern();
        state = GameState.InWave;
	}

	void GeneratePlatform ()
	{
		Platforms = new GameObject("Platforms");
		GameObject GroundPlatformPrefab = Resources.Load("Platform") as GameObject;
		for (int i = 0; i < PlatformRows; i++)
		{
			for (int j = 0; j < PlatformColumns; j++)
			{
				GameObject createdPlatform = Instantiate(GroundPlatformPrefab) as GameObject;
				PlatformFields[i, j] = createdPlatform.GetComponent<Platform>();
				createdPlatform.transform.FindChild("Model").transform.localScale = new Vector3(PlatformWidth, PlatformHeight, 1);
				createdPlatform.transform.parent = Platforms.transform;
				createdPlatform.transform.position = new Vector2(i * PlatformWidth + PlatformWidth / 2, j * PlatformHeight + PlatformHeight / 2);
				createdPlatform.GetComponent<Platform>().PositionX = i;
				createdPlatform.GetComponent<Platform>().PositionY = j;
				createdPlatform.renderer.material.mainTextureScale = new Vector2(createdPlatform.renderer.material.mainTexture.width / 48 * 15, createdPlatform.renderer.material.mainTexture.height / 48 * 15);
				if (Random.Range(0, 2) == 1)
				{
					createdPlatform.transform.FindChild("Light").light.color = Color.green;
					createdPlatform.GetComponent<Platform>().ChangeMyType(Platform.PlatformColorType.Type1);
				}
				else
				{
					createdPlatform.transform.FindChild("Light").light.color = Color.red;
					createdPlatform.GetComponent<Platform>().ChangeMyType(Platform.PlatformColorType.Type2);
				}
			}
		}
	}

	void GenerateWinPattern ()
	{
		WinPattern.SetActive(false);
        WinPattern.transform.parent = Camera.main.transform;
		WinPattern.transform.localPosition = new Vector3(-35, -35, 1);
		for (int i = 0; i < PlatformRows; i++)
		{
			for (int j = 0; j < PlatformColumns; j++)
            {
				GameObject createdBlock = Instantiate(_block) as GameObject;
				WinPatternBlocks[i, j] = createdBlock;
				createdBlock.transform.parent = WinPattern.transform;
				createdBlock.transform.localPosition =  new Vector3(8 + i * 16 + i * 2, 8 + j * 16 + j * 2, 0);
				if (Random.Range(0, 2) == 1)
				{
					WinPatternTypes[i, j] = Platform.PlatformColorType.Type1;
					createdBlock.renderer.material.color = Color.green;
				}
				else
				{
					WinPatternTypes[i, j] = Platform.PlatformColorType.Type2;
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
	public bool CheckIfWinPatternIsReached ()
	{
		for (int i = 0; i < PlatformRows; i++)
		{
			for (int j = 0; j < PlatformColumns; j++)
			{
				if (WinPatternTypes[i, j] != PlatformFields[i, j].CurrentPlatformType)
				{
                    return false;
				}
				else if (i == PlatformRows - 1 && j == PlatformColumns - 1)
				{
                    return true;
				}
			}
		}
        return false;
	}
}