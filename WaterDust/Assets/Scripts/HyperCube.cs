using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperCube : MonoBehaviour
{
    [SerializeField]
    public int Id;
    public bool Water;
    
    public int Impulse;
    public bool YesPath;
    public Vector2 Attraction;
    public int _strategy;
    [SerializeField]
    public List<Point> PathList;
    public bool PrintPathList;
    public string NameCube;
    Key3D _key3DSide;

    void Start()
    {
        Impulse = ModelSceneScr.ImpulseStart;
        Attraction = Vector2.zero;
        PathList = null;
        //_keySideShift = new Key3D(0,0,0);
        name = "Cube_Id="+ Id;
    }

    // Update is called once per frame
    void Update()
    {
        if (new ModelSceneScr().GetModelSceneScr().RandomImpulse) {
            if (PathList == null)
            {
                if (Random.Range(0.0f, 10.0f) < 1)
                {
                    Impulse += 1;
                }
            }
        }
        if (PathList != null) {
            _strategy = PathList.Count;
            if (PrintPathList) {
                int count = 0;
                foreach(var item in PathList)
                {
                    print(Id+"    @   x=" + item.X + "_y=" + item.Y + "_count="+ count);
                    count++;
                }

            }
        }
        if (PrintPathList)
        {
            if (Impulse == 0 && PathList == null)
            {
                
                
            }
            new ModelSceneScr().GetModelSceneScr().PrintMap(Id,3);
            
        }
        YesPath = PathList != null ? true : false;
    }

    public void Refresh() {
        _key3DSide = new Key3D((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }

    public void SetMoveCube(Key3D keySideShift)
    {
        _key3DSide = keySideShift;
    }
    public Key3D GetMoveCube()
    {
       return  _key3DSide;
    }
    public int GetImpulse()
    {
        return Impulse;
    }
    public void SpendImpulse(int x) {
        Impulse += x;
    }
    public void SetAttraction(Vector2 Attract)
    {
        Attraction = Attract;
    }
    public void SetStrategy(Vector2 Attract)
    {
        /*
        if (_strategy == false) {
            _strategy = true;
        }
      */
    }
}
