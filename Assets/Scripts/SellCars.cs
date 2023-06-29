using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellCars : MonoBehaviour
{
    [SerializeField] private CarPanel carPanel;
    [SerializeField] private Button sellCar;
    [SerializeField] private EquipCars equipCar;
    [SerializeField] private TMP_Dropdown ownedCarForSellingDropdown;
    private List<TMP_Dropdown.OptionData> ownedCarsList = new List<TMP_Dropdown.OptionData>();
    private Action<string> onSellCarSuccessAction;
    private Action<string> onSellCarFailureAction;
    private Action onSellCarConnectionErrorAction;
    private string currentSellCarId = "";
    private void Start()
    {
        onSellCarSuccessAction += onSellCarSuccessMethod;
        onSellCarFailureAction += onSellCarFailureMethod;
        onSellCarConnectionErrorAction += onSellCarConnectionErrorMethod;
        sellCar.onClick.AddListener(SellCar);
        ownedCarForSellingDropdown.onValueChanged.AddListener(ownedCarForSellingDropdownChange);
        ownedCarForSellingDropdown.ClearOptions();
    }
    private void SellCar()
    {
        APICalls.SellCar(currentSellCarId, PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onSellCarSuccessAction, onSellCarFailureAction, onSellCarConnectionErrorAction);
    }
    private void onSellCarSuccessMethod(string jsonData)
    {
        if (currentSellCarId == equipCar.GetCurrentlyEquippedCar())
        {
            equipCar.ResetCurrentlyEquipedCar();
        }
        carPanel.CarBoughtOrSold(carIdSold: currentSellCarId);
    }
    private void onSellCarFailureMethod(string jsonData)
    {
        EquipCarAndBuyCarDownloadType equipCarDownloadType = JsonUtility.FromJson<EquipCarAndBuyCarDownloadType>(jsonData);
        Debug.Log(equipCarDownloadType.code + " " + equipCarDownloadType.message);
    }
    private void onSellCarConnectionErrorMethod()
    {
        Debug.Log("Connection Error");
    }
    private void ownedCarForSellingDropdownChange(int index)
    {
        currentSellCarId = ownedCarForSellingDropdown.options[index].text;
    }
    public List<TMP_Dropdown.OptionData> GetOwnedCarsList()
    {
        return ownedCarsList;
    }
    public void SetOwnedCarsList(List<TMP_Dropdown.OptionData> ownedCarsList)
    {
        this.ownedCarsList = ownedCarsList;
        ownedCarForSellingDropdown.ClearOptions();
        ownedCarForSellingDropdown.AddOptions(ownedCarsList);
        if (ownedCarsList.Count > 0)
        {
            currentSellCarId = ownedCarsList[0].text;
            sellCar.interactable = true;
        }
        else
        {
            sellCar.interactable = false;
        }
    }
}
