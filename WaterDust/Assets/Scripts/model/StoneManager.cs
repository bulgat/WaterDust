using System.Collections;
using UnityEngine;

namespace Assets.Scripts.model
{
    public class StoneManager
    {
        public HyperCubeModel SetStoneRandom(HyperCubeModel hyperCubeItemCube, ModelSceneScr _modelSceneScr)
        {
            Key3D keySide = new CubeSpaceEmpty().GetCubeSpaceEmpty(hyperCubeItemCube, true, _modelSceneScr);

            //if (keySide == null)
            //{
            //return null;
            //}
            //set stoun cast cube
            HyperCubeModel hyperCubeModel = hyperCubeItemCube;
            if (hyperCubeItemCube.EqualKey(keySide) == false)
            {
                hyperCubeModel = _modelSceneScr.GetNewCube(false, keySide.X, keySide.Y, keySide.Z);
            }

            return hyperCubeModel;
        }

    }
}