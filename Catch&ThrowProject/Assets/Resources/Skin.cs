﻿using UnityEngine;

namespace Assets.Resources
{
    [CreateAssetMenu]
    public class Skin : ScriptableObject
    {
        public Texture playerTexture;
        public Texture maskTexture;

        public Color mainColor;

        public bool used;
    }
}
