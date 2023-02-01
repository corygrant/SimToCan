using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterfaces
{
    public interface ICanInterface
    {
        bool Init();
        bool Start();
        bool Stop();
        bool Write(CanData canData);

    }
}
