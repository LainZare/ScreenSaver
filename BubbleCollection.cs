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
    /// <summary>
    /// 泡泡集合，用来管理所有的泡泡
    /// </summary>
    internal class BubbleCollection
    {
        List<Bubble> bubbles = new List<Bubble>();
        // 屏幕的宽度和高度
        int _width;
        int _height;

        /// <summary>
        /// 泡泡集合的构造函数
        /// </summary>
        /// <param name="width">屏幕宽度</param>
        /// <param name="height">屏幕高度</param>
        public BubbleCollection(int width, int height)
        {
            _width = width;
            _height = height;
            // 泡泡的数量，num*2
            int num = 5;
            // 泡泡的速度
            int speed = 5;
            Random random = new Random();
            // 生成泡泡
            for (int i = 0; i < num; i++)
            {
                bubbles.Add(new Bubble(i * width / num, 100,
                    Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
                    random.Next(-speed, speed), random.Next(0, speed)));
                bubbles.Add(new Bubble(i * width / num, height-100,
                    Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
                    random.Next(-speed, speed), random.Next(-speed, 0)));
            }
        }
        /// <summary>
        /// 控制所有泡泡移动
        /// </summary>
        public void Move()
        {
            // 泡泡四个边界所处的位置
            int up, down, left, right;
            int i, j;
            for (i = 0; i < bubbles.Count; i++)
            {
                #region 与边界的碰撞处理
                up = bubbles[i].Y - bubbles[i].R;
                down = bubbles[i].Y + bubbles[i].R;
                left = bubbles[i].X - bubbles[i].R;
                right = bubbles[i].X + bubbles[i].R;

                if (left <= 0)
                {
                    // 已经发生交错，先回退至屏幕内，防止鬼畜，下同
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
                #endregion

                #region 与其他泡泡的碰撞处理
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

                        // 简化的碰撞计算，二者交换速度
                        int tempSpeed;
                        tempSpeed = bubbles[i].XSpeed;
                        bubbles[i].XSpeed = bubbles[j].XSpeed;
                        bubbles[j].XSpeed = tempSpeed;
                        tempSpeed = bubbles[i].YSpeed;
                        bubbles[i].YSpeed = bubbles[j].YSpeed;
                        bubbles[j].YSpeed = tempSpeed;
                        // 只考虑两个泡泡的碰撞，避免鬼畜
                        break;
                    }
                }
                #endregion

                // 碰撞处理完成，移动
                bubbles[i].Move();
            }
        }
        
        /// <summary>
        /// 为所有泡泡绘图
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            foreach (var bubble in bubbles)
            {
                bubble.Draw(g);
            }
        }
    }
}
