using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script2D.model
{
    public class RealUnit
    {
        public Point2D Position { get; set; }
        public int Step { private set; get; }
        public float Time;
        public List<SuperNode> RealUnitPathList { private set; get; }
        public RealUnit(Point2D position)
        {
            Position = position;
            RealUnitPathList = new List<SuperNode>();
        }
        public void SetStep(float time)
        {
            Step++;
            Time = time+5;
        }
        public void SetPath(List<SuperNode> realUnitPathList)
        {
            RealUnitPathList = realUnitPathList;
        }
    }
}
