using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSaver
{
    public partial class BubbleScreenSaver : Form
    {
        DoubleBufferedPanel panel;
        public BubbleScreenSaver()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint,
              true);
            this.UpdateStyles();
        }
        private void BubbleScreenSaver_Load(object sender, EventArgs e)
        {
            panel = new DoubleBufferedPanel();
            Controls.Add(panel);
        }


        private void BubbleScreenSaver_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                panel.DisposeUnit();
                Close();
            }
        }
    }
}