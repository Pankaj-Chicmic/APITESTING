using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipCars : MonoBehaviour
{
    [SerializeField] private BuyCars buyCars;
    [SerializeField] private SellCars sellCars;
    [SerializeField] private CarPanel carPanel;
    [SerializeField] private Button equipCar;
    [SerializeField] private TextMeshProUGUI equipedCarMessage;
    [SerializeField] private TextMeshProUGUI loadingScreenMessage;
    [SerializeField] private TMP_Dropdown ownedCarForEquipingDropdown;
    private List<TMP_Dropdown.OptionData> ownedCarsList = new List<TMP_Dropdown.OptionData>();
    private string currentEquipCarId = "";
    private string currentlyEquippedCarId = "";
    private bool checkingAlreadyBoughtCar;
    private Action<string> onEquipCarSuccessAction;
    private Action<string> onEquipCarFailureAction;
    private Action onEquipCarConnectionErrorAction;
    private int index = 0;
    private void Start()
    {
        index = 0;
        onEquipCarFailureAction += onEquipCarFailureMethod;
        onEquipCarSuccessAction += onEquipCarSuccessMethod;
        onEquipCarConnectionErrorAction += onEquipCarConnectionErrorMethod;
        StartCoroutine(GetAllBoughtCars());
        ownedCarForEquipingDropdown.ClearOptions();
        equipCar.onClick.AddListener(EquipCar);
        ownedCarForEquipingDropdown.onValueChanged.AddListener(ownedCarForEquipingDropdownChange);
    }
    private void EquipCar()
    {
        APICalls.EquipCar(currentEquipCarId, PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onEquipCarSuccessAction, onEquipCarFailureAction, onEquipCarConnectionErrorAction);
    }
    private void onEquipCarSuccessMethod(string jsonData)
    {
        if (checkingAlreadyBoughtCar)
        {
            checkingAlreadyBoughtCar = false;
            carPanel.CarBoughtOrSold(carIdAlreadyBought : carPanel.allCarIds[index-1]);
        }
        else
        {
            equipedCarMessage.text = "Equipped Car " + currentEquipCarId;
            currentlyEquippedCarId = currentEquipCarId;
        }
    }
    private void onEquipCarFailureMethod(string jsonData)
    {
        EquipCarAndBuyCarDownloadType equipCarDownloadType = JsonUtility.FromJson<EquipCarAndBuyCarDownloadType>(jsonData);
        if (checkingAlreadyBoughtCar)
        {
            checkingAlreadyBoughtCar = false;
            if(equipCarDownloadType.message== "Record not fond")
            {
                carPanel.CarBoughtOrSold(carIdNotAlreadyBought : carPanel.allCarIds[index-1]);
            }
        }
        else
        {
            Debug.Log("Equiping Car " + jsonData);
        }
    }
    private void onEquipCarConnectionErrorMethod()
    {
        Debug.Log("Connection Error");
        if (checkingAlreadyBoughtCar)
        {
            checkingAlreadyBoughtCar = false;
        }
    }
    private void ownedCarForEquipingDropdownChange(int index)
    {
        currentEquipCarId = ownedCarForEquipingDropdown.options[index].text;
    }
    private IEnumerator GetAllBoughtCars()
    {
        loadingScreenMessage.gameObject.SetActive(true);
        while (index < carPanel.allCarIds.Count)
        {
            if (!checkingAlreadyBoughtCar)
            {
                checkingAlreadyBoughtCar = true;
                APICalls.EquipCar(carPanel.allCarIds[index], acessToken: PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onEquipCarSuccessAction, onFailure: onEquipCarFailureAction, onConnectionError: onEquipCarConnectionErrorAction); ;
                index++;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        loadingScreenMessage.gameObject.SetActive(false);
    }
    public List<TMP_Dropdown.OptionData> GetOwnedCarsList()
    {
        return ownedCarsList;
    }
    public void SetOwnedCarsList(List<TMP_Dropdown.OptionData> ownedCarsList)
    {
        this.ownedCarsList = ownedCarsList;
        ownedCarForEquipingDropdown.ClearOptions();
        ownedCarForEquipingDropdown.AddOptions(ownedCarsList);
        if (ownedCarsList.Count > 0)
        {
            currentEquipCarId = ownedCarsList[0].text;
            equipCar.interactable = true;
        }
        else
        {
            equipCar.interactable = false;
        }
    }
    public void ResetCurrentlyEquipedCar()
    {
        equipedCarMessage.text = "";
    }
    public string GetCurrentlyEquippedCar()
    {
        return currentlyEquippedCarId;
    }
}
