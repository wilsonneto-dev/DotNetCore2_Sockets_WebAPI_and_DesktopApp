using AppWindows.Domain.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppWindows
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            App app = new App(this);
            app.Run();

        }

        public void Log(String text)
        {
            this.lblLog.Text += text + "\n";
        }

    }
}
