using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchRogue.Core;
using ArchRogue.Interface;

namespace ArchRogue.Systems
{
    //Verify what i can get out of this class later.
    public class Body : IBody
    {
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
    }
}
