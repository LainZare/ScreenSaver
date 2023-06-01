using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScreenSaver
{
    class Bubble
    {

        internal Point Center { get; private set; }
        public int X { get => Center.X;  set => Center.X = value; }
        public int Y { get => Center.Y;  set => Center.Y = value; }
        public int R { get; set; }
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }

        private Color _color;
        public Bubble(int x, int y, Color color)
        {
            Center = new Point(x,y);
            R = 70;
            _color = color;
        }
        public Bubble(int x, int y, Color color, int xSpeed, int ySpeed) : this(x, y, color)
        {
            XSpeed = xSpeed;
            YSpeed = ySpeed;
        }

        public void Move()
        {
            X += XSpeed;
            Y += YSpeed;
        }
        //public void Move(int windowH, int windowW, List<Bubble> bubbles)
        //{
        //    Move();
        //    if (X - _r < 0 || X + _r > windowW)
        //    {
        //        _xSpeed = -_xSpeed;
        //    }
        //    if (Y - _r < 0 || Y + _r > windowH)
        //    {
        //        _ySpeed = -_ySpeed;
        //    }
        //    for (int i = 0; i < bubbles.Count; i++)
        //    {
        //        if (bubbles[i] != this)
        //        {
        //            if (Point.DistanceOf(_center, bubbles[i]._center) < _r + bubbles[i]._r)
        //            {
        //                _xSpeed = -_xSpeed;
        //                _ySpeed = -_ySpeed;
        //                bubbles[i]._xSpeed = -bubbles[i]._xSpeed;
        //                bubbles[i]._ySpeed = -bubbles[i]._ySpeed;
        //            }
        //        }
        //    }
        //}

        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(_color), X - R, Y - R, 2*R, 2*R);
        }
    }
}