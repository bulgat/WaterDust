using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Script2D;
using Assets.Script2D.model;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

public class ModelMain3d 
{
    //public GameObject WaterColumn;

    //public List<Point2D> TownPlaceList;
    public TownManager townManager;
    //List <UnitModel> UnitPlaceList;
    public UnitManager unitManager;

    List<List<Column>> Landscape_List;
    public Dictionary<string, Column> LandscapeDictionary;

    List<GameObject> GraphicList;
  
    int SizeMap = 0;
    public List<Point2D> IndexFontainList;
    int FontainCount = 0;
    


    public void Start()
    {
        this.townManager = new TownManager();
        this.unitManager = new UnitManager(this.townManager);
        //this.UnitPlaceList = new List<UnitModel>();
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
        //DrawWater();
 
        this.townManager.DeployTownList(LandscapeDictionary,2);
        DeployTree();
        this.unitManager.DeployUnit(LandscapeDictionary,this.SizeMap, 2);
        /*
        foreach(var item in this.unitManager.UnitPlaceList)
        {
            TestPath(item);
        }
        */

        System.Diagnostics.Debug.WriteLine("000 Start--------------");
        
        Task taskUpdateLandscape = Task.Factory.StartNew(()=> {
            
            while (true)
            {
                
                Thread.Sleep(100);
                
                StepUpdateModel();
                
            }
        });
        /*
        Task t = new Task(()=> {
            System.Diagnostics.Debug.WriteLine("0010  --- ---------------");
            UnityEngine.Debug.Log("0000000000");
            while (true)
            {
                UnityEngine.Debug.Log("1111000000");
                //StepUpdateModel();
                System.Diagnostics.Debug.WriteLine("0011 --- ---------------");
            }
        });
        t.Start();
        */
        //taskUpdateLandscape.Start();
    }
    /*
    void TestPath(UnitModel UnitPlace)
    {
        FindPathAltitude findPath = new FindPathAltitude();

        //long DestinationNode_ID_Player = ((int)(2) * 100) + (int)(1);
        //Point2D DestinationNode_ID_Player = new Point2D(1,2);
        Point2D DestinationNode_Player = UnitPlace.Position;
        //long StartNode_ID_Fiend = ((int)2 * 100) + (int)2;
        //Point2D StartNode_ID_Fiend = new Point2D(2, 2);
        Point2D StartNode_Fiend = this.townManager.TownPlaceList[0];

        List<long[]> preparationMap_ar_ar = new PreparationFindPath().GetPreparationMap(LandscapeDictionary,SizeMap);
        List<long[]> preparationMapAltitude_ar = new PreparationFindPath().GetPreparationAltitudeMap(LandscapeDictionary,SizeMap);
        //preparationMapAltitude_ar[2][2] = 2;
        //preparationMapAltitude_ar[2][1] = 2;
        //preparationMapAltitude_ar[2][0] = 1;
        //preparationMap_ar_ar[(int)WaterCube.GetPointCube().X][(int)WaterCube.GetPointCube().Z] = 0;
        //preparationMap_ar_ar[(int)waterCubeEnd.X][(int)waterCubeEnd.Z] = 0;
        //new PreparationFindPath().PrintMap(preparationMap_ar_ar, SizeMap);
        //new PreparationFindPath().PrintMap(preparationMapAltitude_ar, SizeMap);

        int wallObstacle = 1;
        UnitPlace.Path = findPath.findShortestPath(DestinationNode_Player, StartNode_Fiend,
            preparationMap_ar_ar, preparationMapAltitude_ar, wallObstacle, "manhattan", 10, 14);
        System.Diagnostics.Debug.WriteLine(DestinationNode_Player.ToString()+" ==== "+ StartNode_Fiend .ToString()+ " ====ko  = " + UnitPlace.Path.Count);
        UnityEngine.Debug.Log(" 03------------");
        foreach (var item in UnitPlace.Path)
        {
            System.Diagnostics.Debug.WriteLine( "Ad ----- Path =" + item.ToString() + "------ ------"  );
        }
     }
  */
    void DeployTree()
    {
        List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false).ToList();
        //int rnd = UnityEngine.Random.Range(0, openColumnList.Count);
        //TownPlace = openColumnList[rnd].Value.Position;
        Column column = this.townManager.GetRandomColumn(LandscapeDictionary,openColumnList);
        column.Tree = true;
    }
    /*
    void DeployUnit()
    {
        List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false && a.Value.Tree == false).ToList();
        
        Column column = this.townManager.GetRandomColumn(LandscapeDictionary,openColumnList);
        column.Unit = true;
        UnitPlace.Position = column.Position;
    }
    */
    
    public bool changeView = false;

    public void StepUpdateModel()
    {
        
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
                            this.townManager.DestroyDeployTown(LandscapeDictionary,checkColumn);
                        }
                        if (checkColumn.Tree)
                        {
                            checkColumn.Tree = false;
                        }
                        if (checkColumn.Unit)
                        {
                            checkColumn.Unit = false;
                        }
                        

                        changeView = true;

                    }
                }
            }
        }
        
        if (changeView)
        {
            this.unitManager.MoveUnit(LandscapeDictionary);
            /*
            var unitPoint = UnitPlace.GetNextPath();
            Column column = LandscapeDictionary[unitPoint.ToString()];
            column.Unit = true;
            */
        }
        
        if (ParamModel.LeakWaterOn)
        {

            LeakEvaporation leakEvaporation = new LeakEvaporation();

            if (leakEvaporation.LeakWater(LandscapeDictionary))
            {
                
                FontainCount++;
                FontainCount = FontainCount >= IndexFontainList.Count ? 0 : FontainCount;

                //LandscapeDictionary[IndexFontain.ToString()].DebugWater = true;
                leakEvaporation.LeakCube.Water -= 1;

                LandscapeDictionary[IndexFontainList[FontainCount].ToString()].Water += 1;
                
            }
        }
        

        foreach (var item in LandscapeDictionary)
        {
            item.Value.TurnMove = false;
        }
        
        //return changeView;
    }

}
