using System.Windows.Forms;

namespace ASCOM.LocalServer
{
    public partial class FrmMain : Form
    {
        private delegate void SetTextCallback(string text);

        public FrmMain()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Visible = false;
        }

        private void label1_Click(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}