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

    public float currentExposure, targetExposure;
    public Color currentCol, targetCol;

    public float lerpSmooth;

    public Renderer skyBoxRender;
    

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
        
        RefreshSky();
        
    }

    private void RefreshSky () {
        skyBoxRender.sharedMaterial.SetColor("_Tint", currentCol);
        skyBoxRender.sharedMaterial.SetFloat("_Exposure", currentExposure);
    }

}
