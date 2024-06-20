using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame_withExtension
{
    internal class Room
    {
        public string roomName = "unknown";
        private bool isStartRoom, isEndRoom, isLoosingRoom;
        private Room northRoom, eastRoom, southRoom, westRoom;
        private Wall northWall, eastWall, southWall, westWall;
        private List<Item> items;



        //
        //          Constructor
        //
        public Room(string roomName, bool isStartroom, bool isEndRoom, bool isLoosingRoom) 
        { 
            this.roomName = roomName;
            this.isStartRoom = isStartroom;
            this.isEndRoom = isEndRoom;
            this.isLoosingRoom = isLoosingRoom;


            this.northWall = new Wall();
            this.eastWall = new Wall();
            this.southWall = new Wall();
            this.westWall = new Wall();

            SetAllWallsPasstrough(true);
        }




        //
        //          Wall Methods 
        //
        public void SetAllWallsPasstrough(bool condition)
        {
            this.northWall.SetPasstrough(condition);
            this.eastWall.SetPasstrough(condition);
            this.southWall.SetPasstrough(condition);
            this.westWall.SetPasstrough(condition);
        }
        public bool IsPasstroughTo(char direction)
        {
            switch (direction)
            {
                case 'N':
                    return this.northWall.IsPasstrough();
                    break;
                case 'E':
                    return this.eastWall.IsPasstrough();
                    break;
                case 'S':
                    return this.southWall.IsPasstrough();
                    break;
                case 'W':
                    return this.westWall.IsPasstrough();
                    break;
                default: return false;
            }
        }
        public void ToogleWallPasstrough(char direction, bool condition)
        {
            switch (direction)
            {
                case 'N':
                    this.northWall.SetPasstrough(condition);break;
                case 'E':
                    this.eastWall.SetPasstrough(condition); break;
                case 'S':
                    this.southWall.SetPasstrough(condition); break;
                case 'W':
                    this.westWall.SetPasstrough(condition); break;
                default:
                    break;
            }
        }


        //
        //          Room Methods
        //
        public void ConnectRoom(Room room, char direction)
        {
            switch (direction)
            {
                case 'N':
                    this.northRoom = room;
                    break;
                case 'E':
                    this.eastRoom = room;
                    break;
                case 'S':
                    this.southRoom = room;
                    break;
                case 'W':
                    this.westRoom = room;
                    break;
                default:
                    break;
            }

            //UpdateWalls();
        }
        public Room GetConnectedRoom(char direction)
        {
            direction = Char.ToUpper(direction);

            switch (direction)
            {
                case 'N':
                    return this.northRoom;
                case 'E':
                    return this.eastRoom;
                case 'S':
                    return this.southRoom;
                case 'W':
                    return this.westRoom;
                default:
                    return null;
            }
        }
        public string GetRoomName()
        {
            return this.roomName;
        }



        //
        //          Item Methods
        //
        public void AddContent(Item item)
        {
            if (items == null)
            {
                items = new List<Item>();
            }
            items.Add(item);
        }
        public List<Item> GetContent() { return items; }
        public bool HasItems()
        {
            if (this.items != null && this.items.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //
        //          
        //
        public void SetStartingRoom() { this.isStartRoom = true; }
        public void SetEndRoom() {  this.isEndRoom = true; }
        public bool IsEndRoom() { return this.isEndRoom; }
        public void SetLoosingRoom() { this.isLoosingRoom = true; }
        public bool IsLoosingRoom() { return this.isLoosingRoom; }




        //
        //          Not used 
        //
        public Room GetCurrentRoom()
        {
            return this;
        }
        public bool IsStartingRoom() { return this.isStartRoom; }
        public void EnterRoom()
        {
            Console.WriteLine("You are in the {0}.", roomName);
        }


    }
}
