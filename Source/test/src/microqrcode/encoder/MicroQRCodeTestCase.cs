/**
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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace ZXing.MicroQrCode.Internal.Test
{
    class MicroQRCodeTestCase
    {

        [Test]
        public void test()
        {
            var microQrCode = new MicroQRCode();

            // First, test simple setters and getters.
            // We use numbers of version 7-H.
            microQrCode.Mode = Mode.ALPHANUMERIC;
            microQrCode.ECLevel = ErrorCorrectionLevel.L;
            microQrCode.Version = 15;

            Assert.AreEqual(Mode.ALPHANUMERIC, microQrCode.Mode);
            Assert.AreEqual(ErrorCorrectionLevel.L, microQrCode.ECLevel);
            Assert.AreEqual(15, microQrCode.Version);
        }

        [Test]
        public void testQRCodeWriter()
        {
            // The QR should be multiplied up to fit, with extra padding if necessary
            int bigEnough = 50;  //Please note that 4 is always addded to the code as padding.
            MicroQRCodeWriter writer = new MicroQRCodeWriter();
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
            hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
            BitMatrix matrix = writer.encode("123456", BarcodeFormat.MICRO_QR_CODE, bigEnough,
                bigEnough, hints);

            Assert.NotNull(matrix);
            //Please note that 4 is always addded to the code as padding.
            Assert.AreEqual(bigEnough+4, matrix.Width);
            Assert.AreEqual(bigEnough+4, matrix.Height);
        }

        /*
        [Test]
        public void testQRCodeWriterToFileErrorM()
        {
            try
            {
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.MICRO_QR_CODE,

                    Options = new MicroQrCodeEncodingOptions
                    {
                        Height = 50,
                        Width = 50,
                        ErrorCorrection = ErrorCorrectionLevel.M,
                        Version = 3

                    },
                    Renderer = (IBarcodeRenderer<Bitmap>) Activator.CreateInstance(typeof(BitmapRenderer))
                };
                var encoded = writer.Write("123456");
                encoded.Save(@"C:\Data\microqr\testcase-microqr-1-ecM.bmp");
            }
            catch (Exception exc)
            {
            }
        }
        */
        /*
        [Test]
        public void testQRCodeWriterToFileFiftyErrorM()
        {
            try
            {

                //Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
                //hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                //hints.Add(EncodeHintType.MICROQRCODE_VERSION, 3);
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.MICRO_QR_CODE,

                    Options = new MicroQrCodeEncodingOptions
                    {
                        Height = 50,
                        Width = 50,
                        ErrorCorrection = ErrorCorrectionLevel.M,
                        Version = 3
                    },
                    Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(typeof(BitmapRenderer))
                };
                var encoded = writer.Write("Hello");
                encoded.Save(@"C:\Data\microqr\testcase-microqr-1-50x50-ecM.bmp");
            }
            catch (Exception exc)
            {
                
            }
        }
        */
        /*
        [Test]
        public void TestQrCodeWriterToFileM1()
        {
            String fileName = @"C:\Data\microqr\testcase-microqr-1-40x40-M1.bmp";
            String value = "12345";

            try
            {
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.MICRO_QR_CODE,

                    Options = new MicroQrCodeEncodingOptions
                    {
                        Height = 40,
                        Width = 40,
                        Version = 1
                    },
                    Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(typeof(BitmapRenderer))
                };
                var encoded = writer.Write("12345");
                encoded.Save(fileName);

                String decoded = DecodeFile(fileName);

                Assert.AreEqual(value, decoded);

            }
            catch (Exception exc)
            {

            }

        }
        */
        /*
        [Test]
        public void testFileWriterHelloWorld()
        {

            String fileName = @"C:\Data\microqr\HelloWorld.bmp";
            String value = "HELLO WORLD234";
            try
            {
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.MICRO_QR_CODE,

                    Options = new MicroQrCodeEncodingOptions
                    {
                        Height = 50,
                        Width = 50,
                        ErrorCorrection = ErrorCorrectionLevel.L,
                        Version = 3
                    },
                    Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(typeof(BitmapRenderer))
                };
                var encoded = writer.Write(value);
                encoded.Save(fileName);


                String decoded = DecodeFile(fileName);

                Assert.AreEqual(value,decoded);

            }
            catch (Exception exc)
            {
                Assert.Fail("Failed to encode/decode barcode Exception thrown");
            }
        }
        */

        private String DecodeFile(String file)
        {
            var fileName = file;

            using (var bitmap = (Bitmap)Bitmap.FromFile(fileName))
            {
                    return Decode(bitmap, false, null);
            }
        }

        private String Decode(Bitmap image, bool tryMultipleBarcodes, IList<BarcodeFormat> possibleFormats)
        {

            String decodedText = string.Empty;
       
            BarcodeReader barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new DecodingOptions { TryHarder = true }
            };

            barcodeReader.ResultFound += result =>
            {

            };



            Result[] results = null;
          
            if (tryMultipleBarcodes)
                results = barcodeReader.DecodeMultiple(image);
            else
            {
                var result = barcodeReader.Decode(image);
                if (result != null)
                {
                    //results = new[] { result };
                    return result.Text;
                }
            }

            if (results == null)
            {
                decodedText = "No barcode recognized";
            }

            if (results != null)
            {
                foreach (var result in results)
                {
                    decodedText += result.Text;
                }
            }
            return decodedText;
        }
    }
}
