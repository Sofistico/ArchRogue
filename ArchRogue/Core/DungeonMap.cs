﻿using OpenTK;
using RLNET;
using RogueSharp;
using System.Collections.Generic;
using System.Linq;

namespace ArchRogue.Core
{
    // Our custom DungeonMap class extends the base RogueSharp Map class
    public class DungeonMap : Map
    {
        //Generate rooms
        public List<Rectangle> Rooms;

        //Doors
        public List<Door> Doors { get; set; }

        //Stars up and down
        public Stairs StairsUp{ get; set; }
        public Stairs StairsDown{ get; set; }

        //Adds monsters to read
        private readonly List<Monster> _monsters;

        public DungeonMap()
        {
            //Clear the monsters to make sure that they will not try to act
            Game.SchedulingSystem.Clear();
            Rooms = new List<Rectangle>();
            //Initilize the lists for our map
            _monsters = new List<Monster>();
            Doors = new List<Door>();
        }

        // The Draw method will be called each time the map is updated
        // It will render all of the symbols/colors for each cell to the map sub console
        public void Draw(RLConsole mapConsole,RLConsole statConsole)
        {
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }

            // Keep an index so we know which position to draw monster stats at
            int i = 0;
            // Iterate through each monster on the map and draw it after drawing the Cells
            foreach (Monster monster in _monsters)
            {
                monster.Draw(mapConsole, this);
                // When the monster is in the field-of-view also draw their stats
                if (IsInFov(monster.X, monster.Y))
                {
                    // Pass in the index to DrawStats and increment it afterwards
                    monster.DrawStats(statConsole, i);
                    i++;
                }
            }

            foreach(Door door in Doors)
            {
                door.Draw(mapConsole, this);
            }

            // Add the following code after we finish drawing doors.
            StairsUp.Draw(mapConsole, this);
            StairsDown.Draw(mapConsole, this);
        }

        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            // When we haven't explored a cell yet, we don't want to draw anything
            if (!cell.IsExplored)
            {
                return;
            }

            // When a cell is currently in the field-of-view it should be drawn with ligher colors
            if (IsInFov(cell.X, cell.Y))
            {
                // Choose the symbol to draw based on if the cell is walkable or not
                // '.' for floor and '#' for walls
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            // When a cell is outside of the field of view draw it with darker colors
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }

        // This method will be called any time we move the player to update field-of-view
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            // Compute the field-of-view based on the player's location and awareness
            ComputeFov(player.X, player.Y, player.Awareness, true);
            // Mark all cells in field-of-view as having been explored
            foreach(Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }

        }

        // Returns true when able to place the Actor on the cell or false otherwise
        public bool SetActorPosition(Actor actor, int x, int y)
        {
            // Only allow actor placement if the cell is walkable
            if(GetCell(x, y).IsWalkable)
            {
                // The cell the actor was previously on is now walkable
                SetIsWalkable(actor.X, actor.Y, true);
                //Update actor position
                actor.X = x;
                actor.Y = y;
                //The new cell the actor is on is now unwalkable
                SetIsWalkable(actor.X, actor.Y, false);

                // Try to open a door if one exists here
                OpenDoor(actor, x, y);

                //Update the field of view of the player after walking
                if (actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        //A helper method for setting the IsWalkable property on a Cell
        public void SetIsWalkable(int x, int y, bool IsWalkable)
        {
            Cell cell = (Cell)GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, IsWalkable, cell.IsExplored);
        }

        // Sets the player position so that the MapGenerator can call it
        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
            Game.SchedulingSystem.Add( player );
        }

        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);
            // After adding the monster to the map make sure to make the cell not walkable
            SetIsWalkable(monster.X, monster.Y, false);
            Game.SchedulingSystem.Add(monster);

        }

        //Method to remove monster from dungeon
        public void RemoveMonster(Monster monster)
        {
            _monsters.Remove(monster);
            //after removing monster from map, make sure the cell is walkable again
            SetIsWalkable(monster.X, monster.Y, true);
            Game.SchedulingSystem.Remove(monster);

        }

        //Helper method to get monster
        public Monster GetMonsterAt(int x, int y)
        {
            return _monsters.FirstOrDefault(m => m.X == x && m.Y == y);
        }

        // Look for a random location in the room that is walkable.
        public Point GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if (DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Game.Random.Next(1, room.Width - 2) + room.X;
                    int y = Game.Random.Next(1, room.Height - 2) + room.Y;
                    if (IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }

            // If we didn't find a walkable location in the room return null
            return default;
        }

        // Iterate through each Cell in the room and return true if any are walkable
        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for(int x = 1; x <= room.Width - 2; x++)
            {
                for(int y = 1; y <= room.Width - 2; y++)
                {
                    if(IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Create a method to place a player into a room without monsters in there
        /*public bool StarterRoom(Player player)
        {
            player = Game.Player;
      
        }*/

        // Return the door at the x,y position or null if one is not found.
        public Door GetDoor(int x, int y)
        {
            return Doors.SingleOrDefault(d => d.X == x && d.Y == y);
        }

        // The actor opens the door located at the x,y position
        private void OpenDoor(Actor actor, int x, int y)
        {
            Door door = GetDoor(x, y);
            if (door != null && !door.IsOpen)
            {
                door.IsOpen = true;
                var cell = GetCell(x, y);
                // Once the door is opened it should be marked as transparent and no longer block field-of-view
                SetCellProperties(x, y, true, cell.IsWalkable, cell.IsExplored);
            }
        }

        //Create a method to close the door by pressing a key
        private void CloseDoor (Actor actor, int x, int y)
        {
            Door door = GetDoor(x, y);
            if (door != null && door.IsOpen)
            {
                door.IsOpen = false;
                var cell = GetCell(x, y);
                //Once the door is closed, it should return as transparent
                SetCellProperties(x, y, false, cell.IsWalkable, cell.IsExplored);
            }
        }

        public bool CanMoveDownToNextLevel()
        {
            Player player = Game.Player;
            return StairsDown.X == player.X && StairsDown.Y == player.Y;
        }
    }

}
