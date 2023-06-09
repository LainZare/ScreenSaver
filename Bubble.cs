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
        public int R { get; private set; }
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }
        public Color Color { get ; private set ; }
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
            Color = color;
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
    }
}