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

    public TownManager townManager;

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
    
 
        this.townManager.DeployTownList(LandscapeDictionary,2);
        DeployTree(15);
        this.unitManager.DeployUnit(LandscapeDictionary,this.SizeMap, 2);
       

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

    void DeployTree(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            List<KeyValuePair<string, Column>> openColumnList = LandscapeDictionary.Where(a => a.Value.Water == 0 && a.Value.Town == false).ToList();

            Column column = this.townManager.GetRandomColumn(LandscapeDictionary, openColumnList);
            column.Tree = true;
            UnityEngine.Debug.Log(" 01  "+ column.Position.ToString()+ "  --------" + column.Water);
        }
    }
  
    
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

    }

}
