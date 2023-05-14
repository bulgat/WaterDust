using Assets.Script2D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TownManager 
{
    public List<Point2D> TownPlaceList;
    System.Random rand;
    public TownManager()
    {
        rand = new System.Random();
    }
    public void DeployTownList(Dictionary<string, Column> LandscapeDictionary,int Count)
    {
        //TownPlaceList = new List<Point2D>();
        //List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0).ToList();

        for (int i = 0; i < Count; i++)
        {
            //Column column = GetRandomColumn(LandscapeDictionary,openColumnList);
            //column.Town = true;
            //TownPlaceList.Add(column.Position);
            DeployOneTown(LandscapeDictionary);
            
        }
    }
    public List<KeyValuePair<string, Column>> GetSpaceColumnList(Dictionary<string, Column> LandscapeDictionary)
    {
        return LandscapeDictionary.Where(a => a.Value.Water == 0).ToList();
    }
    public void DeployOneTown(Dictionary<string, Column> LandscapeDictionary)
    {
        TownPlaceList = new List<Point2D>();
        //List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0).ToList();
        List<KeyValuePair<string, Column>> openColumnList = GetSpaceColumnList(LandscapeDictionary);
        Column column = GetRandomColumn(LandscapeDictionary, openColumnList);
        column.Town = true;
        TownPlaceList.Add(column.Position);
        UnityEngine.Debug.Log(  "  0005555 ----- -"+ column.Position.ToString());
    }
    public Column GetRandomColumn(Dictionary<string, Column> LandscapeDictionary,List<KeyValuePair<string, Column>> openColumnList)
    {
        
        //int rnd = UnityEngine.Random.Range(0, openColumnList.Count);
        int rnd = rand.Next(0, openColumnList.Count);
        var placeRnd = openColumnList[rnd].Value.Position;
        Column column = LandscapeDictionary[placeRnd.ToString()];
        return column;
    }
    public void DestroyDeployTown(Dictionary<string, Column> LandscapeDictionary,Column checkColumn)
    {
        Point2D townDelete = TownPlaceList.Where(a => a.ToString() == checkColumn.Position.ToString()).FirstOrDefault();
        TownPlaceList.Remove(townDelete);
        DeployOneTown(LandscapeDictionary);
    }
}
