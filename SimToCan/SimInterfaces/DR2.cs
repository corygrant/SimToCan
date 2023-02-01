using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimInterfaces
{
    public class DR2 : ISimInterface
    {
        private DR2_Data dr2Data = new DR2_Data();
        private SimData simData = new SimData();

        private int[] _tirePressure = new int[4];
        private int[] _brakeTemp = new int[4];

        public DataUpdatedHandler DataUpdated { get; set; }

        public async void Start()
        {
            for(int i=0; i<4; i++)
            {
                _tirePressure[i] = 28;
                _brakeTemp[i] = 500;
            }

            try
            {
                int PORT = 20777;
                UdpClient udpClient = new UdpClient();
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

                var from = new IPEndPoint(0, 0);
                await Task.Run(() =>
                {
                    while (true)
                    {
                        var recvBuffer = udpClient.Receive(ref from);
                        int rawsize = Marshal.SizeOf(typeof(DR2_Data));
                        if (recvBuffer.Length == rawsize)
                        {
                            dr2Data = (DR2_Data)RawDeserialize(recvBuffer, 0, typeof(DR2_Data));

                            //Format data into sim data format
                            simData.Rpm = Convert.ToInt16(dr2Data.RPM * 10);
                            if(dr2Data.Gear < 0)
                            {
                                simData.Gear = 9;
                            }
                            else
                            {
                                simData.Gear = Convert.ToInt16(dr2Data.Gear);
                            }

                            simData.Speed = Convert.ToInt16(dr2Data.Speed);
                            simData.IgnOn = true;
                            simData.EngineOn = true;
                            simData.PitLimOn = true;
                            simData.BrakeBias = 50.0;
                            simData.FuelLevel = 100;

                            simData.TirePressure = _tirePressure;
                            simData.BrakeTemp = _brakeTemp;

                            simData.CarId = 2;

                            OnDataUpdated(new SimDataEventArgs(simData));
                        }
                    }
                });

            }
            catch(Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void OnDataUpdated(SimDataEventArgs e)
        {
            if(DataUpdated != null)
            {
                DataUpdated(this, e);
            }
        }

        private object RawDeserialize(byte[] rawData, int position, Type anyType)
        {
            int rawsize = Marshal.SizeOf(anyType);
            if (rawsize > rawData.Length)
                return null;
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            object retobj = Marshal.PtrToStructure(buffer, anyType);
            Marshal.FreeHGlobal(buffer);
            return retobj;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
        [Serializable]
        private struct DR2_Data
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
