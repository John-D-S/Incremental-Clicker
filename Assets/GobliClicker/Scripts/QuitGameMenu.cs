using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject quitMenu;

    private void Start()
    {
        if (quitMenu.activeInHierarchy)
        {
            quitMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            quitMenu.SetActive(true);
        }
    }

    public void ReturnToGame()
    {
        quitMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
