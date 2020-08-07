using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SimpleCharacterManagment
{
    public class Hero : LivingBeing {

        private string _characterClass;
        private int _experiencePoints;
        private int _currentLocationX;
        private int _currentLocationY;
        private int _oldLocationX;
        private int _oldLocationY;
        private int _flashlightBatteryLevel;
        private List<FOVData> _oldListOfRoomsPlayerCanSeeUsingFlashlight;
        private List<FOVData> _currentLlistOfRoomsPlayerCanSeeUsingFlashlight;
        private int _flashlightPower;
        
        // CLASSIC RPG CHARACTER STATS
        public string CharacterClass {
            get { return _characterClass; }
            set { _characterClass = value; }
        }
        public int ExperiencePoints {
            get { return _experiencePoints; }
            set { _experiencePoints = value; }
        }
        // LOCALIZATION
        public int CurrentLocationX {
            get { return _currentLocationX; }
            set { _currentLocationX = value; }
        }
        public int CurrentLocationY {
            get { return _currentLocationY; }
            set { _currentLocationY = value; }
        }
        public int OldLocationX {
            get { return _oldLocationX; }
            set { _oldLocationX = value; }
        }
        public int OldLocationY {
            get { return _oldLocationY; }
            set { _oldLocationY = value; }
        }
        public List<FOVData> CurrentListOfRoomsPlayerCanSeeUsingFlashlight {
            get { return _currentLlistOfRoomsPlayerCanSeeUsingFlashlight; }
             set { _currentLlistOfRoomsPlayerCanSeeUsingFlashlight = value; }
        }
        public List<FOVData> OldListOfRoomsPlayerCanSeeUsingFlashlight {
            get { return _oldListOfRoomsPlayerCanSeeUsingFlashlight; }
            set { _oldListOfRoomsPlayerCanSeeUsingFlashlight = value; }
        }
        // ACCESORIES, ITEMS, OTHER STUFF
        public int FlashlightPower 
        { 
            get { return _flashlightPower; }
            set { 
                if(value >= 0) {
                    _flashlightPower = value; 
                    Debug.WriteLine($"Ustawiono moc latarki na wartość : {_flashlightPower}.");    
                }
            }
        }
        public int FlashlightBatteryLevel {
            get { 
                return _flashlightBatteryLevel; 
            }
            set {
                _flashlightBatteryLevel = value;
                if (_flashlightBatteryLevel <= 0) {
                    FlashlightPower-=2;
                    _flashlightBatteryLevel = 0;
                }
            }
        }

        public Hero(string name, int age, int maxHitPoints, int currentHitPoints, int gold, string characterClass, int experiencePoints, int _flashlightBatteryLevel = 100, int _currentLocationX = 0, int _currentLocationY = 0, int level = 1) :
               base(name, age, maxHitPoints, currentHitPoints, gold, level) {
            CurrentLocationX = _currentLocationX;
            CurrentLocationY = _currentLocationY;
            CharacterClass = characterClass;
            ExperiencePoints = experiencePoints;
            FlashlightBatteryLevel = _flashlightBatteryLevel;
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
            DrainFlashLightBatteryLevel();
        }
        public void SaveOldLocation(int x , int y) {
            OldLocationX = x;
            OldLocationY = y;
        }
        private void DrainFlashLightBatteryLevel() {
            if(_flashlightBatteryLevel > 0) {
                if (_flashlightPower >= 3) {
                    FlashlightBatteryLevel -= 5;
                } else {
                    FlashlightBatteryLevel -= 1;
                }
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

        public FOVData GetPlayerCurrentLocation() {
            return new FOVData(_currentLocationX, _currentLocationY);
        }

        // Generate list of coordinates based on current location, which is in range of player view 
        private List<FOVData> GenerateFieldOfView(int fov_range, int current_x, int current_y) {
            // Generate list of counters for example: of range 3 it would be <3,2,1,0,1,2,3>
            List<int> counter = new List<int>();
            for (int i = 0; i >= 0;) {
                if (i < fov_range && counter.Count < fov_range + 1) {
                    counter.Add(i);
                    i++;
                }
                if (i <= fov_range && counter.Count >= fov_range) {
                    counter.Add(i);
                    i--;
                }
            }
            int fov = fov_range, x, y;

            // Creating new list based on counter list, bassically counters work is hold information 
            //    for how manny indexes should loop shift x value, for range <-3,+3> when counter = 3
            List<FOVData> FieldOfViewCoordsDict = new List<FOVData>();
            for (int i = 0; i < counter.Count; i++) {
                y = current_y + fov;

                if (counter[i] <= 0) {
                    x = current_x + counter[i];
                    FieldOfViewCoordsDict.Add(new FOVData(x, y));
                } else {
                    for (int j = (-counter[i]); j <= counter[i]; j++) {
                        x = current_x + j;
                        FieldOfViewCoordsDict.Add(new FOVData(x, y));
                    }
                }

                fov -= 1;
            }
            return FieldOfViewCoordsDict;
        }

        // Its return a passed value true/false depend on if we want use flashlight or not.
        //     this same time gener 
        public bool UseFlashlight(bool isTurnedOn) {
            // is is turned ON generate visible rooms , if not, then dont :D
            if (isTurnedOn) {
                // Override current map and reveal rooms in range of light
                _oldListOfRoomsPlayerCanSeeUsingFlashlight = GenerateFieldOfView(_flashlightPower, _oldLocationX, _oldLocationY);
                _currentLlistOfRoomsPlayerCanSeeUsingFlashlight = GenerateFieldOfView(_flashlightPower, _currentLocationX, _currentLocationY);
            }
            return isTurnedOn;
        }

        public void ExploreRoom(List<List<string>> mapa, List<RewardItems> rewardList, int x , int y ) {
            RewardItems reward = rewardList.Where(r => r.LocalizationX == x && r.LocalizationY == y && r.IsCollected == false).FirstOrDefault();
            if (reward != null) {
                Console.WriteLine($"You found a {reward.RewardType}, want to open,collect it ? t/n");
                if(Console.ReadLine().ToLower() == "t") {
                    Console.WriteLine(reward.Description);
                    AddExperience(reward.Experience);
                    AddGold(reward.Gold);
                    SetLevelAndMaximumHitPoints();
                    mapa[x][y] = Mapa.currentPlayerLocalisation;
                    reward.IsCollected = true;
                    if(reward.RewardType == "FlashlightBooster") {
                        FlashlightPower += reward.BoosterValue;
                        
                    }
                }
            }
        }

    }
}