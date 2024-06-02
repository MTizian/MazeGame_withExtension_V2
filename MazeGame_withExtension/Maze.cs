using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame_withExtension
{
    internal class Maze
    {
        private Room startRoom, winningRoom, losingRoom;
        private Room[,] roomsInMaze;
        private int mazeWidth, mazeHeight;
        private Random random;
        private List<Room> roomList, rooms;
        private int difficulty;

        
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
            List<Room> roomsToPlace = new List<Room>(rooms);

            // Run the loop 5 times for each difficulty level
            for (int n = 0; n < 5 * difficulty; n++)
            {
                if (roomsToPlace.Count > 0)
                {
                    List<Room> roomsToRemove = new List<Room>();

                    foreach (Room room in roomsToPlace)
                    {
                        PlaceRoomRandom(room);
                        roomsToRemove.Add(room);
                    }

                    // Remove placed rooms from roomsToPlace
                    foreach (Room room in roomsToRemove)
                    {
                        roomsToPlace.Remove(room);
                    }
                }
            }



            //Setzte die Verbindung von Räumen die nebeneinander liegen
            ConnectRooms();




            //Setzt die Wichtigen Räume

            foreach (Room room in rooms)
            {
                if (room.IsStartingRoom())
                {
                    this.startRoom = room;
                }
                if (room.IsEndRoom())
                {
                    this.winningRoom = room;
                }
                if (room.IsLoosingRoom())
                {
                    this.losingRoom = room;
                }
            }

/*
            ///////////////Items
            ///

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


        private void ConnectRooms()
        {
            for (int i = 0; i < this.mazeWidth; i++)
            {
                for (int j = 0; j < this.mazeHeight; j++)
                {
                    Room currentRoom = roomsInMaze[i, j];

                    //Wenn CurrentRoom ist nicht NULL
                    if (currentRoom != null)
                    {
                        if (i > 0)
                        {
                            //Wenn in nord richtung ein raum ist 
                            if (roomsInMaze[i - 1, j] != null)
                            {
                                //Setze die Verbindung 
                                currentRoom.ConnectRoom(roomsInMaze[i - 1, j], 'N');
                            }
                        }

                        if (j < this.mazeWidth - 1)
                        {
                            //Wenn in ost richtung ein raum ist 
                            if (roomsInMaze[i, j + 1] != null)
                            {
                                //Setze die Verbindung 
                                currentRoom.ConnectRoom(roomsInMaze[i, j + 1], 'E');
                            }
                        }

                        if (i < this.mazeHeight - 1)
                        {
                            //Wenn in Süd richtung ein raum ist 
                            if (roomsInMaze[i + 1, j] != null)
                            {
                                //Setze die Verbindung 
                                currentRoom.ConnectRoom(roomsInMaze[i + 1, j], 'S');
                            }
                        }

                        if (j > 0)
                        {
                            //Wenn in West richtung ein raum ist 
                            if (roomsInMaze[i, j - 1] != null)
                            {
                                //Setze die Verbindung 
                                currentRoom.ConnectRoom(roomsInMaze[i, j - 1], 'W');
                            }
                        }

                    }
                }
            }
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

        private void PlaceRoomRandom(Room room)
        {
            int x, y;
            //suche nach passender Koordinate
            do
            {
                //zufällige x Koordinate innerhalb des Maze
                x = random.Next(this.mazeWidth);
                //zufällige y innerhalb des Maze
                y = random.Next(this.mazeHeight);

                //Wiederhole solange bis Die koordinate frei ist
            } while (this.roomsInMaze[y, x] != null);


            //setze Room in passende Koordinate
            if (room != null)
            {
                this.roomsInMaze[y, x] = room;
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
            }
        }

       

        public List<Room> GetRoomList() { return this.roomList; }

        public Room getStartRoom() { return this.startRoom; }

        public Room getWinningRoom() { return this.winningRoom; }

        public Room getLosingRoom() { return this.losingRoom; }
    }

}
