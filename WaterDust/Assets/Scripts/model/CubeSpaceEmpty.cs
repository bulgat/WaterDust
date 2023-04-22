using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CubeSpaceEmpty 
{
    public Key3D GetCubeSpaceEmpty(HyperCubeModel hyperCubeItemCube, bool EmptyPlace, ModelSceneScr _modelSceneScr)
    {
        //check left, right, forward, back

        for (var i = 0; i < _modelSceneScr.checkSide.Length / _modelSceneScr.checkSide.Rank; i++)
        {
            Key3D keySide = new Key3D((hyperCubeItemCube.GetPointCube().X + _modelSceneScr.checkSide[i, 0]), hyperCubeItemCube.GetPointCube().Y, (hyperCubeItemCube.GetPointCube().Z + _modelSceneScr.checkSide[i, 1]));

            if (0 > keySide.X || 0 > keySide.Z || _modelSceneScr.SizeWorld<= keySide.X || _modelSceneScr.SizeWorld <= keySide.Z)
            {
                //  Запрет на просмотр выйдет за индекс
                continue;
                //throw new Exception($"= Error out index  keySide.X = {keySide.X}");
            }
            int sideCube;
            try
            {

                sideCube = _modelSceneScr._cubeModelList.GetCube(keySide.X, keySide.Y, keySide.Z);
            }
            catch (ApplicationException e)
            {
                UnityEngine.Debug.Log($"Error! x = {keySide.X} y={keySide.Y} z={keySide.Z}  error =  {e.Message} ");
                throw;
            }
            if (EmptyPlace)
            {
                if (sideCube == 0)
                {

                    if (_modelSceneScr.AllowMoveCube(keySide.X, keySide.Z))
                    {

                        return keySide;

                    }
                }
            }
            else
            {
                if (sideCube > 0)
                {
                    if (_modelSceneScr.AllowMoveCube(keySide.X, keySide.Z))
                    {
                        return keySide;
                    }
                }
            }
        }
        return new Key3D(hyperCubeItemCube.GetPointCube().X, hyperCubeItemCube.GetPointCube().Y, hyperCubeItemCube.GetPointCube().Z);
    }
}
