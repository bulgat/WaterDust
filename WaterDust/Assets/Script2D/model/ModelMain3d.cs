﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Script2D;
using Assets.Script2D.model;

public class ModelMain3d 
{
    public GameObject WaterColumn;
    List<List<Column>> Landscape_List;
    public Dictionary<string, Column> LandscapeDictionary;
    //int xStart = -3;
    //int yStart = -3;
    List<GameObject> GraphicList;
    /*
    List<Point2D> CheckCubeList = new List<Point2D>() {
        new Point2D(0,1),
        new Point2D(-1,0),
        new Point2D(1,0),
        new Point2D(0,-1),

        new Point2D(-1,1),
        new Point2D(1,1),
        new Point2D(-1,-1),
        new Point2D(1,-1)
    };
    */
    int SizeMap = 0;
    //Column LeakCube;
    public List<Point2D> IndexFontainList;
    int FontainCount = 0;

    //bool LeakWaterOn = true;
    //int LeakWaterSum = 20;

    //private float nextActionTime = 0.0f;
    //public float period = 10000.1f;
    //55
    //int AlluviumRandom = 6;
    //int PrecipitationMudRandom = 10;

    public void Start()
    {
        //ParamModel;
        SizeMap = ParamModel.SizeMap;
        Landscape_List = new List<List<Column>>();
        for (int i = 0; i < SizeMap; i++)
        {
            List<Column> xList = new List<Column>();
            for (int z = 0; z < SizeMap; z++)
            {
                xList.Add(new Column(1, 7));
            }
            Landscape_List.Add(xList);

        }
        IndexFontainList = new List<Point2D>() { new Point2D(SizeMap / 3 + 1, SizeMap / 3 + 5),
        new Point2D(SizeMap - 6 , SizeMap -15)
        };
        Debug.Log("Start  IndexFont = "  );

        new ScenarioBuilder().CreateIslandVulcan( Landscape_List,SizeMap / 4, SizeMap / 4, SizeMap / 4, SizeMap / 2);
        new ScenarioBuilder().CreateIslandPlato(Landscape_List, SizeMap / 2, SizeMap / 5, SizeMap);


 

        LandscapeDictionary = new Dictionary<string, Column>();
        int countX = 0;
        foreach (List<Column> firstList in Landscape_List)
        {
            int countY = 0;
            foreach (Column secondColumn in firstList)
            {

                Point2D p = new Point2D(countX, countY);
                secondColumn.Position = p;
                LandscapeDictionary.Add(p.ToString(), secondColumn);
                countY++;
            }
            countX++;
        }
        GraphicList = new List<GameObject>();
        //DrawWater();
    }
    /*
    public void Update()
    {
        //if (Time.time > nextActionTime)
        //{
        //nextActionTime += period;
        // execute block of code here
        if (StepUpdateModel())
        {
            RemoveWater();
            DrawWater();
        }
        //}


    }
    */
    /*
    void CreateIslandVulcan(int Start, int StartY, int EndX, int EndY)
    {
        for (int i = Start; i < Start + EndX; i++)
        {
            for (int z = StartY; z < StartY + EndY; z++)
            {
                if (Start + EndX - 1 == i && z == StartY + 2)
                {

                    Landscape_List[i][z] = new Column(11, 0);
                    continue;
                }
                if (i == Start || i == Start + EndX - 1)
                {
                    // Debug.Log((i == SizeMap / 3) + "=== ===" + (i == SizeMap / 2 - 2));
                    Landscape_List[i][z] = new Column(15, 0);
                    continue;
                }
                if (z == StartY || z == StartY + EndY - 1)
                {
                    Landscape_List[i][z] = new Column(15, 0);
                    continue;
                }

                Landscape_List[i][z] = new Column(12, 0);
            }
        }
        //leak

    }
    */
    /*
    void CreateIslandPlato(int StartX, int StartY, int End)
    {
        for (int i = StartX; i < End; i++)
        {
            for (int z = StartY; z < End; z++)
            {

                Landscape_List[i][z] = new Column(11, 0);
            }
        }
    }
    */
    
    /*
    void RemoveWater()
    {
        foreach (var item in GraphicList)
        {
            Destroy(item);
        }
        GraphicList.Clear();
    }
    */
    // Update is called once per frame

