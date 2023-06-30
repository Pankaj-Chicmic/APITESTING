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
    private Action<string> onEquipCarSuccessAction;
    private Action<string> onEquipCarFailureAction;
    private Action onEquipCarConnectionErrorAction;
    private string currentEquipCarId = "";
    private string currentlyEquippedCarId = "";
    private bool checkingAlreadyBoughtCar;
    private int index = 0;

    private void Start()
    {
        // Assign event handlers to action delegates
        onEquipCarFailureAction += onEquipCarFailureMethod;
        onEquipCarSuccessAction += onEquipCarSuccessMethod;
        onEquipCarConnectionErrorAction += onEquipCarConnectionErrorMethod;

        // Retrieve all bought cars
        StartCoroutine(GetAllBoughtCars());

        // Clear dropdown options and assign listeners
        ownedCarForEquipingDropdown.ClearOptions();
        equipCar.onClick.AddListener(EquipCar);
        ownedCarForEquipingDropdown.onValueChanged.AddListener(ownedCarForEquipingDropdownChange);
    }

    private void EquipCar()
    {
        // Call the API to equip the selected car
        APICalls.EquipCar(currentEquipCarId, PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onEquipCarSuccessAction, onEquipCarFailureAction, onEquipCarConnectionErrorAction);
    }

    private void onEquipCarSuccessMethod(string jsonData)
    {
        if (checkingAlreadyBoughtCar)
        {
            // The car was already bought, notify the car panel
            checkingAlreadyBoughtCar = false;
            carPanel.CarBoughtOrSold(carIdAlreadyBought: carPanel.allCarIds[index - 1]);
        }
        else
        {
            // The car was equipped successfully, update the message and currently equipped car ID
            equipedCarMessage.text = "Equipped Car " + currentEquipCarId;
            currentlyEquippedCarId = currentEquipCarId;
        }
    }

    private void onEquipCarFailureMethod(string jsonData)
    {
        // Handle the failure case
        EquipCarAndBuyCarDownloadType equipCarDownloadType = JsonUtility.FromJson<EquipCarAndBuyCarDownloadType>(jsonData);
        if (checkingAlreadyBoughtCar)
        {
            // The car was not already bought, notify the car panel
            checkingAlreadyBoughtCar = false;
            if (equipCarDownloadType.message == "Record not fond")
            {
                carPanel.CarBoughtOrSold(carIdNotAlreadyBought: carPanel.allCarIds[index - 1]);
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
        // Update the currently selected car for equipping
        currentEquipCarId = ownedCarForEquipingDropdown.options[index].text;
    }

    public IEnumerator GetAllBoughtCars()
    {
        index = 0;
        loadingScreenMessage.gameObject.SetActive(true);

        while (index < carPanel.allCarIds.Count)
        {
            if (!checkingAlreadyBoughtCar)
            {
                // Check if the car is already bought
                equipedCarMessage.text = "Checking For Ownership Of Car : " + carPanel.allCarIds[index];
                checkingAlreadyBoughtCar = true;
                APICalls.EquipCar(carPanel.allCarIds[index], accessToken: PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onSuccess: onEquipCarSuccessAction, onFailure: onEquipCarFailureAction, onConnectionError: onEquipCarConnectionErrorAction);
                index++;
            }
            yield return new WaitForSeconds(0.25f);
        }

        // Reset messages and loading screen
        equipedCarMessage.text = "";
        loadingScreenMessage.gameObject.SetActive(false);
    }

    public List<TMP_Dropdown.OptionData> GetOwnedCarsList()
    {
        return ownedCarsList;
    }

    public void SetOwnedCarsList(List<TMP_Dropdown.OptionData> ownedCarsList)
    {
        // Set the owned cars list and update the dropdown options
        this.ownedCarsList = ownedCarsList;
        ownedCarForEquipingDropdown.ClearOptions();
        ownedCarForEquipingDropdown.AddOptions(ownedCarsList);

        if (ownedCarsList.Count > 0)
        {
            // Set the initial selected car for equipping
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
        // Reset the equipped car message
        equipedCarMessage.text = "";
    }

    public string GetCurrentlyEquippedCar()
    {
        // Get the currently equipped car ID
        return currentlyEquippedCarId;
    }
}
