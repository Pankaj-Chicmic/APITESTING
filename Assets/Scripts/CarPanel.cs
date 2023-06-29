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
    private void Start()
    {
        goToLoggedInPanel.onClick.AddListener(GoToLoggedInPanel);
    }
    public void CarBoughtOrSold(string carIdBought ="",string carIdSold = "",string carIdAlreadyBought="",string carIdNotAlreadyBought="")
    {
        List<TMP_Dropdown.OptionData> allCarsList;
        List<TMP_Dropdown.OptionData> ownedCarList;
        if (carIdBought != "")
        {
            allCarsList = buyCars.GetAllCarsList();
            foreach(var carId in allCarsList)
            {
                if (carId.text == carIdBought)
                {
                    allCarsList.Remove(carId);
                    break;
                }
            }
            buyCars.SetAllCarsList(allCarsList);
            ownedCarList = equipCars.GetOwnedCarsList();
            ownedCarList.Add(new TMP_Dropdown.OptionData(carIdBought));
            sellCars.SetOwnedCarsList(ownedCarList);
            equipCars.SetOwnedCarsList(ownedCarList);
        }
        if (carIdSold != "")
        {
            allCarsList = buyCars.GetAllCarsList();
            allCarsList.Add(new TMP_Dropdown.OptionData(carIdSold));
            buyCars.SetAllCarsList(allCarsList);
            ownedCarList = equipCars.GetOwnedCarsList();
            foreach(var carId in ownedCarList)
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
            allCarsList = buyCars.GetAllCarsList();
            allCarsList.Add(new TMP_Dropdown.OptionData(carIdNotAlreadyBought));
            buyCars.SetAllCarsList(allCarsList);
        }
        if (carIdAlreadyBought != "")
        {
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
        
    }
}
public class EquipCarAndBuyCarDownloadType
{
    public string status;
    public int code;
    public string message;
    public string data;
}