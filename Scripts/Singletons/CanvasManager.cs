using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class CanvasManager : MonoBehaviour
{
    #region Singleton
    public static CanvasManager CANVAS_MANAGER;
    #endregion
    [SerializeField] private Canvas canvas; // Every ui element we see on the screen is placed on this canvas
    [SerializeField] private GameObject buttonsBackgroundImage; //The background image of the buttons 
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI alertText; //Alert text to inform the user that something that he is doing is not allowed
    private const float spaceBetweenButtons = 30f; //Space between the buttons 
    #region Resources
    private TextMeshProUGUI woodAmount;
    private TextMeshProUGUI goldAmount;
    private TextMeshProUGUI meatAmount;
    private TextMeshProUGUI waveLevel;
    #endregion
    private List<MyButton> activeButtons; //Currently active buttons on the canvas
    private ISelectable selectedObject; //Last selected object
    private WorkerManager WORKER_MANAGER; //Reference to the WorkerManager Singleton

    /// <summary>
    /// Called on script loaded
    /// </summary>
    private void Awake()
    {
        CANVAS_MANAGER = this;
        activeButtons = new List<MyButton>();
        woodAmount = GameObject.FindWithTag("WoodAmount").GetComponent<TextMeshProUGUI>();
        goldAmount = GameObject.FindWithTag("GoldAmount").GetComponent<TextMeshProUGUI>();
        meatAmount = GameObject.FindWithTag("MeatAmount").GetComponent<TextMeshProUGUI>();
        waveLevel = GameObject.FindWithTag("WaveLevel").GetComponent<TextMeshProUGUI>();
    }
    /// <summary>
    /// Called on first frame
    /// </summary>
    private void Start()
    {
        WORKER_MANAGER = WorkerManager.WORKER_MANAGER;
        HideButtonsBackgroundImage();
        exitButton.onClick.AddListener(() => OnObjectUnselected());
    }
    /// <summary>
    /// Manages objects that are selected
    /// If first selected object is worker and after that building is selected, the worker is going to go toward the previous selected building
    /// If first selected object is heal tower and after that worker is selected, all the workers in defined radius are going to be healed
    /// </summary>
    /// <param name="currentSelectedObject">Selected object</param>
    public void UpdateCanvas(ISelectable currentSelectedObject) //ne e fucking ic dobro!!!!
    {
        //if (currentSelectedObject == selectedObject) return;
        if (selectedObject != null) selectedObject.Unselect();
        if (currentSelectedObject is Building building && selectedObject is Worker worker)
        {
            worker.GoToward(building.gameObject);
            selectedObject = null;
            return;
        }
        else if (currentSelectedObject is Worker _worker && selectedObject is HealTower healTower)
        {
            IEnumerator healCoroutine = healTower.HealWorkers(_worker);
            StartCoroutine(healCoroutine);
            selectedObject = null;
            return;
        }
        else if (currentSelectedObject != null) currentSelectedObject.Select();
        selectedObject = currentSelectedObject;

    }
    public List<MyButton> GetActiveButtons()
    {
        return activeButtons;
    }
    public void SetSelectedObject(ISelectable selected)
    {
        selectedObject = selected;
    }
    public ISelectable GetSelectedObject()
    {
        return selectedObject;
    }
    /// <summary>
    /// Show the buttons on the screen that selected object have and apply button task to each of these buttons
    /// Show the background image of the buttons. The size of the image is calculated based on the number of buttons that need to be shown
    /// </summary>
    /// <param name="buttons">Buttons that the building have</param>
    /// <param name="building">The building we selected</param>
    /// <param name="buildings">Buildings that corresponds to the buttons</param>
    public void ShowButtonsAndHandleButtonTask<T>(MyButton[] buttons, ITask building, T[] buildings) where T : MonoBehaviour
    {
        //If the object does not have any buttons to display
        if (buttons.Length == 0) return;

        #region Set X Position For The First Button
        float imageWidth = buttons[0].GetComponent<RectTransform>().rect.width;
        float xPosImage = (-buttons.Length / 2) * (imageWidth + spaceBetweenButtons);
        if (buttons.Length % 2 == 0) xPosImage += (imageWidth + spaceBetweenButtons) / 2;
        #endregion

        for (int i = 0; i < Mathf.Max(buttons.Length, buildings.Length); i++)
        {
            //Instantiate button in the scene ( make it visible to the user )
            MyButton button = Instantiate(buttons[i]);
            if (i < buildings.Length)
            {
                button.Init<T>(buildings[i]);
            }
            else if(!button.transform.tag.Equals("Destroy"))
            {
                button.Init();
            }
            //Set parent for the button
            button.transform.SetParent(canvas.transform, false);
            //
            button.GetComponent<RectTransform>().anchoredPosition = new Vector3(xPosImage, 130);
            //For each button add offset
            xPosImage += (imageWidth + spaceBetweenButtons);
            //Add functionality to the button
            button.GetComponent<Button>().onClick.AddListener(() => building.ButtonTask(button));
            if (i < buildings.Length) building.GetDictionary().Add(button, buildings[i]);
            //Add the button in the list for all active buttons
            activeButtons.Add(button);
        }
    }
    public void RemoveActiveButtons()
    {
        activeButtons.ForEach(button => Destroy(button.gameObject));
        activeButtons.Clear();
        activeButtons.TrimExcess();
        HideButtonsBackgroundImage();
    }
    /// <summary>
    /// Remove the active buttons from the canvas, return the active plane color its default color, and hide the buttons image background
    /// </summary>
    public void OnObjectUnselected()
    {
        selectedObject.Unselect();
        selectedObject = null;
    }
    /// <summary>
    /// Show background image for the buttons and set its size
    /// </summary>
    /// <param name="length">Number of buttons</param>
    public void ShowButtonsBackgroundImage(int length)
    {
        buttonsBackgroundImage.GetComponent<Image>().rectTransform.sizeDelta = new Vector3(520 + (length - 1) * 245, 190);
        buttonsBackgroundImage.SetActive(true);
    }
    public void HideButtonsBackgroundImage()
    {
        buttonsBackgroundImage.SetActive(false);
    }
    public void ShowWoodAmount(int woodAmount)
    {
        this.woodAmount.text = woodAmount.ToString();
    }
    public void ShowGoldAmount(int goldAmount)
    {
        this.goldAmount.text = goldAmount.ToString();
    }
    public void ShowMeatAmount(int meatAmount, int maxMeatAmount)
    {
        this.meatAmount.text = meatAmount.ToString() + "/" + maxMeatAmount.ToString();
    }
    public void ShowWaveLevel(int waveLevel)
    {
        this.waveLevel.text = "Wave " + waveLevel.ToString();
    }
    public Canvas GetCanvas()
    {
        return canvas;
    }
    /// <summary>
    /// Alert the user that his action is not allowed at the moment he attempted to execute
    /// </summary>
    /// <param name="message"></param>
    public void DisplayAlertMessageToUser(string message)
    {
        if (alertText.gameObject.active) return;
        alertText.text = message;
        alertText.gameObject.SetActive(true);
        IEnumerator hideText = HideText(alertText);
        StartCoroutine(hideText);
    }
    /// <summary>
    /// Hide the alert message after X seconds
    /// </summary>
    /// <param name="text">Text to hide</param>
    /// <returns></returns>
    private IEnumerator HideText(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(1.5f);
        text.gameObject.SetActive(false);
    }
    public void UpdateButtons()
    {
        if (activeButtons.Count != 0)
            activeButtons
                    .Where(btn => btn.GetCurrentState() != null) //avoid Destroy button
                    .ToList()
                    .ForEach(button => button.RefreshButtonUsability());
    }
}
