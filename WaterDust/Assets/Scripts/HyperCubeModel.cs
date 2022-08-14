using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperCubeModel 
{
    public int Id;
    public bool Water;
    Key3D _key3DSide;
    public int Impulse;
    public List<Point> PathList;
    public bool TakeStone;
    public bool ActiveStone;
    public HyperCubeModel() {
        Impulse = ModelSceneScr.ImpulseStart;
    }
    public void SetMoveCube(Key3D keySideShift)
    {
        _key3DSide = keySideShift;
    }
    public Key3D GetMoveCube()
    {
        return _key3DSide;
    }
    public void SpendImpulse(int x)
    {
        Impulse += x;
    }
    public int GetImpulse()
    {
        return Impulse;
    }
}
