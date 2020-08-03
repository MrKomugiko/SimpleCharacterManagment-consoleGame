using System;
using System.Collections.Generic;
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


        public RewardItems(string rewardType, int experience, int gold, string description, int localizationX, int localizationY, bool isCollected) {
            RewardType = rewardType;
            Experience = experience;
            Gold = gold;
            Description = description;
            LocalizationX = localizationX;
            LocalizationY = localizationY;
            IsCollected = isCollected;
        }

        public void EditQuestAtLocation(int x, int y) {

        }

    }
}
