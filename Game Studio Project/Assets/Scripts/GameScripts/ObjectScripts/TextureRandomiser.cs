using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureRandomiser : MonoBehaviour
{
    public List<Texture> textures;

    public MeshRenderer render;

    public void SetRandom()
    {
        Texture chosenTexture = textures[Random.Range(0, textures.Count)];

        if (render)
            render.material.mainTexture = chosenTexture;
    }
}
