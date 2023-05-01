using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script2D
{
    public class Point2D
    {
        public int x { set; get; }
        public int z { set; get; }
        public Point2D(int X, int Z)
        {
            this.x = X;
            this.z = Z;
        }
        public override string ToString()
        {
            return this.x + "_" + this.z;
        }
    }
}
