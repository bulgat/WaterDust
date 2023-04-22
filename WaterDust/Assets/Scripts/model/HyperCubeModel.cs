using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperCubeModel 
{
    public int Id;
    public bool Water;
    Key3D key3DSide;
    public int Impulse;
    public List<Point> PathList;
    public bool TakeStone;
    public bool ActiveStone;
    public HyperCubeModel() {
        Impulse = ModelSceneScr.ImpulseStart;
    }
    public void SetMoveCube(Key3D keySideShift)
    {
        this.key3DSide = keySideShift;
    }
    public Key3D GetPointCube()
    {
        return this.key3DSide;
    }
    public void SpendImpulse(int x)
    {
        Impulse += x;
    }
    public bool EqualKey(Key3D Other)
    {
        return (this.key3DSide.X == Other.X && this.key3DSide.Y == Other.Y && this.key3DSide.Z == Other.Z);
    }
    public int GetImpulse()
    {
        return Impulse;
    }
}
