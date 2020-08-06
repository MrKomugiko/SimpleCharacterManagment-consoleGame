using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SimpleCharacterManagment
{
    public class Mapa {
        private List<List<string>> _mapa2D;
        public List<RewardItems> rewardItemsList = new List<RewardItems>();
        public const int MAP_SIZE_X = 10;
        public const int MAP_SIZE_Y = 10;

        public List<List<string>> Mapa2D {
            get { return _mapa2D; }
            set { _mapa2D = value; }
        }

        public static string hiddenRoom = "   ";
        public static string emptyRoomImageString = "   ";
        public static string currentPlayerLocalisation = "[X]";
        public static string playerInRoomWithItemReward = "?X?";
        public static string roomWithItemReward = "[?]";
        public static string roomWithClearedItemReward = "[.]";
        public static string flashlightedEmptyRoom = "[ ]";

        public Mapa(int rows, int columns) {
            List<List<string>> _mapa = new List<List<string>>();

            for (int i = 0; i < columns; i++) {
                List<string> rowOfRooms = new List<string>();
                for (int j = 0; j < rows; j++) {
                    rowOfRooms.Add(hiddenRoom);
                }
                _mapa.Add(rowOfRooms);
            };

            Mapa2D = _mapa;


            //dodanie znajdziek
            rewardItemsList.Add(new RewardItems("treasureChest", 10, 100, "After tought battle with a lock \nREWARD: you get 50 Exp points and 100 Gold.", 2, 2, false));
            rewardItemsList.Add(new RewardItems("FlashlightBooster", 60, 10000, "Nice you gound a Extra Srong battery for your flashlight!!\n[ +1 Vision Area ]", 0, 0, false,1));
            rewardItemsList.Add(new RewardItems("luckyFound", 0, 1000, "Great, you found a nice wallet full of money! \nREWARD: you get 1000 Gold.", 3, 7, false));
            rewardItemsList.Add(new RewardItems("badLuck", 100, -200, "You are the beast, but fighting like a puppy. ou got robbered after long fight, \n" +
                                                      "after all that was a nice exercise !\nREWARD: you get 100 exp points and lost 200 Gold. ", 1, 9, false));

            rewardItemsList.Add(RewardItems.CopyQuestFromLocationToNewLocation(rewardItemsList, 1, 9, 2, 9));

            // Showing localisation of the rewardItems
            // AddManyTreasureToRoomByCoordinatesFromList(rewardItemsList);


        }

        /* Marking map based on old and curent move direction to check special mark when 
          Player step into ItemReward and depends when its collected or not */
        public void UpdateMap(int oldX, int oldY, int x, int y) {
            // if we leave a treasure room return his state to ?
            // TODO: if we open chest, dont save ? mark delete this
            if (CheckIfSomethingIsInRoom(oldX, oldY)) {
                if (CheckIfTreasureIsCollectedOnCoords(oldX, oldY) == false) {
                    _mapa2D[oldX][oldY] = roomWithItemReward;
                } else if (CheckIfTreasureIsCollectedOnCoords(oldX, oldY) == true) {
                    _mapa2D[oldX][oldY] = roomWithClearedItemReward;
                }
            } else {
                _mapa2D[oldX][oldY] = emptyRoomImageString;
            }

            /* Define how to mark Player current location when he walk into treasure
                 chest and how to mark when this chest were colected or not. */
            if (CheckIfSomethingIsInRoom(x, y)) {
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
            int TestCounter = 0;
            foreach (FOVData old_room in oldlightedRooms) {
                try {
                    TestCounter++;
                    _mapa2D[old_room.X_coord][old_room.Y_coord] = hiddenRoom;
                } 
                catch (ArgumentOutOfRangeException) {
                }
            }
                //---------------------------------------------------------------------------------------------------------------
                foreach (FOVData room in currentlightedRooms) {
                    try {
                        if (CheckIfSomethingIsInRoom(room.X_coord, room.Y_coord)) {
                            if (CheckIfTreasureIsCollectedOnCoords(room.X_coord, room.Y_coord)) {
                                _mapa2D[room.X_coord][room.Y_coord] = roomWithClearedItemReward;
                                Debug.WriteLine($"Wykryto pozostałości po znajdźcce => [{room.X_coord}][{room.X_coord}]");
                            } else {
                                _mapa2D[room.X_coord][room.Y_coord] = roomWithItemReward;

                                Debug.WriteLine($"Wykryto nie zebraną znajdźke w pobliżu! => [{room.X_coord}][{room.X_coord}]");
                            }
                        } else {
                            // if room is Empty and nothink there (wall etc.)
                            _mapa2D[room.X_coord][room.Y_coord] = flashlightedEmptyRoom;
                        }

                        // Change player position mark while in in room with treasure what isnt cleared
                        if ((currentx == room.X_coord) && (currenty == room.Y_coord)) {
                            // Mark player current position by [X]
                            _mapa2D[room.X_coord][room.Y_coord] = currentPlayerLocalisation;
                            if (CheckIfSomethingIsInRoom(room.X_coord, room.Y_coord)) {
                                if (!CheckIfTreasureIsCollectedOnCoords(room.X_coord, room.Y_coord)) {
                                    _mapa2D[room.X_coord][room.Y_coord] = playerInRoomWithItemReward;
                                    Debug.WriteLine($"Znajdujesz się w pokoju ze skarbem :D");
                                }
                            }
                           // Debug.WriteLine($"Pozycja gracza => [{room.X_coord}][{room.Y_coord}] ");
                        }
                    } catch (ArgumentOutOfRangeException) {

                    }
                    //if (_mapa2D[room.X_coord][room.Y_coord] == "[X]";)
                    //    _mapa2D[room.X_coord][room.Y_coord] = flashlightedEmptyRoom;

                }
        }

        // Return true if at specific location room contain any "RewardItem"
        private bool CheckIfSomethingIsInRoom(int coord_x,int coord_y) => this.rewardItemsList.Where(r => r.LocalizationX == coord_x && r.LocalizationY == coord_y).FirstOrDefault() != null ? true : false;     
        
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
        private void AddManyTreasureToRoomByCoordinatesFromList(List<RewardItems> treasures) {
            foreach (RewardItems item in treasures) {
                    _mapa2D[item.LocalizationX][item.LocalizationY] = "[?]";
            }
            // TODO: make a class TreasureChest, where would be stored data like gold, exp, monster?
        }


    }
}
