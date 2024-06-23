using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame_withExtension
{
    internal class Wall
    {
        private bool passtrough;

        public Wall()
        {
            this.passtrough = false;
        }


        public void SetPasstrough(bool condition)
        {
            this.passtrough = condition;
        }

        public bool IsPasstrough() {  return passtrough; }

    }
}
