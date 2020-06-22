using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class BeginEndManager : MonoBehaviour
{
    // to drag
    [SerializeField] private GameObject beginScreen;
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject text;

    [SerializeField] private string[] endMessages;


    public void StartGame()
    {
        Debug.Log("Start Game");
        beginScreen.SetActive(false);
        mainScreen.SetActive(true);
    }
    
    public void ReStartGame()
    {
        Debug.Log("Restart Game");
        
        // goes back to begin screen - we could have the end ,message there....
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitMainScreen()
    {
        Debug.Log("Exit Main Screen");
        ReStartGame();
    }

    public void EndGame (int EndMessageIndex)
    {
        Debug.Log("End Game: " + EndMessageIndex);
        EndScreen(endMessages[EndMessageIndex]);
    }

    void EndScreen(string endMessage)
    {
        text.GetComponent<TMPro.TextMeshProUGUI>().text = endMessage;
        endScreen.SetActive(true);
        mainScreen.SetActive(false);
        beginScreen.SetActive(false);
    }
}
