using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCharacterManagment
{
    public class Mapa
    {
        private List<List<string>> _mapa2D;
        public List<RewardItems> rewardItemsList = new List<RewardItems>();
        public const int MAP_SIZE_X = 10;
        public const int MAP_SIZE_Y = 10;

        public List<List<string>> Mapa2D { 
                        get { return _mapa2D; } 
                        set { _mapa2D = value; }
            }

        public Mapa(int rows, int columns) {
            // lista obiektow mapa labirynt [X] skarb?
            // 1 wygenerowanie pustej mapy
            List<List<string>> _mapa = new List<List<string>>();

            string room = " * ";
            for (int i = 0; i < columns; i++) {
                List<string> rowOfRooms = new List<string>();
                for (int j = 0; j < rows; j++) {
                    rowOfRooms.Add(room);
                }
                _mapa.Add(rowOfRooms);
            };

            Mapa2D = _mapa;


            //dodanie znajdziek
            rewardItemsList.Add(new RewardItems("treasureChest", 10, 100, "After tought battle with a lock \nREWARD: you get 50 Exp points and 100 Gold.", 2, 2, false));
            rewardItemsList.Add(new RewardItems("luckyFound", 0, 1000, "Great, you found a nice wallet full of money! \nREWARD: you get 1000 Gold.", 3, 7, false));
            rewardItemsList.Add(new RewardItems("badLuck", 100, -200, "You are the beast, but fighting like a puppy. ou got robbered after long fight, \n" +
                                                      "after all that was a nice exercise !\nREWARD: you get 100 exp points and lost 200 Gold. ", 1, 9, false));

            rewardItemsList.Add(RewardItems.CopyQuestFromLocationToNewLocation(rewardItemsList,1, 9,2,9));
            
        }

        public void UpdateMap(int oldX, int oldY, int x, int y) {
            // if we leave a treasure room return his state to ?
            // TODO: if we open chest, dont save ? mark delete this
            if(rewardItemsList.Where(r => r.LocalizationX == oldX && r.LocalizationY == oldY).FirstOrDefault() != null) 
            {
                if (rewardItemsList.Where(r => r.LocalizationX == oldX && r.LocalizationY == oldY).First().IsCollected == false) 
                    {
                    _mapa2D[oldX][oldY] = "[?]";
                }
                else if (rewardItemsList.Where(r => r.LocalizationX == oldX && r.LocalizationY == oldY).First().IsCollected == true) 
                    {
                    _mapa2D[oldX][oldY] = "[.]";
                }
            } else {
                _mapa2D[oldX][oldY] = "[ ]";
            }


            if (rewardItemsList.Where(r => r.LocalizationX == x && r.LocalizationY == y).FirstOrDefault() != null) {
                if (rewardItemsList.Where(r => r.LocalizationX == x && r.LocalizationY == y).First().IsCollected == false) {
                    _mapa2D[x][y] = "?X?";
                } else if (rewardItemsList.Where(r => r.LocalizationX == x && r.LocalizationY == y).First().IsCollected == true) {
                    _mapa2D[x][y] = "[X]";
                }
            } else {
                _mapa2D[x][y] = "[X]";
            }
        }

        public void ShowMap() {
            foreach (List<string> Location in _mapa2D) {
                Console.Write("\t");
                foreach (var room in Location) {
                    Console.Write(room);
                }
                Console.WriteLine();
            }
        }

        public void AddManyTreasureToRoomByCoordinatesFromList(List<RewardItems> treasures) {
            foreach (RewardItems item in treasures) {
                    _mapa2D[item.LocalizationX][item.LocalizationY] = "[?]";
            }
            // TODO: make a class TreasureChest, where would be stored data like gold, exp, monster?
        }


    }
}
