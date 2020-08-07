using System;
using System.Collections.Generic;

namespace SimpleCharacterManagment
{
    class Program
    {
        private static Mapa CurrentMap;
        private static int heroLastCoordX = 5;
        private static int heroLastCoordY = 5;

        static void Main(string[] args) {
            CurrentMap = new Mapa(Mapa.MAP_SIZE_X, Mapa.MAP_SIZE_Y);
            Hero bohater = new Hero("Kamil", 25, 100, 100, 500, "rougue",1, _currentLocationX: Mapa.player_position_X_from_file, _currentLocationY: Mapa.player_position_Y_from_file);
                 bohater.FlashlightPower = 2;
            //List<FOVData> test = bohater.GenerateFieldOfView();

            Console.WriteLine("--------------------STATS--------------------");
            Console.WriteLine($"\tGracz: {bohater.Name} | Poziom: {bohater.Level}.");
            // Console.WriteLine($" Twoj poziom wynosi {bohater.Level}");
            Console.WriteLine($"\tExp: {bohater.ExperiencePoints} | HP: {bohater.CurrentHitPoints} | Gold: {bohater.Gold}");
            Console.WriteLine($"\tFlashlight power : {bohater.FlashlightPower}⚡ | Battery: {bohater.FlashlightBatteryLevel}%");
            Console.WriteLine("---------------------------------------------");
            //CurrentMap.UpdateMap(bohater.CurrentLocationX, bohater.CurrentLocationY, bohater.CurrentLocationX, bohater.CurrentLocationY);
            //CurrentMap.ShowMap();

            Console.WriteLine("Press ENTER to START GAME, moving by key ARROWS");

            while (true) {
                bohater.SaveOldLocation(bohater.CurrentLocationX, bohater.CurrentLocationY);
                heroLastCoordX = bohater.CurrentLocationX;
                heroLastCoordY = bohater.CurrentLocationY;
                var ch = Console.ReadKey(false).Key;
                    switch (ch) {
                        case ConsoleKey.UpArrow:
                            bohater.MoveHeroToCoords(heroLastCoordX - 1, heroLastCoordY);
                            break;
                        case ConsoleKey.DownArrow:
                            bohater.MoveHeroToCoords(heroLastCoordX + 1, heroLastCoordY);
                            break;
                        case ConsoleKey.LeftArrow:
                            bohater.MoveHeroToCoords(heroLastCoordX, heroLastCoordY - 1);
                            break;
                        case ConsoleKey.RightArrow:
                            bohater.MoveHeroToCoords(heroLastCoordX, heroLastCoordY + 1);
                            break;
                        case ConsoleKey.Escape:
                            break;
                    }

                try {
                    // hardtyped turn ON flashing since start
                    if (bohater.UseFlashlight(true)) {
                        CurrentMap.UpdateMap(heroLastCoordX, heroLastCoordY, bohater.CurrentLocationX, bohater.CurrentLocationY);
                        CurrentMap.UpdateMapWhileUsingFlashlight(bohater.OldListOfRoomsPlayerCanSeeUsingFlashlight, heroLastCoordX, heroLastCoordY,
                                  bohater.CurrentListOfRoomsPlayerCanSeeUsingFlashlight,bohater.CurrentLocationX, bohater.CurrentLocationY);
                    } else {
                        CurrentMap.UpdateMap(heroLastCoordX, heroLastCoordY, bohater.CurrentLocationX, bohater.CurrentLocationY);
                    }
                } catch (ArgumentOutOfRangeException) {
                    CurrentMap.UpdateMap(heroLastCoordX, heroLastCoordY, heroLastCoordX, heroLastCoordY);
                }
                Console.Clear();

                Console.WriteLine("--------------------STATS--------------------");
                Console.WriteLine($"\tGracz: {bohater.Name} | Poziom: {bohater.Level}.");
                // Console.WriteLine($" Twoj poziom wynosi {bohater.Level}");
                Console.WriteLine($"\tExp: {bohater.ExperiencePoints} | HP: {bohater.CurrentHitPoints} | Gold: {bohater.Gold}");
                Console.WriteLine($"\tFlashlight power : {bohater.FlashlightPower} | Battery: {bohater.FlashlightBatteryLevel}%");
                Console.WriteLine("---------------------------------------------");

                CurrentMap.ShowMap();
                bohater.ExploreRoom(CurrentMap.Mapa2D,CurrentMap.rewardItemsList,bohater.CurrentLocationX, bohater.CurrentLocationY);


            }

        }
    }
}