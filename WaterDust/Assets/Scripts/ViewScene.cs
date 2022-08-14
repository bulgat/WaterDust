using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewScene : MonoBehaviour
{
    ModelSceneScr _modelSceneScr;
    List<GameObject> _CubeObjectViewList;
    public GameObject GiperCube;
    public bool ShowCubeWorld;

    // Start is called before the first frame update
    void Start()
    {
        _CubeObjectViewList = new List<GameObject>();

        _modelSceneScr = ModelSceneScr.GetInstanceModelSceneScr();


        if (ShowCubeWorld)
        {
            CreateCubeWorld();
        }
        
        

    }
    void CreateCubeWorld()
    {
        foreach (HyperCubeModel hyperCubeModel in _modelSceneScr._hyperCubeClassList)
        {
            GameObject cube = Instantiate(GiperCube, new Vector3(hyperCubeModel.GetMoveCube().X, hyperCubeModel.GetMoveCube().Y, hyperCubeModel.GetMoveCube().Z), Quaternion.identity);
            HyperCube hyperCube = cube.GetComponent<HyperCube>();
            if (hyperCubeModel.Water)
            {
                var cubeRenderer = cube.GetComponent<Renderer>();
                cubeRenderer.material.SetColor("_Color", Color.cyan);

            }
            hyperCube.Id = hyperCubeModel.Id;
            cube.name = "Cube_Id_" + hyperCubeModel.Id;
            _CubeObjectViewList.Add(cube);
           
        }

    }
    void DeleteCubeWorld()
    {

        foreach (var item in _CubeObjectViewList)
        {
            Destroy(item);
        }
        _CubeObjectViewList = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        if (ShowCubeWorld)
        {

            
            DeleteCubeWorld();
            CreateCubeWorld();
        }
    }
}
