using System;
using System.Collections.Generic;
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

        private Random random;

        private List<Room> roomList, rooms;
        private int difficulty;

        private List<string> roomNames;
                
        public Maze(int width, int height, int difficulty)
        {
            this.difficulty = difficulty;
            this.mazeWidth = width;
            this.mazeHeight = height;
            random = new Random();

            SetRoomList();
            this.rooms = GetRoomList();
            this.roomsInMaze = new Room[width, height];


            GenerateMaze();
        }



        private void GenerateMaze()
        {
            //Setzt die Räume zufällig 
            PlaceRooms();
            //Setzte die Verbindung von Räumen die nebeneinander liegen
            ConnectAllRooms();


            //Stellt Passtrough der Wände an zufälligen stellen auf false
            PlaceWalls();

            //DisconnectRooms();

            //Setzt die Wichtigen Räume
            SetImportantRooms();


            //PlaceItems();
        }

        private void SetImportantRooms()
        {
            for (int i = 0; i < this.mazeHeight * difficulty; i++)
            {
                for (int j = 0; j < this.mazeWidth * difficulty; j++)
                {

                    if (roomsInMaze[i, j].GetRoomName() == "Kitchen")
                    {
                        roomsInMaze[i, j].SetStartingRoom();
                        this.startRoom = roomsInMaze[i, j];
                    }

                    if (roomsInMaze[i, j].GetRoomName() == "Bedroom")
                    {
                        roomsInMaze[i, j].SetEndRoom();
                        this.winningRoom = roomsInMaze[i, j];
                    }

                    if (roomsInMaze[i, j].GetRoomName() == "Torture Chamber")
                    {
                        roomsInMaze[i, j].SetLoosingRoom();
                        this.losingRoom = roomsInMaze[i, j];
                    }
                }
            }
        }

        private void PlaceRooms()
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


        //setzt die Verbindungen von Räumen die nebeneinadner liegen
        private void ConnectAllRooms()
        {
            for (int y = 0; y < this.mazeWidth; y++)
            {
                for (int x = 0; x < this.mazeHeight; x++)
                {
                    Room currentRoom = roomsInMaze[y, x];

                    //Wenn CurrentRoom ist nicht NULL
                    if (currentRoom != null)
                    {
                        //setzt die Verbindung zwischen zwei räumen, wenn der raum 

                        //Wenn currentRoom nicht in erster reihe UND Wenn in nord richtung ein raum ist UND keine Wall ist
                        if (y > 0 && roomsInMaze[y - 1, x] != null)
                        {
                            //Setze die Verbindung 
                            currentRoom.ConnectRoom(roomsInMaze[y - 1, x], 'N');
                        }

                        //Wenn currentRoom nicht in erster spalte ist  UND Wenn in Ost richtung ein raum ist UND keine Wall ist
                        if (x < this.mazeWidth - 1 && roomsInMaze[y, x + 1] != null)
                        {
                            //Setze die Verbindung 
                            currentRoom.ConnectRoom(roomsInMaze[y, x + 1], 'E');                            
                        }

                        //Wenn currentRoom nicht in erster spalte ist  UND Wenn in süd richtung ein raum ist UND keine Wall ist

                        if (y < this.mazeHeight - 1 && roomsInMaze[y + 1, x] != null)
                        {
                            //Setze die Verbindung 
                            currentRoom.ConnectRoom(roomsInMaze[y + 1, x], 'S');
                        }

                        //Wenn currentRoom nicht in erster reihe ist  UND Wenn in west richtung ein raum ist UND keine Wall ist
                        if (x > 0 && roomsInMaze[y, x - 1] != null)
                        {
                            //Setze die Verbindung 
                            currentRoom.ConnectRoom(roomsInMaze[y, x - 1], 'W');
                        }
                    }
                } 
             }
        }

 

        private void PlaceWalls()
        {
            //Set outer Walls Passtrough to false

            //Bei Allen Räumen die ganz links liegen wird die Wall Passtrough auf false gesetzt
            for (int y=0; y < this.mazeHeight; y++)
            {
                if(roomsInMaze[y, 0].IsPasstroughTo('W'))
                {
                    roomsInMaze[y, 0].ToogleWallPasstrough('W', false);
                }
            }

            //Alle Räume ganz oben werden richtung Norden gesperrt
            for (int x=0;x < this.mazeWidth; x++)
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



            
            //Wiederhole für 
            for (int n = 0; n < 5; n++)
            {
                Room tempRoom, oppositeRoom;
                char direction;

                //Auswahl einer Rand Koordinate
                //zufällige x Koordinate innerhalb des Maze
                int x = random.Next(this.mazeWidth);
                //zufällige y innerhalb des Maze
                int y = random.Next(this.mazeHeight);
                //Auswahl einer rand direction


                
                do
                {
                    //zufällige Richtung
                    direction = RandomDirection();
                    //zufälliger Room
                    tempRoom = this.GetRoomAt(x, y);
                    oppositeRoom = tempRoom.GetConnectedRoom(direction);
                }while (oppositeRoom == null);
                


                if (tempRoom.IsPasstroughTo(direction))
                {
                    tempRoom.ToogleWallPasstrough(direction, false);
                    //tempRoom.UpdateWalls();
                    oppositeRoom.ToogleWallPasstrough(OppositeDirection(direction), false);
                    //oppositeRoom.UpdateWalls();
                }
            
            }
        }



        private void PlaceItems()
        {
            /*
            ///////////////Items
            Door redDoor = new Door("Red Door", bathroom, 'S', kitchen, 'N');

            Key redKey = new Key("Red Key", redDoor);

            //Key redKey2 = new Key("Red Key 2", redDoor2);
            //TimeBomb timeBomb = new TimeBomb();

            Door blueDoor = new Door("Blue Door", livingRoom, 'W', exitRoom, 'E');
            Key blueKey = new Key("Blue Key", blueDoor);

            BrickWall brickwall = new BrickWall("Brickwall East", livingRoom, 'E', kitchen, 'W');

            kitchen.AddContent(new WoodenStick());

            livingRoom.AddContent(blueDoor);
            exitRoom.AddContent(blueDoor);

            bathroom.AddContent(redDoor);
            kitchen.AddContent(redDoor);

            kitchen.AddContent(brickwall);

            //bedRoom.addContent(redKey2);
            bedRoom.AddContent(redKey);
            kitchen.AddContent(blueKey);
            */
        }

        private Room GetValidRoom(int x, int y)
        {
            //stell sicher, dass x und y im Maze sind und ein Raum haben
            if (x >= 0 && x < this.mazeWidth && y >= 0 && y < this.mazeHeight && roomsInMaze[x, y] != null)
            {
                return roomsInMaze[x, y];
            }
            else
            {
                // If the coordinates are not valid, try again
                return GetValidRoom(random.Next(this.mazeWidth), random.Next(this.mazeHeight));
            }
        }


        private void PlaceItemRandom(Item item)
        {

        }


        private void SetRoomList()
        {
            if (this.roomList == null)
            {
                this.roomList = new List<Room>{
                    new Room("Kitchen", true ,false ,false),
                    new Room("Living Room", false, false, false),
                    new Room("Bathroom", false, false, false),
                    new Room("Bedroom", false, true, false),
                    new Room("Torture Chamber", false, false, true),
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

                SetRoomNameList();
            }

        }

        private void SetRoomNameList()
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

       

        public List<Room> GetRoomList() { return this.roomList; }

        public Room getStartRoom() { return this.startRoom; }

        public Room getWinningRoom() { return this.winningRoom; }

        public Room getLosingRoom() { return this.losingRoom; }



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


        private Room GetRoomAt(int x, int y)
        { 
            return this.roomsInMaze[y, x];
        }
    }

}
