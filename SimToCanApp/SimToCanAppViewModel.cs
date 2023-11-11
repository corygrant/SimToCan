using CanInterfaces;
using SimInterfaces;
using SimToCanApp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace SimToCan
{
    enum CAN_Interface
    {
        USB2CAN,
        PCAN
    }

    enum Sim
    {
        ACC,
        DR2,
        F1_2020,
        WRC
    }

    internal class SimToCanAppViewModel : ViewModelBase
    {
        private CAN_Interface CAN_INTERFACE = CAN_Interface.USB2CAN;
        private Sim SIM = Sim.DR2;

        private ICanInterface can;
        private ISimInterface sim;

        private bool _isStarted;
        public SimToCanAppViewModel()
        {
            Sims = new ObservableCollection<SimInterfaces>()
            {
                new SimInterfaces(){ Id=1, Name="ACC"},
                new SimInterfaces(){ Id=2, Name="DR2"},
                new SimInterfaces(){ Id=3, Name="F1_2020"},
                new SimInterfaces(){ Id=4, Name="WRC"}
            };

            Cans = new ObservableCollection<CanInterfaces>()
            {
                new CanInterfaces(){ Id=1, Name="USB2CAN"},
                new CanInterfaces(){ Id=2, Name="PCAN"}
            };

            StartBtnCmd = new SimToCanApp.RelayCommand(Start, CanStart);
            StopBtnCmd = new SimToCanApp.RelayCommand(Stop, CanStop);

            StartBtnColor = new SolidColorBrush(Colors.Gray);
            StopBtnColor = new SolidColorBrush(Colors.Red);

            int canId = (int)Settings.Default.SelectedCanInterface;
            int simId = (int)Settings.Default.SelectedSimInterface;

            if(canId > 0)
            {
                SelectedCan = Cans.First(x => x.Id == canId);
            }
            
            if(simId > 0)
            {
                SelectedSim = Sims.First(x => x.Id == simId);
            }
        }
        public void WindowClosing()
        {
            Settings.Default.SelectedCanInterface = SelectedCan.Id;
            Settings.Default.SelectedSimInterface = SelectedSim.Id;
            Settings.Default.Save();
            Stop(null);
        }


        private void Start(object parameter)
        {
            switch (SelectedCan.Name)
            {
                case "USB2CAN":
                    can = new USB2CAN();
                    break;

                case "PCAN":
                    can = new PCAN();
                    break;
            }

            switch (SelectedSim.Name)
            {
                case "ACC":
                    sim = new ACC();
                    break;

                case "DR2":
                    sim = new DR2();
                    break;
                case "F1_2020":
                    sim = new F1_2020();
                    break;
                case "WRC":
                    sim = new WRC();
                    break;
            }

            sim.DataUpdated += Sim_DataUpdated;

            if (can.Init())
            {
                if (can.Start())
                {
                    sim.Start();
                    _isStarted = true;
                    StartBtnColor = new SolidColorBrush(Colors.Green);
                }
            }
        }

        private bool CanStart(object parameter)
        {
            return ((SelectedCan != null && SelectedSim != null) && !_isStarted);
        }

        private void Stop(object parameter)
        {
            if(can != null)can.Stop();
            StartBtnColor = new SolidColorBrush(Colors.Gray);
            _isStarted = false;
        }

        private bool CanStop(object parameter)
        {
            return _isStarted;
        }

        private void Sim_DataUpdated(object sender, SimDataEventArgs e)
        {
            var data = new CanData();

            data.Id = 0x64;
            data.Len = 8;
            data.Payload = SimDash.Messages.Build100(e.simData.Speed, e.simData.FuelLevel, e.simData.Rpm, e.simData.BrakeBias);
            can.Write(data);

            data.Id = 0x65;
            data.Len = 8;
            data.Payload = SimDash.Messages.Build101(e.simData.TirePressure);
            can.Write(data);

            data.Id = 0x66;
            data.Len = 8;
            var bd = new SimDash.Messages.BoolData();
            bd.Ign = e.simData.IgnOn;
            bd.Running = e.simData.EngineOn;
            bd.PitLim = e.simData.PitLimOn;
            bd.Abs = e.simData.Abs;
            bd.Tc = e.simData.Tc;
            if (e.simData.MaxRpm < 3000) e.simData.MaxRpm = 7000;
            data.Payload = SimDash.Messages.Build102(e.simData.Gear, bd, e.simData.MaxRpm);
            can.Write(data);

            data.Id = 0x67;
            data.Len = 8;
            data.Payload = SimDash.Messages.Build103(e.simData.BrakeTemp);
            can.Write(data);
        }

        #region ComboBoxes
        private ObservableCollection<SimInterfaces> _sims;
        public ObservableCollection<SimInterfaces> Sims
        {
            get { return _sims; }
            set { _sims = value; }
        }

        private SimInterfaces _selectedSim;
        public SimInterfaces SelectedSim
        {
            get { return _selectedSim; }
            set { _selectedSim = value; }
        }

        private ObservableCollection<CanInterfaces> _cans;
        public ObservableCollection<CanInterfaces> Cans
        {
            get { return _cans; }
            set { _cans = value; }
        }
        private CanInterfaces _selectedCan;
        public CanInterfaces SelectedCan
        {
            get { return _selectedCan; }
            set { _selectedCan = value; }
        }
        #endregion

        #region Buttons
        private Brush _startBtnColor;
        public Brush StartBtnColor
        {
            get => _startBtnColor;
            set
            {
                _startBtnColor= value;
                OnPropertyChanged("StartBtnColor");
            }
        }
        private Brush _stopBtnColor;
        public Brush StopBtnColor
        {
            get => _stopBtnColor;
            set
            {
                _stopBtnColor = value;
                OnPropertyChanged("StopBtnColor");
            }
        }
        public ICommand StartBtnCmd
        {
            get; set;
        }
        public ICommand StopBtnCmd
        {
            get; set;
        }
        #endregion
    }

    public class SimInterfaces
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }

    public class CanInterfaces
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
