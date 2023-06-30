using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class LoggedInPanel : MonoBehaviour, PanelInterface
{
    // Serialized fields are exposed in the Unity Editor for easy configuration
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

    // Action delegates for handling email update events
    private Action<string> onEmailUpdateSuccessAction;
    private Action<string> onEmailUpdateFailureAction;
    private Action onEmailUpdateConnectionErrorAction;

    public bool clickedCarPanel = false;

    private void Start()
    {
        // Assign event handlers to action delegates
        onEmailUpdateSuccessAction += onEmailUpdateSuccessMethod;
        onEmailUpdateFailureAction += onEmailUpdateFailureMethod;
        onEmailUpdateConnectionErrorAction += onEmailUpdateConnectionErrorMethod;

        // Assign button click listeners
        emailUpdateButton.onClick.AddListener(EmailUpdate);
        logOutButton.onClick.AddListener(LogOut);
        getBuyCarButton.onClick.AddListener(GetBuyCarPanel);
    }

    private void EmailUpdate()
    {
        // Check if the email input field is empty
        if (emailInputField.text == "")
        {
            return;
        }

        // Validate the email using a regular expression
        if (!ValidateEmail(emailInputField.text))
        {
            emailUpdateMessage.text = "Wrong Email";
            emailUpdateMessage.gameObject.SetActive(true);
            return;
        }

        // Call the API to update the email
        APICalls.EmailUpdate(emailInputField.text, PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName), onSuccess: onEmailUpdateSuccessAction, onFailure: onEmailUpdateFailureAction, onConnectionError: onEmailUpdateConnectionErrorAction);
    }

    private void GetBuyCarPanel()
    {
        clickedCarPanel = true;
        StartCoroutine(ui.ChangePanel(loggedInPanel, carPanel, 0));
    }

    private void LogOut()
    {
        // Clear player preferences for access token, username, and email
        PlayerPrefs.SetString(AllConstants.playerPrefAccessTokenVariableName, "");
        PlayerPrefs.SetString(AllConstants.playerPrefUserNameVariableName, "");
        PlayerPrefs.SetString(AllConstants.playerPrefEmailVariableName, "");

        // Activate the login panel and reset bought car information
        loginInPanel.SetActive(true);
        carPanel.GetComponent<CarPanel>().ResetBoughtCarInfo();

        // Transition to the login panel
        StartCoroutine(ui.ChangePanel(loggedInPanel, loginInPanel, 0));

        // Set the loggedOut flag to true if the car panel was previously clicked
        carPanel.GetComponent<CarPanel>().loggedOut = true && clickedCarPanel;
    }

    private void onEmailUpdateSuccessMethod(string jsonData)
    {
        // Parse the JSON response into an EmailUpdateType object
        EmailUpdateType emailUpdateType = JsonUtility.FromJson<EmailUpdateType>(jsonData);

        // Update the player preference for email
        PlayerPrefs.SetString(AllConstants.playerPrefEmailVariableName, emailInputField.text);

        // Show a success message
        emailUpdateMessage.gameObject.SetActive(true);
        emailUpdateMessage.text = "Email Updated Successfully";
    }

    private void onEmailUpdateFailureMethod(string jsonData)
    {
        // Parse the JSON response into an EmailUpdateType object
        EmailUpdateType emailUpdateType = JsonUtility.FromJson<EmailUpdateType>(jsonData);

        // Show an appropriate failure message based on the response
        emailUpdateMessage.gameObject.SetActive(true);
        if (emailUpdateType != null && emailUpdateType.message != null && emailUpdateType.message == AllConstants.emailDuplicateMessage)
        {
            emailUpdateMessage.text = "Email Already in Use";
        }
        else
        {
            emailUpdateMessage.text = "Email Update Failed " + emailUpdateType?.message;
        }
    }

    private void onEmailUpdateConnectionErrorMethod()
    {
        // Show a connection error message
        emailUpdateMessage.gameObject.SetActive(true);
        emailUpdateMessage.text = "Connection Error";
    }

    private bool ValidateEmail(string email)
    {
        // Regular expression pattern to validate email format
        string pattern = "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$";

        // Check if the email matches the pattern (case insensitive)
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    public void ChangeEmailFieldStatus()
    {
        // Change the color and visibility of the email input field based on email validation
        if (!ValidateEmail(emailInputField.text) && emailInputField.text != "")
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
        // Set the text for the logged in details label
        loggedInDetails.text = "Logged in as " + PlayerPrefs.GetString(AllConstants.playerPrefUserNameVariableName);
    }

    public void ClearTexts()
    {
        // Clear the text and visibility of UI elements related to email update
        emailUpdateMessage.text = "";
        emailInputField.text = "";
        emailIdMessage.gameObject.SetActive(false);
    }
}

// Class representing the response structure for email update API
public class EmailUpdateType
{
    public string status;
    public int code;
    public string message;
    public string data;
}
