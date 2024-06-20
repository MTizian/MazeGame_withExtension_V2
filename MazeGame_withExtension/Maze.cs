using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace MazeGame_withExtension
{
    internal class Maze
    {
        private Room startRoom, winningRoom, losingRoom;

        //2d Array for maze
        private Room[,] roomsInMaze;
        private int mazeWidth, mazeHeight;
        private int difficulty;

        private Random random;

        private List<Room> roomList;
        private List<Item> itemList;
        private List<Door> doorsList;
        private List<Key> keyList;
        private char[] directions = { 'N', 'E', 'S', 'W' };




        private List<string> roomNames;
                
        public Maze(int width, int height, int difficulty)
        {
            this.difficulty = difficulty;
            this.mazeWidth = width;
            this.mazeHeight = height;
            random = new Random();

            SetRoomList();
            
            this.roomsInMaze = new Room[width, height];

            GenerateMaze();
        }

        private void GenerateMaze()
        {
            PlaceRooms();           //Setzt die Räume zufällig           

            SetImportantRooms();    //Setzt die Wichtigen Räume

            ConnectAllRooms();      //Setzte die Verbindung von Räumen die nebeneinander liegen

            PlaceWalls();           //Stellt Passtrough der Wände an zufälligen stellen auf false


            SetItemList();
            SetDoorList();
            SetKeyList();

            PlaceItems();       //Platziere und erstelle Items

        }

        //
        //          Maze Generierung
        //
        private void PlaceRooms() //Setzt in Jedes Matrix feld ein raum und gibt den räumen zufällige namen 
        {
            // Erstelle eine Liste aller verfügbaren Raum-Namen
            List<string> roomNamesList = new List<string>(roomNames);

            // Überprüfe, ob genug Namen für die Räume vorhanden sind
            int totalRooms = this.mazeHeight * difficulty * this.mazeWidth * difficulty;
            if (roomNamesList.Count < totalRooms)
            {
                throw new InvalidOperationException("Nicht genügend Raum-Namen verfügbar, um jeden Raum einen einzigartigen Namen zuzuweisen.");
            }

            // Mische die Liste der Raum-Namen zufällig
            Random rand = new Random();
            roomNamesList = roomNamesList.OrderBy(x => rand.Next()).ToList();

            // Setzt in jedes Feld einen Raum und weist einen zufälligen, aber einzigartigen Namen zu
            int index = 0;
            for (int i = 0; i < this.mazeHeight * difficulty; i++)
            {
                for (int j = 0; j < this.mazeWidth * difficulty; j++)
                {
                    roomsInMaze[i, j] = new Room(roomNamesList[index], false, false, false);
                    index++;
                }
            }
        }
        private void SetImportantRooms() //Setzt starting, winning, und loosing room
        {
            for (int y = 0; y < this.mazeHeight * difficulty; y++)
            {
                for (int x = 0; x < this.mazeWidth * difficulty; x++)
                {

                    if (roomsInMaze[y, x].GetRoomName() == "Kitchen")
                    {
                        roomsInMaze[y, x].SetStartingRoom();
                        this.startRoom = roomsInMaze[y, x];
                    }

                    if (roomsInMaze[y, x].GetRoomName() == "Bedroom")
                    {
                        roomsInMaze[y, x].SetEndRoom();
                        this.winningRoom = roomsInMaze[y, x];
                    }

                    if (roomsInMaze[y, x].GetRoomName() == "Torture Chamber")
                    {
                        roomsInMaze[y, x].SetLoosingRoom();
                        this.losingRoom = roomsInMaze[y, x];
                    }
                }
            }
        }
        private void ConnectAllRooms() //setzt die Verbindungen von Räumen die nebeneinadner liegen
        {
            for (int y = 0; y < this.mazeWidth; y++)
            {
                for (int x = 0; x < this.mazeHeight; x++)
                {
                    Room currentRoom = roomsInMaze[y, x];
                    Room roomToConnect = null;

                    //Wenn CurrentRoom ist nicht NULL
                    if (currentRoom != null)
                    {
                        //setzt die Verbindung zwischen zwei räumen, wenn der raum 

                        //Wenn currentRoom nicht in erster reihe UND Wenn in nord richtung ein raum ist
                        if (y > 0 && roomsInMaze[y - 1, x] != null)
                        {
                            roomToConnect = roomsInMaze[y - 1, x];
                            currentRoom.ConnectRoom(roomToConnect, 'N');
                        } 

                        //Wenn currentRoom nicht in erster spalte ist  UND Wenn in Ost richtung ein raum ist
                        if (x < this.mazeWidth - 1 && roomsInMaze[y, x + 1] != null)
                        {
                            roomToConnect = roomsInMaze[y, x + 1];
                            currentRoom.ConnectRoom(roomToConnect, 'E');
                        }

                        //Wenn currentRoom nicht in erster spalte ist  UND Wenn in süd richtung ein raum ist

                        if (y < this.mazeHeight - 1 && roomsInMaze[y + 1, x] != null)
                        {
                            roomToConnect = roomsInMaze[y + 1, x];
                            currentRoom.ConnectRoom(roomToConnect, 'S');
                        }

                        //Wenn currentRoom nicht in erster reihe ist  UND Wenn in west richtung ein raum ist UND keine Wall ist
                        if (x > 0 && roomsInMaze[y, x - 1] != null)
                        {
                            roomToConnect = roomsInMaze[y, x - 1];
                            currentRoom.ConnectRoom(roomToConnect, 'W'); 
                        }
                    }
                } 
             }
            UpdateRoomList();
        }
        private void PlaceWalls()
        {
            PlaceEdgeWalls();

            ///////////////////////////////////////////////////////////////////////////////7
            //                         Setze 5 Zufällige Wände 
            ////////////////////////////////////////////////////////////////////////////////
            //Wiederhole für 
            for (int n = 0; n < 5; n++)
            {
                Room tempRoom, oppositeRoom;
                char direction;

                do
                {
                    //zufällige Richtung
                    direction = RandomDirection();
                    //zufälliger Room

                    tempRoom = this.RandomRoom();
                    oppositeRoom = tempRoom.GetConnectedRoom(direction);
                } while (oppositeRoom == null);

                if (tempRoom.IsPasstroughTo(direction))
                {
                    tempRoom.ToogleWallPasstrough(direction, false);
                    //tempRoom.UpdateWalls();
                    oppositeRoom.ToogleWallPasstrough(OppositeDirection(direction), false);
                    //oppositeRoom.UpdateWalls();
                }

            }



            //Hier Funktion zum umstellen aller wände des Winningroom

            int winningRoomIndex = roomList.FindIndex(room => room.IsEndRoom());//finde index von winningroom



            //winningRoom = roomList[winningRoomIndex];
            roomList[winningRoomIndex].SetAllWallsPasstrough(false);//setze alle walls auf nicht passtrough im winningroom IN DER LISTE



            //durchlaufe die Matrix und suche nach dem winning room         wäre vlt. prktischer eine coord variable in room zu haben => einfacherer zugang
            for (int y = 0; y < this.mazeHeight * difficulty; y++)
            {
                for (int x = 0; x < this.mazeWidth * difficulty; x++)
                {
                    if (roomsInMaze[y, x].IsEndRoom())
                    {
                        //setze alle Wände auf nicht Passtrough IN DER MATRIX
                        roomsInMaze[y, x].SetAllWallsPasstrough(false);

                        //Setze die angrenzenden Wände des WinningRoom aus passtrough false
                        foreach(char dir in this.directions)
                        {
                            if (this.winningRoom.GetConnectedRoom(dir) != null) { this.winningRoom.GetConnectedRoom(dir).ToogleWallPasstrough(OppositeDirection(dir), false); }
                        }
                    }
                }
            }
        }


         
        private void PlaceEdgeWalls()
        {
            ///////////////////////////////////////////////////////////////////////////////
            //               Setze die Passtrough der Rand Wände auf false
            ////////////////////////////////////////////////////////////////////////////////

            //Bei Allen Räumen die ganz links liegen wird die Wall Passtrough auf false gesetzt
            for (int y = 0; y < this.mazeHeight; y++)
            {
                if (roomsInMaze[y, 0].IsPasstroughTo('W'))
                {
                    roomsInMaze[y, 0].ToogleWallPasstrough('W', false);
                }
            }

            //Alle Räume ganz oben werden richtung Norden gesperrt
            for (int x = 0; x < this.mazeWidth; x++)
            {
                if (roomsInMaze[0, x].IsPasstroughTo('N'))
                {
                    roomsInMaze[0, x].ToogleWallPasstrough('N', false);
                }
            }


            //Alle Räume ganz unten werden richtung Osten gesperrt
            for (int y = 0; y < this.mazeHeight; y++)
            {
                if (roomsInMaze[y, this.mazeWidth - 1].IsPasstroughTo('E'))
                {
                    roomsInMaze[y, this.mazeWidth - 1].ToogleWallPasstrough('E', false);
                }
            }


            //Alle Räume ganz unten werden richtung Süden gesperrt
            for (int x = 0; x < this.mazeWidth; x++)
            {
                if (roomsInMaze[this.mazeHeight - 1, x].IsPasstroughTo('S'))
                {
                    roomsInMaze[this.mazeHeight - 1, x].ToogleWallPasstrough('S', false);
                }
            }


            UpdateRoomList();
        }
        private void PlaceItems()
        {

            //setze endroom
            this.winningRoom = roomList.Find(room => room.IsEndRoom());


            //Füge Tür in EndRoom
            //this.winningRoom.AddContent(doorsList[0]);

            Room tempRoom;
            char randDir;
            while (true)
            {
                randDir = RandomDirection();

                if(this.winningRoom.GetConnectedRoom(randDir) != null) 
                {
                    tempRoom = this.winningRoom.GetConnectedRoom(randDir);
                    break; 
                }
            }


            Door mysteriousDoor = new Door("Mysterious Door", tempRoom,randDir, this.winningRoom,OppositeDirection(randDir));


            tempRoom.AddContent(mysteriousDoor);



            //Füge Key in zufälligen Raum
            Room keyRoom = null;
            do
            {
                keyRoom = RandomRoom();
            } while (keyRoom.IsEndRoom() || keyRoom.IsLoosingRoom());
            
            keyRoom.AddContent(new Key("Mysterious Key", mysteriousDoor));

            //Setze jedes Item aus der Itemliste in ein Zufälligen Raum
            foreach (Item item in itemList) { RandomRoom().AddContent(item); }



            UpdateRoomList();
        }
   










        private void UpdateRoomList()
        {
            for (int y = 0; y < this.mazeWidth; y++)
            {
                for (int x = 0; x < this.mazeHeight; x++)
                {
                    Room currentRoom = roomsInMaze[y, x];
          
                        int currentRoomIndex = this.roomList.FindIndex(room => room.GetRoomName() == currentRoom.GetRoomName());
                        this.roomList[currentRoomIndex] = currentRoom;
                    
                }
            }
        }











        //
        //          List Erstellungen
        //
        private void SetRoomList() //Erstelle ein Liste mit allen Räumen
        {
            if (this.roomList == null)
            {
                this.roomList = new List<Room>{
                    new Room("Kitchen", true ,false ,false),
                    new Room("Bedroom", false, true, false),
                    new Room("Torture Chamber", false, false, true),
                    new Room("Living Room", false, false, false),
                    new Room("Bathroom", false, false, false),
                    new Room("Library", false, false, false),
                    new Room("Dining Room", false, false, false),
                    new Room("Garage", false, false, false),
                    new Room("Basement", false, false, false),
                    new Room("Attic", false, false, false),
                    new Room("Study", false, false, false),
                    new Room("Guest Room", false, false, false),
                    new Room("Pantry", false, false, false),
                    new Room("Garden", false, false, false),
                    new Room("Office", false, false, false),
                    new Room("Home Theater", false, false, false),
                    new Room("Hallway", false, false, false),
                    new Room("Laundry Room", false, false, false),
                    new Room("Game Room", false, false, false),
                    new Room("Fitness Room", false, false, false),
                    new Room("Music Room", false, false, false),
                    new Room("Art Studio", false, false, false),
                    new Room("Conservatory", false, false, false),
                    new Room("Sunroom", false, false, false),
                    new Room("Observatory", false, false, false)};

                //Übertrage die Namen aus der Liste in eigene Liste
                SetRoomNameList();
            }

        }
        private void SetRoomNameList() //erstelle Liste für die Namen der Räume
        {
            if (this.roomNames == null)
            {
                this.roomNames = new List<string>();


                foreach (Room room in this.roomList)
                {
                    this.roomNames.Add(room.GetRoomName());
                }

            }
        }
        private void SetDoorList() //Erstelle eine Liste mit allesn türen       Bisschen quatsch aber weil wird doch nicht benutzt
        {
            if (this.doorsList == null)
            {

                // Find the end room
                Room endRoom = this.roomList.Find(room => room.IsEndRoom());
                char dir;
                do
                {
                    dir = RandomDirection();
                } while (endRoom.GetConnectedRoom(dir) != null);
                

                this.doorsList = new List<Door>
                {
                    new Door("Mysterious Door", endRoom.GetConnectedRoom(dir), dir, endRoom, OppositeDirection(dir))
                };
            }
            
        }
        private void SetKeyList() //Erstelle eine Liste mit alle Keys für die türen
        {
            if (this.keyList == null)
            {
                this.keyList = new List<Key>
                {
                    new Key("Mysterious Key", this.doorsList[0])
            };
            }
            
        }
        private void SetItemList() //Erstelle eine Liste mit allen Items
        {
            if(this.itemList == null)
            {
                this.itemList = new List<Item>
                {
                new Item("Lamp", false),
                new Item("Sword", true),
                new Item("Shield", true),
                new Item("Potion", true),
                new Item("Rock", false),
                new Item("Chair", false),
                new Item("Table", false),
                new Item("Book", true),
                new Item("Chest", false),
                new Item("Coin", true),
                new Item("Ring", true),
                new Item("Apple", true),
                new Item("Goblet", true),
                new Item("Helmet", true)
                };
            }
        }

        //
        //          Randomizer
        //
        private Room RandomRoom()
        {
            //zufällige x Koordinate innerhalb des Maze
            int x = random.Next(this.mazeWidth);
            //zufällige y innerhalb des Maze
            int y = random.Next(this.mazeHeight);

            return this.roomsInMaze[y, x];
        }
        private char RandomDirection()
        {
            int randint = random.Next(1, 5);

            switch (randint)
            {
                case 1:
                    return 'N';
                    break;
                case 2:
                    return 'E';
                    break;
                case 3:
                    return 'S';
                    break;
                case 4:
                    return 'W';
                    break;
                 default:
                    return 'X';
                    break;

            }
        }







        


        public List<Room> GetRoomList() { return this.roomList; }

        public Room getStartRoom() { return this.startRoom; }

        public Room getWinningRoom() { return this.winningRoom; }

        public Room getLosingRoom() { return this.losingRoom; }

        private char OppositeDirection(char direction)
        {
            switch (direction)
            {
                case 'N':
                    return 'S';
                    break;
                case 'E':
                    return 'W';
                    break;
                case 'S':
                    return 'N';
                    break;
                case 'W':
                    return 'E';
                    break;
                default:
                    return 'X';
                    break;

            }
        }

    }
}
