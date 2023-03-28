using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;

public class ModelSceneScr
{
    private List<HyperCubeModel> _hyperCubeClassList;
    public int SizeWorld = 20;
    public int Yremove = 1;//-3
    static ModelSceneScr _modelSceneScr = null;
    public bool RandomImpulse = false;
    ModelList _cubeModelList;
    public static int ImpulseStart = 10;
    public bool PrintSteck;
    static int _floor = 2;
    int _maxFloor = _floor + 6;// + 6;
    public AltitudeMap _altitudeMap;
    public AltitudeMap _waterAltitudeMap;
    System.Random rand = new System.Random();
    public List<HyperCubeModel> GetHyperCubeClassList()
    {
        return new List<HyperCubeModel>(_hyperCubeClassList);
    }

    public static ModelSceneScr GetInstanceModelSceneScr()
    {
        if (_modelSceneScr == null)
        {
            _modelSceneScr = new ModelSceneScr().Init();
        }
        return _modelSceneScr;
    }

    Dictionary<Key3D, GameObject> _allCube_Dic;
    int indexCubeIncr;
    DictionarySpecialComparer dictionarySpecialComparer = new DictionarySpecialComparer();
    public ModelSceneScr Init()
    {
        _modelSceneScr = new ModelSceneScr();
        _cubeModelList = new ModelList();

        _altitudeMap = new AltitudeMap();
        _waterAltitudeMap = new AltitudeMap();

        _hyperCubeClassList = new List<HyperCubeModel>();



        _allCube_Dic = new Dictionary<Key3D, GameObject>(dictionarySpecialComparer);
        _cubeModelList.InitAllCube(_modelSceneScr.SizeWorld, _maxFloor, _modelSceneScr.SizeWorld);

        for (int i = 0; i < 10; i++)
        {

            for (int z = 0; z < 10; z++)
            {
                for (int y = _floor; y < _maxFloor; y++)
                {
                    bool water = false;
                    if (y >= 2 + _floor && i != 0 && z != 0 && i != 9)
                    {

                        water = true;
                    }

                    HyperCubeModel hyperCubeModel = GetNewCube(water, i, y, z);

                    // create water


                    _hyperCubeClassList.Add(hyperCubeModel);

                    _cubeModelList.SetCube(hyperCubeModel.GetMoveCube().X, hyperCubeModel.GetMoveCube().Y, hyperCubeModel.GetMoveCube().Z, hyperCubeModel.Id);

                }
            }
        }


        // delete
        Key3D keyDel3D = new Key3D(3, (_floor), 3);

        HyperCubeModel t = GetCubeGameObject(keyDel3D);

        RemoveCube(t);


        _altitudeMap.Init(SizeWorld, SizeWorld);
        _waterAltitudeMap.Init(SizeWorld, SizeWorld);


        //TaskUpdate(this);
        ThreadStart writeSecond = new ThreadStart(WriteSecond);
        Thread thread = new Thread(WriteSecond);
        thread.Start();

        return this;
    }
    public static void TaskUpdate(ModelSceneScr modelSceneScr)
    {
        Task.Delay(3000);
        Task.Run(() => {
            while (true)
            {
                modelSceneScr.Update();
                Debug.Log(" @@@@@=======  move down active stone =  ");
            }
        });

    }
    static void WriteSecond()
    {
        Task.Delay(100);
        while (true)
        {
            _modelSceneScr.Update();
            //Thread.Sleep(10000);
            System.Diagnostics.Debug.WriteLine("-  по дороге = " + Thread.CurrentThread.GetHashCode());
        }
    }
    public void Update()
    {

        _removeKeyList = new List<Key3D>();
        List<Key3D> removeKeyDestroyList = new List<Key3D>();
        List<HyperCubeModel> addHyperCubeModelList = new List<HyperCubeModel>();

        foreach (HyperCubeModel hyperCubeItemCube in _hyperCubeClassList)
        {

            if (hyperCubeItemCube.Water)
            {

                if (MoveDownCube(hyperCubeItemCube, addHyperCubeModelList, false))
                {
                    continue;
                }

                //check left, right, forward, back
                if (hyperCubeItemCube.GetImpulse() >= 0)
                {
                    Key3D keySide = GetCubeSpaceEmpty(hyperCubeItemCube, true);
                    if (keySide != null)
                    {
                        //Debug.Log("Move SideCube id =" + hyperCubeItemCube.Id + "  =[" + hyperCubeItemCube.GetMoveCube().X + "," + hyperCubeItemCube.GetMoveCube().Y + "," + hyperCubeItemCube.GetMoveCube().Z + "]==   ( x =" + keySide.X + " y=" + keySide.Y + " z=" + keySide.Z + " )===  =");
                        // Move
                        new MoveCubePhysicRealModel().MoveCubePhysicReal(_cubeModelList, hyperCubeItemCube, keySide.X, keySide.Y, keySide.Z);

                        // break;
                        continue;
                    }
                }

                if (hyperCubeItemCube.GetImpulse() <= 0)
                {
                    if (hyperCubeItemCube.PathList != null)
                    {
                        if (hyperCubeItemCube.PathList.Count > 0)
                        {

                            MoveCubePathFind(hyperCubeItemCube, addHyperCubeModelList);

                        }
                        else
                        {
                            hyperCubeItemCube.PathList = null;
                        }
                    }
                }
                if (hyperCubeItemCube.GetImpulse() <= 0)
                {
                    bool newCountPath = false;
                    if (hyperCubeItemCube.PathList == null)
                    {
                        newCountPath = true;
                    }

                    if (hyperCubeItemCube.PathList != null)
                    {
                        if (hyperCubeItemCube.PathList.Count == 0)
                        {
                            newCountPath = true;
                        }
                    }

                    if (newCountPath)
                    {
                        List<Point> pathList = SetPathInCube(hyperCubeItemCube, null);


                        if (pathList != null)
                        {


                            // Move with path
                            Key3D idRemoveCubeStream = MoveCubePathFind(hyperCubeItemCube, addHyperCubeModelList);
                            if (idRemoveCubeStream != null)
                            {
                                //Key3D keyDown = new Key3D(hyperCubeItemCube.GetMoveCube().X, hyperCubeItemCube.GetMoveCube().Y - 1, hyperCubeItemCube.GetMoveCube().Z);
                                Debug.Log(" VE idRemoveCubeStream = " + idRemoveCubeStream);
                                HyperCubeModel removeCube = GetCubeGameObject(idRemoveCubeStream);
                                if (removeCube != null)
                                {
                                    RemoveCube(removeCube);
                                    hyperCubeItemCube.TakeStone = true;

                                }
                                //_removeFlowDict.Add();
                                ////RemoveCubeOutWorld();
                                break;
                                //return;
                            }
                        }
                    }

                }


            }
            else
            {
                //move down active stone
                if (hyperCubeItemCube.ActiveStone)
                {
                    //Debug.Log("В _ id = " + hyperCubeItemCube.Id + "____  Set ActiveStone = ");
                    if (MoveDownCube(hyperCubeItemCube, addHyperCubeModelList, true))
                    {
                        
                        continue;
                    }
                    else
                    {
                        // hyperCubeItemCube.ActiveStone = false;
                    }
                }
            }

            //remove super down cube.

        }

        //remove
        RemoveCubeOutWorld();

        //add  HyperCubeModelList
        foreach (var itemHyperCubeModel in addHyperCubeModelList)
        {
            _hyperCubeClassList.Add(itemHyperCubeModel);

            _cubeModelList.SetCube(itemHyperCubeModel.GetMoveCube().X, itemHyperCubeModel.GetMoveCube().Y, itemHyperCubeModel.GetMoveCube().Z, itemHyperCubeModel.Id);
        }

        PrintSteckUpdate(PrintSteck);
        CreateMap3Dtop();
    }

