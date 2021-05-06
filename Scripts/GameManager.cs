using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private bool mobile;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settings;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button unpauseButton;
    [SerializeField] private Canvas winLosePauseCanvas;
    [Header("WIN AND LOSE SETTINGS")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI diamondsReward;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip buttonClick;

    public enum GameState { win,lose,pause,unpause};
    private ISelectable selectedObject;
    private RaycastHit hit;
    private Vector2 touchPosition;
    private CanvasManager CANVAS_MANAGER;
    private SoundManager SOUND_MANAGER;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        pauseButton.onClick.AddListener(() => GameStatus(GameState.pause));
        unpauseButton.onClick.AddListener(() => GameStatus(GameState.unpause));
    }
    private void Start()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
    }
    private void Update()
    {
        if(mobile) CheckForInput(); //mobile
        else CheckForInputPC(); //pc
    }
    private bool checkDifference(Vector2 pos1, Vector2 pos2)
    {
        if (Mathf.Abs(pos1.x - pos2.x) >= 8f || Mathf.Abs(pos1.y - pos2.y) >= 8f) return false;
        return true;
    }
    private void CheckForInput() //mobile
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began) touchPosition = Input.GetTouch(0).position;
            if (Input.touches[0].phase == TouchPhase.Ended && checkDifference(Input.GetTouch(0).position, touchPosition) && !EventSystem.current.currentSelectedGameObject)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && Input.touchCount == 1)
                {
                    selectedObject = hit.collider.GetComponent<ISelectable>();
                    CANVAS_MANAGER.UpdateCanvas(selectedObject);
                }
            }
        }
    }
    private void CheckForInputPC() //pc
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    selectedObject = hit.collider.GetComponent<ISelectable>();
                    CANVAS_MANAGER.UpdateCanvas(selectedObject);
                }
            }
        }
    }

    public void GameStatus(GameState state)
    {
        if (state == GameState.win) //win
        {
            SOUND_MANAGER.BackgroundMusicStatus(true);
            SavedData data = SaveSystem.LoadData();
            data.diamondsAmount += (ResourceManager.RESOURCE_MANAGER.GetWaveLevel() * 100);
            SaveSystem.SaveData(data.workerData, data.items, data.wallData, data.towerData, data.arcaneVaultData, data.diamondsAmount);
            winLosePauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
            SOUND_MANAGER.PlaySound(winSound, 0.17f);
            level.text = "-wave " + ResourceManager.RESOURCE_MANAGER.GetWaveLevel() + "-";
            score.text = (ResourceManager.RESOURCE_MANAGER.GetWaveLevel() * 1000).ToString();
            diamondsReward.text = (ResourceManager.RESOURCE_MANAGER.GetWaveLevel() * 100).ToString();
            settings.SetActive(true);
            winScreen.SetActive(true);
        }
        else if(state == GameState.lose) //lose
        {
            SOUND_MANAGER.BackgroundMusicStatus(true);
            SavedData data = SaveSystem.LoadData();
            data.diamondsAmount += (ResourceManager.RESOURCE_MANAGER.GetWaveLevel() * 100)/ 4;
            SaveSystem.SaveData(data.workerData, data.items, data.wallData, data.towerData, data.arcaneVaultData, data.diamondsAmount);
            winLosePauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
            SOUND_MANAGER.PlaySound(loseSound, 0.17f);
            score.text = (ResourceManager.RESOURCE_MANAGER.GetWaveLevel() * 250).ToString();
            level.text = "-wave " + ResourceManager.RESOURCE_MANAGER.GetWaveLevel() + "-";
            diamondsReward.text = ((ResourceManager.RESOURCE_MANAGER.GetWaveLevel() * 100)/4).ToString();
            settings.SetActive(true);
            loseScreen.SetActive(true);
        }
        else if(state == GameState.pause)
        {
            winLosePauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
            SOUND_MANAGER.PlaySound(buttonClick, 0.3f);
            pauseScreen.SetActive(true);
        }
        else
        {
            winLosePauseCanvas.gameObject.SetActive(false);
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            SOUND_MANAGER.PlaySound(buttonClick, 0.3f);
        }

    }
    public void ChangeScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

}


