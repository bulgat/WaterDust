using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColumn : MonoBehaviour
{
    public string Name;
    void OnMouseDown()
    {
        Debug.Log("OnMouseDown = "+ Name);
    }
}
