using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScreenSaver
{
    /// <summary>
    /// 泡泡类
    /// </summary>
    class Bubble
    {
        internal Point Center { get; private set; }
        public int X { get => Center.X;  set => Center.X = value; }
        public int Y { get => Center.Y;  set => Center.Y = value; }
        public int R { get; set; }
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }

        private Color _color;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">圆心横坐标</param>
        /// <param name="y">圆心纵坐标</param>
        /// <param name="color">泡泡颜色</param>
        public Bubble(int x, int y, Color color)
        {
            Center = new Point(x,y);
            R = 70;
            _color = color;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">圆心横坐标</param>
        /// <param name="y">圆心纵坐标</param>
        /// <param name="color">泡泡颜色</param>
        /// <param name="xSpeed">横轴初始速度</param>
        /// <param name="ySpeed">纵轴初始速度</param>
        public Bubble(int x, int y, Color color, int xSpeed, int ySpeed) : this(x, y, color)
        {
            XSpeed = xSpeed;
            YSpeed = ySpeed;
        }
        /// <summary>
        /// 按照当前速度移动一步
        /// </summary>
        public void Move()
        {
            X += XSpeed;
            Y += YSpeed;
        }
        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(_color), X - R, Y - R, 2*R, 2*R);
        }
    }
}