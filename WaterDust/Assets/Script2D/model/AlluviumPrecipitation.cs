using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script2D.model
{
    public class AlluviumPrecipitation
    {
        public bool AlluviumStone(Column itemValue)
        {
            if (itemValue.Mud == false)
            {
                if (1 >= itemValue.Water)
                {
                    var rand = new System.Random();
                    //int rnd = UnityEngine.Random.Range(0, ParamModel.AlluviumRandom);
                    int rnd = rand.Next(0, ParamModel.AlluviumRandom);
                    if (0 == rnd)
                    {
                        itemValue.Stone -= 1;
                    }
                    return true;
                }
            }
            return false;
        }
        public void PrecipitationMud(Column checkColumn)
        {
            if (checkColumn.Mud)
            {
                if (checkColumn.Water > 2)
                {
                    var rand = new System.Random();
                    //int rnd = UnityEngine.Random.Range(0, ParamModel.PrecipitationMudRandom);
                    int rnd = rand.Next(0, ParamModel.PrecipitationMudRandom);
                    if (0 == rnd)
                    {
                        checkColumn.Stone += 1;
                        checkColumn.Mud = false;
                    }
                }
            }
        }
    }
    
}
