using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortProgramm.ClassFolder
{
    public class CaptcaResult
    {
        public string CaptchaCode { get; set; }
        public byte[] CaptchaByteData { get; set; }
        public string CaptchaBase64 => Convert.ToBase64String(CaptchaByteData);
        public DateTime TimeStamp { get; set; }
    }
    public class Captcha
    {
        static readonly string Later = "234567890QWERTYUIOPLKJHGFDSAZXCVBNM";

        public static string GenerateCaptcha()
        {
            Random random = new Random();

            byte maxR = (byte)(Later.Length - 1);

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                byte Index = (byte)random.Next(maxR);
                stringBuilder.Append(Later[Index]);
            }
            return stringBuilder.ToString();
        }


        public static CaptcaResult GenerateImage(int width, int height, string CaptchaCode)
        {
            using (Bitmap baseMap = new Bitmap(width, height))
            using (Graphics graph = Graphics.FromImage(baseMap))
            {
                Random random = new Random();

                graph.Clear(GetRandomLightColor());

                DrawCaptcha();
                DrawLine();
                RipleEffect();

                MemoryStream MS = new MemoryStream();

                baseMap.Save(MS, ImageFormat.Png);

                return new CaptcaResult { CaptchaCode = CaptchaCode, CaptchaByteData = MS.ToArray(), TimeStamp = DateTime.Now };

                System.Drawing.Color GetRandomLightColor()
                {
                    return System.Drawing.Color.FromArgb(random.Next(160), random.Next(100), random.Next(160));
                }

                System.Drawing.Color GetRandomDeepColor()
                {
                    return System.Drawing.Color.FromArgb(random.Next(240), random.Next(170), random.Next(250));
                }


                int GetFontSize(int ImageWight, int CaptchaCodeCount)
                {
                    return ImageWight / CaptchaCodeCount;
                }

                void DrawCaptcha()
                {
                    SolidBrush FontBrush = new SolidBrush(System.Drawing.Color.Black);
                    int FontSize = GetFontSize(width, CaptchaCode.Length);
                    Font font = new Font(System.Drawing.FontFamily.GenericSerif, FontSize, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);

                    for (int i = 0; i < CaptchaCode.Length; i++)
                    {
                        FontBrush.Color = GetRandomDeepColor();


                        int ShiftPx = FontSize / 5;

                        float x = i * FontSize + random.Next(-ShiftPx, ShiftPx) * 2;

                        int maxY = height - FontSize;

                        if (maxY < 0) maxY = 0;
                        float y = random.Next(0, maxY);

                        graph.DrawString(CaptchaCode[i].ToString(), font, FontBrush, x, y);

                    }
                }

                void DrawLine()
                {
                    System.Drawing.Pen linePen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

                    for (int i = 0; i < random.Next(3, 5); i++)
                    {
                        linePen.Color = GetRandomDeepColor();

                        System.Drawing.Point Startpoint = new System.Drawing.Point(random.Next(0, width), random.Next(0, height));
                        System.Drawing.Point Endpoint = new System.Drawing.Point(random.Next(0, width), random.Next(0, height));
                        graph.DrawLine(linePen, Startpoint, Endpoint);
                    }
                }

                void RipleEffect()
                {
                    short nWave = 6;
                    int nWidth = baseMap.Width;
                    int nHeight = baseMap.Height;
                    System.Drawing.Point[,] pt = new System.Drawing.Point[nWidth, nHeight];

                    for (int x = 0; x < nWidth; ++x)
                    {
                        for (int y = 0; y < nHeight; ++y)
                        {
                            var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                            var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);
                            var newX = x + xo;
                            var newY = y + yo;
                            if (newX > 0 && newX < nWidth)
                            {
                                pt[x, y].X = (int)newX;
                            }
                            else
                            {
                                pt[x, y].X = (int)newX;
                            }

                            if (newX > 0 && newY < nWidth)
                            {
                                pt[x, y].Y = (int)newY;
                            }
                            else
                            {
                                pt[x, y].Y = 0;
                            }
                        }
                    }



                    Bitmap bSrc = (Bitmap)baseMap.Clone();


                    BitmapData bitmapData = baseMap.LockBits(new System.Drawing.Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite,
                      System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    BitmapData bmSrc = bSrc.LockBits(new System.Drawing.Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite,
                      System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    int scanline = bitmapData.Stride;
                    IntPtr scan0 = bitmapData.Scan0;
                    IntPtr srcScan0 = bmSrc.Scan0;


                    unsafe
                    {
                        byte* p = (byte*)(void*)scan0;
                        byte* pSrc = (byte*)(void*)srcScan0;

                        int nOffset = bitmapData.Stride - baseMap.Width * 3;



                        for (int y = 0; y < nHeight; ++y)
                        {
                            for (int x = 0; x < nWidth; ++x)
                            {

                                int xOfset = pt[x, y].X;
                                int yOfset = pt[x, y].Y;

                                if (yOfset >= 0 && yOfset < nHeight && xOfset >= 0 && xOfset < nWidth)
                                {
                                    if (pSrc != null)
                                    {
                                        p[0] = pSrc[yOfset * scanline + xOfset * 3];
                                        p[1] = pSrc[yOfset * scanline + xOfset * 3 + 1];
                                        p[2] = pSrc[yOfset * scanline + xOfset * 3 + 2];
                                    }
                                }
                                p += 3;
                            }

                            p += nOffset;

                        }

                        baseMap.UnlockBits(bitmapData);
                        bSrc.UnlockBits(bmSrc);
                        bSrc.Dispose();

                    }

                }

            }


        }
    }
}
