using UnityEngine;
using System.Collections;
public class Game : MonoBehaviour
{
    private int platformRows = 3;
    private int platformColumns = 3;

    private int platformWidth = 512, platformHeight = 512;
    private GameObject platforms;
    private Platform[,] platformFields;

    public Platform.PlatformColorType[,] winPatternTypes = new Platform.PlatformColorType[4, 4];
    private GameObject winPattern, _platform;
    private GameObject[,] winPatternBlocks;
    private GameObject winPatternPositionBlinker;
    private string countDownText;

    public GUIStyle guiStyle;

    public enum GameState
    {
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
    private Object _winPattern;
    public static Game Instance
    {
        get { return (_singleton == null ? new Game() : _singleton); }
    }
    #endregion

    void Awake()
    {
        _singleton = this;
        _block = Resources.Load("WinPatternBlock") as GameObject;
        _winPattern = Resources.Load("WinPatternPositionBlinker") as GameObject;
        _platform = Resources.Load("Platform") as GameObject;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Application.LoadLevel(1);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - Screen.height * 0.2f, 0, 0), countDownText, guiStyle);
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            state = GameState.NotRunning;
            InitiateGame();
            StartCoroutine(StartWaveCountDown());
            StartCoroutine(DisplayPatterForXSeconds(3.0f));
        }
    }

    IEnumerator StartWaveCountDown()
    {
        countDownText = "<color=white><size=50>NEW WAVE</size></color>";
        yield return new WaitForSeconds(1.5f);
        countDownText = "<color=white><size=50>3</size></color>";
        yield return new WaitForSeconds(1f);
        countDownText = "<color=white><size=50>2</size></color>";
        yield return new WaitForSeconds(1f);
        countDownText = "<color=white><size=50>1</size></color>";
        yield return new WaitForSeconds(1f);
        countDownText = "<color=white><size=50>GO!</size></color>";
        yield return new WaitForSeconds(1f);
        countDownText = "";
        //yield return null;
        state = GameState.InWave;
    }

    IEnumerator DisplayPatterForXSeconds(float sec)
    {
        AbleDisableWinPattern(true, 1, 1);
        yield return new WaitForSeconds(sec);
        AbleDisableWinPattern(false, 1, 1);
    }

    void InitiateGame()
    {
        platformFields = new Platform[platformRows, platformColumns];
        winPattern = new GameObject("WinPattern");
        winPatternPositionBlinker = Instantiate(_winPattern) as GameObject;
        winPatternPositionBlinker.transform.parent = winPattern.transform;
        winPatternBlocks = new GameObject[platformRows, platformColumns];

        GeneratePlatform();
        GenerateWinPattern();
    }

    void GeneratePlatform()
    {
        platforms = new GameObject("Platforms");
        GameObject GroundPlatformPrefab = _platform;
        for (int i = 0; i < platformRows; i++)
        {
            for (int j = 0; j < platformColumns; j++)
            {
                GameObject createdPlatform = Instantiate(GroundPlatformPrefab) as GameObject;
                platformFields[i, j] = createdPlatform.GetComponent<Platform>();
                createdPlatform.transform.FindChild("Model").transform.localScale = new Vector3(platformWidth, platformHeight, 1);
                createdPlatform.transform.parent = platforms.transform;
                createdPlatform.transform.position = new Vector2(i * platformWidth + platformWidth / 2, j * platformHeight + platformHeight / 2);
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

    void GenerateWinPattern()
    {
        winPattern.SetActive(false);
        winPattern.transform.parent = Camera.main.transform;
        winPattern.transform.localPosition = new Vector3(-26, -26, 1);
        for (int i = 0; i < platformRows; i++)
        {
            for (int j = 0; j < platformColumns; j++)
            {
                GameObject createdBlock = Instantiate(_block) as GameObject;
                winPatternBlocks[i, j] = createdBlock;
                createdBlock.transform.parent = winPattern.transform;
                createdBlock.transform.localPosition = new Vector3(8 + i * 16 + i * 2, 8 + j * 16 + j * 2, 0);
                if (Random.Range(0, 2) == 1)
                {
                    winPatternTypes[i, j] = Platform.PlatformColorType.Type1;
                    createdBlock.renderer.material.color = Color.green;
                }
                else
                {
                    winPatternTypes[i, j] = Platform.PlatformColorType.Type2;
                    createdBlock.renderer.material.color = Color.red;
                }
            }
        }
    }
    public void AbleDisableWinPattern(bool State, int xPos, int yPos)
    {
        winPattern.SetActive(State);
        winPatternPositionBlinker.transform.position = new Vector3(winPatternBlocks[xPos, yPos].transform.position.x, winPatternBlocks[xPos, yPos].transform.position.y, winPatternBlocks[xPos, yPos].transform.position.z - 0.1f);
    }
    public bool CheckIfWinPatternIsReached()
    {
        for (int i = 0; i < platformRows; i++)
        {
            for (int j = 0; j < platformColumns; j++)
            {
                if (winPatternTypes[i, j] != platformFields[i, j].CurrentPlatformType)
                {
                    return false;
                }
                else if (i == platformRows - 1 && j == platformColumns - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
}