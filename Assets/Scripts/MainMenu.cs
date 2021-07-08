using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
 public void newGame ()
    {
       // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(2);
    }

  public void ExitGame ()
    {
        Debug.Log("exit");
        Application.Quit();
    }

   /*public void Back()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }*/

    public void continueGame()
	{
        PlayerData data = SaveSystem.LoadPlayer();
        int level = data.currentLevel;
       // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(level);
    }

    public void levelSelect()
	{
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(1);
    }

}
