using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyCars : MonoBehaviour
{
    [SerializeField] SellCars sellCars;
    [SerializeField] EquipCars equipCars;
    [SerializeField] CarPanel carPanel;
    [SerializeField] private Button buycar;
    [SerializeField] private TMP_Dropdown allCarsListDropdown;
    private List<TMP_Dropdown.OptionData> allCarsList = new List<TMP_Dropdown.OptionData>();
    private Action<string> onBuyCarSuccessAction;
    private Action<string> onBuyCarFailureAction;
    private Action onBuyCarConnectionErrorAction;
    private string currentBuyCarId = "";
    private void Start()
    {
        onBuyCarSuccessAction += onBuyCarSuccessMethod;
        onBuyCarFailureAction += onBuyCarFailureMethod;
        onBuyCarConnectionErrorAction += onBuyCarConnectionErrorMethod;
        buycar.onClick.AddListener(BuyCar);
        allCarsListDropdown.ClearOptions();
        allCarsListDropdown.onValueChanged.AddListener(allCarsListDropdownChange);
    }
    private void BuyCar()
    {
        APICalls.BuyCar(currentBuyCarId, PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onBuyCarSuccessAction, onBuyCarFailureAction, onBuyCarConnectionErrorAction);
    }
    private void onBuyCarSuccessMethod(string jsonData)
    {
        carPanel.CarBoughtOrSold(carIdBought : currentBuyCarId);
    }
    private void onBuyCarFailureMethod(string jsonData)
    {
        EquipCarAndBuyCarDownloadType equipCarDownloadType = JsonUtility.FromJson<EquipCarAndBuyCarDownloadType>(jsonData);
    }
    private void onBuyCarConnectionErrorMethod()
    {
        Debug.Log("Connection Error");
    }
    private void allCarsListDropdownChange(int index)
    {
        currentBuyCarId = allCarsListDropdown.options[index].text;
    }
    public List<TMP_Dropdown.OptionData> GetAllCarsList()
    {
        return allCarsList;
    }
    public void SetAllCarsList(List<TMP_Dropdown.OptionData> allCarsList)
    {
        this.allCarsList = allCarsList;
        allCarsListDropdown.ClearOptions();
        allCarsListDropdown.AddOptions(allCarsList);
        if (allCarsList.Count > 0)
        {
            buycar.interactable = true;
            currentBuyCarId = allCarsList[0].text;
        }
        else
        {
            buycar.interactable = false;
        }
    }
}
