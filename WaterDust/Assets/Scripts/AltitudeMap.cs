using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeMap 
{
    int[,] altitudeMap_ar;

    public void Init(int Xsize, int Zsize)
    {
        altitudeMap_ar = new int[Xsize, Zsize];
    }
    public void SetAltutude(int X, int Z, int Altutude)
    {
        altitudeMap_ar[X, Z] = Altutude;
    }
    public int GetAltutude(int X, int Z)
    {
        return altitudeMap_ar[X, Z];
    }
}
