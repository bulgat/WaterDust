using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNode 
{
	public long finalCost;
	public long GroundCost;
	public long Hcost;
	public SuperNode parent;
	public int row;
	public int column;
	public int id;
    public override string ToString()
    {
        return row+"_"+column;
    }

}
