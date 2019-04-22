﻿using Unity.Entities;

[System.Serializable]
public struct GravityComponentData : IComponentData
{
    public float mass;
    public float delay;
    public float velocity;
}

public class GravityComponent : ComponentDataProxy<GravityComponentData>{ }