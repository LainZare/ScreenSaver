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
        }
        private void BubbleScreenSaver_Load(object sender, EventArgs e)
        {
            // 使用自定义的带双缓冲的Panel
            panel = new DoubleBufferedPanel();
            Controls.Add(panel);
        }


        private void BubbleScreenSaver_KeyDown(object sender, KeyEventArgs e)
        {
            // 按Esc键退出
            if (e.KeyCode == Keys.Escape)
            {
                panel.DisposeUnit();
                Close();
            }
        }
    }
}