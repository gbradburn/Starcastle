using System;
using UnityEngine;

public partial class Shield
{
    [Serializable]
    public class RingDefinition
    {
        public string Name;
        public float Radius;
        public int Segments;
        public Color RingColor;
    }
}
