using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{

    // NOTE: Drag ProgressUI prefab into Main Camera, and set pos 0,0,0, rot 0,0,0, scale 1,1,1


    public List<Color> levelCols;

    public Renderer barRenderer, ballRenderer;

    public TextMesh levelText;

    public Transform ballScalar, barScalar;

    public ParticleSystem winParticle;

    private Color currentLevelCol;

    private int currentLevel = 1;

    public static ProgressUI instance;

    private void Start()
    {
        instance = this;
    }
    
    public int GetCurrentLevel ()
    {
        return currentLevel + 1;
    }

    public void SetLevelProgress (float progress) { //0 to 1  //call this from game progression
        progress = Mathf.Clamp(progress, 0, 1);
        if (progress < 0.3f) {
            SetLocalScale(ballScalar, progress * (1/0.3f));
            SetLocalScale(barScalar, 0);
        }
        else {
            SetLocalScale(ballScalar, 1);
            SetLocalScale(barScalar, new Vector3(1, (progress * (1/0.7f)) - 0.35f, 1));
        }
    }

    private void SetLocalScale (Transform obj, float scale) {
        SetLocalScale(obj, new Vector3(scale, scale, scale));
    }
    private void SetLocalScale (Transform obj, Vector3 scale) {
        obj.localScale = scale;
    }

    public int SetLevel (int level)
    { //start at level 0   call this from game progression. Return the color of the new level
        currentLevel = level;
        levelText.text = "// Level " + (level + 1);
        if (level != 0) winParticle.Emit(10);

        level = level % (levelCols.Count);
        currentLevelCol = levelCols[level];
        levelText.color = ballRenderer.material.color = barRenderer.material.color = currentLevelCol;
        UIMaster.instance.LevelColor = currentLevelCol;
        return level;
    }
}
