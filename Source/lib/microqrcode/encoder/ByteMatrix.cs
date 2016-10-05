﻿using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;


namespace ZXing.MicroQrCode.Internal
{
    /// <summary> A class which wraps a 2D array of bytes. The default usage is signed. If you want to use it as a
    /// unsigned container, it's up to you to do byteValue & 0xff at each location.
    /// 
    /// JAVAPORT: The original code was a 2D array of ints, but since it only ever gets assigned
    /// -1, 0, and 1, I'm going to use less memory and go with bytes.
    /// 
    /// </summary>
    /// <author>  dswitkin@google.com (Daniel Switkin)
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class ByteMatrix
    {
        public int Height
        {
            get
            {
                return height;
            }

        }
        public int Width
        {
            get
            {
                return width;
            }

        }
        public sbyte[][] Array
        {
            get
            {
                return bytes;
            }

        }

        //UPGRADE_NOTE: Final was removed from the declaration of 'bytes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private sbyte[][] bytes;
        //UPGRADE_NOTE: Final was removed from the declaration of 'width '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int width;
        //UPGRADE_NOTE: Final was removed from the declaration of 'height '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int height;

        public ByteMatrix(int width, int height)
        {
            bytes = new sbyte[height][];
            for (int i = 0; i < height; i++)
            {
                bytes[i] = new sbyte[width];
            }
            this.width = width;
            this.height = height;
        }

        public sbyte get_Renamed(int x, int y)
        {
            return bytes[y][x];
        }

        public void set_Renamed(int x, int y, sbyte value_Renamed)
        {
            bytes[y][x] = value_Renamed;
        }

        public void set_Renamed(int x, int y, int value_Renamed)
        {
            bytes[y][x] = (sbyte)value_Renamed;
        }

        public void clear(sbyte value_Renamed)
        {
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    bytes[y][x] = value_Renamed;
                }
            }
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder(2 * width * height + 2);
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    switch (bytes[y][x])
                    {

                        case 0:
                            result.Append(" 0");
                            break;

                        case 1:
                            result.Append(" 1");
                            break;

                        default:
                            result.Append("  ");
                            break;

                    }
                }
                result.Append('\n');
            }
            return result.ToString();
        }

        /// <summary>
        /// Converts this ByteMatrix to a black and white bitmap.
        /// </summary>
        /// <returns>A black and white bitmap converted from this ByteMatrix.</returns>
        public Bitmap ToBitmap()
        {
            const byte BLACK = 0;
            const byte WHITE = 255;
            sbyte[][] array = this.Array;
            int width = this.Width;
            int height = this.Height;
            byte[] pixels = new byte[width];

            //Here create the Bitmap to the known height, width and format
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            bmp.SetResolution(96.0F, 96.0F);
            //Create a BitmapData and Lock all pixels to be written
            BitmapData bmpData =
              bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                           ImageLockMode.WriteOnly, bmp.PixelFormat);

            for (int y = 0; y < height; y++)
            {
                int offset = (y * bmpData.Stride);
                for (int x = 0; x < width; x++)
                {
                    pixels[x] = array[y][x] == 0 ? BLACK : WHITE;
                }

                //Copy the data from the byte array into BitmapData.Scan0
                Marshal.Copy(pixels, 0, (IntPtr)((int)bmpData.Scan0 + (y * bmpData.Stride)), pixels.Length);
            }

            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            //Return the bitmap
            return bmp;
        }
    }
}
