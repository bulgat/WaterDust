using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script2D.model
{
    public class ManagerColumn
    {
        public Column GetColumn(Column columnParent, List<Column> gradeList)
        {

            if (columnParent.VectorForce > 0)
            {
                if (columnParent.VectorInertia != null)
                {
                    Column child = gradeList.Where(a => a.Position.ToString() == columnParent.VectorInertia.ToString()).FirstOrDefault();
                    if (child != null)
                    {
                        return child;
                    }
                }
            }

            if (0 == UnityEngine.Random.Range(0, 12))
            {
                int rnd = UnityEngine.Random.Range(0, gradeList.Count);
                return gradeList[rnd];
            }
            return gradeList.First();
        }
        public List<Column> GradeColumnList(Dictionary<string, Column> LandscapeDictionary,Column ParentItem)
        {

            List<Column> gradeList = new List<Column>();
            foreach (Point2D check in ParamInner.CheckCubeList)
            {
                Point2D checkCount = new Point2D(ParentItem.Position.x + check.x, ParentItem.Position.z + check.z);
                //Point checkPoint = check;
                if (0 <= checkCount.x && ParamModel.SizeMap > checkCount.x)
                {
                    if (0 <= checkCount.z && ParamModel.SizeMap > checkCount.z)
                    {
                        if (ParentItem.GetSum() > LandscapeDictionary[checkCount.ToString()].GetSum())
                        {
                            gradeList.Add(LandscapeDictionary[checkCount.ToString()]);
                        }
                    }
                }
            }

            //return gradeList.OrderByDescending(a=>a.GetSum()).ToList();
            return gradeList;
        }
    }

}
