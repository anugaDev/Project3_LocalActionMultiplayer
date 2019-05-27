using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skin : ScriptableObject
{
    public Texture playerTexture;
    public Texture maskTexture;

    public Color mainColor;

    public bool used;
}
