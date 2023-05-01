using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script2D.model
{
    public class ScenarioBuilder
    {
        public void CreateIslandVulcan(List<List<Column>> Landscape_List,int Start, int StartY, int EndX, int EndY)
        {
            for (int i = Start; i < Start + EndX; i++)
            {
                for (int z = StartY; z < StartY + EndY; z++)
                {
                    if (Start + EndX - 1 == i && z == StartY + 2)
                    {

                        Landscape_List[i][z] = new Column(11, 0);
                        continue;
                    }
                    if (i == Start || i == Start + EndX - 1)
                    {
                        // Debug.Log((i == SizeMap / 3) + "=== ===" + (i == SizeMap / 2 - 2));
                        Landscape_List[i][z] = new Column(15, 0);
                        continue;
                    }
                    if (z == StartY || z == StartY + EndY - 1)
                    {
                        Landscape_List[i][z] = new Column(15, 0);
                        continue;
                    }

                    Landscape_List[i][z] = new Column(12, 0);
                }
            }
            //leak

        }
        public void CreateIslandPlato(List<List<Column>> Landscape_List, int StartX, int StartY, int End)
        {
            for (int i = StartX; i < End; i++)
            {
                for (int z = StartY; z < End; z++)
                {

                    Landscape_List[i][z] = new Column(11, 0);
                }
            }
        }
    }
}
