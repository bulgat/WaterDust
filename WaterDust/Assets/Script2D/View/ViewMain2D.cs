using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;

public class ViewMain2D : MonoBehaviour
{
    public GameObject WaterColumn;
    public GameObject Town;

    List<GameObject> GraphicList;
    int xStart = -3;
    int yStart = -3;
    ModelMain3d modelMain3d;
    void Start()
    {
        this.modelMain3d = new ModelMain3d();
        this.modelMain3d.Start();
        this.GraphicList = new List<GameObject>();
        DrawWater();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.modelMain3d.StepUpdateModel())
        {
            RemoveWater();
            DrawWater();
        }
    }
    void DrawWater()
    {
        //int count = 0;
        foreach (var item in this.modelMain3d.LandscapeDictionary)
        {

            GameObject waterStone = Instantiate(WaterColumn, new Vector2(xStart + item.Value.Position.x, yStart), Quaternion.identity);
            waterStone.transform.localScale = new Vector3(1, item.Value.Stone, 1);
            waterStone.transform.position = new Vector3(xStart + item.Value.Position.x, yStart + (float)item.Value.Stone / 2, item.Value.Position.z);
            waterStone.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;

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
                if (this.modelMain3d.IndexFontainList.Where(a => a.ToString() == item.Key).Any())
                {
                    waterCube.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
                }
               
                WaterColumn water = waterCube.transform.GetChild(0).GetComponent<WaterColumn>();
                water.Name = item.Value.Position.ToString();
                GraphicList.Add(waterCube);

            }
            if (item.Value.Town)
            {
                DrawnTown(item.Value);
            }
        }
        
    }
    void DrawnTown(Column column)
    {
        //Column column = this.modelMain3d.LandscapeDictionary[this.modelMain3d.TownPlace.ToString()];
        GameObject town = Instantiate(Town, new Vector3(xStart + this.modelMain3d.TownPlace.x,
            yStart + column.Stone + (float)column.Water / 2, column.Position.z), Quaternion.identity);
        GraphicList.Add(town);
    }
    void RemoveWater()
    {
        foreach (var item in GraphicList)
        {
            Destroy(item);
        }
        GraphicList.Clear();
    }
}
