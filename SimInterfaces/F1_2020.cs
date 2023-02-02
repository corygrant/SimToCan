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
    public class F1_2020 : ISimInterface
    {
        private F1_2020_Data.PacketMotionData motionData = new F1_2020_Data.PacketMotionData();
        private F1_2020_Data.PacketSessionData sessionData = new F1_2020_Data.PacketSessionData();
        private F1_2020_Data.PacketLapData lapData = new F1_2020_Data.PacketLapData();
        private F1_2020_Data.PacketEventData eventData= new F1_2020_Data.PacketEventData();
        private F1_2020_Data.PacketParticipantsData participantsData= new F1_2020_Data.PacketParticipantsData();
        private F1_2020_Data.PacketCarSetupsData carSetupsData= new F1_2020_Data.PacketCarSetupsData();
        private F1_2020_Data.PacketCarTelemetryData carTelemetryData = new F1_2020_Data.PacketCarTelemetryData();
        private F1_2020_Data.PacketCarStatusData carStatusData = new F1_2020_Data.PacketCarStatusData();
        private F1_2020_Data.PacketFinalClassificationData finalClassificationData= new F1_2020_Data.PacketFinalClassificationData();
        private F1_2020_Data.PacketLobbyInfoData lobbyInfoData= new F1_2020_Data.PacketLobbyInfoData();

        private int _motionDataBit = 0x001;
        private int _sessionDataBit = 0x002;
        private int _lapDataBit = 0x004;
        private int _eventDataBit = 0x008;
        private int _participantDataBit = 0x010;
        private int _carSetupsDataBit = 0x020;
        private int _carTelemetryDataBit = 0x040;
        private int _carStatusDataBit = 0x080;
        private int _finalClassificationDataBit = 0x100;
        private int _lobbyInfoDataBit = 0x200;

        private int _motionDataRawSize;
        private int _sessionDataRawSize;
        private int _lapDataRawSize;
        private int _eventDataRawSize;
        private int _participantDataRawSize;
        private int _carSetupsDataRawSize;
        private int _carTelemetryDataRawSize;
        private int _carStatusDataRawSize;
        private int _finalClassificationDataRawSize;
        private int _lobbyInfoDataRawSize;

        private int _packetBits;

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

            simData.TirePressure = _tirePressure;
            simData.BrakeTemp = _brakeTemp;

            _motionDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketMotionData));
            _sessionDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketSessionData));
            _lapDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketLapData));
            _eventDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketEventData));
            _participantDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketParticipantsData));
            _carSetupsDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketCarSetupsData));
            _carTelemetryDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketCarTelemetryData));
            _carStatusDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketCarStatusData));
            _finalClassificationDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketFinalClassificationData));
            _lobbyInfoDataRawSize = Marshal.SizeOf(typeof(F1_2020_Data.PacketLobbyInfoData));

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
                        var headerData = (F1_2020_Data.HeaderData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.HeaderData));
                        switch (headerData.PacketId)
                        {
                            case F1_2020_Data.PacketId.Motion:
                                if (!(recvBuffer.Length == _motionDataRawSize)) break;
                                motionData = (F1_2020_Data.PacketMotionData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketMotionData));
                                _packetBits |= _motionDataBit;
                                break;

                            case F1_2020_Data.PacketId.Session:
                                if (!(recvBuffer.Length == _sessionDataRawSize)) break;
                                sessionData = (F1_2020_Data.PacketSessionData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketSessionData));
                                _packetBits |= _sessionDataBit;
                                break;

                            case F1_2020_Data.PacketId.LapData:
                                if (!(recvBuffer.Length == _lapDataRawSize)) break;
                                lapData = (F1_2020_Data.PacketLapData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketLapData));
                                _packetBits |= _lapDataBit;
                                break;

                            case F1_2020_Data.PacketId.Event:
                                if (!(recvBuffer.Length == _eventDataRawSize)) break;
                                eventData = (F1_2020_Data.PacketEventData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketEventData));
                                _packetBits |= _eventDataBit;
                                break;

                            case F1_2020_Data.PacketId.Participants:
                                if (!(recvBuffer.Length == _participantDataRawSize)) break;
                                participantsData = (F1_2020_Data.PacketParticipantsData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketParticipantsData));
                                _packetBits |= _participantDataBit;
                                break;

                            case F1_2020_Data.PacketId.CarSetups:
                                if (!(recvBuffer.Length == _carSetupsDataRawSize)) break;
                                carSetupsData = (F1_2020_Data.PacketCarSetupsData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketCarSetupsData));
                                _packetBits |= _carSetupsDataBit;
                                break;

                            case F1_2020_Data.PacketId.CarTelemetry:
                                if (!(recvBuffer.Length == _carTelemetryDataRawSize)) break;
                                //Car Telemetry
                                carTelemetryData = (F1_2020_Data.PacketCarTelemetryData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketCarTelemetryData));
                                simData.Speed = Convert.ToInt16((double)carTelemetryData.CarTelemetryData[0].Speed * 0.621371);
                                simData.Rpm = carTelemetryData.CarTelemetryData[0].EngineRPM;
                                if (carTelemetryData.CarTelemetryData[0].Gear < 0)
                                {
                                    simData.Gear = 9;
                                }
                                else
                                {
                                    simData.Gear = Convert.ToInt16(carTelemetryData.CarTelemetryData[0].Gear);
                                }

                                for(int i=0; i<4; i++)
                                {
                                    simData.TirePressure[i] = Convert.ToInt16(carTelemetryData.CarTelemetryData[0].TyresPressure[i]);
                                    simData.BrakeTemp[i] = carTelemetryData.CarTelemetryData[0].BrakesTemperature[i];
                                }
                                simData.IgnOn = carTelemetryData.CarTelemetryData[0].Drs == 1;

                                _packetBits |= _carTelemetryDataBit;
                                break;

                            case F1_2020_Data.PacketId.CarStatus:
                                if (!(recvBuffer.Length == _carStatusDataRawSize)) break;
                                carStatusData = (F1_2020_Data.PacketCarStatusData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketCarStatusData));
                                simData.PitLimOn = carStatusData.CarStatusData[0].PitLimiterStatus == 1;
                                //simData.Tc = carStatusData.CarStatusData[0].TractionControl > 0;
                                //simData.Abs = carStatusData.CarStatusData[0].AntiLockBrakes == 1;
                                simData.MaxRpm = carStatusData.CarStatusData[0].MaxRPM;
                                simData.FuelLevel = Convert.ToInt16(carStatusData.CarStatusData[0].FuelInTank);

                                _packetBits |= _carStatusDataBit;
                                break;

                            case F1_2020_Data.PacketId.FinalClassification:
                                if (!(recvBuffer.Length == _finalClassificationDataRawSize)) break;
                                finalClassificationData = (F1_2020_Data.PacketFinalClassificationData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketFinalClassificationData));
                                _packetBits |= _finalClassificationDataBit;
                                break;

                            case F1_2020_Data.PacketId.LobbyInfo:
                                if (!(recvBuffer.Length == _lobbyInfoDataRawSize)) break;
                                lobbyInfoData = (F1_2020_Data.PacketLobbyInfoData)RawDeserialize(recvBuffer, 0, typeof(F1_2020_Data.PacketLobbyInfoData));
                                _packetBits |= _lobbyInfoDataBit;
                                break;
                        }

                        //Use to only update when all packets are received
                        //NOTE: Only motion, lap data, car telemetry and car status are updated at frequency set in menu
                        //ONLY USE THOSE BITS for fast update rate
                        int checkBits = _motionDataBit | _lapDataBit | _carTelemetryDataBit | _carStatusDataBit;
                        if ((_packetBits & checkBits) == checkBits) //Telemetry and status received
                        //if((_packetBits & 0x1FF) == 0x1FF) //All packets received
                        {
                            _packetBits = 0;
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
