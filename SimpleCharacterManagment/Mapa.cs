using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleCharacterManagment
{
    public class Mapa
    {
        private List<List<string>> _mapa2D;
        public List<FOVData> _wallsInMap= new List<FOVData>();
        public List<RewardItems> rewardItemsList = new List<RewardItems>();
        public static int MAP_SIZE_X = 10;
        public static int MAP_SIZE_Y = 10;
        public static int player_position_X_from_file = 0;
        public static int player_position_Y_from_file = 0;

        public List<List<string>> Mapa2D {
            get { return _mapa2D; }
            set { _mapa2D = value; }
        }

        public static string wall = "███";
        public static string visitedWall = "░░░";
        public static string hiddenOrEmptyRoom = "   ";// █ ▓ ▒ ░
        public static string currentPlayerLocalisation = " X ";
        public static string playerInRoomWithItemReward = "⚑X⚑";
        public static string roomWithItemReward = " ? ";
        public static string roomWithClearedItemReward = " . ";
        public static string flashlightedEmptyRoom = "░░░";
     
        public Mapa(int rows, int columns) {

            List<List<string>> _mapa = new List<List<string>>();
            _mapa = GenerateMapFromTextFile();
            Mapa2D = _mapa;

            rewardItemsList = RewardItems.InitializeGameItemsList();
        }

        /* Marking map based on old and curent move direction to check special mark when 
             Player step into ItemReward and depends when its collected or not */
        public void UpdateMap(int oldX, int oldY, int x, int y) {
            // if we leave a treasure room return his state to ?
            // TODO: if we open chest, dont save ? mark delete this
            if (CheckIfChestsInRoom(oldX, oldY)) {
                if (CheckIfTreasureIsCollectedOnCoords(oldX, oldY) == false) {
                    _mapa2D[oldX][oldY] = roomWithItemReward;
                } else if (CheckIfTreasureIsCollectedOnCoords(oldX, oldY) == true) {
                    _mapa2D[oldX][oldY] = roomWithClearedItemReward;
                }
            } else {
                _mapa2D[oldX][oldY] = hiddenOrEmptyRoom;
            }

            /* Define how to mark Player current location when he walk into treasure
                 chest and how to mark when this chest were colected or not. */
            if (CheckIfChestsInRoom(x, y)) {
                if (CheckIfTreasureIsCollectedOnCoords(x, y) == false) {
                    _mapa2D[x][y] = playerInRoomWithItemReward;
                }
                if (CheckIfTreasureIsCollectedOnCoords(x, y) == true) {
                    _mapa2D[x][y] = currentPlayerLocalisation;
                }
            } else {
                _mapa2D[x][y] = currentPlayerLocalisation;
            }
            // Store passed current player localisation

        }

        public void UpdateMapWhileUsingFlashlight(List<FOVData> oldlightedRooms, int oldx, int oldy, List<FOVData> currentlightedRooms, int currentx, int currenty) {
            // how to clear old changes after move 
            // TOOD: Reset old visible map
            // check every item in actual / new list
            foreach (FOVData old_room in oldlightedRooms) {
                try {
                    _mapa2D[old_room.X_coord][old_room.Y_coord] = hiddenOrEmptyRoom;
                } catch (ArgumentOutOfRangeException) {
                }
            }
            //---------------------------------------------------------------------------------------------------------------
            foreach (FOVData room in currentlightedRooms) {
                try {
                    if (CheckIfChestsInRoom(room.X_coord, room.Y_coord)) {
                        if (CheckIfTreasureIsCollectedOnCoords(room.X_coord, room.Y_coord)) {
                            _mapa2D[room.X_coord][room.Y_coord] = roomWithClearedItemReward;
                            Debug.WriteLine($"Wykryto pozostałości po znajdźcce => [{room.X_coord}][{room.X_coord}]");
                        } else {
                            _mapa2D[room.X_coord][room.Y_coord] = roomWithItemReward;

                            Debug.WriteLine($"Wykryto nie zebraną znajdźke w pobliżu! => [{room.X_coord}][{room.X_coord}]");
                        }
                    }
                    // if room is Empty and nothink there (wall etc.)
                    foreach (FOVData wallCoord in _wallsInMap) {
                        if ((wallCoord.X_coord == room.X_coord) && (wallCoord.Y_coord == room.Y_coord)) {
                            if (CheckIfThereIsAWall(room.X_coord, room.Y_coord)) {
                                _mapa2D[room.X_coord][room.Y_coord] = wall;
                            } else {
                                _mapa2D[room.X_coord][room.Y_coord] = flashlightedEmptyRoom;
                            }
                        }

                    }
                    // Change player position mark while in in room with treasure what isnt cleared
                    if ((currentx == room.X_coord) && (currenty == room.Y_coord)) {
                        // Mark player current position by [X]
                        _mapa2D[room.X_coord][room.Y_coord] = currentPlayerLocalisation;
                        if (CheckIfChestsInRoom(room.X_coord, room.Y_coord)) {
                            if (!CheckIfTreasureIsCollectedOnCoords(room.X_coord, room.Y_coord)) {
                                _mapa2D[room.X_coord][room.Y_coord] = playerInRoomWithItemReward;
                                Debug.WriteLine($"Znajdujesz się w pokoju ze skarbem :D");
                            }
                        }
                    Debug.WriteLine($"Pozycja gracza => [{room.X_coord}][{room.Y_coord}] ");
                    }
                } catch (ArgumentOutOfRangeException) {
                }
            }
        }

        // Check if this room contain a Wall
        private bool CheckIfThereIsAWall(int coord_x, int coord_y) => _mapa2D[coord_x][coord_y] == wall ? true : false;


        // Return true if at specific location room contain any "RewardItem"
        private bool CheckIfChestsInRoom(int coord_x, int coord_y) => this.rewardItemsList.Where(r => r.LocalizationX == coord_x && r.LocalizationY == coord_y).FirstOrDefault() != null ? true : false;

        // Check room with specific coordinates to check if "RewardItem" whos were here was collected
        private bool CheckIfTreasureIsCollectedOnCoords(int coord_x, int coord_y) => this.rewardItemsList.Where(r => r.LocalizationX == coord_x && r.LocalizationY == coord_y).First().IsCollected;

        // Iterate through list with c
        public void ShowMap() {
            foreach (List<string> Location in _mapa2D) {
                Console.Write("\t");
                foreach (var room in Location) {
                    Console.Write(room);
                }
                Console.WriteLine();
            }
        }


        // Updating map marking every "RewardItem" at localisation.
        private void UnhideTreasuresOnMap(List<RewardItems> treasures) {
            foreach (RewardItems item in treasures) {
                _mapa2D[item.LocalizationX][item.LocalizationY] = roomWithItemReward;
            }
            ShowMap();
            // TODO: make a class TreasureChest, where would be stored data like gold, exp, monster?
        }
        private void HidePlaceTreasuresOnMap(List<RewardItems> treasures) {
            foreach (RewardItems item in treasures) {
                _mapa2D[item.LocalizationX][item.LocalizationY] = hiddenOrEmptyRoom;
            }
            ShowMap();
            // TODO: make a class TreasureChest, where would be stored data like gold, exp, monster?
        }

        private List<List<string>> GenerateMapFromTextFile() {
            /*
                -----LEGENDA-------
                0 -> puste
                1 -> sciana
                X -> gracz
                ? -> znajdźka
            */
            string unwantedchars = "\rn";
            string mapTilesDataAll = "";
            string MAP_name = "";
            string MAP_x_string = "", MAP_y_string = "";
            string mapTemplate = File.ReadAllText("C:\\Users\\MrKom\\Desktop\\Map1.txt");
            for (int i = 0; i < mapTemplate.Length; i++) {
                if (unwantedchars.Contains(mapTemplate[i].ToString())) { i++; continue; }
                if (mapTemplate[i].ToString() == "<") {
                    for (int j = i + 1; j < mapTemplate.Length; j++) {
                        i = j;
                        if (mapTemplate[i].ToString() == ">") { break; } else {
                            if (MAP_name.Length < 8) {
                                MAP_name += mapTemplate[i].ToString();
                            } else if (MAP_x_string.Length < 2) {
                                MAP_x_string += mapTemplate[i].ToString();
                            } else if (MAP_y_string.Length < 2) {
                                MAP_y_string += mapTemplate[i].ToString();
                            } else {
                                mapTilesDataAll += mapTemplate[i].ToString();
                            }
                        }
                    }
                }
            }
            MAP_SIZE_X = Convert.ToInt32(MAP_x_string);
            MAP_SIZE_Y = Convert.ToInt32(MAP_y_string);

            var testMapa2D = new List<List<string>>();
            int index = 0;
            for (int i = 0; i < MAP_SIZE_Y; i++) {
                List<string> rowOfRooms = new List<string>();
                for (int j = 0; j < MAP_SIZE_X; j++) 
                    {
                    switch (mapTilesDataAll[index++].ToString()) {
                        case "0":
                        rowOfRooms.Add(hiddenOrEmptyRoom);
                            break;
                        case "1":
                            // To not showin at start walls diplay these as empty room
                            //     after explorate it should be visible ? 
                            // TODO: Chave to change this to adding walls coorinates on the fly, currently i had to first let draw map with walls , 
                            //     then check/ find "walls" and last redraw map to show empty/hidden room 
                            rowOfRooms.Add(wall);
                            break;
                        case "X":
                            rowOfRooms.Add(currentPlayerLocalisation);
                            player_position_X_from_file = j;
                            player_position_Y_from_file = i;
                            break;
                        case "?":
                            // Same as upper, hide at the first run
                            //     just display hidden room => dont showing nothing
                            //rowOfRooms.Add(roomWithItemReward);
                            rowOfRooms.Add(hiddenOrEmptyRoom);
                            break;
                    }
                    }
                testMapa2D.Add(rowOfRooms);
            };
            return testMapa2D;
        }
    
        public void ExtractCoordinatesOfWallsInMapFromFileAndSaveInMemory() {

            for (int i = 0; i < MAP_SIZE_Y; i++) {
                for (int j = 0; j < MAP_SIZE_X; j++) {
                    if (_mapa2D[i][j] == wall) {
                        int x= i, y = j;
                        FOVData WallCoords = new FOVData(x, y);
                        _wallsInMap.Add(WallCoords);
                        _mapa2D[i][j] = hiddenOrEmptyRoom;
                    }
                }
            }
        }
  
        
        public void DEBUG_Commands(string command) {
            List<RewardItems> hiddenObjects = rewardItemsList;
            if(command == "uh-") {
                UnhideTreasuresOnMap(hiddenObjects);
            }
            else if (command == "uh+") { 
                HidePlaceTreasuresOnMap(hiddenObjects);
            }

            if (command == "rm") {
                ShowMap();
            }

        }
    
    }
}