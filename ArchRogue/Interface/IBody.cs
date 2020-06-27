using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchRogue.Interface
{
    public interface IBody
    {
        int Head { get; set; }
        int Neck { get; set; }
        int Torso { get; set; }
        int Waist { get; set; }
        int R_Leg { get; set; }
        int L_Leg { get; set; }
        int R_Arm { get; set; }
        int L_Arm { get; set; }
    }
}
