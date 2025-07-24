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

        public RealUnit(Point2D position)
        {
            Position = position;
        }
    }
}
