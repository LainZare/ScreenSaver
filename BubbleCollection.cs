using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScreenSaver
{
    internal class BubbleCollection
    {
        List<Bubble> bubbles = new List<Bubble>();
        int _width;
        int _height;

        public BubbleCollection(int width, int height)
        {
            int num = 5;
            int speed = 5;
            _width = width;
            _height = height;
            Random random = new Random();
            for (int i = 1; i < num; i++)
            {
                bubbles.Add(new Bubble(i * width / num, 100,
                    Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
                    random.Next(-speed, speed), random.Next(0, speed)));
                bubbles.Add(new Bubble(i * width / num, height-100,
                    Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
                    random.Next(-speed, speed), random.Next(-speed, 0)));
            }
        }

        public void Move()
        {
            // 泡泡四个边界的坐标
            int up, down, left, right;
            int i, j;
            for (i = 0; i < bubbles.Count; i++)
            {
                up = bubbles[i].Y - bubbles[i].R;
                down = bubbles[i].Y + bubbles[i].R;
                left = bubbles[i].X - bubbles[i].R;
                right = bubbles[i].X + bubbles[i].R;

                if (left <= 0)
                {
                    // 已经发生交错，先回退，防止鬼畜
                    bubbles[i].X -= left;
                    bubbles[i].XSpeed = -bubbles[i].XSpeed;
                }
                if (right >= _width)
                {
                    bubbles[i].X -= right - _width;
                    bubbles[i].XSpeed = -bubbles[i].XSpeed;
                }

                if (up <= 0)
                {
                    bubbles[i].Y -= up;
                    bubbles[i].YSpeed = -bubbles[i].YSpeed;
                }
                if (down >= _height)
                {
                    bubbles[i].Y -= down - _height;
                    bubbles[i].YSpeed = -bubbles[i].YSpeed;
                }

                for (j = i + 1; j < bubbles.Count; j++)
                {
                    if (Point.DistanceOf(bubbles[i].Center, bubbles[j].Center) <= bubbles[i].R + bubbles[j].R)
                    {
                        #region 弃用
                        //// 只靠虑两个泡泡间的碰撞
                        //// 已重叠，回退
                        //bubbles[i].X -= (bubbles[j].X - bubbles[i].X) * ((bubbles[i].R + bubbles[j].R) / Point.DistanceOf(bubbles[i].Center, bubbles[j].Center) - 1);
                        //bubbles[i].Y -= (bubbles[j].Y - bubbles[i].Y) * ((bubbles[i].R + bubbles[j].R) / Point.DistanceOf(bubbles[i].Center, bubbles[j].Center) - 1);

                        //// 使用简化的动量与能量计算
                        //int sign = (bubbles[i].XSpeed - bubbles[j].XSpeed) > 0 ? 1 : -1;
                        //int m = bubbles[i].XSpeed + bubbles[j].XSpeed;
                        //int n = bubbles[i].XSpeed * bubbles[i].XSpeed + bubbles[j].XSpeed * bubbles[j].XSpeed;
                        //bubbles[i].XSpeed = (int)(sign * Math.Sqrt(2 * n - m * m) + m) / 2;
                        //bubbles[j].XSpeed = (int)(m - sign * Math.Sqrt(2 * n - m * m)) / 2;

                        //sign = (bubbles[i].YSpeed - bubbles[j].YSpeed) > 0 ? 1 : -1;
                        //m = bubbles[i].YSpeed + bubbles[j].YSpeed;
                        //n = bubbles[i].YSpeed * bubbles[i].YSpeed + bubbles[j].YSpeed * bubbles[j].YSpeed;
                        //bubbles[i].YSpeed = (int)(sign * Math.Sqrt(2 * n - m * m) + m) / 2;
                        //bubbles[j].YSpeed = (int)(m - sign * Math.Sqrt(2 * n - m * m)) / 2;
                        #endregion

                        int tempSpeed;
                        tempSpeed = bubbles[i].XSpeed;
                        bubbles[i].XSpeed = bubbles[j].XSpeed;
                        bubbles[j].XSpeed = tempSpeed;
                        tempSpeed = bubbles[i].YSpeed;
                        bubbles[i].YSpeed = bubbles[j].YSpeed;
                        bubbles[j].YSpeed = tempSpeed;

                        break;
                    }
                }
                bubbles[i].Move();
            }
        }
        public void Draw(Graphics g)
        {
            foreach (var bubble in bubbles)
            {
                bubble.Draw(g);
            }
        }
    }
}
