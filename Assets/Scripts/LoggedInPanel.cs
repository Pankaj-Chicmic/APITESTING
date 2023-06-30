using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class LoggedInPanel : MonoBehaviour,PanelInterface
{
    [SerializeField] private UI ui;
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private GameObject loginInPanel;
    [SerializeField] private GameObject carPanel;
    [SerializeField] private EquipCars equipCars;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private Button emailUpdateButton;
    [SerializeField] private Button logOutButton;
    [SerializeField] private Button getBuyCarButton;
    [SerializeField] private TextMeshProUGUI emailUpdateMessage;
    [SerializeField] private TextMeshProUGUI loggedInDetails;
    [SerializeField] private TextMeshProUGUI emailIdMessage;
    private Action<string> onEmailUpdateSuccessAction;
    private Action<string> onEmailUpdateFailureAction;
    private Action onEmailUpdateConnectionErrorAction;
    public bool clickedCarPanel = false;
    private void Start()
    {
        onEmailUpdateSuccessAction += onEmailUpdateSuccessMethod;
        onEmailUpdateFailureAction += onEmailUpdateFailureMethod;
        onEmailUpdateConnectionErrorAction += onEmailUpdateConnectionErrorMethod;
        emailUpdateButton.onClick.AddListener(EmailUpdate);
        logOutButton.onClick.AddListener(LogOut);
        getBuyCarButton.onClick.AddListener(GetBuyCarPanel);
    }
    private void EmailUpdate()
    {
        if (emailInputField.text == "")
        {
            return;
        }
        if (!ValidateEmail(emailInputField.text))
        {
            emailUpdateMessage.text = "Wrong Email";
            emailUpdateMessage.gameObject.SetActive(true);
            return;
        }
        APICalls.EmailUpdate(emailInputField.text, PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onSuccess: onEmailUpdateSuccessAction, onFailure: onEmailUpdateFailureAction, onConnectionError: onEmailUpdateConnectionErrorAction);
    }
    private void GetBuyCarPanel()
    {
        clickedCarPanel = true;
        StartCoroutine(ui.ChangePanel(loggedInPanel, carPanel,0));
    }
    private void LogOut()
    {
        PlayerPrefs.SetString(AllConstants.playerPrefAccessTokenVariableName, "");
        PlayerPrefs.SetString(AllConstants.playerPrefUserNameVariableName, "");
        PlayerPrefs.SetString(AllConstants.playerPrefEmailVariableName, "");
        loginInPanel.SetActive(true);
        carPanel.GetComponent<CarPanel>().ResetBoughtCarInfo();
        StartCoroutine(ui.ChangePanel(loggedInPanel, loginInPanel, 0));
        carPanel.GetComponent<CarPanel>().loggedOut = true && clickedCarPanel;
    }
    private void onEmailUpdateSuccessMethod(string jsonData)
    {
        EmailUpdateType emailUpdateType = JsonUtility.FromJson<EmailUpdateType>(jsonData);
        PlayerPrefs.SetString(AllConstants.playerPrefEmailVariableName, emailInputField.text);
        emailUpdateMessage.gameObject.SetActive(true);
        emailUpdateMessage.text = "Email Updated Successfully";
    }
    private void onEmailUpdateFailureMethod(string jsonData)
    {
        EmailUpdateType emailUpdateType = JsonUtility.FromJson<EmailUpdateType>(jsonData);
        emailUpdateMessage.gameObject.SetActive(true);
        if (emailUpdateType != null && emailUpdateType.message != null && emailUpdateType.message == AllConstants.emailDuplicateMessage)
        {
            emailUpdateMessage.text = "Email ALready in Used";
        }
        else emailUpdateMessage.text = "Email Update Failed " + emailUpdateType?.message;
    }
    private void onEmailUpdateConnectionErrorMethod()
    {
        emailUpdateMessage.gameObject.SetActive(true);
        emailUpdateMessage.text = "Connection Error";
    }
    private bool ValidateEmail(string email)
    {
        string pattern = "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
    public void ChangeEmailFieldStatus()
    {
        if (!ValidateEmail(emailInputField.text) && emailInputField.text!="")
        {
            emailInputField.textComponent.color = Color.red;
            emailIdMessage.gameObject.SetActive(true);
        }
        else
        {
            emailInputField.textComponent.color = Color.black;
            emailIdMessage.gameObject.SetActive(false);
        }
    }
    public void SetLoggedInDetails()
    {
        loggedInDetails.text = "Logged in as " + PlayerPrefs.GetString(AllConstants.playerPrefUserNameVariableName);
    }
    public void ClearTexts()
    {
        emailUpdateMessage.text = "";
        emailInputField.text = "";
        emailIdMessage.gameObject.SetActive(false);
    }
}
public class EmailUpdateType
{
    public string status;
    public int code;
    public string message;
    public string data;
}