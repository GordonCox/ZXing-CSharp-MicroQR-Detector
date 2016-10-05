/*
* Copyright 2007 ZXing authors
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
//using BitMatrix = com.google.zxing.common.BitMatrix;
//using Decoder = com.google.zxing.microqrcode.decoder.Decoder;
//using DecoderResult = com.google.zxing.common.DecoderResult;
//using Detector = com.google.zxing.microqrcode.detector.Detector;
//using DetectorResult = com.google.zxing.common.DetectorResult;

using System;
using System.Collections.Generic;
using ZXing.Common;
using ZXing.MicroQrCode.Internal;

namespace ZXing.MicroQrCode
{
	/// <summary>
    /// This implementation can detect and decode Micro QR Codes in an image.
	/// </summary>
	/// <author>  
    /// Sean Owen
	/// </author>
	/// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
	/// </author>
	public class MicroQRCodeReader : Reader
	{
        /// <summary>
        /// Gets the decoder.
        /// </summary>
        /// <returns></returns>
        protected Decoder getDecoder()
        {
            return decoder;
        }

        //UPGRADE_NOTE: Final was removed from the declaration of 'NO_POINTS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly ResultPoint[] NO_POINTS = new ResultPoint[0];
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'decoder '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Decoder decoder = new Decoder();

        /// <summary>
        /// Locates and decodes a Micro QR code in an image.
        ///
        /// <returns>a String representing the content encoded by the Micro QR code</returns>
        /// </summary>
        public Result decode(BinaryBitmap image)
        {
            return decode(image, null);
        }

        public virtual Result decode(BinaryBitmap image, IDictionary<DecodeHintType, object> hints)
		{
			DecoderResult decoderResult;
			ResultPoint[] points;
            //This was added to check if the barcode could be decoded without the detector.
            //The detector logic was never written and it does not function.
            //hints.Add(DecodeHintType.PURE_BARCODE,true);
			if (hints != null && hints.ContainsKey(DecodeHintType.PURE_BARCODE))
			{
                var bits = extractPureBits(image.BlackMatrix);
                if (bits == null)
                    return null;
                decoderResult = decoder.decode(bits, hints);
                points = NO_POINTS;
			}
			else
			{
                //throw new System.NotImplementedException("Detector(image.BlackMatrix).detect(hints) Not Implemented...");
                
                DetectorResult detectorResult = new Detector(image.BlackMatrix).detect(hints);
                if (detectorResult == null)
                    return null;
                decoderResult = decoder.decode(detectorResult.Bits, hints);
                points = detectorResult.Points;
            }
			
			Result result = new Result(decoderResult.Text, decoderResult.RawBytes, points, BarcodeFormat.MICRO_QR_CODE);
			if (decoderResult.ByteSegments != null)
			{
				result.putMetadata(ResultMetadataType.BYTE_SEGMENTS, decoderResult.ByteSegments);
			}
			if (decoderResult.ECLevel != null)
			{
				result.putMetadata(ResultMetadataType.ERROR_CORRECTION_LEVEL, decoderResult.ECLevel.ToString());
			}
			return result;
		}

        /// <summary>
        /// Resets any internal state the implementation has after a decode, to prepare it
        /// for reuse.
        /// </summary>
        public void reset()
        {
            // do nothing
        }

        /// <summary>
        /// This method detects a code in a "pure" image -- that is, pure monochrome image
        /// which contains only an unrotated, unskewed, image of a code, with some white border
        /// around it. This is a specialized method that works exceptionally fast in this special
        /// case.
        /// 
        /// <seealso cref="ZXing.Datamatrix.DataMatrixReader.extractPureBits(BitMatrix)" />
        /// </summary>
        private static BitMatrix extractPureBits(BitMatrix image)
        {
            int[] leftTopBlack = image.getTopLeftOnBit();
            int[] rightBottomBlack = image.getBottomRightOnBit();
            if (leftTopBlack == null || rightBottomBlack == null)
            {
                return null;
            }

            float moduleSize;
            if (!MicroQRCodeReader.moduleSize(leftTopBlack, image, out moduleSize))
                return null;

            int top = leftTopBlack[1];
            int bottom = rightBottomBlack[1];
            int left = leftTopBlack[0];
            int right = rightBottomBlack[0];

            // Sanity check!
            if (left >= right || top >= bottom)
            {
                return null;
            }

            if (bottom - top != right - left)
            {
                // Special case, where bottom-right module wasn't black so we found something else in the last row
                // Assume it's a square, so use height as the width
                right = left + (bottom - top);
                if (right >= image.Width)
                {
                    // Abort if that would not make sense -- off image
                    return null;
                }
            }

            int matrixWidth = (int)Math.Round((right - left + 1) / moduleSize);
            int matrixHeight = (int)Math.Round((bottom - top + 1) / moduleSize);
            if (matrixWidth <= 0 || matrixHeight <= 0)
            {
                return null;
            }
            if (matrixHeight != matrixWidth)
            {
                // Only possibly decode square regions
                return null;
            }

            // Push in the "border" by half the module width so that we start
            // sampling in the middle of the module. Just in case the image is a
            // little off, this will help recover.
            int nudge = (int)(moduleSize / 2.0f);
            top += nudge;
            left += nudge;

            // But careful that this does not sample off the edge
            // "right" is the farthest-right valid pixel location -- right+1 is not necessarily
            // This is positive by how much the inner x loop below would be too large
            int nudgedTooFarRight = left + (int)((matrixWidth - 1) * moduleSize) - right;
            if (nudgedTooFarRight > 0)
            {
                if (nudgedTooFarRight > nudge)
                {
                    // Neither way fits; abort
                    return null;
                }
                left -= nudgedTooFarRight;
            }
            // See logic above
            int nudgedTooFarDown = top + (int)((matrixHeight - 1) * moduleSize) - bottom;
            if (nudgedTooFarDown > 0)
            {
                if (nudgedTooFarDown > nudge)
                {
                    // Neither way fits; abort
                    return null;
                }
                top -= nudgedTooFarDown;
            }

            // Now just read off the bits
            BitMatrix bits = new BitMatrix(matrixWidth, matrixHeight);
            for (int y = 0; y < matrixHeight; y++)
            {
                int iOffset = top + (int)(y * moduleSize);
                for (int x = 0; x < matrixWidth; x++)
                {
                    if (image[left + (int)(x * moduleSize), iOffset])
                    {
                        bits[x, y] = true;
                    }
                }
            }
            return bits;
        }

        private static bool moduleSize(int[] leftTopBlack, BitMatrix image, out float msize)
        {
            int height = image.Height;
            int width = image.Width;
            int x = leftTopBlack[0];
            int y = leftTopBlack[1];
            bool inBlack = true;
            int transitions = 0;
            while (x < width && y < height)
            {
                if (inBlack != image[x, y])
                {
                    if (++transitions == 5)
                    {
                        break;
                    }
                    inBlack = !inBlack;
                }
                x++;
                y++;
            }
            if (x == width || y == height)
            {
                msize = 0.0f;
                return false;
            }
            msize = (x - leftTopBlack[0]) / 7.0f;
            return true;
        }

	    /// <summary>
	    /// This method detects a code in a "pure" image -- that is, pure monochrome image
	    /// which contains only an unrotated, unskewed, image of a code, with some white border
	    /// around it. This is a specialized method that works exceptionally fast in this special
	    /// case.
	    /// 
	    /// <seealso cref="ZXing.Datamatrix.DataMatrixReader.extractPureBits(BitMatrix)" />
	    /// </summary>
	    private static BitMatrix extractPureBits(BitMatrix image, int topLeft, int bottomRight)
	    {
	        
	            int[] leftTopBlack = new int[2] {topLeft, topLeft};
	            int[] rightBottomBlack = new int[2] {bottomRight, bottomRight};
	            if (leftTopBlack == null || rightBottomBlack == null)
	            {
	                return null;
	            }

	            float moduleSize;
	            if (!MicroQRCodeReader.moduleSize(leftTopBlack, image, out moduleSize))
	                return null;

	            int top = leftTopBlack[1];
	            int bottom = rightBottomBlack[1];
	            int left = leftTopBlack[0];
	            int right = rightBottomBlack[0];

	            // Sanity check!
	            if (left >= right || top >= bottom)
	            {
	                return null;
	            }

	            if (bottom - top != right - left)
	            {
	                // Special case, where bottom-right module wasn't black so we found something else in the last row
	                // Assume it's a square, so use height as the width
	                right = left + (bottom - top);
	                if (right >= image.Width)
	                {
	                    // Abort if that would not make sense -- off image
	                    return null;
	                }
	            }

	            int matrixWidth = (int) Math.Round((right - left + 1)/moduleSize);
	            int matrixHeight = (int) Math.Round((bottom - top + 1)/moduleSize);
	            if (matrixWidth <= 0 || matrixHeight <= 0)
	            {
	                return null;
	            }
	            if (matrixHeight != matrixWidth)
	            {
	                // Only possibly decode square regions
	                return null;
	            }

	            // Push in the "border" by half the module width so that we start
	            // sampling in the middle of the module. Just in case the image is a
	            // little off, this will help recover.
	            int nudge = (int) (moduleSize/2.0f);
	            top += nudge;
	            left += nudge;

	            // But careful that this does not sample off the edge
	            // "right" is the farthest-right valid pixel location -- right+1 is not necessarily
	            // This is positive by how much the inner x loop below would be too large
	            int nudgedTooFarRight = left + (int) ((matrixWidth - 1)*moduleSize) - right;
	            if (nudgedTooFarRight > 0)
	            {
	                if (nudgedTooFarRight > nudge)
	                {
	                    // Neither way fits; abort
	                    return null;
	                }
	                left -= nudgedTooFarRight;
	            }
	            // See logic above
	            int nudgedTooFarDown = top + (int) ((matrixHeight - 1)*moduleSize) - bottom;
	            if (nudgedTooFarDown > 0)
	            {
	                if (nudgedTooFarDown > nudge)
	                {
	                    // Neither way fits; abort
	                    return null;
	                }
	                top -= nudgedTooFarDown;
	            }

	            // Now just read off the bits
	            BitMatrix bits = new BitMatrix(matrixWidth, matrixHeight);
	            for (int y = 0; y < matrixHeight; y++)
	            {
	                int iOffset = top + (int) (y*moduleSize);
	                for (int x = 0; x < matrixWidth; x++)
	                {
	                    if (image[left + (int) (x*moduleSize), iOffset])
	                    {
	                        bits[x, y] = true;
	                    }
	                }
	            }
	            return bits;
	        
	    }

	}
}