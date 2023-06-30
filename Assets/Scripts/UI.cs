using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    // Serialized fields allow private variables to be exposed in the Inspector
    [SerializeField] GameObject loggedInPanel; // Reference to the logged in panel GameObject
    [SerializeField] GameObject logInPanel; // Reference to the login panel GameObject
    [SerializeField] GameObject guestLoginPanel; // Reference to the guest login panel GameObject

    private void Start()
    {
        // Check if the access token stored in PlayerPrefs is empty
        if (PlayerPrefs.GetString(AllConstants.playerPrefAccessTokenVariableName) == "")
        {
            // If the access token is empty, activate the guest login panel
            guestLoginPanel.SetActive(true);
        }
        else
        {
            // If the access token is not empty, activate the logged in panel
            loggedInPanel.SetActive(true);
            // Call the SetLoggedInDetails method of the LoggedInPanel script attached to the logged in panel
            loggedInPanel.GetComponent<LoggedInPanel>().SetLoggedInDetails();
        }
    }

    public IEnumerator ChangePanel(GameObject toSetFalse, GameObject toSetTrue, int time, TextMeshProUGUI textChange = null, string text = "")
    {
        // Check if a TextMeshProUGUI component is provided
        if (textChange != null)
        {
            // Activate the TextMeshProUGUI component and set its text
            textChange.gameObject.SetActive(true);
            textChange.text = text;
        }

        // Wait for the specified time duration
        yield return new WaitForSeconds(time);

        // Check if a TextMeshProUGUI component is provided
        if (textChange != null)
        {
            // Deactivate the TextMeshProUGUI component
            textChange.gameObject.SetActive(false);
        }

        // Deactivate the GameObject specified by toSetFalse
        toSetFalse.SetActive(false);

        // Activate the GameObject specified by toSetTrue
        toSetTrue.SetActive(true);

        // Call the ClearTexts method of the PanelInterface script attached to the GameObject specified by toSetTrue
        toSetTrue.GetComponent<PanelInterface>().ClearTexts();
    }
}



