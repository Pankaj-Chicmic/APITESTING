using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class CarPanel : MonoBehaviour, PanelInterface
{
    [SerializeField] private UI ui;
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private Button goToLoggedInPanel;
    [SerializeField] private BuyCars buyCars;
    [SerializeField] private SellCars sellCars;
    [SerializeField] private EquipCars equipCars;
    [SerializeField] public List<string> allCarIds;
    public bool loggedOut = false;

    private void Start()
    {   
        goToLoggedInPanel.onClick.AddListener(GoToLoggedInPanel);
    }

    public void CarBoughtOrSold(string carIdBought = "", string carIdSold = "", string carIdAlreadyBought = "", string carIdNotAlreadyBought = "")
    {
        List<TMP_Dropdown.OptionData> allCarsList;
        List<TMP_Dropdown.OptionData> ownedCarList;

        if (carIdBought != "")
        {
            // Car was bought, update the lists accordingly
            allCarsList = buyCars.GetAllCarsList();

            // Remove the bought car from the allCarsList
            foreach (var carId in allCarsList)
            {
                if (carId.text == carIdBought)
                {
                    allCarsList.Remove(carId);
                    break;
                }
            }
            buyCars.SetAllCarsList(allCarsList);

            // Add the bought car to the ownedCarList of sellCars and equipCars
            ownedCarList = equipCars.GetOwnedCarsList();
            ownedCarList.Add(new TMP_Dropdown.OptionData(carIdBought));
            sellCars.SetOwnedCarsList(ownedCarList);
            equipCars.SetOwnedCarsList(ownedCarList);
        }

        if (carIdSold != "")
        {
            // Car was sold, update the lists accordingly
            allCarsList = buyCars.GetAllCarsList();
            allCarsList.Add(new TMP_Dropdown.OptionData(carIdSold));
            buyCars.SetAllCarsList(allCarsList);

            ownedCarList = equipCars.GetOwnedCarsList();

            // Remove the sold car from the ownedCarList of sellCars and equipCars
            foreach (var carId in ownedCarList)
            {
                if (carId.text == carIdSold)
                {
                    ownedCarList.Remove(carId);
                    break;
                }
            }
            sellCars.SetOwnedCarsList(ownedCarList);
            equipCars.SetOwnedCarsList(ownedCarList);
        }

        if (carIdNotAlreadyBought != "")
        {
            // Car was not already bought, add it to the allCarsList of buyCars
            allCarsList = buyCars.GetAllCarsList();
            allCarsList.Add(new TMP_Dropdown.OptionData(carIdNotAlreadyBought));
            buyCars.SetAllCarsList(allCarsList);
        }

        if (carIdAlreadyBought != "")
        {
            // Car was already bought, add it to the ownedCarList of sellCars and equipCars
            ownedCarList = equipCars.GetOwnedCarsList();
            ownedCarList.Add(new TMP_Dropdown.OptionData(carIdAlreadyBought));
            sellCars.SetOwnedCarsList(ownedCarList);
            equipCars.SetOwnedCarsList(ownedCarList);
        }
    }

    private void GoToLoggedInPanel()
    {
        StartCoroutine(ui.ChangePanel(this.gameObject, loggedInPanel, 0));
    }

    public void ClearTexts()
    {
        if (loggedOut)
        {
            // Clear texts and retrieve all bought cars
            StartCoroutine(equipCars.GetAllBoughtCars());
        }
        loggedOut = false;
    }

    public void ResetBoughtCarInfo()
    {
        // Reset the bought car information in buyCars, sellCars, and equipCars
        equipCars.SetOwnedCarsList(new List<TMP_Dropdown.OptionData>());
        buyCars.SetAllCarsList(new List<TMP_Dropdown.OptionData>());
        sellCars.SetOwnedCarsList(new List<TMP_Dropdown.OptionData>());
    }
}

public class EquipCarAndBuyCarDownloadType
{
    public string status;
    public int code;
    public string message;
    public string data;
}
