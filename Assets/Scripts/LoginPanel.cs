using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class LoginPanel : MonoBehaviour, PanelInterface
{
    [SerializeField] private UI ui;
    [SerializeField] private GameObject logInPanel;
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private GameObject guestLoginPanel;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button goToGuestLoginPanelButton;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TextMeshProUGUI loginMessageText;
    [SerializeField] private TextMeshProUGUI emailIdMessage;

    private Action<string> onLoginSuccessAction;
    private Action<string> onLoginFailureAction;
    private Action onLoginConnectionErrorAction;

    void Start()
    {
        // Assign event handlers
        onLoginSuccessAction += OnLoginSuccessMethod;
        onLoginFailureAction += OnLoginFailureMethod;
        onLoginConnectionErrorAction += OnLoginConnectionErrorMethod;

        // Assign button click listeners
        loginButton.onClick.AddListener(Login);
        goToGuestLoginPanelButton.onClick.AddListener(GoToGuestLoginPanel);
    }

    private void Login()
    {
        if (emailInputField.text == "") return;

        // Call the Login API
        APICalls.Login(emailInputField.text, onSuccess: onLoginSuccessAction, onFailure: onLoginFailureAction, onConnectionError: onLoginConnectionErrorAction);
    }

    private void GoToGuestLoginPanel()
    {
        // Change the panel to the guest login panel
        StartCoroutine(ui.ChangePanel(logInPanel, guestLoginPanel, 0));
    }

    private void OnLoginSuccessMethod(string jsonData)
    {
        // Parse the response data
        GuestLoginDownloadHandlerType guestLoginDownloadHandlerType = JsonUtility.FromJson<GuestLoginDownloadHandlerType>(jsonData);

        // Store the access token in Player Preferences
        PlayerPrefs.SetString(AllConstants.playerPrefAccessTokenVariableName, guestLoginDownloadHandlerType.data);

        // Change the panel to the logged in panel with a success message
        StartCoroutine(ui.ChangePanel(logInPanel, loggedInPanel, 2, loginMessageText, "Successfully logged in as "));

        // Set the logged in details in the logged in panel
        loggedInPanel.GetComponent<LoggedInPanel>().SetLoggedInDetails();
    }

    private void OnLoginFailureMethod(string jsonData)
    {
        // Parse the response data
        GuestLoginDownloadHandlerType guestLoginDownloadHandlerType = JsonUtility.FromJson<GuestLoginDownloadHandlerType>(jsonData);

        // Display appropriate error messages based on the response
        if (guestLoginDownloadHandlerType != null && guestLoginDownloadHandlerType.message != null && guestLoginDownloadHandlerType.message == AllConstants.loginRecordNotFoundMessage)
        {
            loginMessageText.text = "Record Not Available";
        }
        else
        {
            loginMessageText.text = "Check Network Connection and try again";
        }
    }

    private void OnLoginConnectionErrorMethod()
    {
        // Display connection error message
        loginMessageText.gameObject.SetActive(true);
        loginMessageText.text = "Connection Error, Check Internet Connection";
    }

    private bool ValidateEmail(string email)
    {
        // Regular expression pattern for email validation
        string pattern = "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$";

        // Check if the email matches the pattern
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    public void ChangeEmailFieldStatus()
    {
        if (!ValidateEmail(emailInputField.text) && emailInputField.text != "")
        {
            // Set the email text color to red and display error message
            emailInputField.textComponent.color = Color.red;
            emailIdMessage.gameObject.SetActive(true);
        }
        else
        {
            // Set the email text color to black and hide error message
            emailInputField.textComponent.color = Color.black;
            emailIdMessage.gameObject.SetActive(false);
        }
    }

    public void ClearTexts()
    {
        // Clear the input field and login message
        emailInputField.text = "";
        loginMessageText.text = "";
    }
}
