﻿using ArchRogue.Interface;
using RLNET;
using RogueSharp;

namespace ArchRogue.Core
{
    public class Actor : IActor, IDrawable, IScheduleable, IBody
    {
        //IActor
        private int _attack;
        private int _attackChance;
        private int _awareness;
        private int _defense;
        private int _defenseChance;
        private int _gold;
        private int _health;
        private int _maxHealth;
        private int _mana;
        private int _maxMana;
        private string _name;
        private int _speed;

        public int Attack
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = value;
            }
        }

        public int AttackChance
        {
            get
            {
                return _attackChance;
            }
            set
            {
                _attackChance = value;
            }
        }

        public int Awareness
        {
            get
            {
                return _awareness;
            }
            set
            {
                _awareness = value;
            }
        }

        public int Defense
        {
            get
            {
                return _defense;
            }
            set
            {
                _defense = value;
            }
        }

        public int DefenseChance
        {
            get
            {
                return _defenseChance;
            }
            set
            {
                _defenseChance = value;
            }
        }

        public int Gold
        {
            get
            {
                return _gold;
            }
            set
            {
                _gold = value;
            }
        }

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }

        public int MaxMana
        {
            get
            {
                return _maxMana;
            }
            set
            {
                _maxMana = value;

            }
        }

        public int Mana
        {
            get
            {
                return _mana;
            }
            set
            {
                _mana = value;

            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        //IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw(RLConsole console, IMap map)
        {
            // Don't draw actors in cells that haven't been explored
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }
            // Only draw the actor with the color and symbol when they are in field-of-view
            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set(X, Y, Color, Colors.FloorBackground, '.');
            }
        }

        // IScheduleable
        public int Time
        {
            get
            {
                return Speed;
            }
        }

        //IBody

        private int _head;
        private int _neck;
        private int _torso;
        private int _waist;
        private int _rleg;
        private int _lleg;
        private int _rarm;
        private int _larm;

        public int Head
        {
            get
            {
                return _head;
            }
            set
            {
                _head = value;
            }
        }
        public int Neck
        {
            get
            {
                return _neck;
            }
            set
            {
                _neck = value;
            }
        }
        public int Torso
        {
            get
            {
                return _torso;
            }
            set
            {
                _torso = value;
            }
        }
        public int Waist
        {
            get
            {
                return _waist;
            }
            set
            {
                _waist = value;
            }
        }
        public int R_Leg
        {
            get
            {
                return _rleg;
            }
            set
            {
                _rleg = value;
            }
        }
        public int L_Leg
        {
            get
            {
                return _lleg;
            }
            set
            {
                _lleg = value;
            }
        }
        public int R_Arm
        {
            get
            {
                return _rarm;
            }
            set
            {
                _rarm = value;
            }
        }
        public int L_Arm
        {
            get
            {
                return _larm;
            }
            set
            {
                _larm = value;
            }
        }

        //Method that gives a human a humanoid body
        public void HumanoidBody()
        {
            Head = 1;
            Neck = 1;
            Torso = 1;
            Waist = 1;
            L_Leg = 1;
            R_Leg = 1;
            L_Arm = 1;
            R_Arm = 1;
        }
    }
}
