using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script2D.controller
{
    public class Controller
    {
        ModelMain3d _modelMain3d;
        public Controller(ModelMain3d modelMain3d)
        {
            _modelMain3d = modelMain3d;
        }
        public void ClickWaterColumn(string key)
        {
             Debug.Log( " == o  = " + key);
            _modelMain3d.AddStoneColumn(key);
            //_modelMain3d
        }
    }
}
