using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public static class Constants
{
    public const float Scale = 2.5f / 6371f;
    public const float EarthRadius = 6371f;
    public const float EarthAcceleration = 9.83f;
    public const float MoonRadius = 1737f;
    public const float EarthAngularSpeed = 7.3e-5f * 180 / Mathf.PI;
    public const float MoonAngularSpeed = 2.7e-6f * 180 / Mathf.PI;
    public const float SimulationSpeedMultiplier = 24;

    public const float GravitationalConstant = 6.6743e-11f;
    public const float EarthMass = 5.972e24f;
    
    public static readonly Vector2 LowOrbitAltitude = new Vector2(160, 2000);
    public static readonly Vector2 MediumOrbitAltitude = new Vector2(2000, 35786);
    public static readonly Vector2 GeostationaryOrbitAltitude = new Vector2(35786, 35786);
    public static readonly Vector2 MoonOrbitAltitude = new Vector2(160, 13000);

    public static float SimulatedEarthAngularSpeed => EarthAngularSpeed * SimulationSpeedMultiplier;
    public static float SimulatedMoonAngularSpeed => MoonAngularSpeed * SimulationSpeedMultiplier;
    public static float ScaledEarthRadius => EarthRadius * Scale;
}

