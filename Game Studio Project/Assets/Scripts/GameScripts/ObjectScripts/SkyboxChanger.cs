using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SkyboxChanger : MonoBehaviour
{

    [System.Serializable]
    public struct SkyboxStyle {
        public Color skyCol;
        public float exposure;
    }


    [SerializeField]
    public List<SkyboxStyle> levelStyles;

    float currentExposure, targetExposure, lastExposure;
    Color currentCol, targetCol;

    public float lerpSmooth;

    public Material skyBoxMat;
    

    public void SetSkyboxStart (int level) {
        currentExposure = targetExposure = levelStyles[level].exposure;
        currentCol = targetCol = levelStyles[level].skyCol;
        RefreshSky();


    }

    public void SetSkybox (int level) {
        targetCol = levelStyles[level].skyCol;
        targetExposure = levelStyles[level].exposure;
    }

    void FixedUpdate () {
        currentCol = Color.Lerp(currentCol, targetCol, Time.deltaTime / lerpSmooth);
        currentExposure = Mathf.Lerp(currentExposure, targetExposure, Time.deltaTime / lerpSmooth);

        //checks for differences
        if (lastExposure != currentExposure)
            RefreshSky();

        lastExposure = currentExposure;
    }

    private void RefreshSky () {
       // skyBoxMat.
    }

}