    HyperCubeModel SetStoneRandom(HyperCubeModel hyperCubeItemCube)
    {
        Key3D keySide = GetCubeSpaceEmpty(hyperCubeItemCube, true);

        if (keySide == null)
        {
            return null;
        }
        //set stoun cast cube


        HyperCubeModel hyperCubeModel = GetNewCube(false, keySide.X, keySide.Y, keySide.Z);


        return hyperCubeModel;
    }


    private HyperCubeModel GetNewCube(bool Water, int x, int y, int z)
    {
        indexCubeIncr++;

        HyperCubeModel hyperCubeModel = new HyperCubeModel();
        hyperCubeModel.Id = indexCubeIncr;

        hyperCubeModel.Water = Water;

        Key3D key3D = new Key3D(x, y, z);

        hyperCubeModel.SetMoveCube(key3D);

        return hyperCubeModel;
    }


    List<Key3D> _removeKeyList;
    int[,] checkSide = new[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
    private Key3D GetIdDownCube(HyperCubeModel hyperCubeItemCube)
    {
        Key3D keyDown = new Key3D(hyperCubeItemCube.GetMoveCube().X, hyperCubeItemCube.GetMoveCube().Y - 1, hyperCubeItemCube.GetMoveCube().Z);
        // move down.
        return keyDown;


        //return _cubeModelList.GetCube(keyDown.X, keyDown.Y, keyDown.Z);
    }
    private void SendAttractionDownImpulse(Key3D sub_ar)
    {
        // send attraction
        for (var i = 0; i < checkSide.Length / checkSide.Rank; i++)
        {
            if ((Convert.ToInt32(sub_ar.X) + checkSide[i, 0]) >= 0 && (Convert.ToInt32(sub_ar.Z) + checkSide[i, 1]) >= 0)
            {

                Key3D keySide = new Key3D(Convert.ToInt32(sub_ar.X) + checkSide[i, 0], Convert.ToInt32(sub_ar.Y), Convert.ToInt32(sub_ar.Z) + checkSide[i, 1]);

            }
        }
    }


    bool MoveDownCube(HyperCubeModel hyperCubeItemCube, List<HyperCubeModel> addHyperCubeModelList, bool Stone)
    {
        Key3D downCube = GetIdDownCube(hyperCubeItemCube);
        int downCubeId = _cubeModelList.GetCube(downCube.X, downCube.Y, downCube.Z);


        Key3D keyDown = new Key3D(hyperCubeItemCube.GetMoveCube().X, hyperCubeItemCube.GetMoveCube().Y - 1, hyperCubeItemCube.GetMoveCube().Z);


        if (downCubeId == 0)
        {


            //Key3D keyDown = new Key3D(hyperCubeItemCube.GetMoveCube().X, hyperCubeItemCube.GetMoveCube().Y - 1, hyperCubeItemCube.GetMoveCube().Z);

            SendAttractionDownImpulse(keyDown);

            //Debug.Log("ВНИЗ Id = " + hyperCubeItemCube.Id + " cube downCube id=" + downCube + " +++ key cube = "+ hyperCubeItemCube.GetMoveCube().X+","+ hyperCubeItemCube.GetMoveCube().Y+"," + hyperCubeItemCube.GetMoveCube().Z + "       DOWN POSITION  x= " + keyDown.X+","+ keyDown.Y+","+ keyDown.Z);

            hyperCubeItemCube.SpendImpulse(60);


            new MoveCubePhysicRealModel().MoveCubePhysicReal(_cubeModelList, hyperCubeItemCube, keyDown.X, keyDown.Y, keyDown.Z);

            hyperCubeItemCube.PathList = null;

            //cast stone cube
            if (hyperCubeItemCube.TakeStone)
            {
                Debug.Log("================ 00   == =");
                HyperCubeModel hyperCubeModel = SetStoneRandom(hyperCubeItemCube);

                if (hyperCubeModel == null)
                {

                    //continue;
                    return true;
                }
                //hyperCubeModel.ActiveStone = false;
                hyperCubeModel.ActiveStone = true;

                //set stoun cast cube
                addHyperCubeModelList.Add(hyperCubeModel);

                hyperCubeItemCube.TakeStone = false;
            }

            //continue;
            return true;
        }
        else
        {
            if (Stone)
            {
                if (downCubeId != 0)
                {

                    HyperCubeModel hyperCubeWater = GetCubeWithId(downCubeId);
                    if (hyperCubeWater.Water)
                    {
                        Debug.Log("Stone   " + hyperCubeItemCube.Id + " id      downCube=  " + downCube + "  waterCube  y= " + hyperCubeItemCube.GetMoveCube().Y);

                        Key3D keyUpWater = new Key3D(hyperCubeItemCube.GetMoveCube().X, hyperCubeItemCube.GetMoveCube().Y, hyperCubeItemCube.GetMoveCube().Z);

                        //water cube up
                        new MoveCubePhysicRealModel().MoveCubePhysicReal(_cubeModelList, hyperCubeWater, keyUpWater.X, keyUpWater.Y, keyUpWater.Z);

                        new MoveCubePhysicRealModel().MoveCubePhysicReal(_cubeModelList, hyperCubeItemCube, keyDown.X, keyDown.Y, keyDown.Z);

                    }
                }

            }
        }

        return false;

    }








    private Key3D MoveCubePathFind(HyperCubeModel itemCubeValue, List<HyperCubeModel> addHyperCubeModelList)
    {
        Key3D itemCubeKey = itemCubeValue.GetMoveCube();


        // Move cube.
        if (itemCubeValue.PathList != null)
        {
            if (itemCubeValue.PathList.Count > 0)
            {

                if (_modelSceneScr.AllowMoveCube((int)itemCubeValue.PathList[itemCubeValue.PathList.Count - 1].X, (int)itemCubeValue.PathList[itemCubeValue.PathList.Count - 1].Y))
                {
                    itemCubeValue = new MoveCubePhysicRealModel().MoveCubePhysicReal(_cubeModelList, itemCubeValue,
                            itemCubeValue.PathList[0].X,
                            itemCubeValue.GetMoveCube().Y,// .transform.position.y,
                            itemCubeValue.PathList[0].Y);

                    itemCubeValue.PathList.RemoveAt(0);

                    //SetColor(itemCubeValue, 0);

                    // Take Stone
                    if (itemCubeValue.TakeStone == false)
                    {
                        //random Take Stone
                        //float randomNum = Mathf.Floor(UnityEngine.Random.Range(0f, 5.0f));
                        float randomNum = rand.Next(0, 5);
                        if (randomNum == 0)
                        {
                            //float randomSelectNum = Mathf.Floor(UnityEngine.Random.Range(0f, 2.0f));
                            float randomSelectNum = rand.Next(0,2);
                            Key3D idRemoveCube = null;

                            if (randomSelectNum == 0)
                            {
                                idRemoveCube = GetIdDownCube(itemCubeValue);
                                //idRemoveCube = _cubeModelList.GetCube(removeCubeKey3D.X, removeCubeKey3D.Y, removeCubeKey3D.Z);
                            }
                            else
                            {
                                // take side stone
                                idRemoveCube = GetCubeSpaceEmpty(itemCubeValue, false);
                                //if (keySide != null)
                                //{
                                //idRemoveCube = _cubeModelList.GetCube(keySide.X, keySide.Y, keySide.Z);
                                Debug.Log(" Si  idRemoveCube =" + idRemoveCube + ", === x =");

                                //}
                            }
                            return idRemoveCube;
                        }

                    }

                    // Cast stone

                    if (itemCubeValue.TakeStone)
                    {
                        //Random cast stone
                        //float randomNum = Mathf.Floor(UnityEngine.Random.Range(0f, 5.0f));
                        float randomNum = rand.Next(0,5);
                        if (randomNum == 0)
                        {
                            HyperCubeModel hyperCubeModel = SetStoneRandom(itemCubeValue);

                            if (hyperCubeModel == null)
                            {

                            }
                            else
                            {
                                hyperCubeModel.ActiveStone = true;
                                addHyperCubeModelList.Add(hyperCubeModel);

                                itemCubeValue.TakeStone = false;
                            }


                        }
                    }


                    if (itemCubeValue.PathList.Count == 0)
                    {
                        itemCubeValue.PathList = null;
                    }

                }

            }
        }
        return null;
    }


    private List<Point> SetPathInCube(HyperCubeModel WaterCube, HyperCubeModel itemCubeCenterKey)
    {
        bool nullCube = false;
        Key3D keyRandom = null;
        if (itemCubeCenterKey == null)
        {

            // flow random
            // big operation
            keyRandom = GetRandomCube(WaterCube, WaterCube.GetMoveCube().Y);

            nullCube = true;
        }

        Key3D itemCubeKey = null;
        if (itemCubeCenterKey == null)
        {
            itemCubeKey = keyRandom;
        }
        else
        {
            HyperCubeModel itemCubeCenterHyperCubeKey = itemCubeCenterKey;
            itemCubeKey = itemCubeCenterHyperCubeKey.GetMoveCube();
        }

        _modelSceneScr.GetCubeWithKey(itemCubeKey);

        if (itemCubeKey != WaterCube.GetMoveCube())
        {
            if (nullCube == false)
            {

                //SetColor(WaterCube, 0);
                WaterCube.PathList = GetPathMoveCube(WaterCube, itemCubeKey);

                return WaterCube.PathList;

            }
            //SetColor(WaterCube, 0);
            WaterCube.PathList = GetPathMoveCube(WaterCube, itemCubeKey);


            return WaterCube.PathList;


        }

        return null;
    }

    private Key3D GetRandomCube(HyperCubeModel waterCube, int LevelY)
    {
        Grid grid = _modelSceneScr.GetRandomPreparationMap(waterCube, LevelY);
        Key3D keySide = new Key3D(grid.SpotX, grid.SpotY, grid.SpotZ);

        return keySide;

    }

    private Key3D GetCubeSpaceEmpty(HyperCubeModel hyperCubeItemCube, bool EmptyPlace)
    {
        //check left, right, forward, back

        for (var i = 0; i < checkSide.Length / checkSide.Rank; i++)
        {
            Key3D keySide = new Key3D((hyperCubeItemCube.GetMoveCube().X + checkSide[i, 0]), hyperCubeItemCube.GetMoveCube().Y, (hyperCubeItemCube.GetMoveCube().Z + checkSide[i, 1]));

            var sideCube = _cubeModelList.GetCube(keySide.X, keySide.Y, keySide.Z);

            if (EmptyPlace)
            {
                if (sideCube == 0)
                {

                    if (_modelSceneScr.AllowMoveCube(keySide.X, keySide.Z))
                    {

                        return keySide;

                    }
                }
            }
            else
            {
                if (sideCube > 0)
                {
                    if (_modelSceneScr.AllowMoveCube(keySide.X, keySide.Z))
                    {
                        return keySide;
                    }
                }
            }
        }
        return null;
    }

    private void RemoveCubeOutWorld()
    {
        List<HyperCubeModel> removeKeyDestList = DeleteOutWorld();
        foreach (HyperCubeModel itemCube in removeKeyDestList)
        {

            RemoveCube(itemCube);

        }
    }

    private void CreateMap3Dtop()
    {
        // int[,] altitudeMap_ar = new int[SizeWorld, SizeWorld];

        for (int GridX = 0; GridX < SizeWorld; GridX++)
        {

            for (int GridZ = 0; GridZ < SizeWorld; GridZ++)
            {
                bool water = false;
                for (int GridY = _maxFloor - 1; GridY >= _floor; GridY--)
                {
                    //Debug.Log("i   x  = " + GridX + "  T AL  y= " + GridY + " z = " + GridZ + "  ubeKol =" );
                    var quadId = _cubeModelList.GetCube(GridX, GridY, GridZ);
                    //altitudeMap_ar[GridX, GridZ] = GridY;
                    _altitudeMap.SetAltutude(GridX, GridZ, GridY);


                    if (quadId > 0)
                    {
                        //HyperCubeModel hyperCube = _hyperCubeClassList.Where(a => a.Id == quadId).FirstOrDefault();
                        HyperCubeModel hyperCube = GetCubeWithId(quadId);
                        if (hyperCube.Water)
                        {
                            // Water cube
                            if (water == false)
                            {
                                _waterAltitudeMap.SetAltutude(GridX, GridZ, GridY);
                                water = true;
                            }
                        }
                        else
                        {
                            if (water == false)
                            {
                                _waterAltitudeMap.SetAltutude(GridX, GridZ, GridY);
                            }
                            _altitudeMap.SetAltutude(GridX, GridZ, GridY);
                            break;
                        }

                        //break;
                    }
                    if (GridY == _floor)
                    {
                        _waterAltitudeMap.SetAltutude(GridX, GridZ, _floor - 1);
                        _altitudeMap.SetAltutude(GridX, GridZ, _floor - 1);
                        // Debug.Log("quadId = "+quadId +"  -   LET   Y =  " + GridY);
                    }
                }

            }



        }

    }
    private HyperCubeModel GetCubeWithId(int quadId)
    {
        return _hyperCubeClassList.Where(a => a.Id == quadId).FirstOrDefault();
    }
    public void RemoveCube(HyperCubeModel hyperCubeItemCube)
    {
        _cubeModelList.SetCube(hyperCubeItemCube.GetMoveCube().X, hyperCubeItemCube.GetMoveCube().Y, hyperCubeItemCube.GetMoveCube().Z, 0);
        _hyperCubeClassList.Remove(hyperCubeItemCube);

    }
    private HyperCubeModel GetCubeGameObject(Key3D HCube)
    {
        foreach (var hyperCubeItemCube in _hyperCubeClassList)
        {
            if (hyperCubeItemCube.GetMoveCube().GetName() == HCube.GetName())
            {
                return hyperCubeItemCube;
            }
        }
        return null;
    }



    public ModelSceneScr GetModelSceneScr()
    {

        return _modelSceneScr;
    }

    public List<long[]> GetPreparationMap(int LevelY)
    {
        List<long[]> CreateMap_ar = new List<long[]>();

        for (int GridRow = 0; GridRow < SizeWorld; GridRow++)
        {

            long[] arrayLong = new long[SizeWorld];

            CreateMap_ar.Add(arrayLong);

            for (int GridLine = 0; GridLine < SizeWorld; GridLine++)
            {
                Key3D newKey = new Key3D(GridRow, LevelY, GridLine);
                //Debug.Log((_cubeModelList==null)+" %%%%%%%%%%%%% " + GridLine + "   key =  " + newKey.X+"," +newKey.Y+","+ newKey.Z);

                int quadCube = _cubeModelList.GetCube(newKey.X, newKey.Y, newKey.Z);

                //int quad = quad_ar.Count > 0 ? 1 : 0;
                int quad = quadCube > 0 ? 1 : 0;

                CreateMap_ar[GridRow][GridLine] = quad;
            }



        }

        return CreateMap_ar;
    }

    public Grid GetRandomPreparationMap(HyperCubeModel waterCube, int LevelY)
    {
        List<long[]> preparationMapUp = GetPreparationMap(LevelY);
        List<long[]> preparationMapDown = GetPreparationMap(LevelY - 1);

        var gridUp_ar = GetGridList(preparationMapUp, LevelY);
        var gridDown_ar = GetGridList(preparationMapDown, LevelY - 1);

        List<Grid> gridClearList = gridUp_ar.Except(gridDown_ar, new ProductComparer()).ToList();


        List<Grid> AllPlace_ar = gridClearList.Where(a => a.Wall == false).ToList();
        List<Grid> randomPlace_ar = null;
        //random
        List<Grid> distanceCube_ar = GetNitherCube(waterCube, AllPlace_ar);
        if (distanceCube_ar.Count > 0)
        {
            randomPlace_ar = distanceCube_ar;
        }
        else
        {
            randomPlace_ar = AllPlace_ar;
        }

        //int numberCube = (int)UnityEngine.Random.Range(0, randomPlace_ar.Count);
        int numberCube = rand.Next(0, randomPlace_ar.Count);
        Grid randomPlace = randomPlace_ar[numberCube];
        return randomPlace;
    }
    private List<Grid> GetNitherCube(HyperCubeModel waterCube, List<Grid> randomPlace_ar)
    {
        if (randomPlace_ar == null)
        {
            return new List<Grid>();
        }
        int distanceStat = 4;
        List<Grid> distanceCube_ar = new List<Grid>();

        foreach (Grid itemGrid in randomPlace_ar)
        {
            float distance = Vector3.Distance(new Vector3(waterCube.GetMoveCube().X, waterCube.GetMoveCube().Y, waterCube.GetMoveCube().Z), new Vector3(itemGrid.SpotX, itemGrid.SpotY, itemGrid.SpotZ));
            if (distance < distanceStat)
            {
                distanceCube_ar.Add(itemGrid);
            }
        }
        //Debug.Log("  = - -   randomPlace_ar =  "+ distanceCube_ar.Count);
        return distanceCube_ar;
    }
    private List<Grid> GetGridList(List<long[]> preparationMapUp, int LevelY)
    {
        List<Grid> grid_ar = new List<Grid>();
        for (int GridRow = 0; GridRow < new ModelSceneScr().GetModelSceneScr().SizeWorld; GridRow++)
        {

            for (int GridLine = 0; GridLine < new ModelSceneScr().GetModelSceneScr().SizeWorld; GridLine++)
            {
                // 0 - not nothing
                // 1 is cube (claim or water)
                bool wall = (preparationMapUp[GridRow][GridLine] == 0 ? false : true);
                Grid grid = new Grid(GridRow, LevelY, GridLine, wall);
                grid_ar.Add(grid);
            }

        }
        return grid_ar;
    }

    public void PrintMap(int Id, int floorMap)
    {
        List<long[]> createMap_ar = new ModelSceneScr().GetModelSceneScr().GetPreparationMap(floorMap);
        PrintMapCard(createMap_ar, Id);
    }
    public void PrintMapCard(List<long[]> createMap_ar, int Id)
    {
        Debug.Log(Id + " ------------------------------------------------- " + new ModelSceneScr().GetModelSceneScr().SizeWorld);
        for (int GridRow = 0; GridRow < new ModelSceneScr().GetModelSceneScr().SizeWorld; GridRow++)
        {
            string line = "_";
            for (int GridLine = 0; GridLine < new ModelSceneScr().GetModelSceneScr().SizeWorld; GridLine++)
            {
                line += (createMap_ar[GridRow][GridLine] == 0 ? "00" : "77") + ".";
            }
            Debug.Log(" PRINT Imp ==" + GridRow + " IIIIIIIII = " + line);
        }

        Debug.Log(Id + " ------------------------------------------------- ");
    }
    public bool AllowMoveCube(int x, int y)
    {
        if (x >= 0 && y >= 0)
        {
            if (x <= _modelSceneScr.SizeWorld && y <= _modelSceneScr.SizeWorld)
            {
                return true;
            }
        }
        return false;
    }
    public List<Point> GetPathList(HyperCubeModel WaterCube, Key3D waterCubeEnd, FindPath findPath)
    {
        long DestinationNode_ID_Player = ((int)(WaterCube.GetMoveCube().Z) * 100) + (int)(WaterCube.GetMoveCube().X);
        long StartNode_ID_Fiend = ((int)waterCubeEnd.Z * 100) + (int)waterCubeEnd.X;

        List<long[]> preparationMap_ar_ar = _modelSceneScr.GetPreparationMap(waterCubeEnd.Y);
        preparationMap_ar_ar[(int)WaterCube.GetMoveCube().X][(int)WaterCube.GetMoveCube().Z] = 0;
        preparationMap_ar_ar[(int)waterCubeEnd.X][(int)waterCubeEnd.Z] = 0;

        int wallObstacle = 1;
        List<Point> pathList = ConvertToPoint(findPath.findShortestPath(DestinationNode_ID_Player, StartNode_ID_Fiend, preparationMap_ar_ar, wallObstacle, "manhattan", 10, 14));

        if (pathList.Count == 0)
        {
            //new ModelSceneScr().GetModelSceneScr().PrintMapCard(preparationMap_ar_ar, (int)WaterCube.GetMoveCube().Y);
        }
        return pathList;
    }
    List<Point> ConvertToPoint(List<SuperNode> PathList)
    {
        List<Point> pointList = new List<Point>();
        foreach (SuperNode item in PathList)
        {
            pointList.Add(new Point((item.id % 100), (item.id / 100)));
        }
        return pointList;
    }
    public void PrintSteckUpdate(bool PrintSteck)
    {
        if (PrintSteck)
        {
            Debug.Log("print ----------------------------------------------------------------");
            /*
            foreach (GameObject itemCube in _modelSceneScr.HyperCubeObjectList)
            {
                HyperCube hyperCube = itemCube.GetComponent<HyperCube>();
                
            }
            */
            Debug.Log("print ----------------------------------------------------------------");
        }
    }
    public HyperCubeModel GetCubeWithKey(Key3D itemCubeKey)
    {
        foreach (HyperCubeModel itemKey in _hyperCubeClassList)
        {
            if (itemCubeKey == itemKey.GetMoveCube())
            {
                return itemKey;
            }
        }
        return null;
    }
    public List<HyperCubeModel> DeleteOutWorld()
    {
        List<HyperCubeModel> removeKeyDestList = new List<HyperCubeModel>();
        foreach (HyperCubeModel itemCube in _hyperCubeClassList)
        {

            if (itemCube.GetMoveCube().Y <= Yremove)
            {
                // Debug.Log(itemCube.GetMoveCube().Y +" print id ="+ itemCube .Id+ " DeleteOutWorld" );
                removeKeyDestList.Add(itemCube);

            }

        }

        return removeKeyDestList;
    }
    public List<Point> GetPathMoveCube(HyperCubeModel WaterCube, Key3D itemKeyEnd)
    {

        FindPath findPath = new FindPath();


        //_sizeWorld
        if (_modelSceneScr.AllowMoveCube((int)WaterCube.GetMoveCube().X, (int)WaterCube.GetMoveCube().Z))
        {
            List<Point> pathList = GetPathList(WaterCube, itemKeyEnd, findPath);


            if (pathList.Count > 0)
            {
                //Debug.Log("Slave pathList.L= " + pathList.Count + " player One Cu  z  Two Cube  x= " + itemKeyEnd.X + " z = " + itemKeyEnd.Z + "  PreparationMap L =");
                return pathList;
            }
            else
            {

                //Debug.Log("NO PATH slave pathList.L= " + pathList.Count + " player One     Two Cube  x= " + itemKeyEnd.X + " z = " + itemKeyEnd.Z + "  PreparationMap L =");
            }


        }

        return null;
    }
}
class ProductComparer : IEqualityComparer<Grid>
{
    public bool Equals(Grid top, Grid down)
    {
        if (top.SpotX == down.SpotX && top.SpotZ == down.SpotZ)
        {
            if (down.Wall)
            {
                return true;
            }

        }
        return false;
    }
    public int GetHashCode(Grid product)
    {

        return 0;
    }

}