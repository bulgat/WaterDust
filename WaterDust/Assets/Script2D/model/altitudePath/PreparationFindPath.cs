using Assets.Script2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationFindPath 
{
    public List<long[]> GetPreparationMap(Dictionary<string, Column> LandscapeDictionary,int SizeWorld)
    {
        List<long[]> CreateMap_ar = new List<long[]>();

        for (int GridRow = 0; GridRow < SizeWorld; GridRow++)
        {

            long[] arrayLong = new long[SizeWorld];

            CreateMap_ar.Add(arrayLong);

            for (int GridLine = 0; GridLine < SizeWorld; GridLine++)
            {
                Point2D point = new Point2D(GridRow, GridLine);
                Column column = LandscapeDictionary[point.ToString()];
                int quad = 0;
                if (column.Water>0) {
                    quad =1;
                }
                //Key3D newKey = new Key3D(GridRow, LevelY, GridLine);
                //Debug.Log((_cubeModelList==null)+" %%%%%%%%%%%%% " + GridLine + "   key =  " + newKey.X+"," +newKey.Y+","+ newKey.Z);

                //int quadCube = _cubeModelList.GetCube(newKey.X, newKey.Y, newKey.Z);

                //int quad = quad_ar.Count > 0 ? 1 : 0;
                //int quad = quadCube > 0 ? 1 : 0;

                CreateMap_ar[GridRow][GridLine] = quad;
            }



        }

        return CreateMap_ar;
    }
    public List<long[]> GetPreparationAltitudeMap(Dictionary<string, Column> LandscapeDictionary, int SizeWorld)
    {
        List<long[]> CreateMap_ar = new List<long[]>();

        for (int GridRow = 0; GridRow < SizeWorld; GridRow++)
        {

            long[] arrayLong = new long[SizeWorld];

            CreateMap_ar.Add(arrayLong);

            for (int GridLine = 0; GridLine < SizeWorld; GridLine++)
            {
                Point2D point = new Point2D(GridRow, GridLine);
                Column column = LandscapeDictionary[point.ToString()];

                CreateMap_ar[GridRow][GridLine] = column.GetSum();
                //CreateMap_ar[GridRow][GridLine] = 0;
            }



        }

        return CreateMap_ar;
    }
    public void PrintMap(List<long[]> createMap_ar, int SizeWorld)
    {
        Debug.Log(" ------------------------------------------------- " );
        for (int GridRow = 0; GridRow < SizeWorld; GridRow++)
        {
            string line = "_";
            for (int GridLine = 0; GridLine < SizeWorld; GridLine++)
            {
                line += (createMap_ar[GridRow][GridLine]) + ".";
            }
            Debug.Log(" PRINT =" + GridRow + " I = " + line);
        }

        Debug.Log(" ------------------------------------------------- ");
    }
}
