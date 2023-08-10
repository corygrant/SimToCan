using AssettoCorsaSharedMemory;
using System;

namespace SimInterfaces
{
    public class ACC : ISimInterface
    {
        private AssettoCorsa _ac = new AssettoCorsa();
        private bool _isSubscribed;

        private AC_STATUS _status;
        private float _maxFuel;
        private int _maxRpm;
        private AssettoCorsa.CarModel _carModel;

        private SimData _simData = new SimData();

        public DataUpdatedHandler DataUpdated { get; set; }

        public void Start()
        {
            //Set the variable update intervals
            _ac.StaticInfoInterval = 10000;
            _ac.PhysicsInterval = 50;
            _ac.GraphicsInterval = 1000;

            _ac.StaticInfoUpdated += Ac_StaticInfoUpdated;
            _ac.PhysicsUpdated += Ac_PhysicsUpdated;
            _ac.GraphicsUpdated += Ac_GraphicsUpdated;
            _ac.GameStatusChanged += Ac_GameStatusChanged;

            //Used to unsubscribe when ACC is not running
            _isSubscribed = true;

            //Start the interval timers
            _ac.Start();
        }
        private void OnDataUpdated(SimDataEventArgs e)
        {
            if (DataUpdated != null)
            {
                DataUpdated(this, e);
            }
        }

        private void Ac_GameStatusChanged(object sender, GameStatusEventArgs e)
        {
            Console.WriteLine("Status: " + e.GameStatus.ToString());
        }

        private void Ac_GraphicsUpdated(object sender, GraphicsEventArgs e)
        {
            _status = e.Graphics.Status;
        }

        private void Ac_PhysicsUpdated(object sender, PhysicsEventArgs e)
        {
            //Map ACC Physics vars to 
            _simData.Rpm = e.Physics.Rpms;
            _simData.MaxRpm = _maxRpm;
            _simData.IgnOn = e.Physics.IgnitionOn == 1;
            _simData.EngineOn = e.Physics.IsEngineRunning == 1;
            _simData.Tc = e.Physics.TC > 0.0;
            _simData.Abs = e.Physics.Abs > 0.0;
            _simData.PitLimOn = e.Physics.PitLimiterOn == 1;
            _simData.FuelLevel = Convert.ToInt16(e.Physics.Fuel);
            _simData.Speed = Convert.ToInt16((float)e.Physics.SpeedKmh * 0.621371);

            if ((e.Physics.Gear > 0) && (_status == AC_STATUS.AC_LIVE))
            {
                _simData.Gear = e.Physics.Gear - 1;
            }

            _simData.TirePressure = new int[4];
            _simData.BrakeTemp = new int[4];
            for (int i=0; i<4; i++)
            {
                _simData.TirePressure[i] = Convert.ToInt16(e.Physics.WheelsPressure[i]);
                _simData.BrakeTemp[i] = Convert.ToInt16(e.Physics.BrakeTemp[i]);
            }

            //Calculate brake bias
            //For some reason an offset is needed
            int brakeBiasOffset = 0;
            if (_status == AC_STATUS.AC_LIVE)
            {
                AssettoCorsa.BrakeBiasOffset.TryGetValue(_carModel, out brakeBiasOffset);
            }
            _simData.BrakeBias = ((e.Physics.BrakeBias * 100) + brakeBiasOffset);

            OnDataUpdated(new SimDataEventArgs(_simData));

        }

        private void Ac_StaticInfoUpdated(object sender, StaticInfoEventArgs e)
        {
            _maxFuel = e.StaticInfo.MaxFuel;
            _maxRpm = e.StaticInfo.MaxRpm;

            if (Enum.TryParse<AssettoCorsa.CarModel>(e.StaticInfo.CarModel, out _carModel))
            {
                _simData.CarId = Convert.ToByte(_carModel);
            }
            else
            {
                _simData.CarId = 0;
            }
        }
    }
}
