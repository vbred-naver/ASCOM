// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for NoBrand
//
// Description:	 <To be completed by driver developer>
//
// Implements:	ASCOM Focuser interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// date : 2024-04-05
// date : 2024-05-27, serial receive modify
// name : young-jin,kim((vbred@naver.com))

#define FTD2XX
//#define SERIAL

using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.LocalServer;
using ASCOM.Utilities;
using System;
using System.IO.Ports;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using FTD2XX_NET;
using System.Threading;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using ASCOM.Astrometry.NOVASCOM;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;


namespace ASCOM.NoBrand.Focuser
{
    //
    // This code is mostly a presentation layer for the functionality in the FocuserHardware class. You should not need to change the contents of this file very much, if at all.
    // Most customisation will be in the FocuserHardware class, which is shared by all instances of the driver, and which must handle all aspects of communicating with your device.
    //
    // Your driver's DeviceID is ASCOM.NoBrand.Focuser
    //
    // The COM Guid attribute sets the CLSID for ASCOM.NoBrand.Focuser
    // The COM ClassInterface/None attribute prevents an empty interface called _NoBrand from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Focuser Driver for NoBrand.
    /// </summary>
    [ComVisible(true)]
    [Guid("05109b2f-e95f-4e8a-a4bd-5a6a45b4f207")]
    [ProgId("ASCOM.NoBrand.Focuser")]
    [ServedClassName("NoBrand Focuser")] // Driver description that appears in the Chooser, customise as required
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : ReferenceCountedObjectBase, IFocuserV3, IDisposable
    {
        internal static string DriverProgId; // ASCOM DeviceID (COM ProgID) for this driver, the value is retrieved from the ServedClassName attribute in the class initialiser.
        internal static string DriverDescription; // The value is retrieved from the ServedClassName attribute in the class initialiser.

        // connectedState holds the connection state from this driver instance's perspective, as opposed to the local server's perspective, which may be different because of other client connections.
        internal bool connectedState; // The connected state from this driver's perspective)
        internal TraceLogger tl; // Trace logger object to hold diagnostic information just for this instance of the driver, as opposed to the local server's log, which includes activity from all driver instances.
        private bool disposedValue;
        readonly string name = "NBF-H1";
        readonly string busyName = "NINA wait";
        private bool busy = false;  // NINA command overrun 진정시키기(log에 busy_name 으로 표시됨)


#if SERIAL
        private Serial serialPort;
#endif

#if FTD2XX
        // Create new instance of the FTDI device class
        private FTDI mFtdiDevice;
        FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
#endif
        #region Initialisation and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="NoBrand"/> class. Must be public to successfully register for COM.
        /// </summary>
        public Focuser()
        {
            try
            {
                // Pull the ProgID from the ProgID class attribute.
                Attribute attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ProgIdAttribute));
                DriverProgId = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.

                // Pull the display name from the ServedClassName class attribute.
                attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ServedClassNameAttribute));
                DriverDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

                // LOGGING CONFIGURATION
                // By default all driver logging will appear in Hardware log file
                // If you would like each instance of the driver to have its own log file as well, uncomment the lines below

                tl = new TraceLogger("", "NoBrand.Driver"); // Remove the leading ASCOM. from the ProgId because this will be added back by TraceLogger.
                
                SetTraceState();

                // Initialise the hardware if required
                FocuserHardware.InitialiseHardware();

                LogMessage("Focuser", "Starting driver initialisation");
                LogMessage("Focuser", $"ProgID: {DriverProgId}, Description: {DriverDescription}");

                connectedState = false; // Initialise connected to false


                LogMessage("Focuser", "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage("Focuser", $"Initialisation exception: {ex}");
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.NoBrand.Focuser", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Class destructor called automatically by the .NET runtime when the object is finalised in order to release resources that are NOT managed by the .NET runtime.
        /// </summary>
        /// <remarks>See the Dispose(bool disposing) remarks for further information.</remarks>
        ~Focuser()
        {
            // Please do not change this code.
            // The Dispose(false) method is called here just to release unmanaged resources. Managed resources will be dealt with automatically by the .NET runtime.

            Dispose(false);
        }

        /// <summary>
        /// Deterministically dispose of any managed and unmanaged resources used in this instance of the driver.
        /// </summary>
        /// <remarks>
        /// Do not dispose of items in this method, put clean-up code in the 'Dispose(bool disposing)' method instead.
        /// </remarks>
        public void Dispose()
        {
            // Please do not change the code in this method.

            // Release resources now.
            Dispose(disposing: true);

            // Do not add GC.SuppressFinalize(this); here because it breaks the ReferenceCountedObjectBase COM connection counting mechanic
        }

        /// <summary>
        /// Dispose of large or scarce resources created or used within this driver file
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to enable you to release finite system resources back to the operating system as soon as possible, so that other applications work as effectively as possible.
        ///
        /// NOTES
        /// 1) Do not call the FocuserHardware.Dispose() method from this method. Any resources used in the static FocuserHardware class itself, 
        ///    which is shared between all instances of the driver, should be released in the FocuserHardware.Dispose() method as usual. 
        ///    The FocuserHardware.Dispose() method will be called automatically by the local server just before it shuts down.
        /// 2) You do not need to release every .NET resource you use in your driver because the .NET runtime is very effective at reclaiming these resources. 
        /// 3) Strong candidates for release here are:
        ///     a) Objects that have a large memory footprint (> 1Mb) such as images
        ///     b) Objects that consume finite OS resources such as file handles, synchronisation object handles, memory allocations requested directly from the operating system (NativeMemory methods) etc.
        /// 4) Please ensure that you do not return exceptions from this method
        /// 5) Be aware that Dispose() can be called more than once:
        ///     a) By the client application
        ///     b) Automatically, by the .NET runtime during finalisation
        /// 6) Because of 5) above, you should make sure that your code is tolerant of multiple calls.    
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        // Dispose of managed objects here

                        // Clean up the trace logger object
                        if (!(tl is null))
                        {
                            tl.Enabled = false;
                            tl.Dispose();
                            tl = null;
                        }
                    }
                    catch (Exception)
                    {
                        // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method. 
                    }
                }

                try
                {
                    // Dispose of unmanaged objects, if any, here (OS handles etc.)
                }
                catch (Exception)
                {
                    // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method. 
                }

                // Flag that Dispose() has already run and disposed of all resources
                disposedValue = true;
            }
        }

        #endregion

        // PUBLIC COM INTERFACE IFocuserV3 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            try
            {
                if (connectedState) // Don't show if already connected
                {
                    MessageBox.Show("Already connected, just press OK");
                }
                else // Show dialogue
                {
                    LogMessage("SetupDialog", $"Calling SetupDialog.");
                    FocuserHardware.SetupDialog();
                    LogMessage("SetupDialog", $"Completed.");
                }
            }
            catch (Exception ex)
            {
                LogMessage("SetupDialog", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public ArrayList SupportedActions
        {
            get
            {
                try
                {
                    CheckConnected($"SupportedActions");
                    ArrayList actions = FocuserHardware.SupportedActions;
                    LogMessage("SupportedActions", $"Returning {actions.Count} actions.");
                    return actions;
                }
                catch (Exception ex)
                {
                    LogMessage("SupportedActions", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public string Action(string actionName, string actionParameters)
        {
            try
            {
                CheckConnected($"Action {actionName} - {actionParameters}");
                LogMessage("", $"Calling Action: {actionName} with parameters: {actionParameters}");
                string actionResponse = FocuserHardware.Action(actionName, actionParameters);
                LogMessage("Action", $"Completed.");
                return actionResponse;
            }
            catch (Exception ex)
            {
                LogMessage("Action", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public void CommandBlind(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBlind: {command}, Raw: {raw}");
#if SERIAL
                serialPort.ClearBuffers();
                serialPort.Transmit(command);
#endif

#if FTD2XX
                uint numBytesWritten = 0;
                uint rwTimeout = 500;                         // 500ms
                mFtdiDevice.SetTimeouts(0, rwTimeout);
                ftStatus = mFtdiDevice.Write(command, command.Length, ref numBytesWritten);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    LogMessage("Failed to write data", $"{ftStatus}");
                    throw new Exception();
                }
#endif
                LogMessage("CommandBlind", $"Command: {command}");
            }
            catch (Exception ex)
            {
                LogMessage("CommandBlind", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>

        public bool CommandBool(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBool: {command}, Raw: {raw}");
#if SERIAL
                serialPort.ClearBuffers();
                serialPort.Transmit(command);
                LogMessage("CommandBlind", $"Command : {command}");
#endif

#if FTD2XX
                uint numBytesWritten = 0;
                uint rwTimeout = 500;                         // 500ms
                mFtdiDevice.SetTimeouts(0, rwTimeout);
                ftStatus = mFtdiDevice.Write(command, command.Length, ref numBytesWritten);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    LogMessage("Failed to write data", $"{ftStatus}");
                    throw new Exception();
                }
#endif
                bool commandBoolResponse = FocuserHardware.CommandBool(command, raw);
                LogMessage("CommandBool", $"Returning: {commandBoolResponse}.");
                return commandBoolResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandBool", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public string CommandString(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandString: {command}, Raw: {raw}");

#if SERIAL
                serialPort.ClearBuffers();
                serialPort.Transmit(command);
                LogMessage("CommandString", $"Command: {command}");
                return serialPort.ReceiveTerminated("#");
#endif

#if FTD2XX
                uint numBytesWritten = 0;
                uint rwTimeout = 500;                         // 500ms
                ftStatus = mFtdiDevice.Write(command, command.Length, ref numBytesWritten);
                LogMessage("CommandString", $"Command: {command}");

                string rDataTmp = "";
                string rData = " ";                         // null 아님
                uint numBytesRead = 0;
                uint numBytesAvailable = 1;                      // 1 byte
                mFtdiDevice.SetTimeouts(rwTimeout, 0);
                do
                {
                    ftStatus = mFtdiDevice.Read(out rDataTmp, numBytesAvailable, ref numBytesRead);
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        LogMessage("Failed to read data", $"{ftStatus}");
                        throw new Exception();
                    }
                    else
                    {
                        if (numBytesRead > 0)
                        {
                            rData += rDataTmp;
                           if (rDataTmp.Contains("#"))         // read ok
                           {
                                break;
                           }
                        }
                        else                                    //  read timeout
                        {
                            LogMessage("Read timeout", $"{ftStatus}({rwTimeout} ms)");
                            throw new Exception();
                        }
                    }
                    Thread.Sleep(1);
                } while (!rData.Contains("#"));
                rData = rData.TrimStart();
                return rData;
#endif
            }
            catch (Exception ex)
            {
                LogMessage("CommandString", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get
            {
                try
                {
                    // Returns the driver's connection state rather than the local server's connected state, which could be different because there may be other client connections still active.
                    //LogMessage("Connected Get", connectedState.ToString());
                    return connectedState;
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
#if SERIAL
                    string portName;
                    portName = FocuserHardware.comPort;
#endif

#if FTD2XX
                    string portName = FocuserHardware.usbPort;
                    portName = portName.Substring(portName.IndexOf(":") + 1);   // Description:SerialNumber
#endif
                    if (value == connectedState)
                    {
                        LogMessage("Connected Set", "Device already connected, ignoring Connected Set = true");
                        return;
                    }

                    if (value)
                    {
#if SERIAL
                        serialPort = new Serial
                        {
                            PortName = portName,
                            Speed = SerialSpeed.ps9600,
                            Parity = SerialParity.None,
                            DataBits = 8,
                            ReceiveTimeout = 1,
                            Connected = true
                        };
                        connectedState = true;
#endif

#if FTD2XX
                        mFtdiDevice = new FTDI();

                        ftStatus = mFtdiDevice.OpenBySerialNumber(portName);
                        if(ftStatus == FTDI.FT_STATUS.FT_OK)
                        {
                            mFtdiDevice.SetBaudRate(9600);
                            mFtdiDevice.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);
                            mFtdiDevice.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_NONE, 0, 0);
                            mFtdiDevice.SetTimeouts(500, 500);
                            connectedState = true;
                        }
                        else
                        {
                            connectedState = false;
                            throw new Exception("USB 포트를 연결할 수 없습니다");
                        }
#endif
                        FocuserHardware.Connected = connectedState;
                        LogMessage("Connected Set", "Connecting to device");
                    }
                    else
                    {
#if SERIAL
                        serialPort.Connected = false;
                        serialPort.Dispose();
                        serialPort = null;
                        connectedState = false;
#endif

#if FTD2XX
                        mFtdiDevice.Close();
                        connectedState = mFtdiDevice.IsOpen;
#endif
                        FocuserHardware.Connected = connectedState;
                        LogMessage("Connected Set", "Disconnecting from device");
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                try
                {
#if SERIAL
                    string description = FocuserHardware.comPort;
#endif

#if FTD2XX
                    string description = FocuserHardware.usbPort;
#endif
                    LogMessage("Description", $"{description}");
                    return description;
                }
                catch (Exception ex)
                {
                    LogMessage("Description", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverInfo = FocuserHardware.DriverInfo;
                    LogMessage("DriverInfo", driverInfo);
                    return driverInfo;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverInfo", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public string DriverVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverVersion = FocuserHardware.DriverVersion;
                    LogMessage("DriverVersion", driverVersion);
                    return driverVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    short interfaceVersion = FocuserHardware.InterfaceVersion;
                    LogMessage("InterfaceVersion", interfaceVersion.ToString());
                    return interfaceVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("InterfaceVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    LogMessage("Name Get", name);
                    return name;
                }
                catch (Exception ex)
                {
                    LogMessage("Name", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

#endregion

        #region IFocuser Implementation

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute
        {
            get
            {
                try
                {
                    while (busy)
                    {
                        LogMessage($"{busyName}", "");
                        Thread.Sleep(1);
                    }

                    busy = true;
                    bool absolute = Properties.Settings.Default.Absolute;
                    if (absolute)
                    {
                        if (Properties.Settings.Default.ResetPosition) CommandBlind("AY#", true);    // Absolute, AY, Reset
                        else CommandBlind("AS#", true);                                              // Absolute, AS
                        Properties.Settings.Default.ResetPosition = false;
                    }
                    else
                    {
                        if (Properties.Settings.Default.ResetPosition) CommandBlind("AX#", true);    // Relative, AX. Reset
                        else CommandBlind("AC#", true);                                              // Relative, AC
                        Properties.Settings.Default.ResetPosition = false;
                    }
                    LogMessage("Absolute", $"{absolute}");
                    busy = false;
                    return absolute;
                }
                catch (Exception ex)
                {
                    LogMessage("Absolute", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
        }
 
        /// <summary>
        /// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
        /// </summary>
        public void Halt()
        {
            try
            {
                while (busy)
                {
                    LogMessage($"{busyName}", "");
                    Thread.Sleep(1);
                }

                busy = true;
                CommandBlind("HA#", true);
                LogMessage("Halt", "");
                busy = false;
            }
            catch (Exception ex)
            {
                LogMessage("Halt", $"Threw an exception: \r\n{ex}");
                busy = false;
                throw;
            }
        }

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        public bool IsMoving
        {
            get
            {
                try
                {
                    while (busy)
                    {
                        LogMessage($"{busyName}", "");
                        Thread.Sleep(1);
                    }

                    busy = true;
                    bool isMoving = false;
                    string rcv = "";
                    rcv = CommandString("IS#", true);
                    string repResult = rcv.Replace("#", "");
                    if (repResult == "1") isMoving = true;
                    else if (repResult == "0") isMoving = false;
                    LogMessage("IsMoving", $"{isMoving}({rcv})");
                    busy = false;
                    return isMoving;
                }
                catch (Exception ex)
                {
                    LogMessage("IsMoving", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// State of the connection to the focuser.
        /// </summary>
        public bool Link
        {
            get
            {
                try
                {
                    bool link = Connected;
                    //LogMessage("Link Get", link.ToString());
                    return link;
                }
                catch (Exception ex)
                {
                    LogMessage("Link Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    //LogMessage("Link Set", value.ToString());
                    Connected = value;
                }
                catch (Exception ex)
                {
                    LogMessage("Link Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser;
        /// i.e. the maximum number of steps allowed in one move operation.
        /// </summary>
        public int MaxIncrement
        {
            get
            {
                try
                {
                    while (busy)
                    {
                        LogMessage($"{busyName}", "");
                        Thread.Sleep(1);
                    }

                    busy = true;
                    int maxIncrement = int.Parse(Properties.Settings.Default.MaxIncrement);
                    CommandBlind(string.Format("MI{0}#", maxIncrement), true);
                    //LogMessage("MaxIncrement", $"{maxIncrement}");
                    busy = false;
                    return maxIncrement;
                }
                catch (Exception ex)
                {
                    LogMessage("MaxIncrement", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// </summary>
        public int MaxStep
        {
            get
            {
                try
                {
                    while (busy)
                    {
                        LogMessage($"{busyName}", "");
                        Thread.Sleep(1);
                    }

                    busy = true;
                    int maxStep = int.Parse(Properties.Settings.Default.MaxStep);
                    CommandBlind(string.Format("MS{0}#", maxStep), true);
                    //LogMessage("MaxStep", $"{maxStep}");
                    busy = false;
                    return maxStep;
                }
                catch (Exception ex)
                {
                    LogMessage("MaxStep", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// </summary>
        public double StepSize
        {
            get
            {
                try
                {
                    while (busy)
                    {
                        LogMessage($"{busyName}", "");
                        Thread.Sleep(1);
                    }

                    busy = true;
                    int stepSize = int.Parse(Properties.Settings.Default.StepSize);
                    CommandBlind(string.Format("SS{0}#", stepSize), true);
                    //LogMessage("StepSize", $"{stepSize}");
                    busy = false;
                    return stepSize;
                }
                catch (Exception ex)
                {
                    LogMessage("StepSize", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
        /// </summary>
        /// <param name="position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
        public void Move(int position)
        {
            try
            {
                while (busy)
                {
                    LogMessage($"{busyName}", "");
                    Thread.Sleep(1);
                }

                busy = true;
                CommandBlind(string.Format("MO{0}#", position), true);
                FocuserHardware.Move(position);
                //LogMessage("Move", $"{position}");
                busy = false;
            }
            catch (Exception ex)
            {
                LogMessage("Move", $"Threw an exception: \r\n{ex}");
                busy = false;
                throw;
            }
        }

        /// <summary>
        /// Current focuser position, in steps.
        /// </summary>
        public int Position
        {
            get
            {
                try
                {
                    while (busy)
                    {
                        LogMessage($"{busyName}", "");
                        Thread.Sleep(1);
                    }

                    busy = true;
                    int position;
                    string rcv = CommandString("PO#", true);
                    string repResult = rcv.Replace("#", "");
                    position = int.Parse(repResult);
                    LogMessage("Position", $"{position}({rcv})");
                    busy = false;
                    return position;
                }
                catch (Exception ex)
                {
                    LogMessage("Position", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// TempComp를 True로 설정하면 포커서는 온도 추적 모드로 전환됩니다. False로 설정하면 온도 추적이 꺼집니다.
        /// False 인 경우 TempCompAvailable이 속성은 항상 False를 반환합니다.
        /// </summary>
        public bool TempComp
        {
            get
            {
                try
                {
                    string rcv = "";
                    string repResult = "";
                    bool tempComp = false;

                    if (Properties.Settings.Default.TempComp)
                    {
                        while (busy)
                        {
                            LogMessage($"{busyName}", "");
                            Thread.Sleep(1);
                        }

                        busy = true;
                        rcv = CommandString("TG#", true);                       // TempComp Get
                        repResult = rcv.Replace("#", "");
                        if (repResult == "1") tempComp = true;
                        else if (repResult == "0") tempComp = false;
                        LogMessage("TempCompAvailable", $"{tempComp}({rcv})");
                        busy = false;
                        return tempComp;
                    }
                    else
                    {
                        LogMessage("TempCompAvailable", tempComp.ToString());
                        return tempComp;
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("TempComp Get", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
            set
            {
                try
                {
                    //string rcv;
                    while (busy)
                    {
                        LogMessage($"{busyName}", "");
                        Thread.Sleep(1);
                    }

                    busy = true;
                    if (value) CommandBlind("TS#", true);                  // TempComp Set
                    else CommandBlind("TC#", true);                        // TempComp Clear
                    LogMessage("TempComp Set", $"{value}");
                    busy = false;
                }
                catch (Exception ex)
                {
                    LogMessage("TempComp Set", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// TempComp가 False 인 경우 TempCompAvailable이 속성은 항상 False를 반환합니다.
        /// </summary>
        public bool TempCompAvailable
        {
            get
            {
                try
                {
                    bool tempCompAvailable = false;
                    if (Properties.Settings.Default.TempComp) tempCompAvailable = true;
                    else tempCompAvailable = false;
                    LogMessage("TempCompAvailable", tempCompAvailable.ToString());
                    return tempCompAvailable;
                }
                catch (Exception ex)
                {
                    LogMessage("TempCompAvailable", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Current ambient temperature in degrees Celsius as measured by the focuser.
        /// </summary>
        public double Temperature
        {
            get
            {
                try
                {
                    double temperature = 0;
                    if (Properties.Settings.Default.TempProbe)
                    {
                        while (busy)
                        {
                            LogMessage($"{busyName}", "");
                            Thread.Sleep(1);
                        }

                        busy = true;
                        string rcv = CommandString("TE#", true);
                        string repResult = rcv.Replace("#", "");
                        temperature = double.Parse(repResult);
                        LogMessage("Temperature", $"{temperature}({rcv})");
                        busy = false;
                        return temperature;
                    }
                    else
                    {
                        LogMessage("Temperature", temperature.ToString());
                        return temperature;
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Temperature", $"Threw an exception: \r\n{ex}");
                    busy = false;
                    throw;
                }
            }
        }

        #endregion

        #region Private properties and methods
        // Useful properties and methods that can be used as required to help with driver development

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!connectedState)
            {
                throw new NotConnectedException($"{DriverDescription} ({DriverProgId}) is not connected: {message}");
            }
        }

        /// <summary>
        /// Log helper function that writes to the driver or local server loggers as required
        /// </summary>
        /// <param name="identifier">Identifier such as method name</param>
        /// <param name="message">Message to be logged.</param>
        private void LogMessage(string identifier, string message)
        {
            // This code is currently set to write messages to an individual driver log AND to the shared hardware log.

            // Write to the individual log for this specific instance (if enabled by the driver having a TraceLogger instance)
            if (tl != null)
            {
                tl.LogMessageCrLf(identifier, message); // Write to the individual driver log
            }

            // Write to the common hardware log shared by all running instances of the driver.
            // 로그 분리
            // FocuserHardware.LogMessage(identifier, message); // Write to the local server logger
        }

        /// <summary>
        /// Read the trace state from the driver's Profile and enable / disable the trace log accordingly.
        /// </summary>
        private void SetTraceState()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(DriverProgId, FocuserHardware.traceStateProfileName, string.Empty, FocuserHardware.traceStateDefault));
            }
        }

        #endregion
    }
}
