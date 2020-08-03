using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCharacterManagment
{
    public class Hero : LivingBeing
    {
        private string _characterClass;
        private int _experiencePoints;
        private int _currentLocationX;
        private int _currentLocationY;

        public string CharacterClass { 
            get { return _characterClass; } 
            set { _characterClass = value; } 
        }
        public int CurrentLocationX {
            get { return _currentLocationX; }
            set { _currentLocationX = value; }
        }
        public int CurrentLocationY {
            get { return _currentLocationY; }
            set { _currentLocationY = value; }
        }
        public int ExperiencePoints { 
            get { return _experiencePoints; } 
            set { _experiencePoints = value; } 
        }

        public Hero(string name, int age, int maxHitPoints, int currentHitPoints, int gold, string characterClass, int experiencePoints, int _currentLocationX = 0, int _currentLocationY = 0, int level = 1) : 
               base(name, age, maxHitPoints, currentHitPoints, gold, level) {
            CurrentLocationX = _currentLocationX;
            CurrentLocationY = _currentLocationY;
            CharacterClass = characterClass;
            ExperiencePoints = experiencePoints;
        }

        public void AddExperience(int experiencePoints) {
            ExperiencePoints += experiencePoints;
            SetLevelAndMaximumHitPoints();
        }

        public void AddGold(int gold) {
            Gold += gold;
        }

        public void MoveHeroToCoords(int x, int y) {
            if ((x >= 0 && x < Mapa.MAP_SIZE_X) && (y >= 0 && y < Mapa.MAP_SIZE_Y)) {
                CurrentLocationX = x;
                CurrentLocationY = y;
            }
        }

        private void SetLevelAndMaximumHitPoints() {
            int originalLevel = Level;

            Level = (ExperiencePoints / 100) + 1;

            if (Level != originalLevel) {
                MaxHitPoints = Level * 10;
                Console.WriteLine($"Awansowałeś na {Level} poziom");
            }
        }

        public void GetLocation() {
            Console.WriteLine($"[{CurrentLocationX},{CurrentLocationY}]");
        }

        public void ExploreRoom(List<List<string>> mapa, List<RewardItems> rewardList, int x , int y ) {
            RewardItems reward = rewardList.Where(r => r.LocalizationX == x && r.LocalizationY == y && r.IsCollected == false).FirstOrDefault();
            if (reward != null) {
                Console.WriteLine("You found a treasure chest, want to open it ? t/n");
                if(Console.ReadLine().ToLower() == "t") {
                    Console.WriteLine(reward.Description);
                    AddExperience(reward.Experience);
                    AddGold(reward.Gold);
                    SetLevelAndMaximumHitPoints();
                    mapa[x][y] = "[X]";
                    reward.IsCollected = true;
                }
            }
        }

    }
}
