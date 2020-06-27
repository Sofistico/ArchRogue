using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchRogue.Systems;

namespace ArchRogue.Core
{
    public class Player : Actor
    {
        public Player()
        {
            Attack = 2;
            AttackChance = 50;
            Awareness = 15;
            Color = Colors.Player;
            Defense = 2;
            DefenseChance = 40;
            Gold = 0;
            Health = 10;
            MaxHealth = 10;
            MaxMana = 10;
            Mana = 10;
            Name = "Arch";
            Speed = 10;
            Symbol = '@';
            X = 10;
            Y = 10;
        }
        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Name:    {Name}", Colors.Text);
            statConsole.Print(1, 3, $"Health:  {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 5, $"Mana:    {Mana}/{MaxMana}", Colors.Text);
            statConsole.Print(1, 7, $"Attack:  {Attack} ({AttackChance}%)", Colors.Text);
            statConsole.Print(1, 9, $"Defense: {Defense} ({DefenseChance}%)", Colors.Text);
            statConsole.Print(1, 11, $"Gold:    {Gold}", Colors.Gold);
        }
    }
}
