using System.Collections;
using TMPro;
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
    public IEnumerator ChangePanel(GameObject toSetFalse, GameObject toSetTrue, int time, TextMeshProUGUI textChange = null, string text = "")
    {
        if (textChange != null)
        {
            textChange.gameObject.SetActive(true);
            textChange.text = text;
        }
        yield return new WaitForSeconds(time);
        if (textChange != null) textChange.gameObject.SetActive(false);
        toSetFalse.SetActive(false);
        toSetTrue.SetActive(true);
        toSetTrue.GetComponent<PanelInterface>().ClearTexts();
    }
}



