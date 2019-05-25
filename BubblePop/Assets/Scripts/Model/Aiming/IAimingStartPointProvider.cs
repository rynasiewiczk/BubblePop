using System.Net.NetworkInformation;
using UnityEngine;

namespace Project.Aiming
{
    public interface IAimingStartPointProvider
    {
        Vector2 GetAimingStartPoint();
    }
}