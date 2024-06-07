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
        private Wall northWall, eastWall, southWall, westWall;
        private bool hasNorthWall, hasEastWall, hasSouthWall, hasWestWall; //brauch ich eigentlich gar nicht weil immer true ic hwerd nur .isPasstrough brauchen
        private Room northRoom, eastRoom, southRoom, westRoom;
        private List<Item> items;
        private bool isStartRoom, isEndRoom, isLoosingRoom;





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

            this.hasNorthWall = (this.northWall != null);
            this.hasEastWall = (this.eastWall != null);
            this.hasSouthWall = (this.southWall != null);
            this.hasWestWall = (this.westWall != null);

            SetAllWallsPasstrough(true);
            //UpdateWalls();
        }

        public void SetAllWallsPasstrough(bool condition)
        {

            if (this.hasNorthWall)
            {
                this.northWall.SetPasstrough(condition);
            }

            if (this.hasEastWall)
            {
                this.eastWall.SetPasstrough(condition);
            }
            if (this.hasSouthWall)
            {
                this.southWall.SetPasstrough(condition);
            }
            if (this.hasWestWall)
            {
                this.westWall.SetPasstrough(condition);
            }
        }



        public void UpdateWalls()
        { 
            this.hasNorthWall = (this.northRoom == null);
            this.hasEastWall = (this.eastRoom == null);
            this.hasSouthWall = (this.southRoom == null);
            this.hasWestWall = (this.westRoom == null);


            SetAllWallsPasstrough(true); //Alle Wände werden auf Passtrough gestellt
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
                    this.northWall.SetPasstrough(condition);
                    break;
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


        public void SetConnectedRooms(Room north, Room east, Room south, Room west)
        {
            //set references here
            this.northRoom = north;
            this.eastRoom = east;
            this.southRoom = south;
            this.westRoom = west;

            UpdateWalls();

        }
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

        public void DisconnectRoom(Room room, char direction) 
        {
            switch (direction)
            {
                case 'N':
                    this.northRoom = null;
                    break;
                case 'E':
                    this.eastRoom = null;
                    break;
                case 'S':
                    this.southRoom = null;
                    break;
                case 'W':
                    this.westRoom = null;
                    break;
                default:
                    break;
            }
            //UpdateWalls();
        }

        public string GetRoomName()
        {
            return this.roomName;
        }
        public Room GetCurrentRoom()
        {
            return this;
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



        



        public void SetRoomName(string roomName)
        {
            this.roomName = roomName;
        }



        






        public void EnterRoom()
        {
            Console.WriteLine("You are in the {0}.", roomName);
        }



        


        


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





        public void SetStartingRoom() { this.isStartRoom = true; }
        public bool IsStartingRoom() { return this.isStartRoom; }
        public void SetEndRoom() {  this.isEndRoom = true; }
        public bool IsEndRoom() { return this.isEndRoom; }
        public void SetLoosingRoom() { this.isLoosingRoom = true; }
        public bool IsLoosingRoom() { return this.isLoosingRoom; }


    }
}
