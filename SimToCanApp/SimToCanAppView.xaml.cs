using SimToCan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimToCanApp
{
    /// <summary>
    /// Interaction logic for SimToCanAppView.xaml
    /// </summary>
    public partial class SimToCanAppView : Window
    {
        private readonly SimToCanAppViewModel _vm;
        public SimToCanAppView()
        {
            InitializeComponent();
            _vm = new SimToCanAppViewModel();
            DataContext = _vm;

            Closing += SimToCanAppView_Closing;
        }

        private void SimToCanAppView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _vm.WindowClosing();
        }
    }
}
