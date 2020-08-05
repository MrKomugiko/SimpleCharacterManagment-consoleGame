using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCharacterManagment
{
    public class Hero : LivingBeing {
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
        public List<FOVData> GenerateFieldOfView(int fov_range = 1, int current_x = 3, int current_y = 3) {
            List<int> counter = new List<int>();
            for (int i = 0; i >= 0;) {
                if (i < fov_range && counter.Count < fov_range+1) {
                    counter.Add(i);
                    i++;
                }
                if (i <= fov_range && counter.Count >= fov_range) {
                    counter.Add(i);
                    i--;
                }
            }
            int fov = fov_range, x, y;
            List<FOVData> FieldOfViewCoordsDict = new List<FOVData>();
            int fov_range_const = fov_range;
            for (int i = 0; i < counter.Count; i++) {
            y = current_y + fov;

                //if (i > fov) {    
                //    if(counter[i] >= 0) {
                //        for (int j = (-counter[i]); j <= counter[i]; j++) {
                //            x = fov_range_const + j;
                //            FieldOfViewCoordsDict.Add(new FOVData(x, y));
                //        }
                //    }
                //} else {
                    if (counter[i] <= 0) {
                        x = fov_range_const + counter[i];
                        FieldOfViewCoordsDict.Add(new FOVData(x, y));
                    } else {
                        for (int j = (-counter[i]); j <= counter[i]; j++) {
                            x = fov_range_const + j;
                            FieldOfViewCoordsDict.Add(new FOVData(x, y));
                        }
                    }
                //}
                fov -= 1;
            }
            return FieldOfViewCoordsDict;
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


/*
 *         counter = 0;
ITERATION 0 -----------------------------------------------------------------------------------------------------------------------------------------
           for (i = 0; 0(i) <= 7; i++) {
            y = 3(current_y) + 3(fov);
                if (0(i) > 3(fov)) // F A L S E  powinno byc true
                 {    
                    for (-0(-counter); 0(j) <= 0(counter); j++) {
                        x = 3(fov_range_const) + 0(j);
                        FieldOfViewCoordsDict.Add(new FOVData(3(x), 6(y)));
                    }
                    -1(counter--);
                } else {
                    if (0(counter) == 0) T R U E <-------------------------------------- 
                    { 
                        x = 3(fov_range_const) + 0(counter);
                        FieldOfViewCoordsDict.Add(new FOVData(3(x), 6(y)));
                    } else {
                        for (-0(-counter); 0(j) <= 0(counter); j++) {
                            x = 3(fov_range_const) + -0(j);
                            FieldOfViewCoordsDict.Add(new FOVData(3(x), 6(y)));
                        }
                    }
                 1(counter++);
                }
                fov = 2;
            }
ITERATION 1 -----------------------------------------------------------------------------------------------------------------------------------------
           for (i = 1; 1(i) <= 7; i++) {
            y = 3(current_y) + 2(fov);
                if (1(i) > 2(fov)) // F A L S E powinno byc true
                 {  
                 //iteration 0
                    for (-1(-counter); -1(j) <= 1(counter); j++) { // T R U E
                        x = 3(fov_range_const) + -1(j);
                        FieldOfViewCoordsDict.Add(new FOVData(2(x), 5(y)));
                    }
                //iteration 1
                        for (-1(-counter); 0(j) <= 1(counter); j++) { // T R U E
                        x = 3(fov_range_const) + 0(j);
                        FieldOfViewCoordsDict.Add(new FOVData(3(x), 5(y)));
                    }
                //iteration 2
                        for (-1(-counter); 1(j) <= 1(counter); j++) { // T R U E
                        x = 3(fov_range_const) + 1(j);
                        FieldOfViewCoordsDict.Add(new FOVData(4(x), 5(y)));
                    }
                //iteration 3 // F A L S E 
                    -2(counter--);
                } else {
                    if (0(counter) == 0) T R U E <-------------------------------------- 
                    { 
                        x = 3(fov_range_const) + 0(counter);
                        FieldOfViewCoordsDict.Add(new FOVData(3(x), 6(y)));
                    } else {
                        for (-0(-counter); 0(j) <= 0(counter); j++) {
                            x = 3(fov_range_const) + -0(j);
                            FieldOfViewCoordsDict.Add(new FOVData(3(x), 6(y)));
                        }
                    }
                 1(counter++);
                }
                fov = 2;
            }
     */
