using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MyButton : MonoBehaviour
{
    [SerializeField] private Sprite normalButtonLook; //the look of the button if available and if sufficient resources
    [SerializeField] private Sprite insufficientResourcesButtonLook; //the look of the button if available and if insufficient resources
    [SerializeField] private Sprite lockedButtonLook; //the look of the button if not available
    [SerializeField] private Sprite notUsableButtonLook;
    [SerializeField] private TextMeshProUGUI costText; //the text that is going to display the amount of resources required to buy the prefab
    [SerializeField] private Image goldImage;
    [SerializeField] private Image woodImage;
    [SerializeField] private AudioClip moreGoldRequiredSoundEffect;
    [SerializeField] private AudioClip moreLumberRequiredSoundEffect;
    [SerializeField] private bool isSpell;
    private bool isUnlocked = true;
    protected ResourceManager RESOURCE_MANAGER;
    protected IButtonState currentState;
    private ICost prefab;
    protected string resource;
    protected int price;
    #region States
    protected readonly IButtonState startState = new StartState();
    protected readonly IButtonState readyToUseState = new ReadyToUse();
    protected readonly IButtonState notUnlockedState = new NotUnlocked();
    protected readonly IButtonState insufficientResourcesState = new InsufficientResources();
    protected readonly IButtonState notUsableState = new NotUsable();
    protected readonly IButtonState readyToUseSpellState = new ReadyToUseSpell();
    #endregion
    #region Getters
    public bool IsSpell()
    {
        return isSpell;
    }
    public TextMeshProUGUI GetCostText()
    {
        return costText;
    }
    public Image GetGoldImage()
    {
        return goldImage;
    }
    public Image GetWoodImage()
    {
        return woodImage;
    }
    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public AudioClip GetMoreGoldRequiredSoundEffect()
    {
        return moreGoldRequiredSoundEffect;
    }
    public AudioClip GetMoreLumberRequiredSoundEffect()
    {
        return moreLumberRequiredSoundEffect;
    }
    public IButtonState GetReadyToUseState()
    {
        return readyToUseState;
    }
    public IButtonState GetReadyToUseSpellState()
    {
        return readyToUseSpellState;
    }
    public IButtonState GetNotUnlockedState()
    {
        return notUnlockedState;
    }
    public IButtonState GetInsufficientResourcesState()
    {
        return insufficientResourcesState;
    }
    public IButtonState GetNotUsableState()
    {
        return notUsableState;
    }
    public ICost GetICost()
    {
        return prefab;
    }
    public IButtonState GetCurrentState()
    {
        return currentState;
    }
    public virtual string GetResource()
    {
        return resource;
    }
    public virtual int GetPrice()
    {
        return price;
    }
    public Sprite GetNormalButtonLook()
    {
        return normalButtonLook;
    }
    public Sprite GetInsufficientResourcesButtonLook()
    {
        return insufficientResourcesButtonLook;
    }
    public Sprite GetLockedButtonLook()
    {
        return lockedButtonLook;
    }
    public Sprite GetNotUsableButtonLook()
    {
        return notUsableButtonLook;
    }
    #endregion
    #region Setters
    //public void SetICost(ICost prefab)
    //{
    //    this.prefab = prefab;
    //    string[] parts = prefab.GetCost().Split(' ');
    //    price = int.Parse(parts[0]);
    //    resource = parts[1];
    //    SetState(startState);
    //}
    public IButtonState GetNeutralState()
    {
        return startState;
    }
    /// <summary>
    /// Set and execute a state
    /// </summary>
    /// <param name="state">State to be set and executed</param>
    public void SetState(IButtonState state)
    {
        currentState = state;
        currentState.executeAction(this);
    }
    #endregion
    /// <summary>
    /// Called on the first frame
    /// </summary>
    protected virtual void Start()
    {
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
        if(prefab!=null && currentState!=notUnlockedState && currentState!=notUsableState)
        {
            DisplayPriceAndDisplayResource();
        }
    }
    public virtual void Init()
    {
        SetState(startState);
    }
    public void Init<T>(T building) where T : MonoBehaviour
    {
        ICost asBuilding = (ICost)building;
        prefab = asBuilding;
        string[] parts = prefab.GetCost().Split(' ');
        price = int.Parse(parts[0]);
        resource = parts[1];
        isUnlocked = asBuilding.IsUnlocked();
        SetState(startState);
    }
    /// <summary>
    /// Try to buy/place the prefab that is associated with the button that is clicked
    /// </summary>
    public bool BuyPrefab()
    {
        return currentState.Buy();
    }
    /// <summary>
    /// Displat the price below the button and display the resource type ( wood image or gold image )
    /// </summary>
    protected void DisplayPriceAndDisplayResource()
    {
        //string cost = prefab.GetCost(); //example: 1500 wood , 400 gold
        if (resource.Contains("wood"))
        {
            woodImage.gameObject.SetActive(true);
        }
        else
        {
            goldImage.gameObject.SetActive(true);
        }
        costText.text = price.ToString();
    }
    public virtual void RefreshButtonUsability()
    {
        SetState(startState);
    }
}
