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
            CurrentMap.ExtractCoordinatesOfWallsInMapFromFileAndSaveInMemory();
            Hero bohater = new Hero("Kamil", 25, 100, 100, 500, "rougue",1, _currentLocationX: Mapa.player_position_X_from_file, _currentLocationY: Mapa.player_position_Y_from_file);
                 bohater.FlashlightPower = 2;
            //List<FOVData> test = bohater.GenerateFieldOfView();
           
            Console.Clear();
            start:
            Console.WriteLine("--------------------STATS--------------------");
            Console.WriteLine($"\tGracz: {bohater.Name} | Poziom: {bohater.Level}.");
            // Console.WriteLine($" Twoj poziom wynosi {bohater.Level}");
            Console.WriteLine($"\tExp: {bohater.ExperiencePoints} | HP: {bohater.CurrentHitPoints} | Gold: {bohater.Gold}");
            Console.WriteLine($"\tFlashlight power : {bohater.FlashlightPower}⚡ | Battery: {bohater.FlashlightBatteryLevel}%");
            Console.WriteLine("---------------------------------------------");
            //CurrentMap.UpdateMap(bohater.CurrentLocationX, bohater.CurrentLocationY, bohater.CurrentLocationX, bohater.CurrentLocationY);
            CurrentMap.ShowMap();
            //Console.WriteLine("Press ENTER to START GAME, moving by key ARROWS");
            
            while (true) {
                
                bohater.SaveOldLocation(bohater.CurrentLocationX, bohater.CurrentLocationY);
                heroLastCoordX = bohater.CurrentLocationX;
                heroLastCoordY = bohater.CurrentLocationY;
                var ch = Console.ReadKey(false).Key;
                    switch (ch) {
                        case ConsoleKey.UpArrow:
                            bohater.MoveHeroToCoords(CurrentMap, heroLastCoordX - 1, heroLastCoordY);
                            break;
                        case ConsoleKey.DownArrow:
                            bohater.MoveHeroToCoords(CurrentMap, heroLastCoordX + 1, heroLastCoordY);
                            break;
                        case ConsoleKey.LeftArrow:
                            bohater.MoveHeroToCoords(CurrentMap, heroLastCoordX, heroLastCoordY - 1);
                            break;
                        case ConsoleKey.RightArrow:
                            bohater.MoveHeroToCoords(CurrentMap, heroLastCoordX, heroLastCoordY + 1);
                            break;
                        case ConsoleKey.Escape:
                            break;
                    }

                try {
                    // hardtyped turn ON flashing since start
                    if (bohater.UseFlashlight(true)) {
                       // CurrentMap.UpdateMap(heroLastCoordX, heroLastCoordY, bohater.CurrentLocationX, bohater.CurrentLocationY);
                        CurrentMap.UpdateMapWhileUsingFlashlight(bohater.OldListOfRoomsPlayerCanSeeUsingFlashlight, heroLastCoordX, heroLastCoordY,
                                  bohater.CurrentListOfRoomsPlayerCanSeeUsingFlashlight,bohater.CurrentLocationX, bohater.CurrentLocationY);
                    } else {
                        CurrentMap.UpdateMap(heroLastCoordX, heroLastCoordY, bohater.CurrentLocationX, bohater.CurrentLocationY);
                    }
                } catch (ArgumentOutOfRangeException) {
                    CurrentMap.UpdateMap(heroLastCoordX, heroLastCoordY, heroLastCoordX, heroLastCoordY);
                }
                Console.Clear();
                DEBUG:

                Console.WriteLine("--------------------STATS--------------------");
                Console.WriteLine($"\tGracz: {bohater.Name} | Poziom: {bohater.Level}.");
                // Console.WriteLine($" Twoj poziom wynosi {bohater.Level}");
                Console.WriteLine($"\tExp: {bohater.ExperiencePoints} | HP: {bohater.CurrentHitPoints} | Gold: {bohater.Gold}");
                Console.WriteLine($"\tFlashlight power : {bohater.FlashlightPower} | Battery: {bohater.FlashlightBatteryLevel}%");
                Console.WriteLine("---------------------------------------------");

                CurrentMap.ShowMap();
                bohater.ExploreRoom(CurrentMap.Mapa2D,CurrentMap.rewardItemsList,bohater.CurrentLocationX, bohater.CurrentLocationY);
                if(ch == ConsoleKey.Escape) {
                    Console.WriteLine("---------------DEBUG SHORTCUTS---------------\n" +
                                      "\t[F+]  => Flashlight power Up\n" + // OK
                                      "\t[F-]  => Flashlight power Down\n" + // OK
                                      "\t[E+]  => Incerase EXP Points\n" + // OK
                                      "\t[B+]  => Charge battery\n" + // OK
                                      "\t[HP-]  => Take Damage to player\n" + // OK
                                      "\t[UH+] => Hide treasures\n" + // OK
                                      "\t[UH-] => Unreveal Hidden treasures\n" + // OK
                                      "\t[RM]  => Refresh Map\n" + // OK
                                      "\t[EXIT]\n" + // OK
                                      "---------------------------------------------\n");
                    switch (Console.ReadLine().ToLower()) {
                        case "f+":
                            bohater.DEBUG_Commands("f+");
                            break;
                        case "f-":
                            bohater.DEBUG_Commands("f-");
                            break;
                        case "e+":
                            bohater.DEBUG_Commands("e+");
                            break;
                        case "b+":
                            bohater.DEBUG_Commands("b+");
                            break;
                        case "hp-":
                            bohater.DEBUG_Commands("hp-");
                            break;
                        case "uh+":
                            CurrentMap.DEBUG_Commands("uh+");
                            break;
                        case "uh-":
                            CurrentMap.DEBUG_Commands("uh-");
                            break;
                        case "rm":
                            CurrentMap.DEBUG_Commands("rm");
                            break;
                        case "exit":
                            goto start;
                            
                        default:
                            break;
                    }
                    goto DEBUG;
                }
            }

        }
    }
}