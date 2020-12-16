using Unity.Entities;
using Unity.Mathematics;

public struct Thrower : IComponentData
{
    public int TeamIndex;
    public int2 Coord;
    public int2 TargetCoord;
};