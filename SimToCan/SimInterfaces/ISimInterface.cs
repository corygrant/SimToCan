using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimInterfaces
{
    public delegate void DataUpdatedHandler(object sender, SimDataEventArgs e);
    public interface ISimInterface
    {
        void Start();
        DataUpdatedHandler DataUpdated { get; set; }
    }
}
