using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCAN;

namespace CanInterfaces
{
    // Type alias for a PCAN-Basic channel handle
    using TPCANHandle = System.UInt16;
    // Type alias for a CAN-FD bitrate string
    using TPCANBitrateFD = System.String;
    // Type alias for a microseconds timestamp
    using TPCANTimestampFD = System.UInt64;

    public class PCAN : ICanInterface
    {
        #region Defines
        /// <summary>
        /// Sets the PCANHandle (Hardware Channel)
        /// </summary>
        const TPCANHandle PcanHandle = PCANBasic.PCAN_USBBUS1;
        /// <summary>
        /// Sets the desired connection mode (CAN = false / CAN-FD = true)
        /// </summary>
        const bool IsFD = false;
        /// <summary>
        /// Sets the bitrate for normal CAN devices
        /// </summary>
        const TPCANBaudrate Bitrate = TPCANBaudrate.PCAN_BAUD_500K;
        /// <summary>
        /// Sets the bitrate for CAN FD devices. 
        /// Example - Bitrate Nom: 1Mbit/s Data: 2Mbit/s:
        ///   "f_clock_mhz=20, nom_brp=5, nom_tseg1=2, nom_tseg2=1, nom_sjw=1, data_brp=2, data_tseg1=3, data_tseg2=1, data_sjw=1"
        /// </summary>
        const TPCANBitrateFD BitrateFD = "f_clock_mhz=20, nom_brp=5, nom_tseg1=2, nom_tseg2=1, nom_sjw=1, data_brp=2, data_tseg1=3, data_tseg2=1, data_sjw=1";
        #endregion

        #region Members
        /// <summary>
        /// Shows if DLL was found
        /// </summary>
        private bool m_DLLFound;
        #endregion

        public bool Init()
        {
            // Checks if PCANBasic.dll is available, if not, the program terminates
            m_DLLFound = CheckForLibrary();
            if (!m_DLLFound)
                return false;

            TPCANStatus stsResult;
            // Initialization of the selected channel
            stsResult = PCANBasic.Initialize(PcanHandle, Bitrate);

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
                Console.WriteLine("Can not initialize. Please check the defines in the code.");
                return false;
            }

            return true;
        }

        public bool Start()
        {
            if (!(PCANBasic.GetStatus(PcanHandle) == TPCANStatus.PCAN_ERROR_OK)) return false;
            return true;
        }

        public bool Stop()
        {
            if (!m_DLLFound) return false; 
            if(!(PCANBasic.Uninitialize(PCANBasic.PCAN_NONEBUS) == TPCANStatus.PCAN_ERROR_OK)) return false;
            return true;
        }

        public bool Write(CanData canData)
        {
            if(!(PCANBasic.GetStatus(PcanHandle) == TPCANStatus.PCAN_ERROR_OK)) return false;
            if(!(canData.Payload.Length == 8)) return false;

            var msg = new TPCANMsg();
            msg.DATA = new byte[8];
            msg.DATA[0] = canData.Payload[0];
            msg.DATA[1] = canData.Payload[1];
            msg.DATA[2] = canData.Payload[2];
            msg.DATA[3] = canData.Payload[3];
            msg.DATA[4] = canData.Payload[4];
            msg.DATA[5] = canData.Payload[5];
            msg.DATA[6] = canData.Payload[6];
            msg.DATA[7] = canData.Payload[7];
            //msg.DATA = canData.Payload;
            msg.ID = Convert.ToUInt16(canData.Id);
            msg.LEN = Convert.ToByte(canData.Len);
            msg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;

            if(PCANBasic.Write(PcanHandle, ref msg) == TPCANStatus.PCAN_ERROR_OK) return true;

            return false;
        }

        /// <summary>
        /// Checks for availability of the PCANBasic labrary
        /// </summary>
        /// <returns>If the library was found or not</returns>
        private bool CheckForLibrary()
        {
            // Check for dll file
            try
            {
                PCANBasic.Uninitialize(PCANBasic.PCAN_NONEBUS);
                return true;
            }
            catch (DllNotFoundException)
            {
                Console.WriteLine("Unable to find the library: PCANBasic.dll !");
                Console.WriteLine("Press any key to close");
                Console.ReadKey();
            }

            return false;
        }
    }
}
