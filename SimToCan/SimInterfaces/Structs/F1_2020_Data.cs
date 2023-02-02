using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimInterfaces.Structs
{
    public class F1_2020_Data
    {
        public enum PacketId : byte
        {
            Motion = 0,
            Session = 1,
            LapData = 2,
            Event = 3,
            Participants = 4,
            CarSetups = 5,
            CarTelemetry = 6,
            CarStatus = 7,
            FinalClassification = 8,
            LobbyInfo = 9
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct HeaderData
        {
            //Debug Size = 28 bytes
            public UInt16 PacketFormat;
            public byte GameMajorVersion;
            public byte GameMinorVersion;
            public byte PacketVersion;
            public PacketId PacketId;
            public UInt64 SessionUid;
            public float SessionTime;
            public UInt32 FrameIdentifier;
            public byte PlayerCarIndex;
            public byte SecondaryPlayerCarIndex;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketMotionData
        {
            //1464 bytes
            public HeaderData Header;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public CarMotionData[] CarMotion;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] SuspensionPosition; // Note: All wheel arrays have the following order:

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] SuspensionVelocity; // RL, RR, FL, FR

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] SuspensionAcceleration; // RL, RR, FL, FR

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] WheelSpeed;// Speed of each wheel

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] WheelSlip; // Slip ratio for each wheel
            public float LocalVelocityX; // Velocity in local space
            public float LocalVelocityY; // Velocity in local space
            public float LocalVelocityZ; // Velocity in local space
            public float AngularVelocityX; // Angular velocity x-component
            public float AngularVelocityY; // Angular velocity y-component
            public float AngularVelocityZ; // Angular velocity z-component
            public float AngularAccelerationX; // Angular velocity x-component
            public float AngularAccelerationY; // Angular velocity y-component
            public float AngularAccelerationZ; // Angular velocity z-component
            public float FrontWheelsAngle; // Current front wheels angle in radians
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct CarMotionData
        {
            public float WorldPositionX; // World space X position
            public float WorldPositionY; // World space Y position
            public float WorldPositionZ; // World space Z position
            public float WorldVelocityX; // Velocity in world space X
            public float WorldVelocityY; // Velocity in world space Y
            public float WorldVelocityZ; // Velocity in world space Z
            public Int16 WorldForwardDirX; // World space forward X direction (normalised)
            public Int16 WorldForwardDirY; // World space forward Y direction (normalised)
            public Int16 WorldForwardDirZ; // World space forward Z direction (normalised)
            public Int16 WorldRightDirX; // World space right X direction (normalised)
            public Int16 WorldRightDirY; // World space right Y direction (normalised)
            public Int16 WorldRightDirZ; // World space right Z direction (normalised)
            public float GForceLateral; // Lateral G-Force component
            public float GForceLongitudinal; // Longitudinal G-Force component
            public float GForceVertical; // Vertical G-Force component
            public float Yaw; // Yaw angle in radians
            public float Pitch; // Pitch angle in radians
            public float Roll; // Roll angle in radians
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketSessionData
        {
            //Size 251 bytes
            public HeaderData Header;

            public byte Weather;  // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                  // 3 = light rain, 4 = heavy rain, 5 = storm
            public sbyte TrackTemperature; // Track temp. in degrees celsius
            public sbyte AirTemperature; // Air temp. in degrees celsius
            public byte TotalLaps; // Total number of laps in this race
            public UInt16 TrackLength; // Track length in metres
            public byte SessionType;  // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P
                                      // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ
                                      // 10 = R, 11 = R2, 12 = Time Trial
            public sbyte TrackId; // -1 for unknown, 0-21 for tracks, see appendix
            public byte Formula;  // Formula, 0 = F1 Modern, 1 = F1 Classic, 2 = F2,
                                  // 3 = F1 Generic
            public UInt16 SessionTimeLeft; // Time left in session in seconds
            public UInt16 SessionDuration; // Session duration in seconds
            public byte PitSpeedLimit; // Pit speed limit in kilometres per hour
            public byte GamePaused; // Whether the game is paused
            public byte IsSpectating; // Whether the player is spectating
            public byte SpectatorCarIndex; // Index of the car being spectated
            public byte SliProNativeSupport;// SLI Pro support, 0 = inactive, 1 = active
            public byte NumMarshalZones; // Number of marshal zones to follow

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            public MarshalZoneData[] MarshalZones; // List of marshal zones – max 21
            public byte SafetyCarStatus;  // 0 = no safety car, 1 = full safety car
                                          // 2 = virtual safety car
            public byte NetworkGame; // 0 = offline, 1 = online
            public byte NumWeatherForecastSamples; // Number of weather samples to follow

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public WeatherForecastSampleData[] WeatherForecastSamples; // Array of weather forecast samples
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct MarshalZoneData
        {
            public float ZoneStart; // Fraction (0..1) of way through the lap the marshal zone starts
            public sbyte ZoneFlag; // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct WeatherForecastSampleData
        {
            public byte SessionType;   // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P, 5 = Q1
                                       // 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ, 10 = R, 11 = R2
                                       // 12 = Time Trial
            public byte TimeOffset;    // Time in minutes the forecast is for
            public byte Weather;       // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                       // 3 = light rain, 4 = heavy rain, 5 = storm
            public sbyte TrackTemperature; // Track temp. in degrees celsius
            public sbyte AirTemperature;   // Air temp. in degrees celsius
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketLapData
        {
            //Size 1190 bytes
            public HeaderData Header;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public LapData[] LapData;

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct LapData
        {
            public float LastLapTime; // Last lap time in seconds
            public float CurrentLapTime; // Current time around the lap in seconds
                                         //UPDATED in Beta 3:
            public UInt16 Sector1TimeInMS; // Sector 1 time in milliseconds
            public UInt16 Sector2TimeInMS; // Sector 2 time in milliseconds
            public float BestLapTime; // Best lap time of the session in seconds
            public byte BestLapNum; // Lap number best time achieved on
            public UInt16 BestLapSector1TimeInMS; // Sector 1 time of best lap in the session in milliseconds
            public UInt16 BestLapSector2TimeInMS; // Sector 2 time of best lap in the session in milliseconds
            public UInt16 BestLapSector3TimeInMS; // Sector 3 time of best lap in the session in milliseconds
            public UInt16 BestOverallSector1TimeInMS;// Best overall sector 1 time of the session in milliseconds
            public byte BestOverallSector1LapNum; // Lap number best overall sector 1 time achieved on
            public UInt16 BestOverallSector2TimeInMS;// Best overall sector 2 time of the session in milliseconds
            public byte BestOverallSector2LapNum; // Lap number best overall sector 2 time achieved on
            public UInt16 BestOverallSector3TimeInMS;// Best overall sector 3 time of the session in milliseconds
            public byte BestOverallSector3LapNum; // Lap number best overall sector 3 time achieved on
            public float LapDistance; // Distance vehicle is around current lap in metres – could
                                      // be negative if line hasn’t been crossed yet
            public float TotalDistance; // Total distance travelled in session in metres – could
                                        // be negative if line hasn’t been crossed yet
            public float SafetyCarDelta; // Delta in seconds for safety car
            public byte CarPosition; // Car race position
            public byte CurrentLapNum; // Current lap number
            public byte PitStatus; // 0 = none, 1 = pitting, 2 = in pit area
            public byte Sector; // 0 = sector1, 1 = sector2, 2 = sector3
            public byte CurrentLapInvalid; // Current lap invalid - 0 = valid, 1 = invalid
            public byte Penalties; // Accumulated time penalties in seconds to be added
            public byte GridPosition; // Grid position the vehicle started the race in
            public byte DriverStatus; // Status of driver - 0 = in garage, 1 = flying lap
                                      // 2 = in lap, 3 = out lap, 4 = on track
            public byte ResultStatus; // Result status - 0 = invalid, 1 = inactive, 2 = active
                                      // 3 = finished, 4 = disqualified, 5 = not classified
                                      // 6 = retired
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketEventData
        {
            public HeaderData Header;                  // Header

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] EventStringCode;     // Event string code, see below
            public EventDataDetails EventDetails;            // Event details - should be interpreted differently for each type
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventFastestLap
        {
            public byte VehicleIndex; // Vehicle index of car achieving fastest lap
            public float LapTime;    // Lap time is in seconds
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventRetirement
        {
            public byte VehicleIndex; // Vehicle index of car retiring
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventTeamMateInPits
        {
            public byte VehicleIndex; // Vehicle index of team mate
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventRaceWinner
        {
            public byte VehicleIndex; // Vehicle index of the race winner
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventPenalty
        {
            public byte PenaltyType;        // Penalty type – see Appendices
            public byte InfringementType;       // Infringement type – see Appendices
            public byte VehicleIndex;             // Vehicle index of the car the penalty is applied to
            public byte OtherVehicleIndex;        // Vehicle index of the other car involved
            public byte Time;                   // Time gained, or time spent doing action in seconds
            public byte LapNum;                 // Lap the penalty occurred on
            public byte PlacesGained;           // Number of places gained by this

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventSpeedTrap
        {
            public byte VehicleIndex;     // Vehicle index of the vehicle triggering speed trap
            public float Speed;             // Top speed achieved in kilometres per hour
            public byte OverallFastestInSession;   // Overall fastest speed in session = 1, otherwise 0
            public byte DriverFastestInSession;    // Fastest speed for driver in session = 1, otherwise 0

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventStartLights
        {
            public byte NumLights;      // Number of lights showing
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventDriveThroughPentaltyServed
        {
            public byte VehicleIndex;                 // Vehicle index of the vehicle serving drive through
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventStopGoPenaltyServed
        {
            public byte VehicleIndex;                 // Vehicle index of the vehicle serving stop go
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventFlashback
        {
            public uint FlashbackFrameIdentifier;  // Frame identifier flashed back to
            public float FlashbackSessionTime;       // Session time flashed back to
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EventButtons
        {
            public uint ButtonStatus;    // Bit flags specifying which buttons are being pressed currently - see appendices
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct EventDataDetails
        {
            [FieldOffset(0)]
            public EventFastestLap FastestLap;

            [FieldOffset(0)]
            public EventRetirement Retirement;

            [FieldOffset(0)]
            public EventTeamMateInPits TeamMateInPits;

            [FieldOffset(0)]
            public EventRaceWinner RaceWinner;

            [FieldOffset(0)]
            public EventPenalty Penalty;

            [FieldOffset(0)]
            public EventSpeedTrap SpeedTrap;

            [FieldOffset(0)]
            public EventStartLights StartLights;

            [FieldOffset(0)]
            public EventDriveThroughPentaltyServed DriveThroughPentaltyServed;

            [FieldOffset(0)]
            public EventStopGoPenaltyServed StopGoPenaltyServed;

            [FieldOffset(0)]
            public EventFlashback Flashback;

            [FieldOffset(0)]
            public EventButtons Buttons;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketParticipantsData
        {
            public HeaderData Header;
            public byte NumActiveCars; // Number of active cars in the data – should match number of
                                       // cars on HUD

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            ParticipantData[] Participants;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct ParticipantData
        {
            public byte AiControlled; // Whether the vehicle is AI (1) or Human (0) controlled
            public byte DriverId; // Driver id - see appendix
            public byte TeamId; // Team id - see appendix
            public byte RaceNumber; // Race number of the car
            public byte Nationality; // Nationality of the driver

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            public char[] Name; // Name of participant in UTF-8 format – null terminated
                                // Will be truncated with … (U+2026) if too long
            public byte YourTelemetry; // The player's UDP setting, 0 = restricted, 1 = public
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketCarSetupsData
        {
            public HeaderData Header;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public CarSetupsData[] CarSetups;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct CarSetupsData
        {
            public byte FrontWing; // Front wing aero
            public byte RearWing; // Rear wing aero
            public byte OnThrottle; // Differential adjustment on throttle (percentage)
            public byte OffThrottle; // Differential adjustment off throttle (percentage)
            public float FrontCamber; // Front camber angle (suspension geometry)
            public float RearCamber; // Rear camber angle (suspension geometry)
            public float FrontToe; // Front toe angle (suspension geometry)
            public float RearToe; // Rear toe angle (suspension geometry)
            public byte FrontSuspension; // Front suspension
            public byte RearSuspension; // Rear suspension
            public byte FrontAntiRollBar; // Front anti-roll bar
            public byte RearAntiRollBar; // Front anti-roll bar
            public byte FrontSuspensionHeight; // Front ride height
            public byte RearSuspensionHeight; // Rear ride height
            public byte BrakePressure; // Brake pressure (percentage)
            public byte BrakeBias; // Brake bias (percentage)
            public float RearLeftTyrePressure; // Rear left tyre pressure (PSI)
            public float RearRightTyrePressure; // Rear right tyre pressure (PSI)
            public float FrontLeftTyrePressure; // Front left tyre pressure (PSI)
            public float FrontRightTyrePressure; // Front right tyre pressure (PSI)
            public byte Ballast; // Ballast
            public float FuelLoad; // Fuel load
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketCarTelemetryData
        {
            //Size 1307
            //Debug size = 1356
            public HeaderData Header;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public CarTelemetryData[] CarTelemetryData;
            public UInt32 ButtonStatus; // Bit flags specifying which buttons are being pressed
                                        // currently - see appendices
                                        // Added in Beta 3:
            public byte MfdPanelIndex; // Index of MFD panel open - 255 = MFD closed
                                       // Single player, race – 0 = Car setup, 1 = Pits
                                       // 2 = Damage, 3 = Engine, 4 = Temperatures
                                       // May vary depending on game mode
            public byte MfdPanelIndexSecondaryPlayer; // See above
            public sbyte SuggestedGear;   // Suggested gear for the player (1-8)
                                          // 0 if no gear suggested
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct CarTelemetryData
        {
            public UInt16 Speed; // Speed of car in kilometres per hour
            public float Throttle; // Amount of throttle applied (0.0 to 1.0)
            public float Steer; // Steering (-1.0 (full lock left) to 1.0 (full lock right))
            public float Brake; // Amount of brake applied (0.0 to 1.0)
            public byte Clutch; // Amount of clutch applied (0 to 100)
            public sbyte Gear; // Gear selected (1-8, N=0, R=-1)
            public UInt16 EngineRPM; // Engine RPM
            public byte Drs; // 0 = off, 1 = on
            public byte RevLightsPercent; // Rev lights indicator (percentage)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] BrakesTemperature; // Brakes temperature (celsius)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] TyresSurfaceTemperature; // Tyres surface temperature (celsius)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] TyresInnerTemperature; // Tyres inner temperature (celsius)
            public UInt16 EngineTemperature; // Engine temperature (celsius)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] TyresPressure; // Tyres pressure (PSI)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] SurfaceType; // Driving surface, see appendices
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketCarStatusData
        {
            public HeaderData Header;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public CarStatusData[] CarStatusData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct CarStatusData
        {
            public byte TractionControl; // 0 (off) - 2 (high)
            public byte AntiLockBrakes; // 0 (off) - 1 (on)
            public byte FuelMix; // Fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max
            public byte FrontBrakeBias; // Front brake bias (percentage)
            public byte PitLimiterStatus; // Pit limiter status - 0 = off, 1 = on
            public float FuelInTank; // Current fuel mass
            public float FuelCapacity; // Fuel capacity
            public float FuelRemainingLaps; // Fuel remaining in terms of laps (value on MFD)
            public UInt16 MaxRPM; // Cars max RPM, point of rev limiter
            public UInt16 IdleRPM; // Cars idle RPM
            public byte MaxGears; // Maximum number of gears
            public byte DrsAllowed;   // 0 = not allowed, 1 = allowed, -1 = unknown
                                      // Added in Beta3:
            public UInt16 DrsActivationDistance;  // 0 = DRS not available, non-zero - DRS will be available
                                                  // in [X] metres

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] TyresWear; // Tyre wear percentage
            public byte ActualTyreCompound;   // F1 Modern - 16 = C5, 17 = C4, 18 = C3, 19 = C2, 20 = C1
                                              // 7 = inter, 8 = wet
                                              // F1 Classic - 9 = dry, 10 = wet
                                              // F2 – 11 = super soft, 12 = soft, 13 = medium, 14 = hard
                                              // 15 = wet
            public byte VisualTyreCompound;   // F1 visual (can be different from actual compound)
                                              // 16 = soft, 17 = medium, 18 = hard, 7 = inter, 8 = wet
                                              // F1 Classic – same as above
                                              // F2 – same as above
            public byte TyresAgeLaps; // Age in laps of the current set of tyres

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] TyresDamage; // Tyre damage (percentage)
            public byte FrontLeftWingDamage; // Front left wing damage (percentage)
            public byte FrontRightWingDamage; // Front right wing damage (percentage)
            public byte RearWingDamage;   // Rear wing damage (percentage)
                                          // Added Beta 3:
            public byte DrsFault; // Indicator for DRS fault, 0 = OK, 1 = fault
            public byte EngineDamage; // Engine damage (percentage)
            public byte GearBoxDamage; // Gear box damage (percentage)
            public sbyte VehicleFiaFlags; // -1 = invalid/unknown, 0 = none, 1 = green
                                          // 2 = blue, 3 = yellow, 4 = red
            public float ErsStoreEnergy; // ERS energy store in Joules
            public byte ErsDeployMode;    // ERS deployment mode, 0 = none, 1 = medium
                                          // 2 = overtake, 3 = hotlap
            public float ErsHarvestedThisLapMGUK; // ERS energy harvested this lap by MGU-K
            public float ErsHarvestedThisLapMGUH; // ERS energy harvested this lap by MGU-H
            public float ErsDeployedThisLap; // ERS energy deployed this lap
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketFinalClassificationData
        {
            public HeaderData Header;
            public byte NumCars; // Number of cars in the final classification

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public FinalClassificationData[] ClassificationData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct FinalClassificationData
        {
            public byte Position; // Finishing position
            public byte NumLaps; // Number of laps completed
            public byte GridPosition; // Grid position of the car
            public byte Points; // Number of points scored
            public byte NumPitStops; // Number of pit stops made
            public byte ResultStatus; // Result status - 0 = invalid, 1 = inactive, 2 = active
                                      // 3 = finished, 4 = disqualified, 5 = not classified
                                      // 6 = retired
            public float BestLapTime; // Best lap time of the session in seconds
            public double TotalRaceTime; // Total race time in seconds without penalties
            public byte PenaltiesTime; // Total penalties accumulated in seconds
            public byte NumPenalties; // Number of penalties applied to this driver
            public byte NumTyreStints; // Number of tyres stints up to maximum

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] TyreStintsActual; // Actual tyres used by this driver

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] TyreStintsVisual; // Visual tyres used by this driver
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct PacketLobbyInfoData
        {
            public HeaderData Header;

            // Packet specific data
            public byte NumPlayers; // Number of players in the lobby data

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public LobbyInfoData[] LobbyPlayers;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [Serializable]
        public struct LobbyInfoData
        {
            public byte AiControlled; // Whether the vehicle is AI (1) or Human (0) controlled
            public byte TeamId; // Team id - see appendix (255 if no team currently selected)
            public byte Nationality; // Nationality of the driver

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            public char[] Name; // Name of participant in UTF-8 format – null terminated
                                // Will be truncated with ... (U+2026) if too long
            public byte ReadyStatus; // 0 = not ready, 1 = ready, 2 = spectating
        }
    }
}
