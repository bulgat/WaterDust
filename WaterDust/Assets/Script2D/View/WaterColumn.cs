using Assets.Script2D.controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColumn : MonoBehaviour
{
    string Name;
    string KeyName;
    Controller _controller;
    public void SetParam(string keyName, Controller controller)
    {
        this.KeyName = keyName;
        _controller = controller;
    }
    void OnMouseDown()
    {
        Debug.Log("OnMouseDown = "+ Name + " KeyName = "+ KeyName);
        _controller.ClickWaterColumn(KeyName);
    }
}
