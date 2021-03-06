﻿using RLNET;
using ArchRogue.Core;
using ArchRogue.Systems;
using RogueSharp.Random;
using System;

namespace ArchRogue
{
    public static class Game
    {
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;

        private static RLRootConsole _rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        //Sets maps and player
        public static Player Player { get; set; }
        public static DungeonMap DungeonMap { get; private set; }

        //Setting the message log
        public static MessageLog MessageLog { get; private set; }

        //Command keys
        private static bool _renderRequired = true;
        public static CommandSystem CommandSystem { get; private set; }

        //Time system
        public static SchedulingSystem SchedulingSystem { get; private set; }

        //Setting the seed and random things
        public static IRandom Random { get; private set; }

        //Setting the char name
        public static string PlayerName { get; private set; }

        private static int _mapLevel = 1;

        public static void Main()
        {
            //Setting the seed on base with the system time
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal8x8.png";

            // The title will appear at the top of the console 
            //Also include the seed code in the title
            string consoleTitle = $"RougeSharp V3 Tutorial - Level {_mapLevel} - Seed {seed}";

            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight,
              8, 8, 1f, consoleTitle);

            //Enter name
            /*_rootConsole.Print(30, 35, "What's yer name, traveler?", RLColor.White);
            PlayerName = Console.ReadLine();
            _rootConsole.Print(50, 35, PlayerName, RLColor.White);*/

            // Create a new MessageLog and print the random seed used to generate the level

            MessageLog = new MessageLog();
            MessageLog.Add("The rogue arrives on level 1");
            MessageLog.Add($"Level created with seed '{seed}'");

            //Initialize the sub consoles that we will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            //Instantiete a new Schedule system
            SchedulingSystem = new SchedulingSystem();

            //Sets maps generator
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7, _mapLevel);
            DungeonMap = mapGenerator.CreateMap();

            //Updater field of view
            DungeonMap.UpdatePlayerFieldOfView();

            //Command setting in main
            CommandSystem = new CommandSystem();

            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;

            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;

            // Set background color and text for each console 
            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Palette.DbDeepWater);
            _messageConsole.Print(1, 1, "Messages", Colors.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Palette.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

            // Begin RLNET's game loop
            _rootConsole.Run();
        }

        // Event handler for RLNET's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if (CommandSystem.IsPlayerTurn)
            {
                if (keyPress != null)
                {
                    if (keyPress.Key == RLKey.Keypad8)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                    }
                    else if (keyPress.Key == RLKey.Keypad2)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                    }
                    else if (keyPress.Key == RLKey.Keypad4)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                    }
                    else if (keyPress.Key == RLKey.Keypad6)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                    }
                    else if (keyPress.Key == RLKey.Keypad1)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.DownLeft);
                    }
                    else if (keyPress.Key == RLKey.Keypad3)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.DownRight);
                    }
                    else if (keyPress.Key == RLKey.Keypad7)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.UpLeft);
                    }
                    else if (keyPress.Key == RLKey.Keypad9)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.UpRight);
                    }

                    //To create a wait command later.
                    else if (keyPress.Key == RLKey.Keypad5)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Center);
                    }

                    else if (keyPress.Key == RLKey.Escape)
                    {
                        _rootConsole.Close();
                    }

                    else if (keyPress.Key == RLKey.Period)
                    {
                        if (DungeonMap.CanMoveDownToNextLevel())
                        {
                            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7, ++_mapLevel);
                            DungeonMap = mapGenerator.CreateMap();
                            MessageLog = new MessageLog();
                            CommandSystem = new CommandSystem();
                            _rootConsole.Title = $"RougeSharp RLNet Tutorial - Level {_mapLevel}";
                            didPlayerAct = true;
                        }
                    }
                }

                if (didPlayerAct)
                {
                    _renderRequired = true;
                    CommandSystem.EndPlayerTurn();
                }

            }         

            else
            {
                CommandSystem.ActivateMonsters();
                _renderRequired = true;
            }
        }

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            //Don't bother redrawing all of the consoles if nothing has changed.
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statConsole.Clear();
                _messageConsole.Clear();

                //Render map and player
                DungeonMap.Draw(_mapConsole, _statConsole);
                Player.Draw(_mapConsole, DungeonMap);
                Player.DrawStats(_statConsole);
                MessageLog.Draw(_messageConsole);

                //Tell RLNET wheere to blit the consoles
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                // Tell RLNET to draw the console that we set
                _rootConsole.Draw();

                _renderRequired = false;
            }           
        }
    }    
}