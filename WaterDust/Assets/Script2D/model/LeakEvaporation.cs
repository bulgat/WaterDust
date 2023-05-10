using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script2D.model
{
    public class LeakEvaporation
    {
        public Column LeakCube;
        public bool LeakWater(Dictionary<string, Column> LandscapeDictionary)
        {
            
            int sumWater = LandscapeDictionary.Values.ToList().Sum(a => a.Water);

            if (ParamModel.LeakWaterSum < sumWater)
            {

                List<Column> list = LandscapeDictionary.Values.Where(a => a.Water > 0).OrderBy(a => a.Stone).OrderByDescending(a => a.Water).ToList();

                var rand = new System.Random();
                // int rnd = UnityEngine.Random.Range(0, list.Count);
                int rnd = rand.Next(0, list.Count);
                
                LeakCube = list[rnd];
                return true;
            }

            return false;
        }
    }
}
