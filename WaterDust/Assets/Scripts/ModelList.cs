using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModelList
{
    private int[,,] _allCube_ar;


    public void InitAllCube(int Xsize, int Ysize, int Zsize)
    {
        
        _allCube_ar = new int[Xsize, Ysize, Zsize];

    }
 
    public void SetCube(int X, int Y, int Z, int Id)
    {

       _allCube_ar[X,Y,Z] = Id;
    }
    public int GetCube(int X, int Y, int Z)
    {
        int indexCube;
        try
        {
            indexCube = _allCube_ar[X, Y, Z];
        } catch (Exception e)
        {
            Debug.LogWarning($"Error index!  x = {X} y={Y} z={Z}  error =  {e.Message} ");
            throw;
        }

        return indexCube;

    }
}
