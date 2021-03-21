using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject quitMenu;

    private void Start()
    {
        //if the quit menu is active when the game starts, set it to inactive so that the player gets straight into the game.
        if (quitMenu.activeInHierarchy)
        {
            quitMenu.SetActive(false);
        }
    }

    private void Update()
    {
        //if the player presses Esc, pause the game and bring up the pause menu
        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            quitMenu.SetActive(true);
        }
    }

    
    /// <summary>
    /// Called by the return button in the pause menu. It sets the pause menu to inactive and sets the timescale back to 1.
    /// </summary>
    public void ReturnToGame()
    {
        quitMenu.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Called by the quit button in the pause menu. It quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
