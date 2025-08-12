using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;
using Assets.Script2D.model;
using UnityEngine.UI;
using Assets.Script2D.controller;
using System;

public class ViewMain2D : MonoBehaviour
{
    public GameObject WaterColumn;
    public GameObject Town;
    public GameObject Tree;
    public GameObject Unit;

    List<GameObject> GraphicList;
    Dictionary<string, GameObject> GraphicDictionary;
    int xStart = -3;
    int yStart = -3;
    public FixedJoystick Joystick;
    public FixedJoystick JoystickRotate;
    public Camera MainCamera;
    Vector3 _target = new Vector3(10, 20, 0);
    public Text LeakWaterSumText;
    public Text AlluviumRandomText;
    public Text PrecipitationMudRandomText;
    Controller _controller;
    ModelMain3d _modelMain3d;

    public Toggle EditStone;
    public Toggle AddStone;

    private GameObject _realUnit;

    void Start()
    {
        this._modelMain3d = new ModelMain3d();
        _controller = new Controller(this._modelMain3d);

        this.GraphicList = new List<GameObject>();
        GraphicDictionary = new Dictionary<string, GameObject>();
        DrawWater();
        MainCamera.transform.LookAt(_target);

        LeakWaterSumText.text = "LeakWaterSum: " + ParamModel.LeakWaterSum.ToString();
        AlluviumRandomText.text = "AlluviumRandom: " + ParamModel.AlluviumRandom.ToString();
        PrecipitationMudRandomText.text= "PrecipitationMudRandom: " + ParamModel.PrecipitationMudRandom.ToString();

        EditStone.onValueChanged.AddListener(OnToggleEditStone);
        AddStone.onValueChanged.AddListener(OnToggleValueChanged);

        _realUnit = DrawnTownTree(this._modelMain3d.LandscapeDictionary[this._modelMain3d.GetRealUnit().RealUnitPathList[this._modelMain3d.GetRealUnit().Step].ToString()], Unit);

    }

    void OnToggleValueChanged(bool addStone)
    {

        _controller.AddStone = addStone;
    }
    void OnToggleEditStone(bool editStone)
    {

        _controller.EditStone = editStone;
    }

    void Update()
    {
        if (this._modelMain3d.StepUpdateModel())
        {
            RemoveWater();
            DrawWater();
            UpdateJoystick();
            UpdateRotateJoystick();
        }



    }
    WaterColumn GetChildColumn(GameObject waterStone) {
        var child = waterStone.transform.GetChild(0);
        WaterColumn waterColumn = child.GetComponent<WaterColumn>();
        return waterColumn;
    }

    void DrawWater()
    {
        foreach (var column in this._modelMain3d.LandscapeDictionary)
        {
            
            GameObject waterStone = Instantiate(WaterColumn, new Vector2(xStart + column.Value.Position.x, yStart), Quaternion.identity);
            waterStone.transform.localScale = new Vector3(1, column.Value.Stone, 1);
            waterStone.transform.position = new Vector3(xStart + column.Value.Position.x, yStart + (float)column.Value.Stone / 2, column.Value.Position.z);
            waterStone.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;

            WaterColumn waterColumn = GetChildColumn(waterStone);

            waterColumn.SetParam(column.Key, _controller);


            GraphicList.Add(waterStone);
            GraphicDictionary.Add(column.Key, waterStone);

            if (column.Value.Water > 0)
            {
                GameObject waterCube = Instantiate(WaterColumn, new Vector2(xStart + column.Value.Position.x, yStart), Quaternion.identity);
                waterCube.transform.localScale = new Vector3(1, column.Value.Water, 1);
                waterCube.transform.position = new Vector3(xStart + column.Value.Position.x, yStart + column.Value.Stone + (float)column.Value.Water / 2, column.Value.Position.z);

                if (column.Value.DebugWater)
                {

                    waterCube.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.yellow;
                }
                if (column.Value.Mud)
                {
                    waterCube.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.cyan;
                }
                if (this._modelMain3d.IndexFontainList.Where(a => a.ToString() == column.Key).Any())
                {
                    waterCube.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
                }
               
                WaterColumn water = waterCube.transform.GetChild(0).GetComponent<WaterColumn>();
                //water.Name = item.Value.Position.ToString();
                water.SetParam(column.Value.Position.ToString(), _controller);
                GraphicList.Add(waterCube);

            }
            if (column.Value.Town)
            {
                GraphicList.Add(DrawnTownTree(column.Value, Town));
            }
            if (column.Value.Tree)
            {
                GraphicList.Add(DrawnTownTree(column.Value, Tree));
            }
            if (column.Value.Unit)
            {
                GraphicList.Add(DrawnTownTree(column.Value, Unit));


            }
        }
        
        MoveRealUnit();

    }
    GameObject DrawnTownTree(Column column, GameObject TownPrefabs)
    {
        //Debug.Log(" = SS   key   lu = ");
        GameObject townTree = Instantiate(TownPrefabs, 
            new Vector3(xStart + this._modelMain3d.TownPlace.x,
            yStart + column.Stone + (float)column.Water / 2,
            column.Position.z),
            Quaternion.identity);
        return townTree;
    }
    void MoveRealUnit()
    {
        if (_realUnit == null)
        {
            return;
        }
        if(this._modelMain3d.GetRealUnit().RealUnitPathList.Count<= this._modelMain3d.GetRealUnit().Step)
        {
            return;
        }
        if (this._modelMain3d.GetRealUnit().Time<Time.time) {
            Debug.Log("###  L= -  -- " + this._modelMain3d.GetRealUnit().RealUnitPathList[this._modelMain3d.GetRealUnit().Step].ToString()+ "  Time.time ="+ Time.time+ "  GetRealUnit() = "+ this._modelMain3d.GetRealUnit().Step);
            Debug.Log("  XXXXX X   L== " + this._modelMain3d.GetRealUnit().RealUnitPathList.Count());
            _controller.StepUnit();
        }

        GameObject targetColumn = this.GraphicDictionary[this._modelMain3d.GetRealUnit().RealUnitPathList[this._modelMain3d.GetRealUnit().Step].ToString()];
        WaterColumn waterColumn = GetChildColumn(targetColumn);
        var column = this._modelMain3d.LandscapeDictionary[this._modelMain3d.GetRealUnit().RealUnitPathList[this._modelMain3d.GetRealUnit().Step].ToString()];

        float speedUnit = 0.1f;


            _realUnit.transform.position = Vector3.MoveTowards(
             new Vector3(_realUnit.transform.position.x,
                 _realUnit.transform.position.y,
                 _realUnit.transform.position.z),
             //targetColumn.transform.position
              new Vector3(targetColumn.transform.position.x, column.Stone, targetColumn.transform.position.z)


             ,
             speedUnit
             );


    }
    void RemoveWater()
    {
        foreach (var item in GraphicList)
        {
            Destroy(item);
        }
        GraphicDictionary.Clear();
        GraphicList.Clear();
    }

    void UpdateJoystick()
    {
        
        MainCamera.GetComponent<Camera>().transform.position = new Vector3(
        MainCamera.GetComponent<Camera>().transform.position.x + Joystick.Horizontal / 5,
        MainCamera.GetComponent<Camera>().transform.position.y + Joystick.Vertical / 5,
        MainCamera.GetComponent<Camera>().transform.position.z);
        
    }
    private Vector3 rotateValue;
    void UpdateRotateJoystick()
    {


        MainCamera.transform.RotateAround(_target, Vector3.up, JoystickRotate.Horizontal*20 * Time.deltaTime);
        MainCamera.transform.LookAt(_target);
    }

}