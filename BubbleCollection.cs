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
        // 控制泡泡的数量
        int _num;
        // 有空余位置
        bool _isReadyToReleaseNewBubble;
        int _maxSpeed;

        /// <summary>
        /// 泡泡集合的构造函数
        /// </summary>
        /// <param name="width">屏幕宽度</param>
        /// <param name="height">屏幕高度</param>
        public BubbleCollection(int width, int height, int num)
        {
            _width = width;
            _height = height;
            _num = num;

            _maxSpeed = 5;
        }
        /// <summary>
        /// 控制所有泡泡移动
        /// </summary>
        public void Move()
        {
            int i, j;
            // 控制泡泡一个一个放出
            if (bubbles.Count < _num)
            {
                _isReadyToReleaseNewBubble = true;
                for (i = 0; i < bubbles.Count; i++)
                {
                    if (bubbles[i].X < 3 * bubbles[i].R && bubbles[i].Y < 3 * bubbles[i].R)
                    {
                        _isReadyToReleaseNewBubble = false;
                    }
                }
                if (_isReadyToReleaseNewBubble)
                {
                    Random random = new Random();
                    bubbles.Add(new Bubble(71, 71, Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
                        random.Next(2, _maxSpeed), random.Next(2, _maxSpeed)));
                }
            }


            // 泡泡四个边界所处的位置
            int up, down, left, right;
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
                        // 泡泡已重叠，需互相远离
                        do
                        {
                            bubbles[i].X -= bubbles[i].XSpeed;
                            bubbles[i].Y -= bubbles[i].YSpeed;
                            bubbles[j].X -= bubbles[j].XSpeed;
                            bubbles[j].Y -= bubbles[j].YSpeed;
                            if (bubbles[i].X > bubbles[j].X)
                            {
                                bubbles[i].X++;
                                bubbles[j].X--;
                            }
                            else
                            {
                                bubbles[i].X--;
                                bubbles[j].X++;
                            }
                            if (bubbles[i].Y > bubbles[j].Y)
                            {
                                bubbles[i].Y++;
                                bubbles[j].Y--;
                            }
                            else
                            {
                                bubbles[i].Y--;
                                bubbles[j].Y++;
                            }
                        } while (Point.DistanceOf(bubbles[i].Center, bubbles[j].Center) <= bubbles[i].R + bubbles[j].R);

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
            }

            // 碰撞处理完成，移动
            foreach (Bubble bubble in bubbles)
            {
                bubble.Move();
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
                g.FillEllipse(new SolidBrush(bubble.Color), bubble.X - bubble.R, bubble.Y - bubble.R, 2 * bubble.R, 2 * bubble.R);
            }
        }
    }
}
