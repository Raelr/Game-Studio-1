﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public struct TentacleData
    {
        public float seed;
        public Transform parentTransform;
        public TentacleComponents components;
        public Vector3 parentOriginRotation;
    }

    private float lastReveal = -1;

    public GameObject tentaclePrefab;

    private List<TentacleData> tentacles = new List<TentacleData>();

    public int tentacleInterval;

    private bool isHidden = false;

    private void Start()
    {
        for (int rotation = 0; rotation < 360; rotation += tentacleInterval)
        {
            tentacles.Add(NewTentacle(rotation));
        }
        MonsterReveal(0);
    }
    
    float revealVal = 0;
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            revealVal -= 0.005f;
            Debug.Log("reveal : " + revealVal);
        }
        if (Input.GetKey(KeyCode.D))
        {
            revealVal += 0.005f;
            Debug.Log("reveal : " + revealVal);
        }
        MonsterReveal(revealVal);
    }

    private TentacleData NewTentacle (float rotation)
    {

        GameObject newTentacle = Instantiate(tentaclePrefab, transform);
        newTentacle.transform.localPosition = Vector3.zero;
        newTentacle.transform.localScale = Vector3.one;
        newTentacle.transform.localEulerAngles = new Vector3(0, 0, rotation);



        TentacleData newTentacleData = new TentacleData();
        newTentacleData.seed = Random.Range(0, 1000);
        newTentacleData.parentTransform = newTentacle.transform;
        newTentacleData.components = newTentacle.GetComponent<TentacleComponents>();
        newTentacleData.parentOriginRotation = newTentacle.transform.localEulerAngles;

        return newTentacleData;
    }



    public void MonsterReveal (float reveal)
    {
        //nothing should happen if the reveal level is still the same
        if (reveal != lastReveal)
            lastReveal = reveal;
        else
            return;

        if (reveal < 0.5f && !isHidden)
        {
            isHidden = true;
            MonsterHide();
        }
        else if (reveal > 0.5f && isHidden)
        {
            isHidden = false;
            MonsterShow();
        }


        foreach (TentacleData data in tentacles)
        {

            float seedOffset = (data.seed % 20) - 10;


            data.components.tentacleInner.localPosition = new Vector3(
                GlobalMethods.Remap(reveal, 0.5f, 1, 40 + seedOffset + 15, 23.4f),
                0,
                GlobalMethods.Remap(reveal, 0.5f, 1, -65 + seedOffset + 15, -15.24f));

            data.components.tentacleHolo.localPosition = new Vector3(
                GlobalMethods.Remap(reveal, 0.5f, 1, 40 + seedOffset, 23.4f),
                0,
                GlobalMethods.Remap(reveal, 0.5f, 1, -65 + seedOffset, -15.24f));

            data.parentTransform.localScale = new Vector3(
                1,
                GlobalMethods.Remap(reveal, 0.7f, 1, 0.2f + (seedOffset / 20), 2.8f),
                1);

            data.parentTransform.localEulerAngles = new Vector3(
                data.parentOriginRotation.x,
                data.parentOriginRotation.y,
                GlobalMethods.Remap(reveal, 0.7f, 1, data.parentOriginRotation.z + 30, data.parentOriginRotation.z)
                );

            data.components.tentacleInnerRenderer.material.SetTextureOffset("_V_SSS_DisplaceTex", new Vector2(0, (reveal / 2) + (seedOffset / 20)));


        }
    }

    private void MonsterShow()
    {
        MonsterState(true);
    }

    private void MonsterHide()
    {
        MonsterState(false);
    }

    private void MonsterState (bool state)
    {
        Debug.Log("set monster " + state);
        foreach (TentacleData data in tentacles)
            data.components.tentacleHoloRenderer.enabled = data.components.tentacleInnerRenderer.enabled = state;
    }




    /*


0.5 insanity:

tentacle parent scale = 1, 0.5, 1

material displacement = 0.4
object rotation = 90, 0, -60
object position = 23.40, 0, -15.24




1 insanity:

tentacle parent scale = 1, 1.5, 1

material displacement = 2.7
object rotation = 90, 0, 0
object position = 23.40, 0, -15.24


     * 
     * 
     */



}
