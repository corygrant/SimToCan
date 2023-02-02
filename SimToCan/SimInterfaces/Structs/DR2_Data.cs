using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimInterfaces.Structs
{
    public class DR2_Data
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
        [Serializable]
        public struct Data
        {
            public float TotalTime;
            public float LapTime;
            public float DistanceDriven;
            public float TotalDistanceDriven;
            public float PositionX;
            public float PositionY;
            public float PositionZ;
            public float Speed; //m/s
            public float VelocityX;
            public float VelocityY;
            public float VelocityZ;
            public float RollVectorX;
            public float RollVectorY;
            public float RollVectorZ;
            public float PitchVectorX;
            public float PitchVectorY;
            public float PitchVectorZ;
            public float SuspensionPosRL;
            public float SuspensionPosRR;
            public float SuspensionPosFL;
            public float SuspensionPosFR;
            public float VelocitySuspensionRL;
            public float VelocitySuspensionRR;
            public float VelocitySuspensionFL;
            public float VelocitySuspensionFR;
            public float VelocityWheelRL;
            public float VelocityWheelRR;
            public float VelocityWheelFL;
            public float VelocityWheelFR;
            public float PositionThrottle;
            public float PositionSteering;
            public float PositionBrake;
            public float PositionClutch;
            public float Gear; //[0 = Neutral, -1 = Reverse]
            public float GForceLateral;
            public float GForceLongitudinal;
            public float Lap;
            public float RPM; //RPM/10
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float BrakeTempRL;
            public float BrakeTempRR;
            public float BrakeTempFL;
            public float BrakeTempFR;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public float Unknown18;
            public float TotalLaps;
            public float LengthOfTrack;
            public float Unknown19;
            public float MaxRPMs;
            public float IdleRPM;
            public float MaxGears;
        }
    }
}
