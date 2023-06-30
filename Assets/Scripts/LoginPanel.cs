using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class LoginPanel : MonoBehaviour,PanelInterface
{
    [SerializeField] private UI ui;
    [SerializeField]private GameObject logInPanel;
    [SerializeField]private GameObject loggedInPanel;
    [SerializeField]private GameObject guestLoginPanel;
    [SerializeField]private Button loginButton;
    [SerializeField]private Button goToGuestLoginPanelButton;
    [SerializeField]private TMP_InputField emailInputField;
    [SerializeField]private TextMeshProUGUI loginMessageText;
    [SerializeField]private TextMeshProUGUI emailIdMessage;
    private Action<string> onLoginSuccessAction;
    private Action<string> onLoginFailureAction;
    private Action onLoginConnectionErrorAction;
    void Start()
    {
        onLoginSuccessAction += OnLoginSuccessMethod;
        onLoginFailureAction += OnLoginFailureMethod;
        onLoginConnectionErrorAction += OnLoginConnectionErrorMethod;
        loginButton.onClick.AddListener(Login);
        goToGuestLoginPanelButton.onClick.AddListener(GoToGuestLoginPanel);
    }
    private void Login()
    {
        if (emailInputField.text == "") return;
        APICalls.Login(emailInputField.text, onSuccess: onLoginSuccessAction, onFailure: onLoginFailureAction, onConnectionError: onLoginConnectionErrorAction);
    }
    private void GoToGuestLoginPanel()
    {
        StartCoroutine(ui.ChangePanel(logInPanel, guestLoginPanel,0));
    }
    private void OnLoginSuccessMethod(string jsonData)
    {
        GuestLoginDownloadHandlerType guestLoginDownloadHandlerType = JsonUtility.FromJson<GuestLoginDownloadHandlerType>(jsonData);
        PlayerPrefs.SetString(AllConstants.playerPrefAccessTokenVariableName, guestLoginDownloadHandlerType.data);
        StartCoroutine(ui.ChangePanel(logInPanel, loggedInPanel, 2, loginMessageText, "SuccesFully logged In as "));
        loggedInPanel.GetComponent<LoggedInPanel>().SetLoggedInDetails();
    }
    private void OnLoginFailureMethod(string jsonData)
    {
        GuestLoginDownloadHandlerType guestLoginDownloadHandlerType = JsonUtility.FromJson<GuestLoginDownloadHandlerType>(jsonData);
        if (guestLoginDownloadHandlerType != null && guestLoginDownloadHandlerType.message != null && guestLoginDownloadHandlerType.message == AllConstants.loginRecordNotFoundMessage)
        {
            loginMessageText.text = "Record Not Available";
        }
        else loginMessageText.text = "Check Network Connection and try again";
    }
    private void OnLoginConnectionErrorMethod()
    {
        loginMessageText.gameObject.SetActive(true);
        loginMessageText.text = "Connection Error, Check Internet Connection";
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
    public void ClearTexts()
    {
        emailInputField.text = "";
        loginMessageText.text = "";
    }
}
