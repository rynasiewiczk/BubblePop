using System;
using UnityEngine;

[Serializable]
public class AimingSettings
{
    public int ScreenWidthAimPositionPercentage = 50;
    public int ScreenHeightAimPositionPercentage = 15;

    public Vector3 GetAimingPositionInViewPortPosition()
    {
        return new Vector3((float)ScreenWidthAimPositionPercentage/100, (float)ScreenHeightAimPositionPercentage/100);
    }
}
