/*
* Copyright 2008 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
using System;
using ZXing.Common;
using ZXing.MicroQrCode.Internal;
using ErrorCorrectionLevel = ZXing.QrCode.Internal.ErrorCorrectionLevel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Encoder = ZXing.MicroQrCode.Internal.Encoder;

namespace ZXing.MicroQrCode
{

    /// <summary>
    /// This object renders a Micro QR Code as a ByteMatrix 2D array of greyscale values.
    /// </summary>
    /// <author>  dswitkin@google.com (Daniel Switkin)
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class MicroQRCodeWriter : Writer
    {
        private const int QUIET_ZONE_SIZE = 2;

        public BitMatrix encode(System.String contents, BarcodeFormat format, int width, int height)
        {

            return encode(contents, format, width, height, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hints"></param>
        /// <returns></returns>
        public BitMatrix encode(System.String contents, BarcodeFormat format, int width, int height, IDictionary<EncodeHintType, object> hints)
        {
            //contents = "12345";
            //width = 17;
            //height = 17;
            //hints = new Dictionary<EncodeHintType, object>();
            //hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
            //I should probably throw an exception here.
            if (!hints.ContainsKey(EncodeHintType.MICROQRCODE_VERSION))
            {
                hints.Add(EncodeHintType.MICROQRCODE_VERSION, 4);
            }

            if (contents == null || contents.Length == 0)
            {
                throw new System.ArgumentException("Found empty contents");
            }

            if (format != BarcodeFormat.MICRO_QR_CODE)
            {
                throw new System.ArgumentException("Can only encode MICRO_QR_CODE, but got " + format);
            }

            if (width < 0 || height < 0)
            {
                throw new System.ArgumentException("Requested dimensions are too small: " + width + 'x' + height);
            }


            //hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

            var errorCorrectionLevel = ErrorCorrectionLevel.L;
            int cVersionNum = -1;
            if (hints != null)
            {
                if (hints.ContainsKey(EncodeHintType.ERROR_CORRECTION))
                {
                    var requestedECLevel = hints[EncodeHintType.ERROR_CORRECTION];
                    if (requestedECLevel != null)
                    {
                        errorCorrectionLevel = requestedECLevel as ErrorCorrectionLevel;
                        if (errorCorrectionLevel == null)
                        {
                            switch (requestedECLevel.ToString().ToUpper())
                            {
                                case "L":
                                    errorCorrectionLevel = ErrorCorrectionLevel.L;
                                    break;
                                case "M":
                                    errorCorrectionLevel = ErrorCorrectionLevel.M;
                                    break;
                                case "Q":
                                    errorCorrectionLevel = ErrorCorrectionLevel.Q;
                                    break;
                                default:
                                    errorCorrectionLevel = ErrorCorrectionLevel.L;
                                    break;
                            }
                        }
                    }
                }
                if (hints.ContainsKey(EncodeHintType.MICROQRCODE_VERSION))
                {
                    int versionNum = (int) hints[EncodeHintType.MICROQRCODE_VERSION];
                    if (versionNum >= 1 && versionNum <= 4)
                        cVersionNum = versionNum;
                }
            }

            MicroQRCode code = new MicroQRCode();
            //Step 1 Encode the Data.
            Encoder.encode(contents, errorCorrectionLevel, hints, code, cVersionNum);

            //Step 2 Render the ByteMatrix using the original code.
            ByteMatrix microQrCodeByteMatrix = renderByteMatrix(code, width, height);

            //Step 3 Convert the Byte Matrix to a QrCode.ByteMatrix (swapping 1 for 0 and 0 for 1)
            ZXing.QrCode.Internal.ByteMatrix qrCodeByteMatrix = new QrCode.Internal.ByteMatrix(width,height);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    // Get the byte value and perform byye swap (swapping 1 for 0 and 0 for 1)
                    byte value = (byte) microQrCodeByteMatrix.get_Renamed(x, y);
                    if (value == 0)
                        qrCodeByteMatrix.set(x, y, 1);
                    else
                    {
                        qrCodeByteMatrix.set(x, y, 0);
                    }
                }
            }

            //Step 4 render the result and return a BitMatrix
            BitMatrix bitMatrix2 = renderResult(qrCodeByteMatrix, code, width, height, QUIET_ZONE_SIZE);

            return bitMatrix2;
        }

        // Note that the input matrix uses 0 == white, 1 == black, while the output matrix uses
        // 0 == black, 255 == white (i.e. an 8 bit greyscale bitmap).
        private static BitMatrix renderResult(ZXing.QrCode.Internal.ByteMatrix bitMatrix, MicroQRCode code, int width, int height, int quietZone)
        {
            ZXing.QrCode.Internal.ByteMatrix input = bitMatrix;
            if (input == null)
            {
                throw new InvalidOperationException();
            }
            int inputWidth = input.Width;
            int inputHeight = input.Height;
            int qrWidth = inputWidth + (quietZone << 1);
            int qrHeight = inputHeight + (quietZone << 1);
            int outputWidth = Math.Max(width, qrWidth);
            int outputHeight = Math.Max(height, qrHeight);

            int multiple = Math.Min(outputWidth / qrWidth, outputHeight / qrHeight);
            // Padding includes both the quiet zone and the extra white pixels to accommodate the requested
            // dimensions. For example, if input is 25x25 the QR will be 33x33 including the quiet zone.
            // If the requested size is 200x160, the multiple will be 4, for a QR of 132x132. These will
            // handle all the padding from 100x100 (the actual QR) up to 200x160.
            int leftPadding = (outputWidth - (inputWidth * multiple)) / 2;
            int topPadding = (outputHeight - (inputHeight * multiple)) / 2;

            var output = new BitMatrix(outputWidth, outputHeight);

            for (int inputY = 0, outputY = topPadding; inputY < inputHeight; inputY++, outputY += multiple)
            {
                // Write the contents of this row of the barcode
                for (int inputX = 0, outputX = leftPadding; inputX < inputWidth; inputX++, outputX += multiple)
                {
                    if (input[inputX, inputY] == 1)
                    {
                        output.setRegion(outputX, outputY, multiple, multiple);
                    }
                }
            }

            return output;
        }


        // Note that the input matrix uses 0 == white, 1 == black, while the output matrix uses
        // 0 == black, 255 == white (i.e. an 8 bit greyscale bitmap).
        private static ByteMatrix renderByteMatrix(MicroQRCode code, int width, int height)
        {
            ByteMatrix input = code.Matrix;
            int inputWidth = input.Width;
            int inputHeight = input.Height;
            int qrWidth = inputWidth + (QUIET_ZONE_SIZE << 1);
            int qrHeight = inputHeight + (QUIET_ZONE_SIZE << 1);
            int outputWidth = System.Math.Max(width, qrWidth);
            int outputHeight = System.Math.Max(height, qrHeight);

            int multiple = System.Math.Min(outputWidth / qrWidth, outputHeight / qrHeight);
            // Padding includes both the quiet zone and the extra white pixels to accommodate the requested
            // dimensions. For example, if input is 25x25 the QR will be 33x33 including the quiet zone.
            // If the requested size is 200x160, the multiple will be 4, for a QR of 132x132. These will
            // handle all the padding from 100x100 (the actual QR) up to 200x160.
            int leftPadding = (outputWidth - (inputWidth * multiple)) / 2;
            int topPadding = (outputHeight - (inputHeight * multiple)) / 2;

            ByteMatrix output = new ByteMatrix(outputWidth, outputHeight);
            sbyte[][] outputArray = output.Array;

            // We could be tricky and use the first row in each set of multiple as the temporary storage,
            // instead of allocating this separate array.
            sbyte[] row = new sbyte[outputWidth];

            // 1. Write the white lines at the top
            for (int y = 0; y < topPadding; y++)
            {
                setRowColor(outputArray[y], (sbyte)SupportClass.Identity(255));
            }

            // 2. Expand the QR image to the multiple
            sbyte[][] inputArray = input.Array;
            for (int y = 0; y < inputHeight; y++)
            {
                // a. Write the white pixels at the left of each row
                for (int x = 0; x < leftPadding; x++)
                {
                    row[x] = (sbyte)SupportClass.Identity(255);
                }

                // b. Write the contents of this row of the barcode
                int offset = leftPadding;
                for (int x = 0; x < inputWidth; x++)
                {
                    // Redivivus.in Java to c# Porting update - Type cased sbyte
                    // 30/01/2010 
                    // sbyte value_Renamed = (inputArray[y][x] == 1)?0:(sbyte) SupportClass.Identity(255);
                    sbyte value_Renamed = (sbyte)((inputArray[y][x] == 1) ? 0 : SupportClass.Identity(255));
                    for (int z = 0; z < multiple; z++)
                    {
                        row[offset + z] = value_Renamed;
                    }
                    offset += multiple;
                }

                // c. Write the white pixels at the right of each row
                offset = leftPadding + (inputWidth * multiple);
                for (int x = offset; x < outputWidth; x++)
                {
                    row[x] = (sbyte)SupportClass.Identity(255);
                }

                // d. Write the completed row multiple times
                offset = topPadding + (y * multiple);
                for (int z = 0; z < multiple; z++)
                {
                    Array.Copy(row, 0, outputArray[offset + z], 0, outputWidth);
                }
            }

            // 3. Write the white lines at the bottom
            int offset2 = topPadding + (inputHeight * multiple);
            for (int y = offset2; y < outputHeight; y++)
            {
                setRowColor(outputArray[y], (sbyte)SupportClass.Identity(255));
            }

            return output;
        }

        private static void setRowColor(sbyte[] row, sbyte value_Renamed)
        {
            for (int x = 0; x < row.Length; x++)
            {
                row[x] = value_Renamed;
            }
        }

    }
}