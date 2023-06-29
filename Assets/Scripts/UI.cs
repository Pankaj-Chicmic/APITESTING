using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject loggedInPanel;
    [SerializeField] GameObject logInPanel;
    [SerializeField] GameObject guestLoginPanel;
    private void Start()
    {
        if (PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName) == "")
        {
            guestLoginPanel.SetActive(true);
        }
        else
        {
            loggedInPanel.SetActive(true);
            loggedInPanel.GetComponent<LoggedInPanel>().SetLoggedInDetails();
        }
    }
}



