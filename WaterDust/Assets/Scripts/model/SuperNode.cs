﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNode 
{
	public long finalCost;
	public long groundCost;
	public long hCost;
	public SuperNode parent;
	public int row;
	public int column;
	public int id;
}