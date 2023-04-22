using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModelList
{
    private int[,,] _allCube_ar;
    //private Dictionary<string,int> _allDictionary_ar;

    public void InitAllCube(int Xsize, int Ysize, int Zsize)
    {
        
        _allCube_ar = new int[Xsize, Ysize, Zsize];
       // _allDictionary_ar = new Dictionary<string, int>();
    }
    /*
    public int[,,] GetAllCube() {
        return _allCube_ar;
    }
    */
    public void SetCube(int X, int Y, int Z, int Id)
    {
        // _allDictionary_ar[X + "_" + Y + "_" + Z] = Id;
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

        //Debug.Log("  "+ X+","+Y+","+Z + " Cube   ****  = "+(_allDictionary_ar == null)+ "  Id = ");
        return indexCube;
        /*
        if (_allDictionary_ar.ContainsKey(X + "_" + Y + "_" + Z)==false) {
            _allDictionary_ar[X + "_" + Y + "_" + Z] = 0;
        }

        return _allDictionary_ar[X + "_" + Y + "_" + Z];
        */
    }
}
