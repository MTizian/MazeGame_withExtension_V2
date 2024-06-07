using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame_withExtension
{
    internal class Coord
    {
        private int x, y;


        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int GetX() {  return x; }
        public int GetY() { return y; }
    }
}
