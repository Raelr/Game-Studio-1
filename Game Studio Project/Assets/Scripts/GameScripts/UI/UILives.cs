using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILives : MonoBehaviour
{
    private UIMaster master;
    public UIMaster Master
    {
        get { return master; }
        set { master = value; }
    }

    public void SetUIMasterReference(UIMaster ui)
    {
        master = ui;
    }

    //put variables here
    private GameObject livesCanvas;
    private GameObject heart1;
    private GameObject heart2;
    private GameObject heart3;

    public void Initialise()
    {
        //initialise variables
        livesCanvas = transform.Find("LivesCanvas").gameObject;
        heart1 = livesCanvas.transform.Find("Life1").gameObject;
        heart2 = livesCanvas.transform.Find("Life2").gameObject;
        heart3 = livesCanvas.transform.Find("Life3").gameObject;
    }

    public void ShowLives(int amount) {
        switch (amount) {
            case 0:
                heart1.Hide();
                heart2.Hide();
                heart3.Hide();
                break;
            case 1:
                heart1.Show();
                heart2.Hide();
                heart3.Hide();
                break;
            case 2:
                heart1.Show();
                heart2.Show();
                heart3.Hide();
                break;
            case 3:
                heart1.Show();
                heart2.Show();
                heart3.Show();
                break;
            default: break;
        }
    }
}
