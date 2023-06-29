using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class GuestLoginPanel : MonoBehaviour,PanelInterface
{
    [SerializeField] private UI ui;
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private GameObject guestLoginPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button guestLoginButton;
    [SerializeField] private Button goToLoginPanelButton;
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TextMeshProUGUI logInResultText;
    [SerializeField] private TextMeshProUGUI userNameMessage;
    private Action<string> onGuestLoginSuccessAction;
    private Action<string> onGuestLoginFailureAction;
    private Action onGuestLoginConnectionErrorAction;
    void Start()
    {
        guestLoginButton.onClick.AddListener(GuestLogin);
        goToLoginPanelButton.onClick.AddListener(GoToLoginPanel);
        onGuestLoginSuccessAction += onGuestLoginSuccessMethod;
        onGuestLoginFailureAction += onGuestLoginFailureMethod;
        onGuestLoginConnectionErrorAction += onGuestLoginConnectionErrorMethod;
    }
    private void GoToLoginPanel()
    {
        StartCoroutine(ui.ChangePanel(guestLoginPanel, loginPanel,0));
    }
    private void GuestLogin()
    {
        if (userNameInputField.text == "")
        {
            return;
        }
        if (!ValidateUsername(userNameInputField.text))
        {
            logInResultText.gameObject.SetActive(true);
            logInResultText.text = "Invalid Username Only Alphabet, numbers and underscore _ is allowed";
            return;
        }
        APICalls.GuestLogin(userNameInputField.text, onSuccess: onGuestLoginSuccessAction, onFailure: onGuestLoginFailureAction, onConnectionError: onGuestLoginConnectionErrorAction);
    }
    private void onGuestLoginSuccessMethod(string jsonData)
    {
        GuestLoginDownloadHandlerType guestLoginDownloadHandlerType = JsonUtility.FromJson<GuestLoginDownloadHandlerType>(jsonData);
        PlayerPrefs.SetString(AllConstants.playerPrefAccessTokenVariableName, guestLoginDownloadHandlerType.data);
        PlayerPrefs.SetString(AllConstants.playerPrefUserNameVariableName, userNameInputField.text);
        StartCoroutine(ui.ChangePanel(guestLoginPanel,loggedInPanel,2,logInResultText, "Successfully Logged in as " + userNameInputField.text));
        loggedInPanel.GetComponent<LoggedInPanel>().SetLoggedInDetails();
    }
    private void onGuestLoginFailureMethod(string jsonData)
    {
        GuestLoginDownloadHandlerType guestLoginDownloadHandlerType = JsonUtility.FromJson<GuestLoginDownloadHandlerType>(jsonData);
        logInResultText.gameObject.SetActive(true);
        if (guestLoginDownloadHandlerType != null && guestLoginDownloadHandlerType.message != null && guestLoginDownloadHandlerType.message == AllConstants.guestLoginDuplicateUserNameMessage)
        {
            logInResultText.text = "UserName Not Available";
        }
        else logInResultText.text = "Check Network Connection and try again";
    }
    private void onGuestLoginConnectionErrorMethod()
    {
        logInResultText.gameObject.SetActive(true);
        logInResultText.text = "Connection Error, Check Internet Connection";
    }
    private bool ValidateUsername(string username)
    {
        string pattern = @"^[A-Za-z0-9\-_]+$";
        return Regex.IsMatch(username, pattern);
    }
    //private IEnumerator ChangePanel(GameObject toSetFalse, GameObject toSetTrue,int time ,TextMeshProUGUI textChange = null, string text = "")
    //{
    //    if (textChange != null)
    //    {
    //        textChange.gameObject.SetActive(true);
    //        textChange.text = text;
    //    }
    //    yield return new WaitForSeconds(time);
    //    if (textChange != null) textChange.gameObject.SetActive(false);
    //    toSetFalse.SetActive(false);
    //    toSetTrue.SetActive(true);
    //    toSetTrue.GetComponent<PanelInterface>().ClearTexts();
    //}
    public void ChangeUserNameFieldStatus()
    {
        if (!ValidateUsername(userNameInputField.text) && userNameInputField.text!="")
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
        userNameInputField.text = "";
        logInResultText.text = "";
    }
}
public class GuestLoginDownloadHandlerType
{
    public string status;
    public int code;
    public string message;
    public string data;
}
