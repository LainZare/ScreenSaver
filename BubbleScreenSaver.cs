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
        List<Bubble> bubbles = new List<Bubble>();
        Random random = new Random();

        Graphics _graphics;
        Timer timer;

        public BubbleScreenSaver()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint,
              true);
            this.UpdateStyles();

            InitializeComponent();
        }
        private void BubbleScreenSaver_Load(object sender, EventArgs e)
        {

            this.BackColor = Color.Black;

            InitBubbles(5);

            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (bubbles.Count < 7)
            {
                InitBubbles(5);
            }
            for (int i = 0; i < bubbles.Count; i++)
            {
                Bubble bubble = bubbles[i];
                bubble.Move();
                bubble.Draw(CreateGraphics());

                // 删除离开屏幕的泡泡
                if (bubble.X < 0 || bubble.X > Width || bubble.Y < 0 || bubble.Y > Height)
                {
                    bubbles.RemoveAt(i);
                    i--;
                }
            }
            Invalidate();
        }

        private void InitBubbles(int num)
        {
            Random random = new Random();
            for (int i = 0; i < num; i++)
            {
                bubbles.Add(new Bubble(i * Width / num, 0, 
                    Color.FromArgb(random.Next(0,255), random.Next(0,255), random.Next(0,255)),
                    random.Next(-5, 5), random.Next(0, 5)));
                bubbles.Add(new Bubble(i * Width / num, Height,
                    Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
                    random.Next(-5, 5), random.Next(-5, 0)));
            }
        }


        private void BubbleScreenSaver_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                timer.Dispose();
                _graphics.Dispose();
                Close();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _graphics = e.Graphics;
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _graphics.Clear(this.BackColor);
            // 绘制图形
            for (int i = 0; i < bubbles.Count; i++)
            {
                bubbles[i].Draw(e.Graphics);
            }
        }

    }
}