    public bool StepUpdateModel()
    {
        bool changeView = false;
        foreach (var item in LandscapeDictionary)
        {
            if (item.Value.Water > 0)
            {
                if (item.Value.TurnMove == false)
                {
                    List<Column> checkCubeList = new ManagerColumn().GradeColumnList(LandscapeDictionary,item.Value).OrderBy(a => a.GetSum()).ToList(); ;
                    if (0 < checkCubeList.Count)
                    {
                        Column checkColumn = new ManagerColumn().GetColumn(item.Value, checkCubeList);

                        checkColumn.VectorForce = item.Value.Water - checkColumn.Water;

                        new DebugPrint().PrintState(item.Value, checkColumn, checkCubeList);

                        //перенос
                        item.Value.Water -= 1;
                        item.Value.VectorForce -= 1;
                        if (item.Value.Mud)
                        {
                            item.Value.Mud = false;
                            checkColumn.Mud = true;
                        }
                        checkColumn.DebugWater = item.Value.DebugWater;
                        checkColumn.Water += 1;
                        checkColumn.TurnMove = true;
                        checkColumn.VectorInertia = new Point2D(
                            checkColumn.Position.x + (checkColumn.Position.x - item.Value.Position.x),
                            checkColumn.Position.z + (checkColumn.Position.z - item.Value.Position.z)
                        );
                        new AlluviumPrecipitation().PrecipitationMud(checkColumn);
                        //перенос земли.
                       
                            if (new AlluviumPrecipitation().AlluviumStone(item.Value))
                            {
                                checkColumn.Mud = true;
                            }
                        

                        changeView = true;

                    }
                }
            }
        }
        if (ParamModel.LeakWaterOn)
        {
            LeakEvaporation leakEvaporation = new LeakEvaporation();
            if (leakEvaporation.LeakWater(LandscapeDictionary))
            {
                FontainCount=++FontainCount >= IndexFontainList.Count ? FontainCount=0 : FontainCount;

                //LandscapeDictionary[IndexFontain.ToString()].DebugWater = true;
                leakEvaporation.LeakCube.Water -= 1;
Debug.Log(FontainCount+"AddWa -------"+ IndexFontainList .Count+ "---------------" + changeView);
                LandscapeDictionary[IndexFontainList[FontainCount].ToString()].Water += 1;
                
            }
        }
        foreach (var item in LandscapeDictionary)
        {
            item.Value.TurnMove = false;
        }
        
        return changeView;
    }
    /*
    bool AlluviumStone(Column itemValue)
    {
        if (itemValue.Mud == false)
        {
            if (1>=itemValue.Water)
            {
                int rnd = Random.Range(0, AlluviumRandom);
                if (0 == rnd)
                {
                    itemValue.Stone -= 1;
                }
                return true;
            }
        }
        return false;
    }
    */
    /*
    void PrecipitationMud(Column checkColumn)
    {
        if (checkColumn.Mud)
        {
            if (checkColumn.Water > 2)
            {

                int rnd = Random.Range(0, PrecipitationMudRandom);
                if (0 == rnd)
                {
                    checkColumn.Stone += 1;
                    checkColumn.Mud = false;
                }
            }
        }
    }*/
    /*
    void PrintState(Column itemValue, Column checkColumn, List<Column> checkCubeList)
    {
        var parent = itemValue.GetSum();
        var child = checkColumn.GetSum();
        var kol = "";
        foreach (var i in checkCubeList)
        {
            kol += "_" + i.GetSum();
        }
        if (itemValue.DebugWater)
        {
            Debug.Log("position = " + itemValue.Position.ToString() + " water =" + itemValue.Water +
                " stone =" + itemValue.Stone +
                "  ap = " + checkColumn.Position.ToString() +
                          " =  count = " + checkCubeList.Count + " [" + kol + "] L parent "
                          + parent + " => " + child + " DebugWater = " + itemValue.DebugWater);
        }

    }*/
    /*
    Column GetColumn(Column columnParent, List<Column> gradeList)
    {

        if (columnParent.VectorForce > 0)
        {
            if (columnParent.VectorInertia != null)
            {
                Column child = gradeList.Where(a => a.Position.ToString() == columnParent.VectorInertia.ToString()).FirstOrDefault();
                if (child != null)
                {
                    return child;
                }
            }
        }

        if (0 == Random.Range(0, 12))
        {
            int rnd = Random.Range(0, gradeList.Count);
            return gradeList[rnd];
        }
        return gradeList.First();
    }
    */
    /*
    List<Column> GradeColumnList(Column ParentItem)
    {

        List<Column> gradeList = new List<Column>();
        foreach (Point2D check in ParamInner.CheckCubeList)
        {
            Point2D checkCount = new Point2D(ParentItem.Position.x + check.x, ParentItem.Position.z + check.z);
            //Point checkPoint = check;
            if (0 <= checkCount.x && SizeMap > checkCount.x)
            {
                if (0 <= checkCount.z && SizeMap > checkCount.z)
                {
                    if (ParentItem.GetSum() > LandscapeDictionary[checkCount.ToString()].GetSum())
                    {
                        gradeList.Add(LandscapeDictionary[checkCount.ToString()]);
                    }
                }
            }
        }

        //return gradeList.OrderByDescending(a=>a.GetSum()).ToList();
        return gradeList;
    }
    */
    /*
    bool LeakWater()
    {

        var sumWater = LandscapeDictionary.Values.ToList().Sum(a => a.Water);
        if (ParamModel.LeakWaterSum < sumWater)
        {

            var list = LandscapeDictionary.Values.Where(a => a.Water > 0).OrderBy(a => a.Stone).OrderByDescending(a => a.Water).ToList();
            int rnd = Random.Range(0, list.Count);
            LeakCube = list[rnd];
            return true;
        }

        return false;
    }
    */
}