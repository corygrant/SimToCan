using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SimInterfaces.Structs;

namespace SimInterfaces
{
    public class DR2 : ISimInterface
    {
        private DR2_Data.Data dr2Data = new DR2_Data.Data();
        private SimData simData = new SimData();

        private int[] _tirePressure = new int[4];
        private int[] _brakeTemp = new int[4];

        public DataUpdatedHandler DataUpdated { get; set; }

        public async void Start()
        {
            for(int i=0; i<4; i++)
            {
                _tirePressure[i] = 0;
                _brakeTemp[i] = 0;
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
                        int rawsize = Marshal.SizeOf(typeof(DR2_Data.Data));
                        if (recvBuffer.Length == rawsize)
                        {
                            dr2Data = (DR2_Data.Data)RawDeserialize(recvBuffer, 0, typeof(DR2_Data.Data));

                            //Format data into sim data format
                            simData.Rpm = Convert.ToInt16(dr2Data.RPM * 10.0);
                            simData.MaxRpm = Convert.ToInt16(dr2Data.MaxRPMs * 10.0);
                            if(dr2Data.Gear < 0)
                            {
                                simData.Gear = 9;
                            }
                            else
                            {
                                simData.Gear = Convert.ToInt16(dr2Data.Gear);
                            }

                            simData.Speed = Convert.ToInt16(dr2Data.Speed);
                            simData.IgnOn = false;
                            simData.EngineOn = false;
                            simData.PitLimOn = false;
                            simData.BrakeBias = 0.0;
                            simData.FuelLevel = 0;

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
        
    }
}
