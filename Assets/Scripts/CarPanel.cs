using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class CarPanel : MonoBehaviour, PanelInterface
{
    [SerializeField] private GameObject carPanel;
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private List<string> carIds;
    [SerializeField] private TMP_Dropdown allCarsListDropdown;
    [SerializeField] private TMP_Dropdown ownedCarForEquipingDropdown;
    [SerializeField] private TMP_Dropdown ownedCarForSellingDropdown;
    [SerializeField] private TextMeshProUGUI equipedCarMessage;
    [SerializeField] private Button buycar;
    [SerializeField] private Button sellCar;
    [SerializeField] private Button equipCar;
    [SerializeField] private Button goToLoggedInPanel;
    private Action<string> onEquipCarSuccessAction;
    private Action<string> onEquipCarFailureAction;
    private Action onEquipCarConnectionErrorAction;
    private Action<string> onBuyCarSuccessAction;
    private Action<string> onBuyCarFailureAction;
    private Action onBuyCarConnectionErrorAction;
    private Action<string> onSellCarSuccessAction;
    private Action<string> onSellCarFailureAction;
    private Action onSellCarConnectionErrorAction;
    private string currentBuyCarId="";
    private string currentSellCarId = "";
    private string currentEquipCarId = "";
    private List<TMP_Dropdown.OptionData> allCarsList = new List<TMP_Dropdown.OptionData>();
    private List<TMP_Dropdown.OptionData> ownedCarsList = new List<TMP_Dropdown.OptionData>();
    private int index;
    private bool equipingCarCalled;
    private void Start()
    {
        onEquipCarSuccessAction += onEquipCarSuccessMethod;
        onEquipCarFailureAction += onEquipCarFailureMethod;
        onEquipCarConnectionErrorAction += onEquipCarConnectionErrorMethod;
        onSellCarSuccessAction += onSellCarSuccessMethod;
        onSellCarFailureAction += onSellCarFailureMethod;
        onSellCarConnectionErrorAction += onSellCarConnectionErrorMethod;
        onBuyCarSuccessAction += onBuyCarSuccessMethod;
        onBuyCarFailureAction += onBuyCarFailureMethod;
        onBuyCarConnectionErrorAction += onBuyCarConnectionErrorMethod;
        allCarsListDropdown.onValueChanged.AddListener(allCarsListDropdownChange);
        ownedCarForEquipingDropdown.onValueChanged.AddListener(ownedCarForEquipingDropdownChange);
        ownedCarForSellingDropdown.onValueChanged.AddListener(ownedCarForSellingDropdownChange);
        buycar.onClick.AddListener(BuyCar);
        sellCar.onClick.AddListener(SellCar);
        equipCar.onClick.AddListener(EquipCar);
        goToLoggedInPanel.onClick.AddListener(GoToLoggedInPanel);
        ClearTexts();
    }
    private void GoToLoggedInPanel()
    {
        StartCoroutine(ChangePanel(carPanel,loggedInPanel,0));
    }
    private void AddOrRemoveToOwnedCarListAtStart(string carIdToAdd = "", string carIdToRemove = "")
    {
        if (carIdToAdd != "") ownedCarsList.Add(new TMP_Dropdown.OptionData(carIdToAdd));
        if (carIdToRemove != "")
        {
            foreach (var v in ownedCarsList)
            {
                if (v.text == carIdToRemove)
                {
                    ownedCarsList.Remove(v);
                    break;
                }
            }
        }
        ownedCarForEquipingDropdown.ClearOptions();
        ownedCarForEquipingDropdown.AddOptions(ownedCarsList);
        if (ownedCarForEquipingDropdown.options.Count > 0)
        {
            equipCar.interactable = true;
            currentEquipCarId = ownedCarForEquipingDropdown.options[0].text;
        }
        else
        {
            equipCar.interactable = false;
        }
        ownedCarForSellingDropdown.ClearOptions();
        ownedCarForSellingDropdown.AddOptions(ownedCarsList);
        if (ownedCarForSellingDropdown.options.Count > 0)
        {
            sellCar.interactable = true;
            currentSellCarId = ownedCarForSellingDropdown.options[0].text;
        }
        else
        {
            sellCar.interactable = false;
        }
    }
    private void AddOrRemoveToAllCarListAtStart(string carIdToAdd = "", string carIdToRemove = "")
    {
        if (carIdToAdd != "") allCarsList.Add(new TMP_Dropdown.OptionData(carIdToAdd));
        if (carIdToRemove != "")
        {
            foreach(var v in allCarsList)
            {
                if (v.text == carIdToRemove)
                {
                    allCarsList.Remove(v);
                    break;
                }
            }
        }
        allCarsListDropdown.ClearOptions();
        allCarsListDropdown.AddOptions(allCarsList);
        if (allCarsListDropdown.options.Count > 0)
        {
            buycar.interactable = true;
            currentBuyCarId = allCarsListDropdown.options[0].text;
        }
        else
        {
            buycar.interactable = false;
        }
    }
    private void BuyCar()
    {
        APICalls.BuyCar(currentBuyCarId, PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onBuyCarSuccessAction, onBuyCarFailureAction, onBuyCarConnectionErrorAction);
    }
    private void SellCar()
    {
        APICalls.SellCar(currentSellCarId,PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName),onSellCarSuccessAction,onSellCarFailureAction,onSellCarConnectionErrorAction);
    }
    private void EquipCar()
    {
        APICalls.EquipCar(currentEquipCarId,PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName),onEquipCarSuccessAction,onEquipCarFailureAction,onSellCarConnectionErrorAction);
    }
    private void onEquipCarSuccessMethod(string jsonData)
    {
        if (equipingCarCalled)
        {
            AddOrRemoveToOwnedCarListAtStart(carIdToAdd: carIds[index - 1]);
            AddOrRemoveToAllCarListAtStart(carIdToRemove: carIds[index - 1]);
            equipingCarCalled = false;
        }
        else
        {
            equipedCarMessage.text = "Equipped Car "+currentEquipCarId;
        }
    }
    private void onEquipCarFailureMethod(string jsonData)
    {
        EquipCarAndBuyCarDownloadType equipCarDownloadType = JsonUtility.FromJson<EquipCarAndBuyCarDownloadType>(jsonData);
        Debug.Log(equipCarDownloadType.code + " " + equipCarDownloadType.message);
        equipingCarCalled = false;
    }
    private void onEquipCarConnectionErrorMethod()
    {
        Debug.Log("Connection Error");
        equipingCarCalled = false;
    }
    private void onSellCarSuccessMethod(string jsonData)
    {
        if (currentEquipCarId == currentSellCarId)
        {
            equipedCarMessage.text = "";
        }
        AddOrRemoveToOwnedCarListAtStart(carIdToRemove: currentSellCarId);
        AddOrRemoveToAllCarListAtStart(carIdToAdd: currentSellCarId);
        equipingCarCalled = false;
    }
    private void onSellCarFailureMethod(string jsonData)
    {
        Debug.Log(jsonData);
        EquipCarAndBuyCarDownloadType equipCarDownloadType = JsonUtility.FromJson<EquipCarAndBuyCarDownloadType>(jsonData);
        Debug.Log(equipCarDownloadType.code + " " + equipCarDownloadType.message);
        equipingCarCalled = false;
    }
    private void onSellCarConnectionErrorMethod()
    {
        Debug.Log("Connection Error");
        equipingCarCalled = false;
    }
    private void onBuyCarConnectionErrorMethod()
    {
        Debug.Log("Connection Error");
    }
    private void onBuyCarSuccessMethod(string jsonData)
    {
        AddOrRemoveToOwnedCarListAtStart(carIdToAdd: currentBuyCarId);
        AddOrRemoveToAllCarListAtStart(carIdToRemove: currentBuyCarId);
    }
    private void onBuyCarFailureMethod(string jsonData)
    {
        EquipCarAndBuyCarDownloadType equipCarDownloadType = JsonUtility.FromJson<EquipCarAndBuyCarDownloadType>(jsonData);
        Debug.Log(equipCarDownloadType.code + " " + equipCarDownloadType.message);
    }
    private void allCarsListDropdownChange(int index)
    {
        currentBuyCarId = allCarsListDropdown.options[index].text;
    }
    private void ownedCarForEquipingDropdownChange(int index)
    {
        currentEquipCarId = ownedCarForEquipingDropdown.options[index].text;
    }
    private void ownedCarForSellingDropdownChange(int index)
    {
        currentSellCarId = ownedCarForSellingDropdown.options[index].text;
    }
    private IEnumerator ChangePanel(GameObject toSetFalse, GameObject toSetTrue, int time, TextMeshProUGUI textChange = null, string text = "")
    {
        if (textChange != null)
        {
            textChange.gameObject.SetActive(true);
            textChange.text = text;
        }
        yield return new WaitForSeconds(time);
        if (textChange != null) textChange.gameObject.SetActive(false);
        toSetFalse.SetActive(false);
        toSetTrue.SetActive(true);
        toSetTrue.GetComponent<PanelInterface>().ClearTexts();
    }

    public void ClearTexts()
    {
        allCarsListDropdown.ClearOptions();
        ownedCarForEquipingDropdown.ClearOptions();
        ownedCarForSellingDropdown.ClearOptions();
        allCarsList.Clear();
        ownedCarsList.Clear();
        foreach (string carId in carIds)
        {
            AddOrRemoveToAllCarListAtStart(carIdToAdd: carId);
        }
        index = 0;
        equipingCarCalled = false;
        StopCoroutine(GetAllBoughtCars());
        StartCoroutine(GetAllBoughtCars());
    }
    private IEnumerator GetAllBoughtCars()
    {
        while (index < carIds.Count)
        {
            if (!equipingCarCalled)
            {
                equipingCarCalled = true;
                APICalls.EquipCar(carIds[index],acessToken : PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName),onEquipCarSuccessAction,onFailure : onEquipCarFailureAction,onConnectionError : onEquipCarConnectionErrorAction);;
                index++;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
public class EquipCarAndBuyCarDownloadType
{
    public string status;
    public int code;
    public string message;
    public string data;
}