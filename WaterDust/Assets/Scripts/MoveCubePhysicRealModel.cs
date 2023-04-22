using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveCubePhysicRealModel 
{
    public HyperCubeModel MoveCubePhysicReal(ModelList CubeModelList, HyperCubeModel hyperCubeItemCube, float x, float y, float z)
    {
   
        CubeModelList.SetCube(hyperCubeItemCube.GetPointCube().X, hyperCubeItemCube.GetPointCube().Y, hyperCubeItemCube.GetPointCube().Z, 0);

        if (0 > x)
        {
            throw new Exception("Попытка выйти за индекс! x="+x );
        }

        Key3D newMoveCube = new Key3D((int)x, (int)y, (int)z);

       // Debug.Log(hyperCubeItemCube.Id + "  " + hyperCubeItemCube.PathList + "  MOVE REAL [" + CubeModelList.GetCube(hyperCubeItemCube.GetMoveCube().X, hyperCubeItemCube.GetMoveCube().Y, hyperCubeItemCube.GetMoveCube().Z) + "]     " + hyperCubeItemCube.GetMoveCube().X + "," + hyperCubeItemCube.GetMoveCube().Y + "," + hyperCubeItemCube.GetMoveCube().Z + "    ===> " + x + "," + y + "," + z);

        hyperCubeItemCube.SetMoveCube(newMoveCube);
        hyperCubeItemCube.SpendImpulse(-1);


        CubeModelList.SetCube(newMoveCube.X, newMoveCube.Y, newMoveCube.Z, hyperCubeItemCube.Id);

       // Debug.Log(hyperCubeItemCube.Id + "   END  MOVE REAL  cube = [" + hyperCubeItemCube.GetMoveCube().X + "," + hyperCubeItemCube.GetMoveCube().Y + "," + hyperCubeItemCube.GetMoveCube().Z + "]__  cube=" + CubeModelList.GetCube(newMoveCube.X, newMoveCube.Y, newMoveCube.Z));

        hyperCubeItemCube.SetMoveCube(newMoveCube);
        return hyperCubeItemCube;


    }
}
