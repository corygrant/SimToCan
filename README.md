# SimToCan
Small application to map racing sim telemetry data to USB CAN devices. 

Used to test or use automotive hardware devices (dashes/steering wheels/etc.) away from real cars.

![Screenshot](/Images/Screenshot.png)

# Use
Using the application is very straightforward, select the sim + CAN device from the combo boxes and press *Start* (and *Stop* to end sim/CAN)

## Adding new sims
New sims can be added using `ISimInterface`
```
public delegate void DataUpdatedHandler(object sender, SimDataEventArgs e);
public interface ISimInterface
{
    void Start();
    DataUpdatedHandler DataUpdated { get; set; }
}
```
- Use `SimData` to define the data your hardware can support.
    - This data is used with all sims and CAN interfaces.
- Reading the sim data should begin on `Start()` by accessing shared memory, opening a UDP port, using a library, etc. 
- New CAN messages are sent when `DataUpdated()` is called. 
- Local sim data should be sent out using the `SimDataEventArgs` of `DataUpdated()`

## Adding new USB to CAN 
New USB CAN interfaces can be added using `ICANInterface`
```
public interface ICanInterface
{
    bool Init();
    bool Start();
    bool Stop();
    bool Write(CanData canData);
}
```

- Use `Init()`, `Start()`, `Stop()` to handle any interface requirements
    - Load/close library, open/close socket, etc.
- Use `Write(CanData data)` to send the CAN message

## Adding new CAN hardware
New CAN hardware (dashes, wheels, etc.) can be added by 
- Updating `SimData` to include the necessary data
- Formatting the data in the correct way when `Sim_DataUpdated()` is called in `SimToCanAppViewModel()`
- Setting the `Id`, `Len` and `Payload` for each `CanData` message
- Writing the `CanData` message using `can.Write(data)`

## Adding new message formats
New CAN message formats can be added in numerous different ways

As an example, see [SimDash.Messages](/SimDash/Messages.cs)

# Supported sims
*Note: Other sims may be supported if they use the same interfaces, these are the only ones that I have tested.*

| Sims  | Tested | Possibly Supported   |
| ----- | ------ | -------------------- |
| Assetto Corsa                 |   | X |
| Assetto Corsa Competizione    | X |   |
| Dirt Rally                    |   | X |
| Dirt Rally 2                  | X |   |
| Dirt 4                        |   | X |
| F1 2016                       |   | X |
| F1 2017                       |   | X |
| F1 2018                       |   | X |
| F1 2019                       |   | X |
| F1 2020                       | X |   |
| F1 2021                       |   | X |
| F1 2022                       |   | X |

Assetto Corsa interface thanks to [mdjarv's assettocorsasharedmemory](https://github.com/mdjarv/assettocorsasharedmemory)

See [my fork](https://github.com/corygrant/assettocorsasharedmemory) for my additions.


# Supported USB to CAN Interfaces
| Device  | Tested |
| ----- | ------ |
| [USB2CAN](https://github.com/corygrant/USB2CAN_HW)                | X |
| [PCAN-USB](https://www.peak-system.com/PCAN-USB.199.0.html?&L=1)  | X |

[USB2CAN Firmware](https://github.com/corygrant/USB2CAN_FW)

# Issues
- Currently changing CAN parameters is not included
- No receiving of CAN messages is supported as I typically use another device setup as a USB HID device to do that, but it could be useful