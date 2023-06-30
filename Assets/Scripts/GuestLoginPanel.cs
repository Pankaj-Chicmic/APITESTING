using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class GuestLoginPanel : MonoBehaviour, PanelInterface
{
    // Serialized fields are exposed in the Unity Editor for easy configuration
    [SerializeField] private UI ui;
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private GameObject guestLoginPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button guestLoginButton;
    [SerializeField] private Button goToLoginPanelButton;
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TextMeshProUGUI logInResultText;
    [SerializeField] private TextMeshProUGUI userNameMessage;

    // Action delegates for handling guest login events
    private Action<string> onGuestLoginSuccessAction;
    private Action<string> onGuestLoginFailureAction;
    private Action onGuestLoginConnectionErrorAction;

    void Start()
    {
        // Assign button click listeners
        guestLoginButton.onClick.AddListener(GuestLogin);
        goToLoginPanelButton.onClick.AddListener(GoToLoginPanel);

        // Assign event handlers to action delegates
        onGuestLoginSuccessAction += onGuestLoginSuccessMethod;
        onGuestLoginFailureAction += onGuestLoginFailureMethod;
        onGuestLoginConnectionErrorAction += onGuestLoginConnectionErrorMethod;
    }

    private void GoToLoginPanel()
    {
        // Transition to the login panel
        StartCoroutine(ui.ChangePanel(guestLoginPanel, loginPanel, 0));
    }

    private void GuestLogin()
    {
        // Check if the username input field is empty
        if (userNameInputField.text == "")
        {
            return;
        }

        // Validate the username
        if (!ValidateUsername(userNameInputField.text))
        {
            logInResultText.gameObject.SetActive(true);
            logInResultText.text = "Invalid Username. Only alphabets, numbers, and underscore (_) are allowed.";
            return;
        }

        // Call the API for guest login
        APICalls.GuestLogin(userNameInputField.text, onSuccess: onGuestLoginSuccessAction, onFailure: onGuestLoginFailureAction, onConnectionError: onGuestLoginConnectionErrorAction);
    }

    private void onGuestLoginSuccessMethod(string jsonData)
    {
        // Parse the JSON response into a GuestLoginDownloadHandlerType object
        GuestLoginDownloadHandlerType guestLoginDownloadHandlerType = JsonUtility.FromJson<GuestLoginDownloadHandlerType>(jsonData);

        // Set the access token and username in player preferences
        PlayerPrefs.SetString(AllConstants.playerPrefAccessTokenVariableName, guestLoginDownloadHandlerType.data);
        PlayerPrefs.SetString(AllConstants.playerPrefUserNameVariableName, userNameInputField.text);

        // Transition to the logged in panel and show a success message
        StartCoroutine(ui.ChangePanel(guestLoginPanel, loggedInPanel, 2, logInResultText, "Successfully Logged in as " + userNameInputField.text));

        // Set the logged in details in the logged in panel
        loggedInPanel.GetComponent<LoggedInPanel>().SetLoggedInDetails();
    }

    private void onGuestLoginFailureMethod(string jsonData)
    {
        // Parse the JSON response into a GuestLoginDownloadHandlerType object
        GuestLoginDownloadHandlerType guestLoginDownloadHandlerType = JsonUtility.FromJson<GuestLoginDownloadHandlerType>(jsonData);

        // Show an appropriate failure message based on the response
        logInResultText.gameObject.SetActive(true);
        if (guestLoginDownloadHandlerType != null && guestLoginDownloadHandlerType.message != null && guestLoginDownloadHandlerType.message == AllConstants.guestLoginDuplicateUserNameMessage)
        {
            logInResultText.text = "Username Not Available";
        }
        else
        {
            logInResultText.text = "Check Network Connection and try again";
        }
    }

    private void onGuestLoginConnectionErrorMethod()
    {
        // Show a connection error message
        logInResultText.gameObject.SetActive(true);
        logInResultText.text = "Connection Error. Check Internet Connection";
    }

    private bool ValidateUsername(string username)
    {
        // Regular expression pattern to validate the username format
        string pattern = @"^[A-Za-z0-9\-_]+$";

        // Check if the username matches the pattern
        return Regex.IsMatch(username, pattern);
    }

    public void ChangeUserNameFieldStatus()
    {
        // Change the color and visibility of the username input field based on username validation
        if (!ValidateUsername(userNameInputField.text) && userNameInputField.text != "")
        {
            userNameInputField.textComponent.color = Color.red;
            userNameMessage.gameObject.SetActive(true);
        }
        else
        {
            userNameInputField.textComponent.color = Color.black;
            userNameMessage.gameObject.SetActive(false);
        }
    }

    public void ClearTexts()
    {
        // Clear the text of the username input field and login result text
        userNameInputField.text = "";
        logInResultText.text = "";
    }
}

// Class representing the response structure for guest login API
public class GuestLoginDownloadHandlerType
{
    public string status;
    public int code;
    public string message;
    public string data;
}
