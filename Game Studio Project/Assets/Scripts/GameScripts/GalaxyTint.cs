using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlternativeArchitecture;

public class GalaxyTint : MonoBehaviour
{
    private GameProgression prog;
    private Renderer render;
    public float startA, endA;
    public int startLevel, endLevel;

    // Start is called before the first frame update
    public void ReTint()
    {
        render = GetComponent<Renderer>();
        prog = GameProgression.instance;
        SetAlpha(map(GameProgression.instance.currentLevel, startLevel, endLevel, startA, endA));
    }


    private void SetAlpha (float newA)
    {
        Color currentCol = render.material.GetColor("_TintColor");
        currentCol.a = newA;
        render.material.SetColor("_TintColor", currentCol);
    }


    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}
