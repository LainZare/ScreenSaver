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
    internal class BubblesPanel : Panel
    {

        int _width;
        int _height;
        // 圆心坐标
        int[][] _points;
        // 半径
        int _r;
        // 各球速度
        int[][] _speeds;
        // 最大速度
        int _maxSpeed;
        // 颜色
        int[][] _colors;

        public BubblesPanel()
        {
            // 设置双缓存
            SetStyle(ControlStyles.DoubleBuffer |
                        ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
                                            true);
            UpdateStyles();

            Dock = DockStyle.Fill;

            InitBubbles(10);
        }
        void InitBubbles(int num)
        {
            _width = Screen.PrimaryScreen.Bounds.Width;
            _height = Screen.PrimaryScreen.Bounds.Height;
            _points = new int[num][];
            _speeds = new int[num][];
            _colors = new int[num][];
            _maxSpeed = 5;

            Random rnd = new Random();
            for (int i = 0; i < num; i++)
            {
                // (x,y), 初始都是(0,0)
                _points[i] = new int[] { 0, 0 };
                // X,Y轴速度
                _speeds[i] = new int[] { rnd.Next(2, _maxSpeed), rnd.Next(2, _maxSpeed) };
                // R,G,B
                _colors[i] = new int[] { rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255) };
            }
        }

        // 后台线程操作
        void Move()
        {
            
            // 泡泡移动


            // 关键代码，重绘，触发OnPaint事件
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 绘图
        }
    }
}
