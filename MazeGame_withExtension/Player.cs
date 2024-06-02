using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame_withExtension
{
    internal class Player
    {
        private Room currentRoom;
        private List<Item> bag;
        private int score;

        public Player(Room currentRoom)
        {
            this.currentRoom = currentRoom;
        }
        public List<Item> GetBag()
        {
            if (bag == null)
            {
                bag = new List<Item>();
            }
            return bag;
        }
        public bool CheckForItems()
        {
            if (this.bag != null && this.bag.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Room GetCurrentRoom() { return this.currentRoom; }
        public bool Move(char direction)
        {
            Room nextRoom = currentRoom.GetConnectedRoom(direction);

            if (nextRoom != null)
            {
                currentRoom = nextRoom;
                return true;
            }
            else
            {
                return false;
            }
        }



        public void AddScore()
        {
            this.score += 1;
        }
        public int GetScore()
        {
            return this.score;

        }
    }
}