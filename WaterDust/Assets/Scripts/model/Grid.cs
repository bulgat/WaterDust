using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public Grid(int spotX, int spotY, int spotZ, bool wall) {
      this.SpotX= spotX;
        this.SpotY= spotY;
        this.SpotZ= spotZ;
        this.Wall= wall;
        this.Name = this.SpotX + "_" + this.SpotY+"_" + this.SpotZ;
}
    public int SpotX;
    public int SpotY;
    public int SpotZ;
    public bool Wall;
    public string Name;
}
