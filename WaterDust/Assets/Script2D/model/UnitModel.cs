using Assets.Script2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel 
{
  public Point2D Position;
    public List<SuperNode> Path;
    public Point2D GetNextPath()
    {
        if (Path != null)
        {
            if (Path.Count > 0)
            {
                var Node = Path[0];
                Path.Remove(Node);
                //Debug.Log("0002  Node ");
                return new Point2D(Node.column, Node.row);
                    //new SuperNode() { column = Position.x,row=Position.z };
            }
        }
        return Position;
    }
}
