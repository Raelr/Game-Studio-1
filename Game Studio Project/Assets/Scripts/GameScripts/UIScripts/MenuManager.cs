using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : InitialisedEntity {

    [Header("Main Menu")]
    [SerializeField]
    Image MainMenuPanel;

    [Header("Lose Screen")]
    [SerializeField]
    Image LosePanel;

    public override void Initialise() {

        base.Initialise();

        GlobalMethods.Hide(LosePanel.gameObject);
    }

    public void QuitGame() {

        Application.Quit();
    }

    public void LoadLoseScreen() {

        GlobalMethods.Show(LosePanel.gameObject);
    }

    public void RestartLevel() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
