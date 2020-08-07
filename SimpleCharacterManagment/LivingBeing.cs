using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCharacterManagment
{
    public abstract class LivingBeing
    {
        private string _name;
        private int _age;
        private int _maxHitPoints;
        private int _currentHitPoints;
        private int _level;
        private int _gold;

        public string Name {
            get { return _name; }
            private set { _name = value; }
        }
        public int Age {
            get {
                return _age;
            }
            private set {
                if (value < 0) {
                    Console.WriteLine("Próba zmiany wieku na wartośc ujemną, powrót do stanu '0'");
                    _age = 0;
                } else {
                    _age = value;
                }
            }
        }
        public int MaxHitPoints {
            get { return _maxHitPoints; }
             set { _maxHitPoints = value; }
        }
        public int CurrentHitPoints {
            get { return _currentHitPoints; }
            private set { _currentHitPoints = value; }
        }
        public int Level {
            get { return _level; }
            set {
                _level = value;
            }
        }
        public int Gold {
            get { return _gold; }
            set { _gold = value; }
        }

        public List<GameItem> Inventory { get; }

        public bool IsDead => CurrentHitPoints <= 0;

        protected LivingBeing(string name, int age, int maxHitPoints, int currentHitPoints,
                              int gold,int level = 1) {
            Name = name;
            Age = age;
            MaxHitPoints = maxHitPoints;
            CurrentHitPoints = currentHitPoints;
            Level = level;
            Gold = gold;

            Inventory = new List<GameItem>();
        }

        public void TakeDamage(int hitPointsOfDamage) {
            CurrentHitPoints -= hitPointsOfDamage;
            if (IsDead) {
                CurrentHitPoints = 0;
                RaiseOnKillMessage();
            }
        }
        private void RaiseOnKillMessage() {
            Console.WriteLine("You have been slain, gonna be ressurected in home.");
        }
        public void AddItemToInventory(GameItem item) {
            Inventory.Add(item);
        }
        public void RemoveItemToInventory(GameItem item) {
            Inventory.Remove(item);
        }
    }
}
