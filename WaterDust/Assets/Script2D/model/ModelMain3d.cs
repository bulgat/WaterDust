using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Script2D;
using Assets.Script2D.model;
using System;

public class ModelMain3d 
{

    public Point2D TownPlace;
    UnitModel _UnitPlace;

    List<List<Column>> Landscape_List;
    public Dictionary<string, Column> LandscapeDictionary;

    List<GameObject> GraphicList;
  
    int SizeMap = 0;
    public List<Point2D> IndexFontainList;
    int FontainCount = 0;

    public List<RealUnit> RealUnitList;
    public static event Action ActionChangeMap;

    public ModelMain3d() {
        Start();


    }



    void Start()
    {
        ActionChangeMap += ChangeMap;
        this.RealUnitList = new List<RealUnit>();
        this._UnitPlace = new UnitModel();
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

        Point2D point2Dtown =  DeployTown();
        Point2D point2Dtree= DeployTree();

        RealUnitList.Add(new RealUnit(DeployUnit()));

        TestPath(point2Dtown, point2Dtree);
    }
    private void ChangeMap()
    {
        //DoStuff
        Debug.Log("@@@NEW  UnitPath  " );
    }


    public void StepUnit()
    {
        RealUnitList.FirstOrDefault().SetStep(Time.time);
    }
    public RealUnit GetRealUnit()
    {
        return RealUnitList.FirstOrDefault();
    }
    void TestPath(Point2D point2Dtown, Point2D point2Dtree)
    {
        Debug.Log(point2Dtown+" = SS   key  getColu = "+ point2Dtree);

        FindPathAltitude findPath = new FindPathAltitude();

        //Point2D DestinationNode_Player = _UnitPlace.Position;

        //Point2D StartNode_Fiend = TownPlace;

        Point2D DestinationNode_Player = point2Dtown;

       Point2D StartNode_Fiend = point2Dtree;

        Debug.Log(DestinationNode_Player+" = column = "+ StartNode_Fiend);

        List<long[]> preparationMap_ar_ar = new PreparationFindPath().GetPreparationMap(LandscapeDictionary,SizeMap);
        List<long[]> preparationMapAltitude_ar = new PreparationFindPath().GetPreparationAltitudeMap(LandscapeDictionary,SizeMap);

        int wallObstacle = 1;
        _UnitPlace.Path = findPath.findShortestPath(DestinationNode_Player, StartNode_Fiend,
            preparationMap_ar_ar, preparationMapAltitude_ar, wallObstacle, "manhattan", 10, 14);

        GetRealUnit().SetPath(findPath.findShortestPath(DestinationNode_Player, StartNode_Fiend,
            preparationMap_ar_ar, preparationMapAltitude_ar, wallObstacle, "manhattan", 10, 14));


     }

    Point2D DeployTown()
    {
        List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a=>a.Value.Water==0).ToList();
 
        Column column = GetRandomColumn(openColumnList);
        column.Town = true;
        TownPlace = column.Position;
        return TownPlace;
    }

    Point2D DeployTree()
    {
        List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false).ToList();
        Column column = GetRandomColumn(openColumnList);
        column.Tree = true;
        return column.Position;
    }
    Point2D DeployUnit()
    {
        List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false && a.Value.Tree == false).ToList();

        Column column = GetRandomColumn(openColumnList);
        column.Unit = true;
        _UnitPlace.Position = column.Position;

        return column.Position;
    }
    Column GetRandomColumn(List<KeyValuePair<string, Column>> openColumnList)
    {
        int rnd = UnityEngine.Random.Range(0, openColumnList.Count);
        var placeRnd = openColumnList[rnd].Value.Position;
        Column column = this.LandscapeDictionary[placeRnd.ToString()];
        return column;
    }


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
                            //несем землю.
                            ActionChangeMap?.Invoke();
                        }
                        //затопление города
                        if (checkColumn.Town)
                        {
                            checkColumn.Town = false;
                            this.DeployTown();
                        }
                        if (checkColumn.Tree)
                        {
                            checkColumn.Tree = false;
                        }
                        if (checkColumn.Unit)
                        {
                            checkColumn.Unit = false;
                        }
                        var unitPoint = _UnitPlace.GetNextPath();
                        Column column = LandscapeDictionary[unitPoint.ToString()];
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

                LandscapeDictionary[IndexFontainList[FontainCount].ToString()].Water += 1;
                
            }
        }
        foreach (var item in LandscapeDictionary)
        {
            item.Value.TurnMove = false;
        }
        
        return changeView;
    }

    public void AddStoneColumn(string key,bool AddStone)
    {
        if (AddStone)
        {
            LandscapeDictionary[key].Stone += 1;

        } else
        {
            LandscapeDictionary[key].Stone -= 1;
        }
    }
}
