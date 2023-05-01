using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script2D.model
{
    public class ParamInner
    {
        public static List<Point2D> CheckCubeList = new List<Point2D>() {
        new Point2D(0,1),
        new Point2D(-1,0),
        new Point2D(1,0),
        new Point2D(0,-1),

        new Point2D(-1,1),
        new Point2D(1,1),
        new Point2D(-1,-1),
        new Point2D(1,-1)
    };
    }
}
