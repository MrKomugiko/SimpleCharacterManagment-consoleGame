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
