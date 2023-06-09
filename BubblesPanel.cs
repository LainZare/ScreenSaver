using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
        static int _width;
        static int _height;

        // 圆心坐标
        static int[][] _points;
        // 半径
        static int _radius;
        // 各球速度
        static int[][] _speeds;
        // 最大速度
        static int _maxSpeed;
        // 颜色
        static int[][] _colors;
        // 已经放出的泡泡数量
        static int _releasedCount = 1;

        public BubblesPanel()
        {
            // 设置双缓存
            SetStyle(ControlStyles.DoubleBuffer |
                        ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
                                            true);
            UpdateStyles();

            Dock = DockStyle.Fill;

            _width = Screen.PrimaryScreen.Bounds.Width;
            _height = Screen.PrimaryScreen.Bounds.Height;


            InitBubbles(100);

            Thread bubbleMove = new Thread(BubblesMove);
            bubbleMove.IsBackground = true;
            bubbleMove.Start();

        }

        // 初始化泡泡
        void InitBubbles(int num)
        {
            _points = new int[num][];
            _speeds = new int[num][];
            _colors = new int[num][];

            _maxSpeed = 5;
            _radius = 70;

            Random rnd = new Random();
            for (int i = 0; i < num; i++)
            {
                // (x,y), 初始都是(71, 71)
                _points[i] = new int[] { 71, 71 };
                // X,Y轴速度
                _speeds[i] = new int[] { rnd.Next(2, _maxSpeed), rnd.Next(2, _maxSpeed) };
                // R,G,B
                _colors[i] = new int[] { rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255) };
            }
        }

        // 后台线程操作
        void BubblesMove()
        {
            int breakCount = 0;
            int i, j;
            // 准备好释放新泡泡
            bool isReadyToReleaseNewBubble;

            // 泡泡四个边界所处的位置
            int up, down, left, right;
            int distance2;

            while (true)
            {
                // 控制刷新速度
                Thread.Sleep(20);
                for (i = 0; i < _releasedCount; i++)
                {
                    #region 与边界的碰撞处理
                    left = _points[i][0] - _radius;
                    right = _points[i][0] + _radius;

                    up = _points[i][1] - _radius;
                    down = _points[i][1] + _radius;

                    if (left <= 0)
                    {
                        // 已经发生交错，先回退至屏幕内，防止鬼畜，下同
                        _points[i][0] -= left;
                        _speeds[i][0] = -_speeds[i][0];
                    }
                    if (right >= _width)
                    {
                        _points[i][0] -= right - _width;
                        _speeds[i][0] = -_speeds[i][0];
                    }

                    if (up <= 0)
                    {
                        _points[i][1] -= up;
                        _speeds[i][1] = -_speeds[i][1];
                    }
                    if (down >= _height)
                    {
                        _points[i][1] -= down - _height;
                        _speeds[i][1] = -_speeds[i][1];
                    }
                    #endregion

                    #region 与其他泡泡的碰撞处理
                    for (j = i + 1; j < _releasedCount; j++)
                    {
                        distance2 = (_points[i][0] - _points[j][0]) * (_points[i][0] - _points[j][0])
                                   + (_points[i][1] - _points[j][1]) * (_points[i][1] - _points[j][1]);

                        if (distance2 <= 4 * _radius * _radius)
                        {
                            // 泡泡已重叠，需互相远离
                            do
                            {
                                if (_points[i][0] > _points[j][0])
                                {
                                    _points[i][0]++;
                                    _points[j][0]--;
                                }
                                else
                                {
                                    _points[i][0]--;
                                    _points[j][0]++;
                                }
                                if (_points[i][1] > _points[j][1])
                                {
                                    _points[i][1]++;
                                    _points[j][1]--;
                                }
                                else
                                {
                                    _points[i][1]--;
                                    _points[j][1]++;
                                }
                                distance2 = (_points[i][0] - _points[j][0]) * (_points[i][0] - _points[j][0]) 
                                           + (_points[i][1] - _points[j][1]) * (_points[i][1] - _points[j][1]);

                                breakCount++;
                                if (breakCount > 70)
                                {
                                    breakCount = 0;
                                    break;
                                }
                            } while (distance2 < 4 * _radius * _radius);

                            // 简化的碰撞计算，二者交换速度
                            int tempSpeed;
                            tempSpeed = _speeds[i][0];
                            _speeds[i][0] = _speeds[j][0];
                            _speeds[j][0] = tempSpeed;
                            tempSpeed = _speeds[i][1];
                            _speeds[i][1] = _speeds[j][1];
                            _speeds[j][1] = tempSpeed;

                            // 只考虑两个泡泡的碰撞，避免鬼畜
                            break;
                        }
                    }
                    #endregion
                }


                // 泡泡自然移动
                for (i = 0; i < _releasedCount; i++)
                {
                    _points[i][0] += _speeds[i][0];
                    _points[i][1] += _speeds[i][1];
                }

                // 控制泡泡一个一个放出
                if (_releasedCount < _points.Length)
                {
                    isReadyToReleaseNewBubble = true;
                    for (i = 0; i < _releasedCount; i++)
                    {
                        if (_points[i][0] < 3 * _radius && _points[i][1] < 3 * _radius)
                        {
                            isReadyToReleaseNewBubble = false;
                        }
                    }
                    if (isReadyToReleaseNewBubble)
                    {
                        _releasedCount++;
                    }
                }

                // 关键代码，重绘，触发OnPaint事件
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 绘图
            for (int i = 0; i < _releasedCount; i++)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(_colors[i][0], _colors[i][1], _colors[i][2])), _points[i][0] - _radius, _points[i][1] - _radius, 2 * _radius, 2 * _radius);
            }
        }
    }
}
