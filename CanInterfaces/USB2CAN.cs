using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterfaces
{
    public class USB2CAN : ICanInterface
    {
        private SerialPort _serial;

        public bool Init()
        {
            try
            {
                _serial = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);
                _serial.Handshake = Handshake.None;
                _serial.DataReceived += _serial_DataReceived;
                _serial.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var ser = (SerialPort)sender;
            Console.WriteLine(ser.ReadExisting());
        }

        public bool Start()
        {
            if (!_serial.IsOpen) return false;

            byte[] data = new byte[8];
            try
            {
                //Open slcan
                data[0] = (byte)'O';
                _serial.Write(data, 0, 1);

                //Get version
                //data[0] = (byte)'V';
                //_serial.Write(data, 0 , 1);

                //Set bitrate
                //data[0] = (byte)'S';
                //data[1] = 6;
                //_serial.Write(data, 0 , 1);

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;

        }

        public bool Stop()
        {
            if(_serial == null) return false;
            if (!_serial.IsOpen) return false;

            byte[] data = new byte[8];
            data[0] = (byte)'C';
            _serial.Write(data, 0, 1);
            _serial.Close();

            return true;
        }

        public bool Write(CanData canData)
        {
            if (!_serial.IsOpen) 
                return false;
            if (!(canData.Payload.Length == 8)) 
                return false;

            try
            {
                byte[] data = new byte[21];
                data[0] = (byte)'t';
                data[1] = (byte)((canData.Id & 0xF00) >> 8);
                data[2] = (byte)((canData.Id & 0xF0) >> 4);
                data[3] = (byte)(canData.Id & 0xF);
                data[4] = (byte)canData.Len;
                data[5] = Convert.ToByte((canData.Payload[0] & 0xF0) >> 4);
                data[6] = Convert.ToByte(canData.Payload[0] & 0xF);
                data[7] = Convert.ToByte((canData.Payload[1] & 0xF0) >> 4);
                data[8] = Convert.ToByte(canData.Payload[1] & 0xF);
                data[9] = Convert.ToByte((canData.Payload[2] & 0xF0) >> 4);
                data[10] = Convert.ToByte(canData.Payload[2] & 0xF);
                data[11] = Convert.ToByte((canData.Payload[3] & 0xF0) >> 4);
                data[12] = Convert.ToByte(canData.Payload[3] & 0xF);
                data[13] = Convert.ToByte((canData.Payload[4] & 0xF0) >> 4);
                data[14] = Convert.ToByte(canData.Payload[4] & 0xF);
                data[15] = Convert.ToByte((canData.Payload[5] & 0xF0) >> 4);
                data[16] = Convert.ToByte(canData.Payload[5] & 0xF);
                data[17] = Convert.ToByte((canData.Payload[6] & 0xF0) >> 4);
                data[18] = Convert.ToByte(canData.Payload[6] & 0xF);
                data[19] = Convert.ToByte((canData.Payload[7] & 0xF0) >> 4);
                data[20] = Convert.ToByte(canData.Payload[7] & 0xF);

                _serial.Write(data, 0, 21);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }
}
