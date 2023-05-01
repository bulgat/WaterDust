using Assets.Script2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column 
{
    public int Stone { set; get; }
    public int Water { set; get; }
    public Point2D Position { set; get; }
    public Point2D VectorInertia { set; get; }
    public int VectorForce { set; get; }
    public bool DebugWater;
    public bool TurnMove { set; get; }
    public bool Mud { set; get; }
    public Column(int stone, int water)
    {
        this.Stone = stone;
        this.Water = water;
    }
    public int GetSum()
    {
        return this.Stone + this.Water;
    }
}
