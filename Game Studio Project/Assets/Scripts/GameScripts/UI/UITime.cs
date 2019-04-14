using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITime : MonoBehaviour
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
    private GameObject timeCanvas;
    private Text timeText;
    private float time;

    public void Initialise()
    {
        //initialise variables
        timeCanvas = transform.Find("TimeCanvas").gameObject;
        timeText = timeCanvas.transform.Find("TimeText").GetComponent<Text>();
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        timeText.text = "Time: " + time.ToString("F2");
    }

}
