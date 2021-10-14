using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zwindows.Utils
{
    public static class MyMath
    {
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
           return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public static int GetArea(int width, int height)
        {
              return width * height;
        }

        public static double GetColorBrightness(int red, int green, int blue)
        {
            return (red / 255.0) * 0.3 + (green / 255.0) * 0.59 + (blue / 255.0) * 0.11;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="correctionFactor">From -1:1 (- is darker), 2: auto decrease brightness</param>
        /// <returns></returns>
        public static System.Drawing.Color CalculateAverageColor(Bitmap bm, float correctionFactor = 2)
        {
            int width = bm.Width;
            int height = bm.Height;
            int red = 0;
            int green = 0;
            int blue = 0;
            int minDiversion = 15; // drop pixels that do not differ by at least minDiversion between color values (white, gray or black)
            int dropped = 0; // keep track of dropped pixels
            long[] totals = new long[] { 0, 0, 0 };
            int bppModifier = bm.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4; // cutting corners, will fail on anything else but 32 and 24 bit images

            BitmapData srcData = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, bm.PixelFormat);
            int stride = srcData.Stride;
            IntPtr Scan0 = srcData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int idx = (y * stride) + x * bppModifier;
                        red = p[idx + 2];
                        green = p[idx + 1];
                        blue = p[idx];
                        if (Math.Abs(red - green) > minDiversion || Math.Abs(red - blue) > minDiversion || Math.Abs(green - blue) > minDiversion)
                        {
                            totals[2] += red;
                            totals[1] += green;
                            totals[0] += blue;
                        }
                        else
                        {
                            dropped++;
                        }
                    }
                }
            }

            int count = Math.Max(1, width * height - dropped);
            float avgR = (float)(totals[2] / count);
            float avgG = (float)(totals[1] / count);
            float avgB = (float)(totals[0] / count);

            var b = (float)GetColorBrightness((int)avgR, (int)avgG, (int)avgB);

            if(b >= 0.6f && correctionFactor == 2)
            {
                correctionFactor = -(b - 0.5f);
            }

            if(correctionFactor == 2)
            {
                correctionFactor = 0;
            }

            Console.WriteLine("Default Brightness: " + b);

            if(correctionFactor != 0)
            {
                if (correctionFactor < 0)
                {
                    correctionFactor = 1 + correctionFactor;
                    avgR *= correctionFactor;
                    avgG *= correctionFactor;
                    avgB *= correctionFactor;
                }
                else
                {
                    avgR = (255 - avgR) * correctionFactor + avgR;
                    avgG = (255 - avgG) * correctionFactor + avgG;
                    avgB = (255 - avgB) * correctionFactor + avgB;
                }
                b = (float)GetColorBrightness((int)avgR, (int)avgG, (int)avgB);
                Console.WriteLine("Adjusted Brightness: " + b);
            }

            return System.Drawing.Color.FromArgb((int)avgR, (int)avgG, (int)avgB);
        }

        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
    }
}
