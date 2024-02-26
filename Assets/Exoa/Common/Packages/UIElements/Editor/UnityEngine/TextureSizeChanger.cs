using System;
using System.Threading;
namespace UnityEngine
{
    public sealed class TextureSizeChanger
    {
        private sealed class ThreadData
        {
            public int start;
            public int end;
            public ThreadData(int s, int e)
            {
                this.start = s;
                this.end = e;
            }
        }
        private static Color[] texColors;
        private static Color[] newColors;
        private static int w;
        private static float ratioX;
        private static float ratioY;
        private static int w2;
        private static int finishCount;
        private static Mutex mutex;
        public static void Point(Texture2D texture, int newWidth, int newHeight)
        {
            TextureSizeChanger.ThreadedScale(texture, newWidth, newHeight, false);
        }
        public static void Bilinear(Texture2D texture, int newWidth, int newHeight)
        {
            TextureSizeChanger.ThreadedScale(texture, newWidth, newHeight, true);
        }
        private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
        {
            TextureSizeChanger.texColors = tex.GetPixels();
            TextureSizeChanger.newColors = new Color[newWidth * newHeight];
            if (useBilinear)
            {
                TextureSizeChanger.ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
                TextureSizeChanger.ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
            }
            else
            {
                TextureSizeChanger.ratioX = (float)tex.width / (float)newWidth;
                TextureSizeChanger.ratioY = (float)tex.height / (float)newHeight;
            }
            TextureSizeChanger.w = tex.width;
            TextureSizeChanger.w2 = newWidth;
            int num = Mathf.Min(SystemInfo.processorCount, newHeight);
            int num2 = newHeight / num;
            TextureSizeChanger.finishCount = 0;
            if (TextureSizeChanger.mutex == null)
            {
                TextureSizeChanger.mutex = new Mutex(false);
            }
            if (num > 1)
            {
                int i;
                TextureSizeChanger.ThreadData threadData;
                for (i = 0; i < num - 1; i++)
                {
                    threadData = new TextureSizeChanger.ThreadData(num2 * i, num2 * (i + 1));
                    ParameterizedThreadStart start = useBilinear ? new ParameterizedThreadStart(TextureSizeChanger.BilinearScale) : new ParameterizedThreadStart(TextureSizeChanger.PointScale);
                    Thread thread = new Thread(start);
                    thread.Start(threadData);
                }
                threadData = new TextureSizeChanger.ThreadData(num2 * i, newHeight);
                if (useBilinear)
                {
                    TextureSizeChanger.BilinearScale(threadData);
                }
                else
                {
                    TextureSizeChanger.PointScale(threadData);
                }
                while (TextureSizeChanger.finishCount < num)
                {
                    Thread.Sleep(1);
                }
            }
            else
            {
                TextureSizeChanger.ThreadData obj = new TextureSizeChanger.ThreadData(0, newHeight);
                if (useBilinear)
                {
                    TextureSizeChanger.BilinearScale(obj);
                }
                else
                {
                    TextureSizeChanger.PointScale(obj);
                }
            }
            //tex.Reinitialize(newWidth, newHeight);
            tex.SetPixels(TextureSizeChanger.newColors);
            tex.Apply();
            TextureSizeChanger.texColors = null;
            TextureSizeChanger.newColors = null;
        }
        private static void BilinearScale(object obj)
        {
            TextureSizeChanger.ThreadData threadData = (TextureSizeChanger.ThreadData)obj;
            for (int i = threadData.start; i < threadData.end; i++)
            {
                int num = (int)Mathf.Floor((float)i * TextureSizeChanger.ratioY);
                int num2 = num * TextureSizeChanger.w;
                int num3 = (num + 1) * TextureSizeChanger.w;
                int num4 = i * TextureSizeChanger.w2;
                for (int j = 0; j < TextureSizeChanger.w2; j++)
                {
                    int num5 = (int)Mathf.Floor((float)j * TextureSizeChanger.ratioX);
                    float value = (float)j * TextureSizeChanger.ratioX - (float)num5;
                    TextureSizeChanger.newColors[num4 + j] = TextureSizeChanger.ColorLerpUnclamped(TextureSizeChanger.ColorLerpUnclamped(TextureSizeChanger.texColors[num2 + num5], TextureSizeChanger.texColors[num2 + num5 + 1], value), TextureSizeChanger.ColorLerpUnclamped(TextureSizeChanger.texColors[num3 + num5], TextureSizeChanger.texColors[num3 + num5 + 1], value), (float)i * TextureSizeChanger.ratioY - (float)num);
                }
            }
            TextureSizeChanger.mutex.WaitOne();
            TextureSizeChanger.finishCount++;
            TextureSizeChanger.mutex.ReleaseMutex();
        }
        private static void PointScale(object obj)
        {
            TextureSizeChanger.ThreadData threadData = (TextureSizeChanger.ThreadData)obj;
            for (int i = threadData.start; i < threadData.end; i++)
            {
                int num = (int)(TextureSizeChanger.ratioY * (float)i) * TextureSizeChanger.w;
                int num2 = i * TextureSizeChanger.w2;
                for (int j = 0; j < TextureSizeChanger.w2; j++)
                {
                    TextureSizeChanger.newColors[num2 + j] = TextureSizeChanger.texColors[(int)((float)num + TextureSizeChanger.ratioX * (float)j)];
                }
            }
            TextureSizeChanger.mutex.WaitOne();
            TextureSizeChanger.finishCount++;
            TextureSizeChanger.mutex.ReleaseMutex();
        }
        private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
        {
            return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
        }
    }
}
