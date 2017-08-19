using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    class Constants
    {
        public const byte DASH = 0;
        public const byte BUTTON_BOX = 1;
        public const byte LED_NO_BLINK = 0;
        public const byte LED_BLINK = 1;

        public const int IRacing = 0;
        public const int Raceroom = 1;
        public const int Assetto = 2;
        public const int RFactor = 3;
        public const int RFactor2 = 4;
        public const int None = 5;

        public const int SharedMemoryReadRate = 20;

        public const char LIST_SEPARATOR = ',';
        public const char ITEM_SEPARATOR = ';';
        public const char SIGN_EQUALS = '=';
        public const char SIGN_AMPERSAND = '&';

        public const int MAX_ARDUINOS_SUPPORTED = 6;

        public static readonly byte[] RPM_PATTERN = { 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 1, 1, 255, 1, 1, 255, 1, 1, 255 };
        public static readonly byte[] BLACK_RGB = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        public static readonly byte[] WHITE_RGB = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 247, 247, 231, 247, 247, 231, 247, 247, 231, 247, 247, 231, 247, 247, 231, 247, 247, 231, 247, 247, 231, 247, 247, 231, 247, 247, 231, 247, 247, 231, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        public static readonly byte[] YELLOW_RGB = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 255, 217, 0, 255, 217, 0, 255, 217, 0, 255, 217, 0, 255, 217, 0, 255, 217, 0, 255, 217, 0, 255, 217, 0, 255, 217, 0, 255, 217, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        public static readonly byte[] BLUE_RGB = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        public enum FLAG_TYPE
        {
            NO_FLAG = 0,
            BLUE_FLAG = 1,
            YELLOW_FLAG = 2,
            BLACK_FLAG = 8,
            WHITE_FLAG = 16,
            CHECKERED_FLAG = 32,
            PENALTY_FLAG = 64,
            SPEED_LIMITER = 128,
            IN_PIT_FLAG = 256
        }

        public static object[] RFactorTelemetryData = new object[]
        {
            "hour.time",
            "speed.kmh.shared",
            "gear.int.shared",
            "engineRPM.float.shared",
            "engineWaterTemp.float.shared",
            "engineOilTemp.float.shared",
            "clutchRPM.float.shared",
            "unfilteredThrottle.float.shared",
            "unfilteredBrake.float.shared",
            "unfilteredSteering.float.shared",
            "unfilteredClutch.float.shared",
            "steeringArmForce.float.shared",
            "fuel.float.shared",
            "engineMaxRPM.float.shared",
            "driverName.byte[].vehicle",
            "vehicleName.byte[].vehicle",
            "totalLaps.short.vehicle",
            "sector.sbyte.vehicle",
            "finishStatus.sbyte.vehicle",
            "lapDist.float.vehicle",
            "pathLateral.float.vehicle",
            "trackEdge.float.vehicle",
            "bestSector1.time.vehicle",
            "bestSector2.time.vehicle",
            "bestLapTime.time.vehicle",
            "lastSector1.time.vehicle",
            "lastSector2.time.vehicle",
            "lastLapTime.time.vehicle",
            "curSector1.time.vehicle",
            "curSector2.time.vehicle",
            "numPitstops.short.vehicle",
            "numPenalties.short.vehicle",
            "isPlayer.byte.vehicle",
            "control.sbyte.vehicle",
            "inPits.byte.vehicle",
            "place.byte.vehicle",
            "vehicleClass.byte[].vehicle",
            "timeBehindNext.time.vehicle",
            "lapsBehindNext.int.vehicle",
            "timeBehindLeader.time.vehicle",
            "lapsBehindLeader.int.vehicle",
            "lapStartET.time.vehicle"
        };

        public static object[] RaceRoomTelemetryData = new object[] {
            "hour.time",
            "EngineRps.Single",
            "MaxEngineRps.Single",
            "FuelPressure.Single",
            "FuelLeft.Single",
            "FuelCapacity.Single",
            "EngineWaterTemp.Single",
            "EngineOilTemp.Single",
            "EngineOilPressure.Single",
            "CarSpeed.kmh",
            "NumberOfLaps.Int32",
            "CompletedLaps.Int32",
            "LapTimeBestSelf.time",
            "LapTimePreviousSelf.time",
            "LapTimeCurrentSelf.time",
            "Position.Int32",
            "NumCars.Int32",
            "Gear.Int32",
            "Temperature.FrontLeft_Left",
            "Temperature.FrontLeft_Center",
            "Temperature.FrontLeft_Right",
            "Temperature.FrontRight_Left",
            "Temperature.FrontRight_Center",
            "Temperature.FrontRight_Right",
            "Temperature.RearLeft_Left",
            "Temperature.RearLeft_Center",
            "Temperature.RearLeft_Right",
            "Temperature.RearRight_Left",
            "Temperature.RearRight_Center",
            "Temperature.RearRight_Right",
            "NumPenalties.Int32",
            "CarCgLocation.Vector3_X",
            "CarCgLocation.Vector3_Y",
            "CarCgLocation.Vector3_Z",
            "CarOrientation.Orientation_Pitch",
            "CarOrientation.Orientation_Yaw",
            "CarOrientation.Orientation_Roll",
            "LocalAcceleration.Vector_X",
            "LocalAcceleration.Vector_Y",
            "LocalAcceleration.Vector_Z",
            "DrsAvailable.Int32",
            "DrsEngaged.Int32",
            "Padding1.Int32",
            "Player.PlayerData_Velocity",
            "EventIndex.Int32",
            "SessionType.Int32",
            "SessionPhase.Int32",
            "SessionIteration.Int32",
            "ControlType.Int32",
            "ThrottlePedal.Single",
            "BrakePedal.Single",
            "ClutchPedal.Single",
            "BrakeBias.Single",
            "TirePressure.FrontLeft_Left",
            "TirePressure.FrontLeft_Center",
            "TirePressure.FrontLeft_Right",
            "TirePressure.FrontRight_Left",
            "TirePressure.FrontRight_Center",
            "TirePressure.FrontRight_Right",
            "TirePressure.RearLeft_Left",
            "TirePressure.RearLeft_Center",
            "TirePressure.RearLeft_Right",
            "TirePressure.RearRight_Left",
            "TirePressure.RearRight_Center",
            "TirePressure.RearRight_Right",
            "TireWearActive.Int32",
            "TireType.Int32",
            "BrakeTemperatures.FrontLeft",
            "BrakeTemperatures.FrontRight",
            "BrakeTemperatures.RearLeft",
            "BrakeTemperatures.RearRight",
            "FuelUseActive.Int32",
            "SessionTimeRemaining.time",
            "LapTimeBestLeader.time",
            "LapTimeBestLeaderClass.time",
            "LapTimeDeltaSelf.time",
            "LapTimeDeltaLeader.time",
            "LapTimeDeltaLeaderClass.time",
            "TimeDeltaFront.time",
            "TimeDeltaBehind.time",
            "PitWindowStatus.Int32",
            "PitWindowStart.Int32",
            "PitWindowEnd.Int32",
            "CutTrackWarnings.Int32",
            "Flags.Yellow",
            "Flags.Blue",
            "Flags.Black"
        };

        public static object[] IRacingTelemetryData = new object[] {
            "hour.time",
            "AirDensity.float",
            "AirPressure.float",
            "AirTemp.float",
            "Alt.float",
            "Brake.float",
            "BrakeRaw.float",
            "CamCameraNumber.int",
            "CamCameraState.bitfield",
            "CamCarIdx.int",
            "CamGroupNumber.int",
            "Clutch.float",
            "CpuUsageBG.float",
            "DCDriversSoFar.int",
            "DCLapStatus.int",
            "DisplayUnits.int",
            "DriverMarker.bool",
            "EngineWarnings.bitfield",  //irsdk_waterTempWarning = 0x01,
                                        //irsdk_fuelPressureWarning = 0x02,
                                        //irsdk_oilPressureWarning = 0x04,
                                        //irsdk_engineStalled = 0x08,
                                        //irsdk_pitSpeedLimiter = 0x10,
                                        //irsdk_revLimiterActive = 0x20,
            "EnterExitReset.int",
            "FogLevel.float",
            "FrameRate.float",
            "FuelLevel.float",
            "FuelLevelPct.float",
            "FuelPress.float",
            "FuelUsePerHour.float",
            "Gear.int",
            "IsDiskLoggingActive.bool",
            "IsDiskLoggingEnabled.bool",
            "IsInGarage.bool",
            "IsOnTrack.bool",
            "IsOnTrackCar.bool",
            "IsReplayPlaying.bool",
            "Lap.int",
            "LapBestLap.int",
            "LapBestLapTime.time",
            "LapBestNLapLap.int",
            "LapBestNLapTime.time",
            "LapCurrentLapTime.time",
            "LapDeltaToBestLap.time",
            "LapDeltaToBestLap_DD.float",
            "LapDeltaToBestLap_OK.bool",
            "LapDeltaToOptimalLap.time",
            "LapDeltaToOptimalLap_DD.float",
            "LapDeltaToOptimalLap_OK.bool",
            "LapDeltaToSessionBestLap.time",
            "LapDeltaToSessionBestLap_DD.float",
            "LapDeltaToSessionBestLap_OK.bool",
            "LapDeltaToSessionLastlLap.time",
            "LapDeltaToSessionLastlLap_DD.float",
            "LapDeltaToSessionLastlLap_OK.bool",
            "LapDeltaToSessionOptimalLap.time",
            "LapDeltaToSessionOptimalLap_DD.float",
            "LapDeltaToSessionOptimalLap_OK.bool",
            "LapDist.float",
            "LapDistPct.float",
            "LapLasNLapSeq.int",
            "LapLastLapTime.time",
            "LapLastNLapTime.time",
            "Lat.double",
            "LatAccel.float",
            "Lon.double",
            "LongAccel.float",
            "ManifoldPress.float",
            "OilLevel.float",
            "OilPress.float",
            "OilTemp.float",
            "OnPitRoad.bool",
            "Pitch.float",
            "PitchRate.float",
            "PitOptRepairLeft.time",
            "PitRepairLeft.float",
            "PitSvFlags.bitfield",
            "PitSvFuel.float",
            "PitSvLFP.float",
            "PitSvLRP.float",
            "PitSvRFP.float",
            "PitSvRRP.float",
            "PlayerCarClassPosition.int",
            "PlayerCarPosition.int",
            "RaceLaps.int",
            "RadioTransmitCarIdx.int",
            "RadioTransmitFrequencyIdx.int",
            "RadioTransmitRadioIdx.int",
            "RelativeHumidity.float",
            "ReplayFrameNum.int",
            "ReplayFrameNumEnd.int",
            "ReplayPlaySlowMotion.bool",
            "ReplayPlaySpeed.int",
            "ReplaySessionNum.int",
            "ReplaySessionTime.double",
            "Roll.float",
            "RollRate.float",
            "RPM.float",
            "SessionFlags.bitfield",
            "SessionLapsRemain.int",
            "SessionNum.int",
            "SessionState.int",
            "SessionTime.dtime",
            "SessionTimeRemain.dtime",
            "SessionUniqueID.int",
            "ShiftGrindRPM.float",
            "ShiftIndicatorPct.float",
            "ShiftPowerPct.float",
            "Skies.int",
            "Speed.kmh",
            "SteeringWheelAngle.float",
            "SteeringWheelAngleMax.float",
            "SteeringWheelPctDamper.float",
            "SteeringWheelPctTorque.float",
            "SteeringWheelPctTorqueSign.float",
            "SteeringWheelPctTorqueSignStops.float",
            "SteeringWheelPeakForceNm.float",
            "SteeringWheelTorque.float",
            "Throttle.float",
            "ThrottleRaw.float",
            "TrackTemp.float",
            "TrackTempCrew.float",
            "VelocityX.float",
            "VelocityY.float",
            "VelocityZ.float",
            "VertAccel.float",
            "Voltage.float",
            "WaterLevel.float",
            "WaterTemp.float",
            "WeatherType.int",
            "WindDir.float",
            "WindVel.float",
            "Yaw.float",
            "YawNorth.float",
            "YawRate.float",
            "CFrideHeight.float",
            "CFshockDefl.float",
            "CFshockVel.float",
            "CFSRrideHeight.float",
            "CRrideHeight.float",
            "CRshockDefl.float",
            "CRshockVel.float",
            "dcABS.float",
            "dcAntiRollFront.float",
            "dcAntiRollRear.float",
            "dcBoostLevel.float",
            "dcBrakeBias.float",
            "dcBrakeBias.float",
            "dcDiffEntry.float",
            "dcDiffExit.float",
            "dcDiffMiddle.float",
            "dcEngineBraking.float",
            "dcEnginePower.float",
            "dcFuelMixture.float",
            "dcRevLimiter.float",
            "dcThrottleShape.float",
            "dcTractionControl.float",
            "dcTractionControl2.float",
            "dcTractionControlToggle.bool",
            "dcWeightJackerLeft.float",
            "dcWeightJackerRight.float",
            "dcWingFront.float",
            "dcWingRear.float",
            "dpFNOMKnobSetting.float",
            "dpFUFangleIndex.float",
            "dpFWingAngle.float",
            "dpFWingIndex.float",
            "dpLrWedgeAdj.float",
            "dpPSSetting.float",
            "dpQtape.float",
            "dpRBarSetting.float",
            "dpRFTruckarmP1Dz.float",
            "dpRRDamperPerchOffsetm.float",
            "dpRrPerchOffsetm.float",
            "dpRrWedgeAdj.float",
            "dpRWingAngle.float",
            "dpRWingIndex.float",
            "dpRWingSetting.float",
            "dpTruckarmP1Dz.float",
            "dpWedgeAdj.float",
            "LFbrakeLinePress.float",
            "LFcoldPressure.float",
            "LFpressure.float",
            "LFrideHeight.float",
            "LFshockDefl.float",
            "LFshockVel.float",
            "LFspeed.float",
            "LFtempCL.float",
            "LFtempCM.float",
            "LFtempCR.float",
            "LFtempL.float",
            "LFtempM.float",
            "LFtempR.float",
            "LFwearL.float",
            "LFwearM.float",
            "LFwearR.float",
            "LRbrakeLinePress.float",
            "LRcoldPressure.float",
            "LRpressure.float",
            "LRrideHeight.float",
            "LRshockDefl.float",
            "LRshockVel.float",
            "LRspeed.float",
            "LRtempCL.float",
            "LRtempCM.float",
            "LRtempCR.float",
            "LRtempL.float",
            "LRtempM.float",
            "LRtempR.float",
            "LRwearL.float",
            "LRwearM.float",
            "LRwearR.float",
            "RFbrakeLinePress.float",
            "RFcoldPressure.float",
            "RFpressure.float",
            "RFrideHeight.float",
            "RFshockDefl.float",
            "RFshockVel.float",
            "RFspeed.float",
            "RFtempCL.float",
            "RFtempCM.float",
            "RFtempCR.float",
            "RFtempL.float",
            "RFtempM.float",
            "RFtempR.float",
            "RFwearL.float",
            "RFwearM.float",
            "RFwearR.float",
            "RRbrakeLinePress.float",
            "RRcoldPressure.float",
            "RRpressure.float",
            "RRrideHeight.float",
            "RRshockDefl.float",
            "RRshockVel.float",
            "RRspeed.float",
            "RRtempCL.float",
            "RRtempCM.float",
            "RRtempCR.float",
            "RRtempL.float",
            "RRtempM.float",
            "RRtempR.float",
            "RRwearL.float",
            "RRwearM.float",
            "RRwearR.float",
            "CarIdxClassPosition.aint",
            "CarIdxEstTime.afloat",
            "CarIdxF2Time.afloat",
            "CarIdxGear.aint",
            "CarIdxLap.aint",
            "CarIdxLapDistPct.afloat",
            "CarIdxOnPitRoad.abool",
            "CarIdxPosition.aint",
            "CarIdxRPM.afloat",
            "CarIdxSteer.afloat",
            "CarIdxTrackSurface.aint"
        };

        public static object[] AssettoTelemetryData = new object[] {
            "hour.time",
            "PacketId.Int32.physics",
            "Gas.Single.physics",
            "Brake.Single.physics",
            "Fuel.Single.physics",
            "Gear.Int32.physics",
            "Rpms.Int32.physics",
            "SteerAngle.Single.physics",
            "SpeedKmh.kmh.physics",
            "Velocity.Single[].physics",
            "AccG.Single[].physics",
            "WheelSlip.Single[].physics",
            "WheelLoad.Single[].physics",
            "WheelsPressure.Single[].physics",
            "WheelAngularSpeed.Single[].physics",
            "TyreWear.Single[].physics",
            "TyreDirtyLevel.Single[].physics",
            "TyreCoreTemperature.Single[].physics",
            "CamberRad.Single[].physics",
            "SuspensionTravel.Single[].physics",
            "Drs.Single.physics",
            "TC.Single.physics",
            "Heading.Single.physics",
            "Pitch.Single.physics",
            "Roll.Single.physics",
            "CgHeight.Single.physics",
            "CarDamage.Single[].physics",
            "NumberOfTyresOut.Int32.physics",
            "PitLimiterOn.Int32.physics",
            "Abs.Single.physics",
            "PacketId.Int32.graphics",
            "Status.AC_STATUS.graphics",
            "Session.AC_SESSION_TYPE.graphics",
            "CurrentTime.time.graphics",
            "LastTime.time.graphics",
            "BestTime.time.graphics",
            "Split.String.graphics",
            "CompletedLaps.Int32.graphics",
            "Position.Int32.graphics",
            "iCurrentTime.Int32.graphics",
            "iLastTime.Int32.graphics",
            "iBestTime.Int32.graphics",
            "SessionTimeLeft.Single.graphics",
            "DistanceTraveled.Single.graphics",
            "IsInPit.Int32.graphics",
            "CurrentSectorIndex.Int32.graphics",
            "LastSectorTime.Int32.graphics",
            "NumberOfLaps.Int32.graphics",
            "TyreCompound.String.graphics",
            "ReplayTimeMultiplier.Single.graphics",
            "NormalizedCarPosition.Single.graphics",
            "CarCoordinates.Single[].graphics",
            "SMVersion.String.static",
            "ACVersion.String.static",
            "NumberOfSessions.Int32.static",
            "NumCars.Int32.static",
            "CarModel.String.static",
            "Track.String.static",
            "PlayerName.String.static",
            "PlayerSurname.String.static",
            "PlayerNick.String.static",
            "SectorCount.Int32.static",
            "MaxTorque.Single.static",
            "MaxPower.Single.static",
            "MaxRpm.Int32.static",
            "MaxFuel.Single.static",
            "SuspensionMaxTravel.Single[].static",
            "TyreRadius.Single[].static",
        };

        public static object[] RFactor2TelemetryData = new object[] {
            "hour.time",
            "mDeltaTime.double.rF2State",                      // time since last update (seconds)
            "mElapsedTime.double.rF2State",                    // game session time
            "mLapNumber.int.rF2State",                         // current lap number
            "mLapStartET.double.rF2State",                     // time this lap was started
            "mVehicleName.byte[].rF2State",                    // current vehicle name
            "mTrackName.byte[].rF2State",                      // current track name
            // Position and derivatives
            "mPos.rF2Vec3.rF2State",                           // world position in meters
            "mLocalVel.rF2Vec3.rF2State",                      // velocity (meters/sec) in local vehicle coordinates
            "mLocalAccel.rF2Vec3.rF2State",                    // acceleration (meters/sec^2) in local vehicle coordinates
            "mSpeed.kmh.rF2State",                             // meters/sec
            // Orientation and derivatives
            "mOri.rF2Vec3[].rF2State",                         // rows of orientation matrix (use TelemQuat conversions if desired), also converts local
                                                               // vehicle vectors into world X, Y, or Z using dot product of rows 0, 1, or 2 respectively
            "mLocalRot.rF2Vec3.rF2State",                      // rotation (radians/sec) in local vehicle coordinates
            "mLocalRotAccel.rF2Vec3.rF2State",                 // rotational acceleration (radians/sec^2) in local vehicle coordinates
            // Vehicle status
            "mGear.int.rF2State",                              // -1=reverse, 0=neutral, 1+=forward gears
            "mEngineRPM.double.rF2State",                      // engine RPM
            "mEngineWaterTemp.double.rF2State",                // Celsius
            "mEngineOilTemp.double.rF2State",                  // Celsius
            "mClutchRPM.double.rF2State",                      // clutch RPM
            // Driver input
            "mUnfilteredThrottle.double.rF2State",             // ranges  0.0-1.0
            "mUnfilteredBrake.double.rF2State",                // ranges  0.0-1.0
            "mUnfilteredSteering.double.rF2State",             // ranges -1.0-1.0 (left to right)
            "mUnfilteredClutch.double.rF2State",               // ranges  0.0-1.0
            // Filtered input (various adjustments for rev or speed limiting, TC, ABS?, speed sensitive steering, clutch work for semi-automatic shifting, etc.)
            "mFilteredThrottle.double.rF2State",               // ranges  0.0-1.0
            "mFilteredBrake.double.rF2State",                  // ranges  0.0-1.0
            "mFilteredSteering.double.rF2State",               // ranges -1.0-1.0 (left to right)
            "mFilteredClutch.double.rF2State",                 // ranges  0.0-1.0
            // Misc
            "mSteeringShaftTorque.double.rF2State",            // torque around steering shaft (used to be mSteeringArmForce, but that is not necessarily accurate for feedback purposes)
            "mFront3rdDeflection.double.rF2State",             // deflection at front 3rd spring
            "mRear3rdDeflection.double.rF2State",              // deflection at rear 3rd spring
            // Aerodynamics
            "mFrontWingHeight.double.rF2State",                // front wing height
            "mFrontRideHeight.double.rF2State",                // front ride height
            "mRearRideHeight.double.rF2State",                 // rear ride height
            "mDrag.double.rF2State",                           // drag
            "mFrontDownforce.double.rF2State",                 // front downforce
            "mRearDownforce.double.rF2State",                  // rear downforce
            // State/damage info
            "mFuel.double.rF2State",                           // amount of fuel (liters)
            "mEngineMaxRPM.double.rF2State",                   // rev limit
            "mScheduledStops.byte.rF2State",                   // number of scheduled pitstops
            "mOverheating.byte.rF2State",                     // whether overheating icon is shown
            "mDetached.byte.rF2State",                        // whether any parts (besides wheels) have been detached
            "mHeadlights.byte.rF2State",                      // whether headlights are on
            "mDentSeverity.byte[].rF2State",                   // dent severity at 8 locations around the car (0=none, 1=some, 2=more)
            "mLastImpactET.double.rF2State",                   // time of last impact
            "mLastImpactMagnitude.double.rF2State",            // magnitude of last impact
            "mLastImpactPos.rF2Vec3.rF2State",                 // location of last impact
            "mMaxImpactMagnitude.double.rF2State",             // Max impact magnitude.  Tracked on every telemetry call, and reset on visit to pits or Session restart.
            "mAccumulatedImpactMagnitude.double.rF2State",     // Accumulated impact magnitude.  Tracked on every telemetry call, and reset on visit to pits or Session restart.
            // Expanded
            "mEngineTorque.double.rF2State",                   // current engine torque (including additive torque) (used to be mEngineTq, but there's little reason to abbreviate it)
            "mCurrentSector.int.rF2State",                     // the current sector (zero-based) with the pitlane stored in the sign bit (example: entering pits from third sector gives 0x80000002)
            "mSpeedLimiter.byte.rF2State",                     // whether speed limiter is on
            "mMaxGears.byte.rF2State",                         // maximum forward gears
            "mFrontTireCompoundIndex.byte.rF2State",           // index within brand
            "mRearTireCompoundIndex.byte.rF2State",            // index within brand
            "mFuelCapacity.double.rF2State",                   // capacity in liters
            "mFrontFlapActivated.byte.rF2State",               // whether front flap is activated
            "mRearFlapActivated.byte.rF2State",                // whether rear flap is activated
            "mRearFlapLegalStatus.byte.rF2State",              // 0=disallowed, 1=criteria detected but not allowed quite yet, 2=allowed
            "mIgnitionStarter.byte.rF2State",                  // 0=off 1=ignition 2=ignition+starter
            "mFrontTireCompoundName.byte[].rF2State",          // name of front tire compound
            "mRearTireCompoundName.byte[].rF2State",           // name of rear tire compound
            "mSpeedLimiterAvailable.byte.rF2State",            // whether speed limiter is available
            "mAntiStallActivated.byte.rF2State",               // whether (hard) anti-stall is activated
            "mVisualSteeringWheelRange.float.rF2State",        // the *visual* steering wheel range
            "mRearBrakeBias.double.rF2State",                  // fraction of brakes on rear
            "mTurboBoostPressure.double.rF2State",             // current turbo boost pressure if available
            "mPhysicsToGraphicsOffset.float[].rF2State",       // offset from static CG to graphical center
            "mPhysicalSteeringWheelRange.float.rF2State",      // the *physical* steering wheel range
            "mExpansionTelem.byte[].rF2State",                 // for future use (note that the slot ID has been moved to mID above)
            "mWheels.rF2Wheel[].rF2State",                     // wheel info (front left, front right, rear left, rear right)
            "mSession.int.rF2State",                           // current session (0=testday 1-4=practice 5-8=qual 9=warmup 10-13=race)
            "mCurrentET.double.rF2State",                      // current time (at last ScoringUpdate)
            "mEndET.double.rF2State",                          // ending time
            "mMaxLaps.int.rF2State",                           // maximum laps
            "mLapDist.double.rF2State",                        // distance around track
            "mNumVehicles.int.rF2State",                       // current number of vehicles
                  // Game phases:
                  // 0 Before session has begun
                  // 1 Reconnaissance laps (race only)
                  // 2 Grid walk-through (race only)
                  // 3 Formation lap (race only)
                  // 4 Starting-light countdown has begun (race only)
                  // 5 Green flag
                  // 6 Full course yellow / safety car
                  // 7 Session stopped
                  // 8 Session over
            "mGamePhase.byte.rF2State",
                  // Yellow flag states (applies to full-course only)
                  // -1 Invalid
                  //  0 None
                  //  1 Pending
                  //  2 Pits closed
                  //  3 Pit lead lap
                  //  4 Pits open
                  //  5 Last lap
                  //  6 Resume
                  //  7 Race halt (not currently used)
            "mYellowFlagState.sbyte.rF2State",
            "mSectorFlag.sbyte[].rF2State",                    // whether there are any local yellows at the moment in each sector (not sure if sector 0 is first or last, so test)
            "mStartLight.byte.rF2State",                       // start light frame (number depends on track)
            "mNumRedLights.byte.rF2State",                     // number of red lights in start sequence
            "mInRealtimeSU.byte.rF2State",                     // in realtime as opposed to at the monitor (reported via ScoringUpdate)
            "mInRealtimeFC.byte.rF2State",                     // in realtime as opposed to at the monitor (reported via last EnterRealtime/ExitRealtime call)
            "mPlayerName.byte[].rF2State",                     // player name (including possible multiplayer override)
            "mPlrFileName.byte[].rF2State",                    // may be encoded to be a legal filename
            "mDarkCloud.double.rF2State",                      // cloud darkness? 0.0-1.0
            "mRaining.double.rF2State",                        // raining severity 0.0-1.0
            "mAmbientTemp.double.rF2State",                    // temperature (Celsius)
            "mTrackTemp.double.rF2State",                      // temperature (Celsius)
            "mWind.rF2Vec3.rF2State",                          // wind speed
            "mMinPathWetness.double.rF2State",                 // minimum wetness on main path 0.0-1.0
            "mMaxPathWetness.double.rF2State",                 // maximum wetness on main path 0.0-1.0
            "mInvulnerable.byte.rF2State",                     // Indicates invulnerability 0 (off), 1 (on)
            "mExpansionScoring.byte[].rF2State",               // Future use.
            "rF2VehScoringInfo[] mVehicles",                   // array of vehicle scoring info's
                                                               // NOTE: everything beyound mNumVehicles is trash.


            "mID.int.rF2VehScoringInfo",                       // slot ID (note that it can be re-used in multiplayer after someone leaves)
            "mDriverName.byte[].rF2VehScoringInfo",            // driver name
            "mVehicleName.byte[].rF2VehScoringInfo",           // vehicle name
            "mTotalLaps.short.rF2VehScoringInfo",              // laps completed
            "mSector.sbyte.rF2VehScoringInfo",                 // 0=sector3, 1=sector1, 2=sector2 (don't ask why)
            "mFinishStatus.sbyte.rF2VehScoringInfo",           // 0=none, 1=finished, 2=dnf, 3=dq
            "mLapDist.double.rF2VehScoringInfo",               // current distance around track
            "mPathLateral.double.rF2VehScoringInfo",           // lateral position with respect to *very approximate* "center" path
            "mTrackEdge.double.rF2VehScoringInfo",             // track edge (w.r.t. "center" path) on same side of track as vehicle
            "mBestSector1.time.rF2VehScoringInfo",           // best sector 1
            "mBestSector2.time.rF2VehScoringInfo",           // best sector 2 (plus sector 1)
            "mBestLapTime.time.rF2VehScoringInfo",           // best lap time
            "mLastSector1.time.rF2VehScoringInfo",           // last sector 1
            "mLastSector2.time.rF2VehScoringInfo",           // last sector 2 (plus sector 1)
            "mLastLapTime.time.rF2VehScoringInfo",           // last lap time
            "mCurSector1.time.rF2VehScoringInfo",            // current sector 1 if valid
            "mCurSector2.time.rF2VehScoringInfo",            // current sector 2 (plus sector 1) if valid
            // no current laptime because it instantly becomes "last"
            "mNumPitstops.short.rF2VehScoringInfo",            // number of pitstops made
            "mNumPenalties.short.rF2VehScoringInfo",           // number of outstanding penalties
            "mIsPlayer.byte.rF2VehScoringInfo",                // is this the player's vehicle
            "mControl.sbyte.rF2VehScoringInfo",                // who's in control: -1=nobody (shouldn't get this), 0=local player, 1=local AI, 2=remote, 3=replay (shouldn't get this)
            "mInPits.byte.rF2VehScoringInfo",                  // between pit entrance and pit exit (not always accurate for remote vehicles)
            "mPlace.byte.rF2VehScoringInfo",                   // 1-based position
            "mVehicleClass.byte[].rF2VehScoringInfo",          // vehicle class
            // Dash Indicators
            "mTimeBehindNext.time.rF2VehScoringInfo",        // time behind vehicle in next higher place
            "mLapsBehindNext.int.rF2VehScoringInfo",           // laps behind vehicle in next higher place
            "mTimeBehindLeader.time.rF2VehScoringInfo",      // time behind leader
            "mLapsBehindLeader.int.rF2VehScoringInfo",         // laps behind leader
            "mLapStartET.double.rF2VehScoringInfo",            // time this lap was started
            // Position and derivatives
            "mPos.rF2Vec3.rF2VehScoringInfo",                  // world position in meters
            "mYaw.double.rF2VehScoringInfo",                   // rad, use (360-yaw*57.2978)%360 for heading in degrees
            "mPitch.double.rF2VehScoringInfo",                 // rad
            "mRoll.double.rF2VehScoringInfo",                  // rad
            "mSpeed.kmh.rF2VehScoringInfo",                 // meters/sec
            "mHeadlights.byte.rF2VehScoringInfo",              // status of headlights
            "mPitState.byte.rF2VehScoringInfo",                // 0=none, 1=request, 2=entering, 3=stopped, 4=exiting
            "mServerScored.byte.rF2VehScoringInfo",            // whether this vehicle is being scored by server (could be off in qualifying or racing heats)
            "mIndividualPhase.byte.rF2VehScoringInfo",         // game phases (described below) plus 9=after formation, 10=under yellow, 11=under blue (not used)
            "mQualification.int.rF2VehScoringInfo",            // 1-based, can be -1 when invalid
            "mTimeIntoLap.double.rF2VehScoringInfo",           // estimated time into lap
            "mEstimatedLapTime.double.rF2VehScoringInfo",      // estimated laptime used for 'time behind' and 'time into lap' (note: this may changed based on vehicle and setup!?)
            "mPitGroup.byte[].rF2VehScoringInfo",              // pit group (same as team name unless pit is shared)
            "mFlag.byte.rF2VehScoringInfo",                    // primary flag being shown to vehicle (currently only 0=green or 6=blue)
            "mUnderYellow.byte.rF2VehScoringInfo",             // whether this car has taken a full-course caution flag at the start/finish line
            "mCountLapFlag.byte.rF2VehScoringInfo",            // 0 = do not count lap or time, 1 = count lap but not time, 2 = count lap and time
            "mInGarageStall.byte.rF2VehScoringInfo",           // appears to be within the correct garage stall
            "mUpgradePack.byte[].rF2VehScoringInfo",           // Coded upgrades
            "mSuspensionDeflection.double.rF2Wheel",           // meters
            "mRideHeight.double.rF2Wheel",                     // meters
            "mSuspForce.double.rF2Wheel",                      // pushrod load in Newtons
            "mBrakeTemp.double.rF2Wheel",                      // Celsius
            "mBrakePressure.double.rF2Wheel",                  // currently 0.0-1.0, depending on driver input and brake balance.rF2Wheel", will convert to true brake pressure (kPa) in future
            "mRotation.double.rF2Wheel",                       // radians/sec
            "mLateralPatchVel.double.rF2Wheel",                // lateral velocity at contact patch
            "mLongitudinalPatchVel.double.rF2Wheel",           // longitudinal velocity at contact patch
            "mLateralGroundVel.double.rF2Wheel",               // lateral velocity at contact patch
            "mLongitudinalGroundVel.double.rF2Wheel",          // longitudinal velocity at contact patch
            "mCamber.double.rF2Wheel",                         // radians (positive is left for left-side wheels, right for right-side wheels)
            "mLateralForce.double.rF2Wheel",                   // Newtons
            "mLongitudinalForce.double.rF2Wheel",              // Newtons
            "mTireLoad.double.rF2Wheel",                       // Newtons
            "mGripFract.double.rF2Wheel",                      // an approximation of what fraction of the contact patch is sliding
            "mPressure.double.rF2Wheel",                       // kPa (tire pressure)
            "mTemperature.double[].rF2Wheel",                  // Kelvin (subtract 273.15 to get Celsius), left/center/right (not to be confused with inside/center/outside!)
            "mWear.double.rF2Wheel",                           // wear (0.0-1.0, fraction of maximum) ... this is not necessarily proportional with grip loss
            "mTerrainName.byte[].rF2Wheel",                    // the material prefixes from the TDF file
            "mSurfaceType.byte.rF2Wheel",                      // 0=dry, 1=wet, 2=grass, 3=dirt, 4=gravel, 5=rumblestrip, 6=special
            "mFlat.byte.rF2Wheel",                             // whether tire is flat
            "mDetached.byte.rF2Wheel",                         // whether wheel is detached
            "mVerticalTireDeflection.double.rF2Wheel",         // how much is tire deflected from its (speed-sensitive) radius
            "mWheelYLocation.double.rF2Wheel",                 // wheel's y location relative to vehicle y location
            "mToe.double.rF2Wheel",                            // current toe angle w.r.t. the vehicle
            "mTireCarcassTemperature.double.rF2Wheel",         // rough average of temperature samples from carcass (Kelvin)
            "mTireInnerLayerTemperature.double[].rF2Wheel",    // rough average of temperature samples from innermost layer of rubber (before carcass) (Kelvin)
            "mExpansion.byte[].rF2Wheel"                       // for future use
        };
    }
}
