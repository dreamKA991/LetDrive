using System;
using UnityEngine;

[Serializable]
public struct CarData
{
    public string Name;
    public int Price;
    public GameObject Prefab;
    public float Speed;
    public float Handling;
    public float Braking;
}