using Assets.Script2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager 
{
    public List<UnitModel> UnitPlaceList;
    TownManager townManager;
    public UnitManager(TownManager TownManager)
    {
        this.UnitPlaceList = new List<UnitModel>();
        this.townManager = TownManager;
    }
    public void DeployUnit(Dictionary<string, Column> LandscapeDictionary, int SizeMap, int Count)
    {
        //List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false && a.Value.Tree == false).ToList();
        for (int i = 0; i < Count; i++)
        {
            DeployOneUnit(LandscapeDictionary, SizeMap,i);
            UnityEngine.Debug.Log("   004 ----- unit");
        }
    }
    public void DeployOneUnit(Dictionary<string, Column> LandscapeDictionary,int SizeMap,int FlagId)
    {
        List<KeyValuePair<string, Column>> openColumnList = this.townManager.GetSpaceColumnList(LandscapeDictionary); 
        Column column = new TownManager().GetRandomColumn(LandscapeDictionary, openColumnList);
        column.Unit = true;

        UnitModel unitModel = new UnitModel(FlagId);
        unitModel.Position = column.Position;
        UnitPlaceList.Add(unitModel);
        
        CountPath(LandscapeDictionary, SizeMap);
    }
    public void MoveUnit(Dictionary<string, Column> LandscapeDictionary)
    {
        foreach(UnitModel UnitPlace in UnitPlaceList)
        {
            Column oldColumn = LandscapeDictionary[UnitPlace.Position.ToString()];
            oldColumn.Unit = false;
            UnityEngine.Debug.Log("$$$  ["+ UnitPlace.Path.Count + "]   77---"+ oldColumn.Position.ToString());

            UnitPlace.GetNextPath();
            Column column = LandscapeDictionary[UnitPlace.Position.ToString()];
            column.Unit = true;
            UnitPlace.Position = column.Position;
            UnityEngine.Debug.Log("$$$  [" + UnitPlace.Path.Count + "]   66-- -" +column.Position.ToString());
             
        }
        
    }
    void CountPath(Dictionary<string, Column> LandscapeDictionary, int SizeMap)
    {
        foreach (var item in UnitPlaceList)
        {
            TestPath(LandscapeDictionary, SizeMap,item);
        }
    }
    void TestPath(Dictionary<string, Column> LandscapeDictionary,int SizeMap, UnitModel UnitPlace)
    {
        FindPathAltitude findPath = new FindPathAltitude();

        //long DestinationNode_ID_Player = ((int)(2) * 100) + (int)(1);
        //Point2D DestinationNode_ID_Player = new Point2D(1,2);
        Point2D DestinationPlayer = UnitPlace.Position;
        //long StartNode_ID_Fiend = ((int)2 * 100) + (int)2;
        //Point2D StartNode_ID_Fiend = new Point2D(2, 2);
        Point2D StartFiend = this.townManager.TownPlaceList[0];

        List<long[]> preparationMap_ar_ar = new PreparationFindPath().GetPreparationMap(LandscapeDictionary, SizeMap);
        List<long[]> preparationMapAltitude_ar = new PreparationFindPath().GetPreparationAltitudeMap(LandscapeDictionary, SizeMap);
        //preparationMapAltitude_ar[2][2] = 2;
        //preparationMapAltitude_ar[2][1] = 2;
        //preparationMapAltitude_ar[2][0] = 1;
        //preparationMap_ar_ar[(int)WaterCube.GetPointCube().X][(int)WaterCube.GetPointCube().Z] = 0;
        //preparationMap_ar_ar[(int)waterCubeEnd.X][(int)waterCubeEnd.Z] = 0;
        //new PreparationFindPath().PrintMap(preparationMap_ar_ar, SizeMap);
        //new PreparationFindPath().PrintMap(preparationMapAltitude_ar, SizeMap);

        int wallObstacle = 1;
        UnitPlace.Path = findPath.findShortestPath(DestinationPlayer, StartFiend,
            preparationMap_ar_ar, preparationMapAltitude_ar, wallObstacle, "manhattan", 10, 14);
        System.Diagnostics.Debug.WriteLine("res = "+DestinationPlayer.ToString() + " ==== " + StartFiend.ToString() + " ====ko  = " + UnitPlace.Path.Count);
        UnityEngine.Debug.Log("START PATH------ --"+ UnitPlace.Path.Count);
        foreach (SuperNode item in UnitPlace.Path)
        {
            UnityEngine.Debug.Log("Ad ----- Path =" + item.ToString() + "------ ------");
        }
    }
}
