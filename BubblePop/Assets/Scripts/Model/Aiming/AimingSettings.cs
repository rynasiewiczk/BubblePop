using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[Serializable]
public class AimingSettings
{
    [OdinSerialize, ShowInInspector] private int _screenWidthAimPositionPercentage = 50;
    [OdinSerialize, ShowInInspector] private int _screenHeightAimPositionPercentage = 15;

    public int MaxAmountOfWallBounces = 1;

    public Vector3 GetAimingPositionInViewPortPosition()
    {
        return new Vector3((float)_screenWidthAimPositionPercentage/100, (float)_screenHeightAimPositionPercentage/100);
    }
}
