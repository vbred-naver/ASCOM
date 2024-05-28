// date : 2024-05-27, form modify


#define FTD2XX
//#define SERIAL

using ASCOM.Utilities;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using FTD2XX_NET;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.NoBrand.Focuser
{
    [ComVisible(false)] // Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
#if SERIAL
        const string NO_PORTS_MESSAGE = "No COM ports found";
#endif
#if FTD2XX
        const string NO_USB_MESSAGE = "No USB ports found";
#endif
        TraceLogger tl; // Holder for a reference to the driver's trace logger

        //int maxIncrement;

        public SetupDialogForm(TraceLogger tlDriver)
        {
            InitializeComponent();

            // Save the provided trace logger for use within the setup dialogue
            tl = tlDriver;

            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void CmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here and update the state variables with results from the dialogue
            Properties.Settings.Default.Absolute = chkAbsolute.Checked;
            Properties.Settings.Default.ResetPosition = chkReset.Checked;
            Properties.Settings.Default.MaxIncrement = txtMaxIncre.Text;
            Properties.Settings.Default.MaxStep = txtMaxStep.Text;
            Properties.Settings.Default.StepSize = txtStepSize.Text;
            Properties.Settings.Default.TempComp = ChkTempComp.Checked;
            Properties.Settings.Default.TempProbe = ChkTempProbe.Checked;
            Properties.Settings.Default.Save();

            tl.Enabled = ChkTrace.Checked;

#if SERIAL
            // Update the COM port variable if one has been selected
            if (comboBoxComPort.SelectedItem is null) // No COM port selected
            {
                tl.LogMessage("Setup OK", $"New configuration values - COM Port: Not selected");
            }
            else if (comboBoxComPort.SelectedItem.ToString() == NO_PORTS_MESSAGE)
            {
                tl.LogMessage("Setup OK", $"New configuration values - NO COM ports detected on this PC.");
            }
            else // A valid COM port has been selected
            {
                FocuserHardware.comPort = (string)comboBoxComPort.SelectedItem;
                tl.LogMessage("Setup OK", $"New configuration values - COM Port: {comboBoxComPort.SelectedItem}");
            }
#endif

#if FTD2XX
            // Update the USB port variable if one has been selected
            if (comboBoxUsbPort.SelectedItem is null) // No COM port selected
            {
                tl.LogMessage("Setup OK", $"New configuration values - USB Port: Not selected");
            }
            else if (comboBoxUsbPort.SelectedItem.ToString() == NO_USB_MESSAGE)
            {
                tl.LogMessage("Setup OK", $"New configuration values - NO USB ports detected on this PC.");
            }
            else // A valid COM port has been selected
            {
                FocuserHardware.usbPort = (string)comboBoxUsbPort.SelectedItem;
                tl.LogMessage("Setup OK", $"New configuration values - USB Port: {comboBoxUsbPort.SelectedItem}");
            }
#endif
        }

        private void CmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("https://ascom-standards.org/");
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {

            // Set the trace checkbox
            ChkTrace.Checked = tl.Enabled;

#if SERIAL
            comboBoxUsbPort.Visible = false;
            groupBox4.Text = "SERIAL Port";

            // set the list of COM ports to those that are currently available
            comboBoxComPort.Items.Clear(); // Clear any existing entries
            using (Serial serial = new Serial()) // User the Serial component to get an extended list of COM ports
            {
                comboBoxComPort.Items.AddRange(serial.AvailableCOMPorts);
            }

            // If no ports are found include a message to this effect
            if (comboBoxComPort.Items.Count == 0)
            {
                comboBoxComPort.Items.Add(NO_PORTS_MESSAGE);
                comboBoxComPort.SelectedItem = NO_PORTS_MESSAGE;
            }

            // select the current port if possible
            if (comboBoxComPort.Items.Contains(FocuserHardware.comPort))
            {
                comboBoxComPort.SelectedItem = FocuserHardware.comPort;
            }
#endif

#if FTD2XX
            comboBoxComPort.Visible = false;
            groupBox4.Text = "USB Port(S/N)";

            uint ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus;
            comboBoxUsbPort.Items.Clear();

            // Create new instance of the FTDI device class
            FTDI mFtdiDevice = new FTDI();

            // Determine the number of FTDI devices connected to the machine
            mFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);

            if (ftdiDeviceCount == 0)
            {
                comboBoxUsbPort.Items.Add(NO_USB_MESSAGE);
                comboBoxUsbPort.SelectedItem = NO_USB_MESSAGE;
            }

            // Allocate storage for device info list
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

            // Populate our device list
            ftStatus = mFtdiDevice.GetDeviceList(ftdiDeviceList);

            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                for (uint i = 0; i < ftdiDeviceCount; i++)
                {
                    comboBoxUsbPort.Items.Add(ftdiDeviceList[i].Description.ToString() + ":" + ftdiDeviceList[i].SerialNumber.ToString());
                }
            }

            // select the current port if possible
            if (comboBoxUsbPort.Items.Contains(FocuserHardware.usbPort))
            {
                comboBoxUsbPort.SelectedItem = FocuserHardware.usbPort;
            }
#endif
            tl.LogMessage("InitUI", $"Set UI controls to Trace: {ChkTrace.Checked}, COM Port: {comboBoxComPort.SelectedItem}");

            chkAbsolute.Checked = Properties.Settings.Default.Absolute;
            txtMaxIncre.Text = Properties.Settings.Default.MaxIncrement;
            txtMaxStep.Text = Properties.Settings.Default.MaxStep;
            txtStepSize.Text = Properties.Settings.Default.StepSize;
            ChkTempComp.Checked = Properties.Settings.Default.TempComp;
            ChkTempProbe.Checked = Properties.Settings.Default.TempProbe;
            chkReset.Checked = Properties.Settings.Default.ResetPosition;
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            // Bring the setup dialogue to the front of the screen
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            else
            {
                TopMost = true;
                Focus();
                BringToFront();
                TopMost = false;
            }
        }

        private void ChkTempProbe_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkTempProbe.Checked == false)
                {
                    ChkTempComp.Checked = false;
                }
            }
            catch
            {
            }
        }

        private void ChkTempComp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkTempComp.Checked == true)
                {
                    ChkTempProbe.Checked = true;
                }
            }
            catch
            {
            }
        }

    }
}