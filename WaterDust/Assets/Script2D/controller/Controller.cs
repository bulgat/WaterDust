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
        public bool AddStone = true;
        public bool EditStone = true;
        public Controller(ModelMain3d modelMain3d)
        {
            _modelMain3d = modelMain3d;
        }
        public void ClickWaterColumn(string key)
        {
            if (EditStone)
            {
                _modelMain3d.AddStoneColumn(key, AddStone);
            } else
            {
                Debug.Log("@@@N UnitPath  ");
                _modelMain3d.SetPlaceColumn(key);
            }
        }
        public void StepUnit()
        {
            _modelMain3d.StepUnit();
        }
    }
}
