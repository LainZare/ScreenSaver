using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScreenSaver
{
    class Bubble
    {
        private Point _center;
        public int X { get => _center.X; private set => _center.X = value; }
        public int Y { get => _center.Y; private set => _center.Y = value; }

        private int _r;
        private Color _color;
        private int _xSpeed;
        private int _ySpeed;

        public Bubble(int x, int y, Color color)
        {

            X = x;
            Y = y;
            _r = 70;
            _color = color;
        }
        public Bubble(int x, int y, Color color, int xSpeed, int ySpeed) : this(x, y, color)
        {
            _xSpeed = xSpeed;
            _ySpeed = ySpeed;
        }

        public void Move()
        {
            X += _xSpeed;
            Y += _ySpeed;
        }

        public void Move(int windowH, int windowW, List<Bubble> bubbles)
        { 

        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(_color), X - _r, Y - _r, 2*_r, 2*_r);
        }
    }
}