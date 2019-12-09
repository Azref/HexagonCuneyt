using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Variables/Integer", order = 8)]
public class RV_Int : ScriptableObject
{
    public int value;

    public static implicit operator int(RV_Int v)
    {
        throw new NotImplementedException();
    }
}