using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorld : MonoBehaviour
{
    ModelSceneScr _modelSceneScr;
    void Start()
    {
        _modelSceneScr = ModelSceneScr.GetInstanceModelSceneScr();
    }

    void Update()
    {
        _modelSceneScr.Update();
    }
}
