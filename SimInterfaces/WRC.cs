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
    public class WRC : ISimInterface
    {
        private WRC_Data.Data wrcData = new WRC_Data.Data();
        private SimData simData = new SimData();

        private int[] _tirePressure = new int[4];
        private int[] _brakeTemp = new int[4];

        public DataUpdatedHandler DataUpdated { get; set; }

        public async void Start()
        {
            for (int i = 0; i < 4; i++)
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
                        int rawsize = Marshal.SizeOf(typeof(WRC_Data.Data));
                        if (recvBuffer.Length == rawsize)
                        {
                            wrcData = (WRC_Data.Data)RawDeserialize(recvBuffer, 0, typeof(WRC_Data.Data));

                            //Format data into sim data format
                            simData.Rpm = Convert.ToInt16(wrcData.EngineCurrentRPM);
                            simData.MaxRpm = Convert.ToInt16(wrcData.EngineMaxRPM);
                            if (wrcData.GearIndex == wrcData.GearIndexReverse)
                            {
                                simData.Gear = 9;
                            }
                            else
                            {
                                simData.Gear = Convert.ToInt16(wrcData.GearIndex);
                            }

                            simData.Speed = Convert.ToInt16(wrcData.Speed * 2.23694);
                            simData.IgnOn = simData.Rpm > (wrcData.EngineIdleRPM - 10);
                            simData.EngineOn = simData.Rpm > (wrcData.EngineIdleRPM - 10);
                            simData.PitLimOn = false;
                            simData.BrakeBias = 0.0;
                            simData.FuelLevel = 0;

                            simData.TirePressure = _tirePressure;
                            _brakeTemp[0] = Convert.ToInt16(wrcData.BrakeTempBL);
                            _brakeTemp[1] = Convert.ToInt16(wrcData.BrakeTempBR);
                            _brakeTemp[2] = Convert.ToInt16(wrcData.BrakeTempFL);
                            _brakeTemp[3] = Convert.ToInt16(wrcData.BrakeTempFR);
                            simData.BrakeTemp = _brakeTemp;

                            simData.CarId = 2;

                            OnDataUpdated(new SimDataEventArgs(simData));
                        }
                    }
                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void OnDataUpdated(SimDataEventArgs e)
        {
            if (DataUpdated != null)
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
