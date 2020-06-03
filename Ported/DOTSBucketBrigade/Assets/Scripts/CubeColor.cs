﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[GenerateAuthoringComponent]
[MaterialProperty("_BaseColor", MaterialPropertyFormat.Float3)]
public struct CubeColor : IComponentData
{
    public float3 Color;
}