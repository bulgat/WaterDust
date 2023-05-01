using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script2D.model
{
    public class DebugPrint
    {
        public void PrintState(Column itemValue, Column checkColumn, List<Column> checkCubeList)
        {
            var parent = itemValue.GetSum();
            var child = checkColumn.GetSum();
            var kol = "";
            foreach (var i in checkCubeList)
            {
                kol += "_" + i.GetSum();
            }
            if (itemValue.DebugWater)
            {
                Debug.Log("position = " + itemValue.Position.ToString() + " water =" + itemValue.Water +
                    " stone =" + itemValue.Stone +
                    "  ap = " + checkColumn.Position.ToString() +
                              " =  count = " + checkCubeList.Count + " [" + kol + "] L parent "
                              + parent + " => " + child + " DebugWater = " + itemValue.DebugWater);
            }

        }
    }
}
