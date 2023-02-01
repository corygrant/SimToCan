using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimInterfaces
{
    public class SimDataEventArgs : EventArgs
    {
        public SimDataEventArgs(SimData simData)
        {
            this.simData = simData;
        }

        public SimData simData { get; private set; }
    }

    public class SimData
    {
        public int Speed { get; set; }
        public int Rpm { get; set; }
        public int[] TirePressure { get; set; } 
        public int[] BrakeTemp { get; set; }
        public int Gear { get; set; }
        public bool IgnOn { get; set; }
        public bool EngineOn { get; set; }
        public bool PitLimOn { get; set; }
        public bool Abs { get; set; }
        public bool Tc { get; set; }
        public double BrakeBias { get; set; }
        public int FuelLevel { get; set; }
        public int CarId { get; set; }
    }
}
