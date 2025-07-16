using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;
using Assets.Script2D.model;
using UnityEngine.UI;
using Assets.Script2D.controller;

public class ViewMain2D : MonoBehaviour
{
    public GameObject WaterColumn;
    public GameObject Town;
    public GameObject Tree;
    public GameObject Unit;

    List<GameObject> GraphicList;
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

    void Start()
    {
        this._modelMain3d = new ModelMain3d();
        _controller = new Controller(this._modelMain3d);
        
        this._modelMain3d.Start();
        this.GraphicList = new List<GameObject>();
        DrawWater();
        MainCamera.transform.LookAt(_target);

        LeakWaterSumText.text = "LeakWaterSum: " + ParamModel.LeakWaterSum.ToString();
        AlluviumRandomText.text = "AlluviumRandom: " + ParamModel.AlluviumRandom.ToString();
        PrecipitationMudRandomText.text= "PrecipitationMudRandom: " + ParamModel.PrecipitationMudRandom.ToString();
    }

    // Update is called once per frame
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
    void DrawWater()
    {
        foreach (var item in this._modelMain3d.LandscapeDictionary)
        {
            
            GameObject waterStone = Instantiate(WaterColumn, new Vector2(xStart + item.Value.Position.x, yStart), Quaternion.identity);
            waterStone.transform.localScale = new Vector3(1, item.Value.Stone, 1);
            waterStone.transform.position = new Vector3(xStart + item.Value.Position.x, yStart + (float)item.Value.Stone / 2, item.Value.Position.z);
            waterStone.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;

            var child = waterStone.transform.GetChild(0);
            WaterColumn waterColumn = child.GetComponent<WaterColumn>();
            waterColumn.SetParam(item.Key, _controller);
            //Debug.Log("item =" + item.Key+ " child = "+ child+ " waterColumn ="+ waterColumn);

            GraphicList.Add(waterStone);

            if (item.Value.Water > 0)
            {
                GameObject waterCube = Instantiate(WaterColumn, new Vector2(xStart + item.Value.Position.x, yStart), Quaternion.identity);
                waterCube.transform.localScale = new Vector3(1, item.Value.Water, 1);

                waterCube.transform.position = new Vector3(xStart + item.Value.Position.x, yStart + item.Value.Stone + (float)item.Value.Water / 2, item.Value.Position.z);

                if (item.Value.DebugWater)
                {

                    waterCube.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.yellow;
                }
                if (item.Value.Mud)
                {
                    waterCube.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.cyan;
                }
                if (this._modelMain3d.IndexFontainList.Where(a => a.ToString() == item.Key).Any())
                {
                    waterCube.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
                }
               
                WaterColumn water = waterCube.transform.GetChild(0).GetComponent<WaterColumn>();
                //water.Name = item.Value.Position.ToString();
                water.SetParam(item.Value.Position.ToString(), _controller);
                GraphicList.Add(waterCube);

            }
            if (item.Value.Town)
            {
                DrawnTownTree(item.Value, Town);
            }
            if (item.Value.Tree)
            {
                DrawnTownTree(item.Value, Tree);
            }
            if (item.Value.Unit)
            {
                DrawnTownTree(item.Value, Unit);
            }
        }
        
    }
    void DrawnTownTree(Column column, GameObject TownPrefabs)
    {
        //Column column = this.modelMain3d.LandscapeDictionary[this.modelMain3d.TownPlace.ToString()];
        GameObject townTree = Instantiate(TownPrefabs, new Vector3(xStart + this._modelMain3d.TownPlace.x,
            yStart + column.Stone + (float)column.Water / 2, column.Position.z), Quaternion.identity);
        GraphicList.Add(townTree);
    }

    void RemoveWater()
    {
        foreach (var item in GraphicList)
        {
            Destroy(item);
        }
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