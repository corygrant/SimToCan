using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimInterfaces.Structs
{
    //Using wrc.json
    //Edit for custom1.json
    public class WRC_Data
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct Data
        {
            public ulong PacketID;
            public float GameTotalTime;
            public float GameDeltaTime;
            public ulong GameFrameCount;
            public float ShiftLightsFraction;
            public float ShiftLightsRPMStart;
            public float ShiftLightsRPMEnd;
            public byte ShiftLightsRPMValid;
            public byte GearIndex;
            public byte GearIndexNeutral;
            public byte GearIndexReverse;
            public byte GearIndexMax;
            public float Speed; //m/s
            public float TransmissionSpeed;
            public float PositionX;
            public float PositionY;
            public float PositionZ;
            public float VelocityX;
            public float VelocityY;
            public float VelocityZ;
            public float AccelX;
            public float AccelY;
            public float AccelZ;
            public float LeftDirectionX;
            public float LeftDirectionY;
            public float LeftDirectionZ;
            public float ForwardDirectionX;
            public float ForwardDirectionY;
            public float ForwardDirectionZ;
            public float UpDirectionX;
            public float UpDirectionY;
            public float UpDirectionZ;
            public float HubPosBL;
            public float HubPosBR;
            public float HubPosFL;
            public float HubPosFR;
            public float HubVeloBL;
            public float HubVeloBR;
            public float HubVeloFL;
            public float HubVeloFR;
            public float ContactPatchForwardBL;
            public float ContactPatchForwardBR;
            public float ContactPatchForwardFL;
            public float ContactPatchForwardFR;
            public float BrakeTempBL;
            public float BrakeTempBR;
            public float BrakeTempFL;
            public float BrakeTempFR;
            public float EngineMaxRPM;
            public float EngineIdleRPM;
            public float EngineCurrentRPM;
            public float ThrottlePos; //0 to 1
            public float BrakePos; //0 to 1
            public float ClutchPos; //0 to 1
            public float SteeringPos; //-1 to 1
            public float HandbrakePos; //0 to 1
            public float StageCurrentTime;
            public double StageCurrentDistance; //m
            public double StageLength; //m
        }
    }
}
