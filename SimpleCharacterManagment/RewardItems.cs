using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCharacterManagment
{
    public class RewardItems
    {
        //chest, random things , trash?
        public string RewardType { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public string Description { get; set; }
        public int LocalizationX { get; set; }
        public int LocalizationY { get; set; }
        public bool IsCollected { get; set; }
        public int BoosterValue { get; set; }


        public RewardItems(string rewardType, int experience, int gold, string description, int localizationX, int localizationY, bool isCollected, int boosterValue=0 ) {
            this.RewardType = rewardType;
            this.Experience = experience;
            this.Gold = gold;
            this.Description = description;
            this.LocalizationX = localizationX;
            this.LocalizationY = localizationY;
            this.IsCollected = isCollected;
            this.BoosterValue = boosterValue;

        }

        public static List<RewardItems> InitializeGameItemsList() {
            //dodanie znajdziek
            var rewardItemsList = new List<RewardItems>();
            rewardItemsList.Add(new RewardItems("treasureChest", 10, 100, "After tought battle with a lock \nREWARD: you get 50 Exp points and 100 Gold.", 8, 1, false));
            rewardItemsList.Add(new RewardItems("FlashlightBooster", 60, 10000, "Nice you gound a Extra Srong battery for your flashlight!!\n[ +1 Vision Area ]", 3, 3, false, 1));
            rewardItemsList.Add(new RewardItems("luckyFound", 0, 1000, "Great, you found a nice wallet full of money! \nREWARD: you get 1000 Gold.", 5, 5, false));
            rewardItemsList.Add(new RewardItems("badLuck", 100, -200, "You are the beast, but fighting like a puppy. ou got robbered after long fight, \n" +
                                                      "after all that was a nice exercise !\nREWARD: you get 100 exp points and lost 200 Gold. ", 9, 1, false));

            //rewardItemsList.Add(RewardItems.CopyQuestFromLocationToNewLocation(rewardItemsList, 9,1, 9, 8));

            // Showing localisation of the rewardItems
            // AddManyTreasureToRoomByCoordinatesFromList(rewardItemsList);
            return rewardItemsList;
        }

        public static RewardItems CopyQuestFromLocationToNewLocation(List<RewardItems> list, int fromOldXcoord, int fromOldYcoord, int toNewXcoord, int toNewYcoord) {
            
            RewardItems item = list.Where(l => l.LocalizationX == fromOldXcoord && l.LocalizationY == fromOldYcoord).FirstOrDefault();
            if (item != null) {
                item.LocalizationX = toNewXcoord;
                item.LocalizationY = toNewYcoord;
            }
            return item;
        }

    }
}
