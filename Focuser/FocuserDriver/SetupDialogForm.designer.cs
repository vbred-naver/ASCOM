namespace ASCOM.NoBrand.Focuser
{
    partial class SetupDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.chkAbsolute = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtStepSize = new System.Windows.Forms.TextBox();
            this.txtMaxStep = new System.Windows.Forms.TextBox();
            this.txtMaxIncre = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ChkTempComp = new System.Windows.Forms.CheckBox();
            this.ChkTempProbe = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkReset = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBoxUsbPort = new System.Windows.Forms.ComboBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ChkTrace = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(236, 197);
            this.cmdOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(72, 31);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.CmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(319, 197);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(70, 31);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancel_Click);
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(9, 17);
            this.comboBoxComPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(155, 20);
            this.comboBoxComPort.TabIndex = 7;
            // 
            // chkAbsolute
            // 
            this.chkAbsolute.AutoSize = true;
            this.chkAbsolute.ForeColor = System.Drawing.SystemColors.Control;
            this.chkAbsolute.Location = new System.Drawing.Point(10, 17);
            this.chkAbsolute.Name = "chkAbsolute";
            this.chkAbsolute.Size = new System.Drawing.Size(73, 16);
            this.chkAbsolute.TabIndex = 9;
            this.chkAbsolute.Text = "Absolute";
            this.chkAbsolute.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtStepSize);
            this.groupBox1.Controls.Add(this.txtMaxStep);
            this.groupBox1.Controls.Add(this.txtMaxIncre);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Location = new System.Drawing.Point(10, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 98);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // txtStepSize
            // 
            this.txtStepSize.Location = new System.Drawing.Point(114, 63);
            this.txtStepSize.Name = "txtStepSize";
            this.txtStepSize.Size = new System.Drawing.Size(70, 21);
            this.txtStepSize.TabIndex = 5;
            // 
            // txtMaxStep
            // 
            this.txtMaxStep.Location = new System.Drawing.Point(114, 39);
            this.txtMaxStep.Name = "txtMaxStep";
            this.txtMaxStep.Size = new System.Drawing.Size(70, 21);
            this.txtMaxStep.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtMaxStep, "Max: 30000");
            // 
            // txtMaxIncre
            // 
            this.txtMaxIncre.Location = new System.Drawing.Point(114, 15);
            this.txtMaxIncre.Name = "txtMaxIncre";
            this.txtMaxIncre.Size = new System.Drawing.Size(70, 21);
            this.txtMaxIncre.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtMaxIncre, "Max: 30000");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Max Step :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(9, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "Max Increment :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Step Size :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ChkTempComp);
            this.groupBox2.Controls.Add(this.ChkTempProbe);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Location = new System.Drawing.Point(10, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(197, 67);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Temperature";
            // 
            // ChkTempComp
            // 
            this.ChkTempComp.AutoSize = true;
            this.ChkTempComp.Location = new System.Drawing.Point(10, 42);
            this.ChkTempComp.Name = "ChkTempComp";
            this.ChkTempComp.Size = new System.Drawing.Size(182, 16);
            this.ChkTempComp.TabIndex = 1;
            this.ChkTempComp.Text = "Temperature Compensation";
            this.ChkTempComp.UseVisualStyleBackColor = true;
            this.ChkTempComp.CheckedChanged += new System.EventHandler(this.ChkTempComp_CheckedChanged);
            // 
            // ChkTempProbe
            // 
            this.ChkTempProbe.AutoSize = true;
            this.ChkTempProbe.Location = new System.Drawing.Point(10, 20);
            this.ChkTempProbe.Name = "ChkTempProbe";
            this.ChkTempProbe.Size = new System.Drawing.Size(133, 16);
            this.ChkTempProbe.TabIndex = 0;
            this.ChkTempProbe.Text = "Temperature Probe";
            this.ChkTempProbe.UseVisualStyleBackColor = true;
            this.ChkTempProbe.CheckedChanged += new System.EventHandler(this.ChkTempProbe_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkAbsolute);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Location = new System.Drawing.Point(214, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(118, 44);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Type";
            // 
            // chkReset
            // 
            this.chkReset.AutoSize = true;
            this.chkReset.ForeColor = System.Drawing.SystemColors.Control;
            this.chkReset.Location = new System.Drawing.Point(10, 20);
            this.chkReset.Name = "chkReset";
            this.chkReset.Size = new System.Drawing.Size(137, 16);
            this.chkReset.TabIndex = 10;
            this.chkReset.Text = "Reset Position(Abs)";
            this.chkReset.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBoxUsbPort);
            this.groupBox4.Controls.Add(this.comboBoxComPort);
            this.groupBox4.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox4.Location = new System.Drawing.Point(214, 115);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(179, 67);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CommPort";
            // 
            // comboBoxUsbPort
            // 
            this.comboBoxUsbPort.FormattingEnabled = true;
            this.comboBoxUsbPort.Location = new System.Drawing.Point(8, 17);
            this.comboBoxUsbPort.Name = "comboBoxUsbPort";
            this.comboBoxUsbPort.Size = new System.Drawing.Size(165, 20);
            this.comboBoxUsbPort.TabIndex = 18;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.NoBrand.Focuser.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(345, 6);
            this.picASCOM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.WindowText;
            this.groupBox6.Controls.Add(this.chkReset);
            this.groupBox6.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox6.Location = new System.Drawing.Point(214, 62);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(179, 45);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Step";
            // 
            // ChkTrace
            // 
            this.ChkTrace.AutoSize = true;
            this.ChkTrace.ForeColor = System.Drawing.SystemColors.Control;
            this.ChkTrace.Location = new System.Drawing.Point(9, 20);
            this.ChkTrace.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ChkTrace.Name = "ChkTrace";
            this.ChkTrace.Size = new System.Drawing.Size(75, 16);
            this.ChkTrace.TabIndex = 6;
            this.ChkTrace.Text = "Trace on";
            this.ChkTrace.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ChkTrace);
            this.groupBox5.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox5.Location = new System.Drawing.Point(10, 190);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(197, 45);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Trace Log";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(402, 242);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NoBrand Focuser Setup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.CheckBox chkAbsolute;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStepSize;
        private System.Windows.Forms.TextBox txtMaxStep;
        private System.Windows.Forms.TextBox txtMaxIncre;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ChkTempComp;
        private System.Windows.Forms.CheckBox ChkTempProbe;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkReset;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox ChkTrace;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBoxUsbPort;
    }
}