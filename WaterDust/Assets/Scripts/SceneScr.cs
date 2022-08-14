using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SceneScr : MonoBehaviour
{
    public GameObject GiperCube;

    ModelSceneScr _modelSceneScr;

    public SceneScr() {
        
    }


    void Start()
    {

    }


    private GameObject GetCubeGameWithId(int Id)
    {
        /*
        foreach (var cube in _modelSceneScr.HyperCubeObjectList)
        {
            HyperCube hyperCubeItemCube = cube.GetComponent<HyperCube>();
            if (hyperCubeItemCube.Id == Id)
            {
                return cube;
            }
        }
        */
        return null;
    }


   


    private void ColorDownCube(int IdCube)
    {
        if (IdCube > 0)
        {
            GameObject colorCube = GetCubeGameWithId(IdCube);
            if (colorCube != null)
            {
                SetColor(colorCube, 1);
            }
        }
    }
    // UPDATE
    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    GameObject GetNeighboringCube(HyperCube hyperCubeItemCube)
    {
        //check left, right, forward, back

        for (var i = 0; i < checkSide.Length / checkSide.Rank; i++)
        {
            Key3D keySide = new Key3D((hyperCubeItemCube.GetMoveCube().X + checkSide[i, 0]), hyperCubeItemCube.GetMoveCube().Y, (hyperCubeItemCube.GetMoveCube().Z + checkSide[i, 1]));
            //List<HyperCube> neighCube_ar = _modelSceneScr.HyperCubeList.Where(a => a.GetMoveCube().GetName() == keySide.GetName()).ToList();
            List<HyperCube> neighCube_ar = null;

            var cubeNeight = _modelSceneScr._allCube_ar[keySide.X, keySide.Y, keySide.Z];

            //if (neighCube_ar.Count > 0)
            if (cubeNeight > 0)
            {
                foreach (GameObject item in _modelSceneScr.HyperCubeObjectList)
                {

                    HyperCube hyperCube = item.GetComponent<HyperCube>();
                    if (hyperCube.Water == true)
                    {
                        if (hyperCube == neighCube_ar.FirstOrDefault())
                        {

                            return item;
                        }
                    }
                }
            }
        }
        return null;
    }
    */
    GameObject GetNeighboringRandomCube(HyperCube hyperCubeItemCube)
    {
        // get all cube in floor Y and water.
        //List<HyperCube> oneLevel_ar = _modelSceneScr.HyperCubeList.Where(a => a.GetMoveCube().Y == hyperCubeItemCube.GetMoveCube().Y && a.Water == true).Where(a => a.GetMoveCube().GetName() != hyperCubeItemCube.GetMoveCube().GetName()).ToList();
        List<HyperCube> oneLevel_ar = null;
        if (oneLevel_ar.Count == 0)
        {
            return null;
        }
        int numberCube = (int)UnityEngine.Random.Range(0, oneLevel_ar.Count);
        Debug.Log(oneLevel_ar.Count + "   ***********" + hyperCubeItemCube.GetMoveCube().Y + "****" + hyperCubeItemCube.GetMoveCube().GetName() + "****= " + numberCube);
        HyperCube selectHyperCube = oneLevel_ar[numberCube];
        /*
        foreach (GameObject item in _modelSceneScr.HyperCubeObjectList)
        {

            HyperCube hyperCube = item.GetComponent<HyperCube>();
            //Debug.Log(numberCube + "   oneLevel_ar = " + oneLevel_ar.Count+"    name = "+ hyperCube.NameCube);
            if (hyperCube == selectHyperCube)
            {

                return item;
            }

        }
        */
        return null;
    }



    private void SetColor(GameObject itemCubeValue, int color)
    {

        var cubeRenderer = itemCubeValue.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        cubeRenderer.material.SetColor("_Color", Color.red);
        if (color > 0)
        {
            cubeRenderer.material.SetColor("_Color", Color.green);
        }
    }
    

}
