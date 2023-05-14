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
    public void DeployUnit(Dictionary<string, Column> LandscapeDictionary,int Count)
    {
        //List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false && a.Value.Tree == false).ToList();
        for (int i = 0; i < Count; i++)
        {
            DeployOneUnit(LandscapeDictionary);
            UnityEngine.Debug.Log("   004 ----- unit");
        }
    }
    public void DeployOneUnit(Dictionary<string, Column> LandscapeDictionary)
    {
        List<KeyValuePair<string, Column>> openColumnList = this.townManager.GetSpaceColumnList(LandscapeDictionary); 
        Column column = new TownManager().GetRandomColumn(LandscapeDictionary, openColumnList);
        column.Unit = true;
        UnitModel unitModel = new UnitModel();
        unitModel.Position = column.Position;
        UnitPlaceList.Add(unitModel);
    }
    public void MoveUnit(Dictionary<string, Column> LandscapeDictionary)
    {
        foreach(var UnitPlace in UnitPlaceList)
        {
            var unitPoint = UnitPlace.GetNextPath();
            Column column = LandscapeDictionary[unitPoint.ToString()];
            column.Unit = true;
        }
        
    }
}
