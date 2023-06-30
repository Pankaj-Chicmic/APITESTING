using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellCars : MonoBehaviour
{
    // Serialized fields allow private variables to be exposed in the Inspector
    [SerializeField] private CarPanel carPanel; // Reference to the CarPanel script
    [SerializeField] private Button sellCar; // Reference to the sell car Button
    [SerializeField] private EquipCars equipCar; // Reference to the EquipCars script
    [SerializeField] private TMP_Dropdown ownedCarForSellingDropdown; // Reference to the dropdown for owned cars

    private List<TMP_Dropdown.OptionData> ownedCarsList = new List<TMP_Dropdown.OptionData>(); // List to store the options for the dropdown
    private Action<string> onSellCarSuccessAction; // Action to handle sell car success
    private Action<string> onSellCarFailureAction; // Action to handle sell car failure
    private Action onSellCarConnectionErrorAction; // Action to handle sell car connection error
    private string currentSellCarId = ""; // Current car ID selected for selling

    private void Start()
    {
        // Initialize the sell car actions
        onSellCarSuccessAction += onSellCarSuccessMethod;
        onSellCarFailureAction += onSellCarFailureMethod;
        onSellCarConnectionErrorAction += onSellCarConnectionErrorMethod;

        // Add listeners to the sell car button and owned car dropdown
        sellCar.onClick.AddListener(SellCar);
        ownedCarForSellingDropdown.onValueChanged.AddListener(ownedCarForSellingDropdownChange);

        // Clear options from the owned car dropdown
        ownedCarForSellingDropdown.ClearOptions();
    }

    private void SellCar()
    {
        // Call the SellCar method from APICalls, passing the current car ID and access token
        APICalls.SellCar(currentSellCarId, PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onSellCarSuccessAction, onSellCarFailureAction, onSellCarConnectionErrorAction);
    }

    private void onSellCarSuccessMethod(string jsonData)
    {
        // Check if the sold car was currently equipped
        if (currentSellCarId == equipCar.GetCurrentlyEquippedCar())
        {
            equipCar.ResetCurrentlyEquipedCar();
        }

        // Inform the car panel that a car was sold
        carPanel.CarBoughtOrSold(carIdSold: currentSellCarId);
    }

    private void onSellCarFailureMethod(string jsonData)
    {
        // Deserialize the JSON response to get the error details
        EquipCarAndBuyCarDownloadType equipCarDownloadType = JsonUtility.FromJson<EquipCarAndBuyCarDownloadType>(jsonData);

        // Log the error details
        Debug.Log(equipCarDownloadType.code + " " + equipCarDownloadType.message);
    }

    private void onSellCarConnectionErrorMethod()
    {
        // Handle connection error
        Debug.Log("Connection Error");
    }

    private void ownedCarForSellingDropdownChange(int index)
    {
        // Update the currentSellCarId based on the selected dropdown option
        currentSellCarId = ownedCarForSellingDropdown.options[index].text;
    }

    public List<TMP_Dropdown.OptionData> GetOwnedCarsList()
    {
        return ownedCarsList;
    }

    public void SetOwnedCarsList(List<TMP_Dropdown.OptionData> ownedCarsList)
    {
        // Set the owned car list and update the dropdown options
        this.ownedCarsList = ownedCarsList;
        ownedCarForSellingDropdown.ClearOptions();
        ownedCarForSellingDropdown.AddOptions(ownedCarsList);

        if (ownedCarsList.Count > 0)
        {
            // If there are owned cars, set the currentSellCarId to the first car and enable the sell car button
            currentSellCarId = ownedCarsList[0].text;
            sellCar.interactable = true;
        }
        else
        {
            // If there are no owned cars, disable the sell car button
            sellCar.interactable = false;
        }
    }
}
