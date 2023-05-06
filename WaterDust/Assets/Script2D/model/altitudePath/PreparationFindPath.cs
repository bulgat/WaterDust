using Assets.Script2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationFindPath 
{
    public List<long[]> GetPreparationMap(int SizeWorld)
    {
        List<long[]> CreateMap_ar = new List<long[]>();

        for (int GridRow = 0; GridRow < SizeWorld; GridRow++)
        {

            long[] arrayLong = new long[SizeWorld];

            CreateMap_ar.Add(arrayLong);

            for (int GridLine = 0; GridLine < SizeWorld; GridLine++)
            {
                Point2D point = new Point2D(GridRow, GridLine);
                //Column column = LandscapeDictionary[point.ToString()];
                //if (column.Water>0) {
                  //  continue;
                //}
                //Key3D newKey = new Key3D(GridRow, LevelY, GridLine);
                //Debug.Log((_cubeModelList==null)+" %%%%%%%%%%%%% " + GridLine + "   key =  " + newKey.X+"," +newKey.Y+","+ newKey.Z);

                //int quadCube = _cubeModelList.GetCube(newKey.X, newKey.Y, newKey.Z);

                //int quad = quad_ar.Count > 0 ? 1 : 0;
                //int quad = quadCube > 0 ? 1 : 0;

                CreateMap_ar[GridRow][GridLine] = 0;
            }



        }

        return CreateMap_ar;
    }
}
