﻿using ArchRogue.Behavior;
using ArchRogue.Systems;
using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchRogue.Core
{
    public class Monster : Actor
    {
        public int? TurnsAlerted { get; set; }

        public void DrawStats(RLConsole statConsole, int position)
        {
            //Starts at y=13, which is lower than player stats
            // Multiply the position by 2 to leave a space between each stat
            int yPosition = 13 + (position * 2);

            // Begin the line by printing the symbol of the monster in the appropriate color
            statConsole.Print(1, yPosition, Symbol.ToString(), Color);

            // Figure out the width of the health bar by dividing current health by max health
            int width = Convert.ToInt32((double)Health / (double)MaxHealth * 16.0);
            int remainingWidth = 16 - width;

            // Set the background colors of the health bar to show how damaged the monster is
            statConsole.SetBackColor(3, yPosition, width, 1, Palette.Primary);
            statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Palette.PrimaryDarkest);

            // Print the monsters name over top of the health bar
            statConsole.Print(2, yPosition, $": {Name}", Palette.DbLight);
        }
        public virtual void PerformAction(CommandSystem commandSystem)
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, commandSystem);
        }
    }
}
