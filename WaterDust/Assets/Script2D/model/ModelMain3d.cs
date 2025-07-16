using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Script2D;
using Assets.Script2D.model;

public class ModelMain3d 
{

    public Point2D TownPlace;
    UnitModel UnitPlace;

    List<List<Column>> Landscape_List;
    public Dictionary<string, Column> LandscapeDictionary;

    List<GameObject> GraphicList;
  
    int SizeMap = 0;
    public List<Point2D> IndexFontainList;
    int FontainCount = 0;
    


    public void Start()
    {
        this.UnitPlace = new UnitModel();
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

        DeployTown();
        DeployTree();
        DeployUnit();

        TestPath();
    }
    void TestPath()
    {
        FindPathAltitude findPath = new FindPathAltitude();

        Point2D DestinationNode_Player = UnitPlace.Position;

        Point2D StartNode_Fiend = TownPlace;

        List<long[]> preparationMap_ar_ar = new PreparationFindPath().GetPreparationMap(LandscapeDictionary,SizeMap);
        List<long[]> preparationMapAltitude_ar = new PreparationFindPath().GetPreparationAltitudeMap(LandscapeDictionary,SizeMap);

        int wallObstacle = 1;
        UnitPlace.Path = findPath.findShortestPath(DestinationNode_Player, StartNode_Fiend,
            preparationMap_ar_ar, preparationMapAltitude_ar, wallObstacle, "manhattan", 10, 14);
        
        foreach(var item in UnitPlace.Path)
        {
           // Debug.Log( "Ad -----" + item.ToString() + "---------------"  );
        }
     }

    void DeployTown()
    {
        List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a=>a.Value.Water==0).ToList();
 
        Column column = GetRandomColumn(openColumnList);
        column.Town = true;
        TownPlace = column.Position;
    }

    void DeployTree()
    {
        List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false).ToList();
        Column column = GetRandomColumn(openColumnList);
        column.Tree = true;
    }
    void DeployUnit()
    {
        List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false && a.Value.Tree == false).ToList();
        
        Column column = GetRandomColumn(openColumnList);
        column.Unit = true;
        UnitPlace.Position = column.Position;
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
                        var unitPoint = UnitPlace.GetNextPath();
                        Column column = LandscapeDictionary[unitPoint.ToString()];
                        column.Unit = true;

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

    public void AddStoneColumn(string key)
    {
        LandscapeDictionary[key].Stone += 1;
    }
}
