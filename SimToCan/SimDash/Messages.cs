using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimDash
{
    public class Messages
    {
        public class BoolData
        {
            public bool Tc { get; set; }
            public bool Abs { get; set; }
            public bool Ign { get; set; }
            public bool Running { get; set; }
            public bool PitLim { get; set; }
        }

        public static byte[] Build100(int mph, int fuel, int rpm, double brakeBias)
        {
            byte[] data = new byte[8];
            data[0] = Convert.ToByte(((uint)mph & 0xFF00) >> 8);
            data[1] = Convert.ToByte((uint)mph & 0x00FF);
            data[2] = Convert.ToByte(((uint)fuel & 0xFF00) >> 8);
            data[3] = Convert.ToByte((uint)fuel & 0x00FF);
            data[4] = Convert.ToByte(((uint)rpm & 0xFF00) >> 8);
            data[5] = Convert.ToByte((uint)rpm & 0x00FF);
            data[6] = Convert.ToByte(((uint)(brakeBias * 10.0) & 0xFF00) >> 8);
            data[7] = Convert.ToByte((uint)(brakeBias * 10.0) & 0x00FF);
            return data;
        }

        public static byte[] Build101(int[] tirePressure)
        {
            byte[] data = new byte[8];
            data[0] = Convert.ToByte((uint)tirePressure[0]);
            data[1] = Convert.ToByte((uint)tirePressure[1]);
            data[2] = Convert.ToByte((uint)tirePressure[2]);
            data[3] = Convert.ToByte((uint)tirePressure[3]);
            data[4] = 0;
            data[5] = 0;
            data[6] = 0;
            data[7] = 0;
            return data;
        }

        public static byte[] Build102(int gear, BoolData bd, int carId)
        {
            uint tc, abs, ign, running, pitLim;
            tc = (uint)(bd.Tc == true ? 1 : 0);
            abs = (uint)(bd.Abs == true ? 1 : 0);
            ign = (uint)(bd.Ign == true ? 1 : 0);
            running = (uint)(bd.Running == true ? 1 : 0);
            pitLim = (uint)(bd.PitLim == true ? 1 : 0);
            byte[] data = new byte[8];
            data[0] = Convert.ToByte((uint)gear);
            data[1] = Convert.ToByte((pitLim << 4) +
                                        (running << 3) +
                                        (ign << 2) +
                                        (abs << 1) +
                                        tc);
            data[2] = Convert.ToByte(carId);
            //data[2] = Convert.ToByte(((uint)maxRPM & 0xFF00) >> 8);
            //data[3] = Convert.ToByte((uint)maxRPM & 0x00FF);
            return data;
        }

        public static byte[] Build103(int[] brakeTemp)
        {
            byte[] data = new byte[8];
            data[0] = Convert.ToByte(((uint)brakeTemp[0] & 0xFF00) >> 8);
            data[1] = Convert.ToByte((uint)brakeTemp[0] & 0x00FF);
            data[2] = Convert.ToByte(((uint)brakeTemp[1] & 0xFF00) >> 8);
            data[3] = Convert.ToByte((uint)brakeTemp[1] & 0x00FF);
            data[4] = Convert.ToByte(((uint)brakeTemp[2] & 0xFF00) >> 8);
            data[5] = Convert.ToByte((uint)brakeTemp[2] & 0x00FF);
            data[6] = Convert.ToByte(((uint)brakeTemp[3] & 0xFF00) >> 8);
            data[7] = Convert.ToByte((uint)brakeTemp[3] & 0x00FF);
            return data;
        }
    }
}
