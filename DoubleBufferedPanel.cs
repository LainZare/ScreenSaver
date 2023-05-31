using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSaver
{
    // Panel本身无法设置双缓冲，所以需要自定义一个Panel
    internal class DoubleBufferedPanel : Panel
    {
        BubbleCollection bubbles;

        //Graphics _graphics;
        Timer _timer;

        public DoubleBufferedPanel()
        {
            SetStyle(ControlStyles.DoubleBuffer |
                             ControlStyles.UserPaint |
                                          ControlStyles.AllPaintingInWmPaint,
                                                       true);
            UpdateStyles();

            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;

            //_graphics = CreateGraphics();
            //_graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            bubbles = new BubbleCollection(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, 5);


            _timer = new Timer();
            _timer.Interval = 50;
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            bubbles.Move(Height, Width);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            bubbles.Draw(g);
        }

        public void DisposeUnit()
        {
            _timer.Dispose();
            //_graphics.Dispose();
        }
    }
}
