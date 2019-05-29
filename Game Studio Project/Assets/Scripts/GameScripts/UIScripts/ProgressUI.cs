using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{

    // NOTE: Drag ProgressUI prefab into Main Camera, and set pos 0,0,0, rot 0,0,0, scale 1,1,1


    public List<Color> levelCols;

    public Renderer barRenderer, ballRenderer;

    public TextMesh levelText;


    public void SetLevelProgress (float progress) { //0 to 1  //call this from game progression
        //do the lerping of ball / bar scaling

    }

    public void SetLevel (int level) { //call this from game progression
        levelText.color = ballRenderer.material.color = barRenderer.material.color = levelCols[level - 1 % 7];
        levelText.text = "// Level " + level;
    }
}
