using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackOn : MonoBehaviour
{
    public GameObject target, tracker;
    private bool currentState;
    public bool inverted = false;

    void Start()
    {
        RefreshState();
    }

    private void FixedUpdate()
    {
        if (tracker.activeInHierarchy != currentState)
        {
            RefreshState();
        }
    }

    private void RefreshState ()
    {
        currentState = tracker.activeInHierarchy;
        target.SetActive(inverted ? !currentState : currentState);
    }
}
