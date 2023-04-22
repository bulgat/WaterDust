using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key3D 
{
    public Key3D(int X, int Y, int Z) {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }
    public string GetName() {
        return this.X + "_" + this.Y + "_" + this.Z;
    }
    public int X { set; get; }
    public int Y { set; get; }
    public int Z { set; get; }



}

