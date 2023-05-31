using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSaver
{
    internal class BubbleCollection
    {
        List<Bubble> bubbles = new List<Bubble>();

        public BubbleCollection(int width, int height, int num)
        {
            int speed = 7;
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

        public void Move(int windowH, int windowW)
        {
            foreach (Bubble bubble in bubbles)
            {
                bubble.Move();
                if (bubble.X - bubble.R < 0 || bubble.X + bubble.R > windowW)
                {
                    bubble.XSpeed = -bubble.XSpeed;
                }
                if (bubble.Y - bubble.R < 0 || bubble.Y + bubble.R > windowH)
                {
                    bubble.YSpeed = -bubble.YSpeed;
                }
            }
            for (int i = 0; i < bubbles.Count; i++)
            {
                for (int j = i + 1; j < bubbles.Count; j++)
                {
                    if (Point.DistanceOf(bubbles[i].Center, bubbles[j].Center) < bubbles[i].R + bubbles[j].R)
                    {
                        bubbles[i].XSpeed = -bubbles[i].XSpeed;
                        bubbles[i].YSpeed = -bubbles[i].YSpeed;
                        bubbles[j].XSpeed = -bubbles[j].XSpeed;
                        bubbles[j].YSpeed = -bubbles[j].YSpeed;
                    }
                }
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
