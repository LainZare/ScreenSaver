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
    // Panel本身无法设置双缓存，所以需要自定义一个Panel
    /// <summary>
    /// 带双缓存的Panel
    /// </summary>
    internal class DoubleBufferedPanel : Panel
    {
        BubbleCollection bubbles;
        Timer _timer;

        public DoubleBufferedPanel()
        {
            // 设置双缓存
            SetStyle(ControlStyles.DoubleBuffer |
                        ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
                                            true);
            UpdateStyles();

            Dock = DockStyle.Fill;

            bubbles = new BubbleCollection(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            _timer = new Timer();
            _timer.Interval = 30;
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            bubbles.Move();
            // 关键代码，重绘，触发OnPaint事件
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            bubbles.Draw(g);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void DisposeUnit()
        {
            _timer.Dispose();
        }
    }
}
