/*
* Copyright 2016 ZXing authors
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
using System.Linq;
using ZXing.Common;
using ZXing.Common.Detector;


namespace ZXing.MicroQrCode.Internal
{

    /// <summary> <p>Encapsulates logic that can detect a Micro QR Code in an image, even if the Micro QR Code
    /// is rotated.</p>
    /// 
    /// </summary>
    /// <author> Michael Cox</author>
    /// <author>Sean Owen</author>
    public class Detector
    {
        virtual protected internal BitMatrix Image
        {
            get
            {
                return image;
            }

        }
        virtual protected internal ResultPointCallback ResultPointCallback
        {
            get
            {
                return resultPointCallback;
            }
        }

        //UPGRADE_NOTE: Final was removed from the declaration of 'image '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private BitMatrix image;
        private ResultPointCallback resultPointCallback;

        public Detector(BitMatrix image)
        {
            this.image = image;
        }

        /// <summary> <p>Detects a QR Code in an image, simply.</p>
        /// 
        /// </summary>
        /// <returns> {@link DetectorResult} encapsulating results of detecting a QR Code
        /// </returns>
        /// <throws>  ReaderException if no QR Code can be found </throws>
        public virtual DetectorResult detect()
        {
            return detect(null);
        }

        /// <summary> <p>Detects a QR Code in an image, simply.</p>
        /// 
        /// </summary>
        /// <param name="hints">optional hints to detector
        /// </param>
        /// <returns> {@link DetectorResult} encapsulating results of detecting a QR Code
        /// </returns>
        /// <throws>  ReaderException if no QR Code can be found </throws>
        public virtual DetectorResult detect(IDictionary<DecodeHintType, object> hints)
        {
            if (hints.ContainsKey(DecodeHintType.NEED_RESULT_POINT_CALLBACK))
            {
                resultPointCallback = hints == null
                    ? null
                    : (ResultPointCallback) hints[DecodeHintType.NEED_RESULT_POINT_CALLBACK];
            }

            FinderPatternFinder finder = new FinderPatternFinder(image, resultPointCallback);
            FinderPatternInfo info = finder.find(hints);

            if (info == null)
            {
                return null;
            }
            return processFinderPatternInfo(info);
        }

        /// <summary>
        /// This method should become the main logic for processing the micro qr code.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected internal virtual DetectorResult processFinderPatternInfo(FinderPatternInfo info)
        {
            //As long as the top left finder info point was found then it is a micro qr code and not a data matrix.
            //If three finder info points are found then it is a qr code and not a micro qr code.
            if (info.TopLeft != null)
            {

                //Added for debugging
                //var orignalImageAsBitMap = GetBitmap(image);
                //orignalImageAsBitMap.Save(@"C:\Data\originalImage.bmp");

                DetectorResult microQrDetectorResult = DetectMicroQrCode(info.TopLeft);

                if (microQrDetectorResult != null)
                    return microQrDetectorResult;
            }
            return null;
        }
        //The transform used to calculate the poition in the image of the barcode according to three corner points and the dimension
        //and out
        /// <summary>
        ///The transform used to calculate the poition in the image of the barcode according to three corner points and the dimension
        ///and output the transformation that will be used to sample the bits from the image.
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="bottomLeft"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        private static PerspectiveTransform createTransform(ResultPoint topLeft, ResultPoint topRight, ResultPoint bottomLeft, int dimension)
        {
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            //float dimMinusThree = (float)dimension - 0.5f;
            float dimMinusThree = (float)dimension - 0.1f;
            //float dimMinusThree = (float)dimension - 1.0f;
            float bottomRightX;
            float bottomRightY;
            float sourceBottomRightX;
            float sourceBottomRightY;
 
            // Micro QR codes don't have an alignment pattern, so just make up the bottom-right point
            bottomRightX = (topRight.X - topLeft.X) + bottomLeft.X;
            bottomRightY = (topRight.Y - topLeft.Y) + bottomLeft.Y;
            sourceBottomRightX = sourceBottomRightY = dimMinusThree;

            return PerspectiveTransform.quadrilateralToQuadrilateral(
               0.1f,
               0.1f,
               dimMinusThree,
               0.1f,
               sourceBottomRightX,
               sourceBottomRightY,
               0.1f,
               dimMinusThree,
               topLeft.X,
               topLeft.Y,
               topRight.X,
               topRight.Y,
               bottomRightX,
               bottomRightY,
               bottomLeft.X,
               bottomLeft.Y);
        }
        /// <summary>
        /// Sample the image bits based on the transform and the dimension of the expected barcode.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="transform"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        private static BitMatrix sampleGrid(BitMatrix image, PerspectiveTransform transform, int dimension)
        {
            GridSampler sampler = GridSampler.Instance;
            return sampler.sampleGrid(image, dimension, dimension, transform);
        }
        /// <summary> <p>Computes the dimension (number of modules on a size) of the QR Code based on the position
        /// of the finder patterns and estimated module size.</p>
        /// </summary>
        private static bool computeDimension(ResultPoint topLeft, ResultPoint topRight, ResultPoint bottomLeft, float moduleSize, out int dimension)
        {
            int tltrCentersDimension = MathUtils.round(ResultPoint.distance(topLeft, topRight) / moduleSize);
            int tlblCentersDimension = MathUtils.round(ResultPoint.distance(topLeft, bottomLeft) / moduleSize);
            dimension = (tltrCentersDimension + tlblCentersDimension) >> 1;
            switch (dimension & 0x03)
            {
                // mod 4
                case 0:
                    dimension++;
                    break;
                // 1? do nothing
                case 2:
                    dimension--;
                    break;
                case 3:
                    return true;
            }
            return true;
        }

        /// <summary>
        ///   <p>Computes an average estimated module size based on the timing pattern
        /// As the timing pattern starts at a different point for each 
        ///   </p>
        /// </summary>
        /// <param name="topRowStartTiming">detected top-left finder pattern center</param>
        /// <param name="topRight">detected top-right finder pattern center</param>
        /// <param name="leftColumnStartTiming">detected top-left finder pattern center</param>
        /// <param name="bottomLeft">detected bottom-left finder pattern center</param>
        /// <returns>estimated module size</returns>
        protected internal virtual float estimateModuleSize(ResultPoint topRowStartTiming, ResultPoint topRight, ResultPoint leftColumnStartTiming, ResultPoint bottomLeft)
        {
            //Calculate the number of transitions for each timing pattern, Transitions start at the timing patters so we must add 1
            var moduleSizeEst1 = (float) transitionsBetween(topRowStartTiming, topRight).Transitions + 1;

            var moduleSizeEst2 = (float) transitionsBetween(leftColumnStartTiming, bottomLeft).Transitions + 1;

            float dx = CalculateDistance(topRowStartTiming, topRight);
            float dy = CalculateDistance(leftColumnStartTiming, bottomLeft);

            var calculatedSize1 = dx / moduleSizeEst1;
            var calculatedSize2 = dy / moduleSizeEst2;

            var result =  (calculatedSize1 + calculatedSize2) / 2.0f;
            return result;
        }
        /// <summary>
        /// Returns a set of ResultPoints and Rotation that are close to one another
        /// based on an acceptable amount of degrees in radians
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="resultPointB"></param>
        /// <param name="points"></param>
        /// <param name="degreesInRadians"></param>
        /// <returns></returns>
        private IEnumerable<ResultPointsAndRotation> GetPointsWithinDegrees(ResultPoint centerPoint, ResultPoint resultPointB, IEnumerable<ResultPointsAndRotation> points,
            double degreesInRadians)
        {
            IList<ResultPointsAndRotation> results = new List<ResultPointsAndRotation>();

                foreach (var resultPointC in points)
                {
                    if (CalculateAngle(centerPoint, resultPointB, resultPointC.Point) < degreesInRadians)
                    {
                        results.Add(resultPointC);
                    }

            }
            return results;
        }
        /// <summary>
        /// Calculate the angle in radians for two points.
        /// </summary>
        /// <param name="resultPointA"></param>
        /// <param name="resultPointB"></param>
        /// <returns></returns>
        private double ComputeAngle(ResultPoint resultPointA, ResultPoint resultPointB)
        {
            //calculate delta x and delta y between the two points
            var deltaY = resultPointB.Y - resultPointA.Y;
            var deltaX = resultPointB.X - resultPointA.X;

            return Math.Atan2(deltaY, deltaX);
        }

        /// <summary>
        /// Compute the angle at resultPointA between resultPointB and resultPointC
        /// </summary>
        /// <param name="resultPointA"></param>
        /// <param name="resultPointB"></param>
        /// <param name="resultPointC"></param>
        /// <returns></returns>
        private double CalculateAngle(ResultPoint resultPointA, ResultPoint resultPointB, ResultPoint resultPointC)
        {

            var sideA = CalculateDistance(resultPointB, resultPointC); //Opposite resultPointA
            var sideB = CalculateDistance(resultPointA, resultPointC); //Opposite resultPointB
            var sideC = CalculateDistance(resultPointA, resultPointB); //Opposite resultPointC
            var angleRadians = Math.Acos((Math.Pow(sideB, 2) + Math.Pow(sideC, 2) - Math.Pow(sideA, 2))/(2*sideB*sideC));

            return angleRadians;
        }
        /// <summary>
        /// Calculate the distance between startPoint and endPoint as a float
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        protected internal virtual float CalculateDistance(ResultPoint startPoint, ResultPoint endPoint)
        {
            int fromX = (int)startPoint.X;
            int fromY = (int)startPoint.Y;
            int toX = (int)endPoint.X;
            int toY = (int)endPoint.Y;

            int xstep = fromX < toX ? 1 : -1;
            int ystep = fromY < toY ? 1 : -1;

            float dx = MathUtils.distance(toX + xstep, toY, fromX, fromY);
            return dx;
        }

        /// <summary> <p>This method traces a line from a point in the image, in the direction towards another point.
        /// It begins in a black region, and keeps going until it finds white, then black, then white again.
        /// It reports the distance from the start to this point.</p>
        /// 
        /// <p>This is used when figuring out how wide a finder pattern is, when the finder pattern
        /// may be skewed or rotated.</p>
        /// </summary>
        private float sizeOfBlackWhiteBlackRun(int fromX, int fromY, int toX, int toY)
        {
            // Mild variant of Bresenham's algorithm;
            // see http://en.wikipedia.org/wiki/Bresenham's_line_algorithm
            bool steep = Math.Abs(toY - fromY) > Math.Abs(toX - fromX);
            if (steep)
            {
                int temp = fromX;
                fromX = fromY;
                fromY = temp;
                temp = toX;
                toX = toY;
                toY = temp;
            }

            int dx = Math.Abs(toX - fromX);
            int dy = Math.Abs(toY - fromY);
            int error = -dx >> 1;
            int xstep = fromX < toX ? 1 : -1;
            int ystep = fromY < toY ? 1 : -1;

            // In black pixels, looking for white, first or second time.
            int state = 0;
            // Loop up until x == toX, but not beyond
            int xLimit = toX + xstep;
            for (int x = fromX, y = fromY; x != xLimit; x += xstep)
            {
                int realX = steep ? y : x;
                int realY = steep ? x : y;

                // Does current pixel mean we have moved white to black or vice versa?
                // Scanning black in state 0,2 and white in state 1, so if we find the wrong
                // color, advance to next state or end if we are in state 2 already
                if ((state == 1) == image[realX, realY])
                {
                    if (state == 2)
                    {
                        return MathUtils.distance(x, y, fromX, fromY);
                    }
                    state++;
                }
                error += dy;
                if (error > 0)
                {
                    if (y == toY)
                    {


                        break;
                    }
                    y += ystep;
                    error -= dx;
                }
            }
            // Found black-white-black; give the benefit of the doubt that the next pixel outside the image
            // is "white" so this last point at (toX+xStep,toY) is the right ending. This is really a
            // small approximation; (toX+xStep,toY+yStep) might be really correct. Ignore this.
            if (state == 2)
            {
                return MathUtils.distance(toX + xstep, toY, fromX, fromY);
            }
            // else we didn't find even black-white-black; no estimate is really possible
            return Single.NaN;

        }
        /// <summary>
        /// Removes the bottom right point by finding the point fathest from the finderPattern
        /// The point farthest from the finder pattern is always the bottom right.
        /// </summary>
        /// <param name="resultPoints"></param>
        /// <param name="finderPattern"></param>
        /// <returns></returns>
        public ResultPoint[] GetThreeCornerResultPoints(ResultPoint[] resultPoints, ResultPoint finderPattern)
        {
            // Point A[0] and D[3] are across the diagonal from one another,
            // as are B[1] and C[2]. Figure out which are the solid black lines
            // by counting transitions

            int[] distances = new int[4];
            distances[0] = (int)CalculateDistance(finderPattern, resultPoints[0]);
            distances[1] = (int)CalculateDistance(finderPattern, resultPoints[1]);
            distances[2] = (int)CalculateDistance(finderPattern, resultPoints[2]);
            distances[3] = (int)CalculateDistance(finderPattern, resultPoints[3]);
            var maxDistance = distances.Max();
            //The Top Left point is the one with the least disance.
            //The Bottom Right is the point opposite the top left.
            //Discard the minimum point as it is the Bottom Right.
            ResultPoint[] cornerResultPoints = new ResultPoint[3];
            int position = 0;
            foreach (var resultPoint in resultPoints)
            {
                if ((int) CalculateDistance(finderPattern, resultPoint) != maxDistance)
                {
                    cornerResultPoints[position] = resultPoint;
                    position ++;
                }
            }

            ResultPoint.orderBestPatterns(cornerResultPoints);
            return cornerResultPoints;
        }

        /// <summary>
        /// Averages the result points (x,y) for a set of result points (x,y).
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private ResultPointsAndRotation GetAverage(IEnumerable<ResultPointsAndRotation> points)
        {
            var xValue = (int)Math.Round(points.Select(x => x.Point.X).Average(),0);
            var yValue = (int)Math.Round(points.Select(y => y.Point.Y).Average(), 0);
            var angleValue = points.Select(a => a.Angle).Average();
            var distanceValue = points.Select(d => d.Distance).Average();

            return new ResultPointsAndRotation(new ResultPoint(xValue, yValue), angleValue, distanceValue);
        }

        /// <summary>
        /// Scales a set of result points from the center for a given value of scale.
        /// </summary>
        /// <param name="resultPoints"></param>
        /// <param name="centerPoint"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private ResultPoint[] ScalePointsMatrix(ResultPoint[] resultPoints, ResultPoint centerPoint,
            float scale)
        {
            return null;
            /*
            Point[] points = new Point[resultPoints.Length];
            for (int i = 0; i < resultPoints.Length; i++)
            {
                points[i] = new Point((int)resultPoints[i].X, (int)resultPoints[i].Y);
            }
            Math.
            // Create a matrix and scale it.
            Matrix myMatrix = new Matrix();

            myMatrix.Translate(centerPoint.X - centerPoint.X * scale, centerPoint.Y - centerPoint.Y * scale);
            myMatrix.Scale(scale, scale);
            myMatrix.TransformPoints(points);


            ResultPoint[] resultPointsAndRotation = new ResultPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                ResultPoint resultPoint = new ResultPoint(points[i].X, points[i].Y);
                resultPointsAndRotation[i] = resultPoint;
            }
            return resultPointsAndRotation;
            */
        }

        private ResultPoint[] ShiftPoints(ResultPoint[] resultPoints, ResultPoint shift)
        {
            ResultPoint[] returnPoints = new ResultPoint[resultPoints.Length];
            for (int i = 0; i < resultPoints.Length; i ++)
            {
                resultPoints[i] = new ResultPoint(resultPoints[i].X - shift.X, resultPoints[i].Y - shift.Y);
            }
            return resultPoints;
        }

        private ResultPoint[] ScalePoints(ResultPoint[] resultPoints, ResultPoint centerPoint,
            float scale)
        {
            ResultPoint[] returnPoints = new ResultPoint[resultPoints.Length];
            //float scaleX = (float)width / boundingPoints.width;
            //float scaleY = (float)height / boundingPoints.height;
            float scaleX = scale;
            float scaleY = scale;
            float bx = centerPoint.X;
            float by = centerPoint.Y;

            for (int i = 0; i < resultPoints.Length; i++)
            {
                float x1 = ((resultPoints[i].X - bx) * scaleX);
                x1 += bx;
                float y1 = ((resultPoints[i].Y - by) * scaleY);
                y1 += by;
                returnPoints[i] = new ResultPoint((int)x1, (int)y1);
            }
            return returnPoints;
        }

        private ResultPoint[] OrderPoints(IEnumerable<ResultPoint> points)
        {
            var pointsList = points.ToList();
            float greatestDistance = 0;
            int oppositeIndex = 0;

            ResultPoint[] corners = new ResultPoint[4];
            var cornerPoint = pointsList[0];
            for (int i = 1; i <= 3; i++)
            {
                var distance = CalculateDistance(cornerPoint, pointsList[i]);
                if (distance > greatestDistance)
                {
                    greatestDistance = distance;
                    oppositeIndex = i;
                }
            }
            List<int> ints = new List<int>() { 1, 2, 3 };
            ints = ints.Where(x => x != oppositeIndex).ToList();
            corners[0] = cornerPoint;
            corners[1] = pointsList[ints[0]];
            corners[2] = pointsList[ints[1]];
            corners[3] = pointsList[oppositeIndex];

            return corners;
        }

        /// <summary>
        /// Sweep across a set of points looking for the black point 
        /// farthest away from the sweepPoint from the center point
        /// The modules size is used to determine the maxium distance
        /// that the resulting black point can exist at.
        /// The sweep is done from -20 to +20 degrees of the sweepPoint.
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="sweepPoint"></param>
        /// <param name="moduleSize"></param>
        /// <returns></returns>
        public ResultPoint SweepAngle(ResultPoint centerPoint, ResultPoint sweepPoint, float moduleSize)
        {
            var distance = CalculateDistance(centerPoint, sweepPoint);
            double angle = ComputeAngle(centerPoint, sweepPoint);

            double sweepStartAngle = angle + (-DegreeToRadian(20));
            double sweepEndAngle = angle + DegreeToRadian(20);

            double oneDegreeInRadians = DegreeToRadian(1.0);
            double sweepAngle = sweepStartAngle;
            
            ResultPoint farthestResultPoint = sweepPoint;

            var maxDistance = distance + (moduleSize * 2);

            //Initalize greatest distance to distance so that no value can be less then distance we never expect the
            //result to be less than the sweepPoint
            //TODO Check if we need to multiply the module size by 2 or not.  A smaller distance might be prefered to
            //TODO to prevent running off the 
            var greatestDistance = distance;

            while (sweepAngle < sweepEndAngle)
            {
                var atAngleAndDistance = GetResultPointAtAngleAndDistance(centerPoint, sweepAngle, maxDistance);
                ResultPoint shrunkResultPoint = ShrinkVertexToBlackModule(centerPoint, atAngleAndDistance, maxDistance);

                var checkDistance = CalculateDistance(centerPoint, shrunkResultPoint);
                if (checkDistance >= greatestDistance)
                {
                    farthestResultPoint = shrunkResultPoint;
                    greatestDistance = checkDistance;
                }

                sweepAngle = sweepAngle + oneDegreeInRadians;
            }
            return farthestResultPoint;
        }

        /// <summary>
        /// The main logic for calculating the pixels used to decode a Micro QR Code.
        /// The topLeft Result Point is in the middle of the finder Pattern
        /// The rest of the caculation to determine the pixels for the Micro QR Code 
        /// is located here.
        /// </summary>
        /// <param name="topLeft"></param>
        /// <returns></returns>
        public DetectorResult DetectMicroQrCode(ResultPoint topLeft)
        {
            //Step 1 Caculate the black vertex points for the finder pattern.
            ResultPoint[] finderPatternPoints = CalculateFinderPatternPoints(topLeft);
            
            //The points are correct but the order of the points could be incorrect.
            var maybeTopLeft = finderPatternPoints[0];
            //var isBlackMaybeTopLeft = image[(int)maybeTopLeft.X, (int)maybeTopLeft.Y];
            var maybeTopRight = finderPatternPoints[1];
            //var isBlackMaybeTopRight = image[(int)maybeTopRight.X, (int)maybeTopRight.Y];
            var maybeBottomLeft = finderPatternPoints[2];
            //var isBlackMaybeBottomLeft = image[(int)maybeBottomLeft.X, (int)maybeBottomLeft.Y];
            var maybeBottomRight = finderPatternPoints[3];
            //var isBlackMaybeBottomRight = image[(int)maybeBottomRight.X, (int)maybeBottomRight.Y];

            //Get the module size
            //add up the distance between each point of the square and average the
            //caculated distance
            //divide by 7 as each black edge of the square will have exactly 7 modules.

            var totalDistance = CalculateDistance(maybeTopLeft, maybeTopRight);
            totalDistance += CalculateDistance(maybeTopLeft, maybeBottomLeft);
            totalDistance += CalculateDistance(maybeBottomRight, maybeTopRight);
            totalDistance += CalculateDistance(maybeBottomRight, maybeBottomLeft);

            //divide the total distance by 4 to get the average 
            float outerSquareDistance = (float)totalDistance / 4;
            //divide by 7 to get the size of the modules.
            float moduleSize = (float)outerSquareDistance / 7;

            //Multiply the modules size by 10.5 so that the edges are inside the first 
            //black module of the timing pattern along two edges of the square
            float timingScaleFactor = (float)(moduleSize * 10.5) / outerSquareDistance;

            //Multiple the modules size by 6 so that the points are inside the outer black square
            //of the finder pattern
            float innerSquareScaleFactor = (float)(moduleSize * 6) / outerSquareDistance;

            //Multiply the modules size by 10.5 so that the edges are inside the first 
            //black module of the timing pattern along two edges of the square
            float largestBarcodeScaleFactor = (float)(moduleSize * 18) / outerSquareDistance;

            //Scale back half a module from the center so that we are 
            //in the middle of the outer black square for the finder pattern.
            var largestBarcodeSquarePoints = ScalePoints(finderPatternPoints, topLeft, largestBarcodeScaleFactor);
            largestBarcodeSquarePoints = OrderPoints(largestBarcodeSquarePoints);


            //Scale back half a module from the center so that we are 
            //in the middle of the outer black square for the finder pattern.
            var innerSquarePoints = ScalePoints(finderPatternPoints, topLeft, innerSquareScaleFactor);
            innerSquarePoints = OrderPoints(innerSquarePoints);

            //Now I need to get the outer white boarder and find two side that are all white.  
            //Vertex of these two sides represent the top left of the barcode.
            //The point currently know as "topLeft" is the center of the finder pattern.

            var outerWhiteSquarePoints = ScalePoints(finderPatternPoints, topLeft, timingScaleFactor);
            outerWhiteSquarePoints = OrderPoints(outerWhiteSquarePoints);

            // Point A and D are across the diagonal from one another,
            // as are B and C. Figure out which are the solid white lines
            // by counting transitions
            var transitions = new List<ResultPointsAndTransitions>(4);
            transitions.Add(transitionsBetween(outerWhiteSquarePoints[0], outerWhiteSquarePoints[1]));
            transitions.Add(transitionsBetween(outerWhiteSquarePoints[0], outerWhiteSquarePoints[2]));
            transitions.Add(transitionsBetween(outerWhiteSquarePoints[3], outerWhiteSquarePoints[1]));
            transitions.Add(transitionsBetween(outerWhiteSquarePoints[3], outerWhiteSquarePoints[2]));
            transitions.Sort(new ResultPointsAndTransitionsComparator());

            // Sort by number of transitions. First two will be the two solid white sides; last two
            // will be the two alternating black/white sides where the timing patterns and data modules intersect the edge
            //of the square.
            var lSideOne = transitions[0];
            var lSideTwo = transitions[1];

            //Find the top Left Point by finding the point that appears twice and then determineing at what position 
            //in the outerWhiteSquarePoints that point exists at.
            ResultPoint[] findTopLeft = new ResultPoint[4];
            findTopLeft[0] = lSideOne.From;
            findTopLeft[1] = lSideOne.To;
            findTopLeft[2] = lSideTwo.From;
            findTopLeft[3] = lSideTwo.To;

            //Find the point that appears twice in the list as this will be the top left point of the outer white square and can 
            //be used to find the top right point of every other square.
            ResultPoint outerWhiteSquareTopLeft = findTopLeft.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;

            /*
            index point 0 = bottomLeft
            index point 1 = topLeft
            index point 2 = topRight
            */
            //TODO Uses the outerWhiteSquarePoint to find the three points that are closest to it and orders them
            //TODO I might need to revise this logic even though it appears to work correctly.
            var finderPatternOuterThreeCornerPoints = GetThreeCornerResultPoints(finderPatternPoints, outerWhiteSquareTopLeft);


            //Find the difference between finderPatternTopLeft and largestBarcodeTopLeft
            //Shift all of the largestBarcodeTopLeft by the difference of the (x,y)
            //This is the largest possible size of a Micro QR Code.
            //TODO Uses the outerWhiteSquarePoint to find the three points that are closest to it and orders them
            //TODO I might need to revise this logic even though it appears to work correctly.
            var largestBarcodeThreeCornerPoints = GetThreeCornerResultPoints(largestBarcodeSquarePoints, outerWhiteSquareTopLeft);
            var xShift = largestBarcodeThreeCornerPoints[1].X - finderPatternOuterThreeCornerPoints[1].X;
            var yShift = largestBarcodeThreeCornerPoints[1].Y - finderPatternOuterThreeCornerPoints[1].Y;
            largestBarcodeSquarePoints = ShiftPoints(largestBarcodeSquarePoints, new ResultPoint(xShift, yShift));
 //           var test = string.Empty;
            //As long as this is true we are in good shape.
            //Technically this should not matter.
            //var isRightTriangle = IsRightTriangle(finderPatternOuterThreeCornerPoints[1], finderPatternOuterThreeCornerPoints[2], finderPatternOuterThreeCornerPoints[0]);

            //Get the transitions for the inner square to the MAX possible barcode size.
            //I need to calulate the points from the innerTopLeft Index to the Max possible size
            //The Max possible size is the module size * 17 (there are at most 17 modules in micro qr code) - 1 (because the inner square is 1 module smaller)
            //The inner line length is already 6 modules the maximum size of microqr code is 17 
            //Since the last two modules of a micro qr code are the boarder I will times 
            //the module size by 17.
            //This will exceed the timing pattern by one module but will not exceed the barcode size
            //because the micro qr code specification dicates a two module white boarder.
            //TODO make sure not to exceed the width or hight of the image.
            //TODO if you will exceed the width or height of the image then extend to 
            //TODO the width or height. 
            var extendLength = moduleSize*18;
            //Extend the inner Result Points for TopRight and Bottom left to the max size.
            var innerThreeCornerPoints = GetThreeCornerResultPoints(innerSquarePoints, outerWhiteSquareTopLeft);
            var extendedTopRight = ExtendVertex(innerThreeCornerPoints[1], innerThreeCornerPoints[2], extendLength);
            var extendedBottomLeft = ExtendVertex(innerThreeCornerPoints[1], innerThreeCornerPoints[0], extendLength);

            //Sweep Logic does not work here as I do not want the farthest away point.
            //ResultPoint sweepExtendedTopRight = SweepAngle(innerThreeCornerPoints[1], extendedTopRight, moduleSize);
            //ResultPoint sweepExtemdedBottomLeft = SweepAngle(innerThreeCornerPoints[1], extendedBottomLeft, moduleSize);

            //Now it would be good to get the last black on the line and recalculate the extended points (top right and bottom left)

            var topRightTransition = transitionsBetween(innerThreeCornerPoints[1], extendedTopRight);
            var bottomLeftTransition = transitionsBetween(innerThreeCornerPoints[1], extendedBottomLeft);
            //This deteremines the end of the timing pattern
            var topRightEndTimingPattern = topRightTransition.EndTimingPattern;
            var bottomLeftEndTimingPattern = bottomLeftTransition.EndTimingPattern;
           
            //Recaculate the number of transitions using the identified EndTiming Pattern.
            //This also allows me to get the start of the last module.
            topRightTransition = transitionsBetween(innerThreeCornerPoints[1], topRightEndTimingPattern);
            var topRightStartLastModule = topRightTransition.StartLastModule;
            bottomLeftTransition = transitionsBetween(innerThreeCornerPoints[1], bottomLeftEndTimingPattern);
            var bottomLeftStartLastModule = bottomLeftTransition.StartLastModule;
            //The number of transitions should be the same.
            if (topRightTransition.Transitions != bottomLeftTransition.Transitions || bottomLeftTransition.Transitions == 0)
            {
                //Bad
                return null;
            }
            //Add 7 modules for the finder pattern
            var dimension = topRightTransition.Transitions + 7;
            //Check to make sure that the caculated dimension based on the number
            //of transitions is correct.
            Version provisionalVersion = Version.getProvisionalVersionForDimension(dimension);
            
            //Get the middle of the corner modules of the timing pattern
            //The transform adds the other 0.5 modules 
            var bottemLeftMiddle = CalculateMidpoint(bottomLeftStartLastModule, bottomLeftEndTimingPattern);
            var topRightMiddle = CalculateMidpoint(topRightStartLastModule, topRightEndTimingPattern);

            //The midpoint between the bottomLeftMiddle and topRightMiddle
            var middleMidpointTopRightBottomLeft = CalculateMidpoint(bottemLeftMiddle, topRightMiddle);

            /*
             * The sweep logic will be the only acurate way of determining the corner of the barcode.
             * These other methods do not allow for the barcode to scale up or down correctly.
             * The sweep will go from the middle point -+ 20 degrees.
             * A distance less then the middleRightBottomLeft from the midpoint will be discarded
             * The greatest value will be used as the corner.
             * 
             * The problem with any other method is that the barcode does not always scan or scale correctly at an angle.
             * This logic will prevent the transformation from cutting off the bottom right module or 
             * cutting off the timing pattern when the barcode is off center at a weird angle.
             */
            ResultPoint sweepTopRight = SweepAngle(middleMidpointTopRightBottomLeft, topRightMiddle, moduleSize);
            ResultPoint sweepBottomLeft = SweepAngle(middleMidpointTopRightBottomLeft, bottemLeftMiddle, moduleSize);

            //Get the points from the black outer square of the finder pattern.
            //The topLeft point of the barcode is at the topLeftIndex
            var barcodeTopLeft = finderPatternOuterThreeCornerPoints[1];
            var barcodeTopRight = sweepTopRight;
            var barcodeBottomLeft = sweepBottomLeft;

            //Create the transform to get only the bits that are part of the barcode.
            PerspectiveTransform transform = createTransform(barcodeTopLeft, barcodeTopRight, barcodeBottomLeft, dimension);
            //BitMatrix transformBits = sampleGrid(image, barcodeTopLeft, barcodeBottomLeft, barcodeBottomRight, barcodeTopRight, dimension,
            //    dimension);

            BitMatrix transformBits = sampleGrid(image, transform, dimension);
            if (transformBits == null)
                return null;
            //Used to debug the transformation and see how it is rendered.
            //var microQrTransfrom = GetBitmap(transformBits);
            //microQrTransfrom.Save(@"C:\Data\microqrtransformed.bmp");

           ResultPoint[] finalPoints = new ResultPoint[] { bottemLeftMiddle, innerThreeCornerPoints[1], topRightMiddle };
           return new DetectorResult(transformBits, finalPoints);
           
        }

        /// <summary>
        /// Checks to see if the estimateMaxDistance is valid and shrinks it if required.
        /// The Max Distance must not exceed the image boundaries and this method checks to make sure
        /// that the image boundaries are respected.
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="estimateMaxDistance"></param>
        /// <returns></returns>
        private float CalculateMaxDistance(ResultPoint centerPoint, float estimateMaxDistance)
        {

            double angleInRadians = 0;
            double oneDegreeInRadians = DegreeToRadian(1);
            float maxDistance = estimateMaxDistance;

            while (angleInRadians < 6.28317 && maxDistance >= 1)
            {
                var calculatedResultPoint = GetResultPointAtAngleAndDistance(centerPoint, angleInRadians, estimateMaxDistance);
                bool valid = isValid(calculatedResultPoint);
                if (!valid)
                {
                    angleInRadians = 0;
                    estimateMaxDistance = estimateMaxDistance - 1;
                }
                angleInRadians = angleInRadians + oneDegreeInRadians;
            }
            return maxDistance;
        }
        /*
        private void GetNonNullPoints(List<ResultPointsAndRotation> allOuterSquarePoints)
        {

            var maxUniquePoints = allOuterSquarePoints.GroupBy(u => u.Point).Select(grp => new
            {
                Point = grp.Key,
                Count = grp.Count()

            }).Max(a => a.Count);

            if (maxUniquePoints == 1)
                maxUniquePoints = 4;

            int takeAmount = (int)maxUniquePoints * 4;

            bool GotAllFour = false;
            while (!GotAllFour)
            {
                //Select the fartheset away points (the corner points)
                //Order the points by distance and take the points so that they can be averaged.
                IEnumerable<ResultPointsAndRotation> topFourOuterSquarePoints =
                    allOuterSquarePoints.OrderByDescending(x => x.Distance).Take(takeAmount).ToList();
                List<ResultPointsAndRotation> nextSet = topFourOuterSquarePoints.ToList();
                var lessThanTenDegrees = DegreeToRadian(10);

                //Selcts the points within 10 degrees of each other.
                if (nextSet.Count > 0)
                {
                    IEnumerable<ResultPointsAndRotation> cornerA = GetPointsWithinDegrees(centerPoint,
                        nextSet.FirstOrDefault().Point, topFourOuterSquarePoints, lessThanTenDegrees);
                }
                nextSet.RemoveAll(x => cornerA.Contains(x));

                if (nextSet.Count > 0)
                {
                    IEnumerable<ResultPointsAndRotation> cornerB = GetPointsWithinDegrees(centerPoint,
                        nextSet.FirstOrDefault().Point, topFourOuterSquarePoints, lessThanTenDegrees);
                }
                nextSet.RemoveAll(x => cornerB.Contains(x));

                if (nextSet.Count > 0)
                {
                    IEnumerable<ResultPointsAndRotation> cornerC = GetPointsWithinDegrees(centerPoint,
                        nextSet.FirstOrDefault().Point, topFourOuterSquarePoints, lessThanTenDegrees);
                }

                nextSet.RemoveAll(x => cornerC.Contains(x));

                if (nextSet.Count > 0)
                {
                    IEnumerable<ResultPointsAndRotation> cornerD = GetPointsWithinDegrees(centerPoint,
                        nextSet.FirstOrDefault().Point, topFourOuterSquarePoints, lessThanTenDegrees);
                    GotAllFour = true;
                }
                takeAmount ++;
            }
        }
        */

        private ResultPoint[] CalculateFinderPatternPoints(ResultPoint centerPoint)
        {

            //Step 1: Estimate the module size
            //The edges of the barcode finder pattern will never be larger then the hypotenuse of a right triange
            //at the size of the black white black run.
            var farRight = new ResultPoint(image.Width, centerPoint.Y);
            var farRightDistance = CalculateDistance(centerPoint, farRight);
            var estimateResultPoint = GetResultPointAtAngleAndDistance(centerPoint, 0, farRightDistance);
            var estimateDistance = sizeOfBlackWhiteBlackRun((int)centerPoint.X, (int)centerPoint.Y, (int)estimateResultPoint.X, (int)estimateResultPoint.Y);
            var estimateHypotenuse = Math.Sqrt(Math.Pow(estimateDistance, 2) + Math.Pow(estimateDistance, 2));
            var estimateModuleSize = estimateHypotenuse/7;
            float estimateMaxDistance = (float)estimateHypotenuse + (float)estimateModuleSize;
            float maxDistance = CalculateMaxDistance(centerPoint, estimateMaxDistance);

            //This algorthim works reasonably well but could probably be improved
            //It calculates all points allong the edge of the black white black run.
            //It then gets the top values
            //the values are grouped if they are within 10 degrees of one another
            //The result is averaged to find the corner points.
            double angleInRadians = 0;
            double oneDegreeInRadians = DegreeToRadian(1);

            List<ResultPointsAndRotation> allOuterSquarePoints = new List<ResultPointsAndRotation>();

            while (angleInRadians < 6.28317)
            {
                var calculatedResultPoint = GetResultPointAtAngleAndDistance(centerPoint, angleInRadians, maxDistance);

                var distance = sizeOfBlackWhiteBlackRun((int)centerPoint.X, (int)centerPoint.Y, (int)calculatedResultPoint.X, (int)calculatedResultPoint.Y);
                distance = GetBlackAtAngleAndDistance(centerPoint, angleInRadians, distance);
                allOuterSquarePoints.Add(new ResultPointsAndRotation(GetResultPointAtAngleAndDistance(centerPoint, angleInRadians, distance), angleInRadians, distance));

                angleInRadians = angleInRadians + oneDegreeInRadians;
            }

            var maxUniquePoints = allOuterSquarePoints.GroupBy(u => u.Point).Select(grp => new
            {
                Point = grp.Key,
                Count = grp.Count()

            }).Max(a => a.Count);

            if (maxUniquePoints == 1)
                maxUniquePoints = 4;

            int takeAmount = (int)maxUniquePoints * 4;

            bool GotAllFour = false;
            IEnumerable<ResultPointsAndRotation> cornerA = null;
            IEnumerable<ResultPointsAndRotation> cornerB = null;
            IEnumerable<ResultPointsAndRotation> cornerC = null;
            IEnumerable<ResultPointsAndRotation> cornerD = null;
            while (!GotAllFour)
            {
                //Select the fartheset away points (the corner points)
                //Order the points by distance and take the points so that they can be averaged.
                IEnumerable<ResultPointsAndRotation> topFourOuterSquarePoints =
                    allOuterSquarePoints.OrderByDescending(x => x.Distance).Take(takeAmount).ToList();
                List<ResultPointsAndRotation> nextSet = topFourOuterSquarePoints.ToList();
                var lessThanTenDegrees = DegreeToRadian(10);

                //Selcts the points within 10 degrees of each other.
                if (nextSet.Count > 0)
                {
                    cornerA = GetPointsWithinDegrees(centerPoint,
                        nextSet.FirstOrDefault().Point, topFourOuterSquarePoints, lessThanTenDegrees);
                    nextSet.RemoveAll(x => cornerA.Contains(x));
                }


                if (nextSet.Count > 0)
                {
                    cornerB = GetPointsWithinDegrees(centerPoint,
                        nextSet.FirstOrDefault().Point, topFourOuterSquarePoints, lessThanTenDegrees);
                    nextSet.RemoveAll(x => cornerB.Contains(x));
                }
                

                if (nextSet.Count > 0)
                {
                    cornerC = GetPointsWithinDegrees(centerPoint,
                        nextSet.FirstOrDefault().Point, topFourOuterSquarePoints, lessThanTenDegrees);
                    nextSet.RemoveAll(x => cornerC.Contains(x));
                }



                if (nextSet.Count > 0)
                {
                    cornerD = GetPointsWithinDegrees(centerPoint,
                        nextSet.FirstOrDefault().Point, topFourOuterSquarePoints, lessThanTenDegrees);
                    GotAllFour = true;
                }
                takeAmount++;
            }

            //Average the points together for each corner.
            var outerSquarePoints = new ResultPointsAndRotation[4];
            outerSquarePoints[0] = GetAverage(cornerA);
            outerSquarePoints[1] = GetAverage(cornerB);
            outerSquarePoints[2] = GetAverage(cornerC);
            outerSquarePoints[3] = GetAverage(cornerD);
            /*
            List<ResultPointsAndRotation> refinedSquarePoints = new List<ResultPointsAndRotation>();
            foreach (ResultPointsAndRotation outerSquarePoint in outerSquarePoints)
            {
                var refinePointAngleInRadians = ComputeAngle(centerPoint, outerSquarePoint.Point);
                var refineCalculatedResultPoint = GetResultPointAtAngleAndDistance(centerPoint, refinePointAngleInRadians, maxDistance);

                var refineDistance = sizeOfBlackWhiteBlackRun((int) centerPoint.X, (int) centerPoint.Y,(int) refineCalculatedResultPoint.X, (int) refineCalculatedResultPoint.Y);
                refinedSquarePoints.Add(
                    new ResultPointsAndRotation(
                        GetResultPointAtAngleAndDistance(centerPoint, refinePointAngleInRadians, refineDistance), refinePointAngleInRadians,
                        refineDistance));
            }

            //All of these points must be the same distance from the center.
            //Average the distance and caculate the points for that distance.
            var finalDistance = GetAverage(refinedSquarePoints).Distance;
            List<ResultPointsAndRotation> finalSquarePoints = new List<ResultPointsAndRotation>();
            foreach (ResultPointsAndRotation finalSquarePoint in refinedSquarePoints)
            {
                var finalPointAngleInRadians = ComputeAngle(centerPoint, finalSquarePoint.Point);
                var finalCalculatedResultPoint = GetResultPointAtAngleAndDistance(centerPoint, finalPointAngleInRadians, (float)finalDistance);

                finalSquarePoints.Add(
                    new ResultPointsAndRotation(finalCalculatedResultPoint, finalPointAngleInRadians,finalDistance));
            }

            var points = finalSquarePoints.Select(x => x.Point).ToArray();
            //var points = refinedSquarePoints.Select(x => x.Point).ToArray();
            */
            var points = outerSquarePoints.Select(x => x.Point).ToArray();
            //Sort the results so that corner A[0] is across from D[3]
            //And corner B[1] is across from C[2]
            points = OrderPoints(points);
            return points;

        }

        /// <summary>
        ///Calculates the midpoint of the ResultPoint 
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <returns></returns>
        private ResultPoint CalculateMidpoint(ResultPoint pointA, ResultPoint pointB)
        {
            var x = (pointA.X + pointB.X)/2;
            var y = (pointA.Y + pointB.Y)/2;
            return new ResultPoint((int) x, (int) y);
        }

        /// <summary>
        /// Checks that all points are true (black)
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="bottomLeft"></param>
        /// <returns></returns>
        private bool CheckPoints(ResultPoint topLeft, ResultPoint topRight, ResultPoint bottomLeft)
        {
            return (image[(int) topLeft.X, (int) topLeft.Y] && 
                    image[(int) topRight.X, (int) topRight.Y] &&
                    image[(int) bottomLeft.X, (int) bottomLeft.Y]);

        }

        /// <summary>
        /// Calculates a point from point a at a distance at the angleInRadians to become point c.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="distance"></param>
        /// <param name="angleInRadians"></param>
        /// <returns></returns>
        private ResultPoint CalculatePoint(ResultPoint a, double distance, double angleInRadians)
        {

            var newx = a.X + distance * Math.Cos(angleInRadians);
            var newy = a.Y + distance * Math.Sin(angleInRadians);
            return new ResultPoint((int)newx, (int)newy);
        }

        /// <summary>
        /// Calculates a point that extends from point a intesects with point b at a distance
        /// to become point c.  
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private ResultPoint CalculatePoint(ResultPoint pointA, ResultPoint pointB, double distance)
        {
            Double magnitude = Math.Sqrt(Math.Pow((pointB.Y - pointA.Y), 2) + Math.Pow((pointB.X - pointA.X), 2));
            var newX = (pointA.X + (distance * ((pointB.X - pointA.X) / magnitude)));
            var newY = (pointA.Y + (distance * ((pointB.Y - pointA.Y) / magnitude)));
            return new ResultPoint((int)newX, (int)newY);
        }

        /// <summary>
        /// Calculates result point so that it extends from the startPoint to the otherPoint to a distance
        /// the distance is shrunk until the result point is on a black pixel.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="otherPoint"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private ResultPoint ShrinkVertexToBlackModule(ResultPoint startPoint, ResultPoint otherPoint, double distance)
        {
            bool isBlack = false;
            ResultPoint result = new ResultPoint();
            while (!isBlack)
            {
                result = ExtendVertex(startPoint, otherPoint, distance);
                isBlack = image[(int) result.X, (int) result.Y];
                if (!isBlack)
                    distance --;
            }
            return new ResultPoint((int)result.X, (int)result.Y);
        }
        /// <summary>
        /// Caculates the result point so that it extends from the startPoint to the otherPoint at a distance.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="otherPoint"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private ResultPoint ExtendVertex(ResultPoint startPoint, ResultPoint otherPoint, double distance)
        {

            //calculate delta x and delta y between the two points
            var deltaY = otherPoint.Y - startPoint.Y;
            var deltaX = otherPoint.X - startPoint.X;

            var angleInRadians = Math.Atan2(deltaY, deltaX);

            var newx = startPoint.X + distance * Math.Cos(angleInRadians);
            var newy = startPoint.Y + distance * Math.Sin(angleInRadians);

            return new ResultPoint((int)newx, (int)newy);

        }

        public ResultPoint[] GetTopRightBottomLeft(ResultPoint topleft, ResultPoint[] target)
        {
            ResultPoint[] result = new ResultPoint[2];
            var index = 0;
            foreach (var resultPoint in target)
            {
                if (!resultPoint.Equals(topleft))
                {
                    result[index] = resultPoint;
                    index ++;
                }
            }
            return result;
        }

        public float GetBlackAtAngleAndDistance(ResultPoint center, double radians, float distance)
        {
            bool isBlack = false;
            while (!isBlack && distance > 1)
            {
                var newBottomRightX = center.X + Math.Cos(radians)*distance;
                var newBottomRightY = center.Y + Math.Sin(radians)*distance;
                if (isValid(new ResultPoint((int) newBottomRightX, (int) newBottomRightY)))
                {
                    isBlack = image[(int) newBottomRightX, (int) newBottomRightY];

                    if (!isBlack)
                    {
                        distance = distance - 1.0f;
                    }
                }
                else
                {
                    distance = distance - 1.0f;
                }
            }
            return distance;
        }

        private ResultPoint GetResultPointAtAngleAndDistance(ResultPoint center, double radians, float distance)
        {
            var newX = center.X + Math.Cos(radians) * distance;
            var newY = center.Y + Math.Sin(radians) * distance;
            return new ResultPoint((int)newX, (int)newY);
        }

        private bool isValid(ResultPoint p)
        {
            return p.X >= 0 && p.X < image.Width && p.Y > 0 && p.Y < image.Height;
        }

        // L2 distance
        private static int distance(ResultPoint a, ResultPoint b)
        {
            return MathUtils.round(ResultPoint.distance(a, b));
        }


        private static BitMatrix sampleGrid(BitMatrix image,
                                                 ResultPoint topLeft,
                                                 ResultPoint bottomLeft,
                                                 ResultPoint bottomRight,
                                                 ResultPoint topRight,
                                                 int dimensionX,
                                                 int dimensionY)
        {

            GridSampler sampler = GridSampler.Instance;

            return sampler.sampleGrid(image,
                                      dimensionX,
                                      dimensionY,
                                      0.5f,
                                      0.5f,
                                      dimensionX - 0.5f,
                                      0.5f,
                                      dimensionX - 0.5f,
                                      dimensionY - 0.5f,
                                      0.5f,
                                      dimensionY - 0.5f,
                                      topLeft.X,
                                      topLeft.Y,
                                      topRight.X,
                                      topRight.Y,
                                      bottomRight.X,
                                      bottomRight.Y,
                                      bottomLeft.X,
                                      bottomLeft.Y);
        }

        //TODO Add a limit to the transitions between so that it does not go past two white modules 
        //If the max microqrcode is 17 modules and the min microqr code is 11 modules
        //There is a good change that I could encouter another black pizel past an 
        //11 module microqr code  to prevent this if the code detects two white modules
        //then you have reached the boarder of the micro qr code and can stop searching
        //for more transtions.
        /// <summary>
        /// Counts the number of black/white transitions between two points, using something like Bresenham's algorithm.
        /// </summary>
        private ResultPointsAndTransitions transitionsBetween(ResultPoint from, ResultPoint to)
        {
            // See QR Code Detector, sizeOfBlackWhiteBlackRun()
            int fromX = (int)from.X;
            int fromY = (int)from.Y;
            int toX = (int)to.X;
            int toY = (int)to.Y;
            ResultPoint startTimingPattern = null;
            ResultPoint endTimingPattern = null;
            ResultPoint startLastModule = null;
            ResultPoint previousTimingPattern = null;
            bool steep = Math.Abs(toY - fromY) > Math.Abs(toX - fromX);
            if (steep)
            {
                int temp = fromX;
                fromX = fromY;
                fromY = temp;
                temp = toX;
                toX = toY;
                toY = temp;
            }

            int dx = Math.Abs(toX - fromX);
            int dy = Math.Abs(toY - fromY);
            int error = -dx >> 1;
            int ystep = fromY < toY ? 1 : -1;
            int xstep = fromX < toX ? 1 : -1;
            int transitions = 0;
            bool inBlack = image[steep ? fromY : fromX, steep ? fromX : fromY];
            for (int x = fromX, y = fromY; x != toX; x += xstep)
            {
                bool isBlack = image[steep ? y : x, steep ? x : y];
                if (!inBlack && isBlack)
                {
                    startLastModule = new ResultPoint(steep ? y : x, steep ? x : y);
                }

                if (isBlack != inBlack)
                {
                    transitions++;
                    inBlack = isBlack;
                    if (startTimingPattern == null)
                    {
                        startTimingPattern = new ResultPoint(steep ? y : x, steep ? x : y);
                    }

                    endTimingPattern = previousTimingPattern;
                }

                previousTimingPattern = new ResultPoint(steep ? y : x, steep ? x : y);


                error += dy;
                if (error > 0)
                {
                    if (y == toY)
                    {
                        break;
                    }
                    y += ystep;
                    error -= dx;
                }
            }
            return new ResultPointsAndTransitions(from, to, transitions, startTimingPattern, endTimingPattern, startLastModule);
        }

        private Boolean IsRightTriangle(ResultPoint topLeft, ResultPoint topRight, ResultPoint bottomLeft)
        {
            var a = (double)CalculateDistance(topLeft, topRight);
            var b = (double)CalculateDistance(topLeft, bottomLeft);
            var d = a * a + b * b;
            var c = Math.Sqrt(d);
            var e = c * c;
            return Math.Ceiling(e) == Math.Ceiling(d);
        }
        /*
        private Bitmap GetBitmap(BitMatrix image)
        {
            var bitmap = new Bitmap(image.Width, image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                var row = image.getRow(y, null);

                for (int x = 0; x < row.Size; x++)
                {
                    var whatisx = row[x];
                    if (!whatisx)
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                    }
                }
            }
            return bitmap;
        }
        */
        /*
        private BitMatrix GetBitMatrix(Bitmap bitmap)
        {
            BitMatrix bitMatrix = new BitMatrix(bitmap.Width, bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                BitArray row = new BitArray(bitmap.Width);
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var color = bitmap.GetPixel(x, y).ToArgb();
                    if (color == Color.White.ToArgb())
                    {
                        row[x] = false;

                    }
                    else
                    {
                        row[x] = true;
                    }
                }
                bitMatrix.setRow(y, row);
            }
            return bitMatrix;
        }
        */
        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        /// <summary>
        /// Simply encapsulates two points and a number of transitions between them.
        /// </summary>
        private sealed class ResultPointsAndTransitions
        {
            public ResultPoint From { get; private set; }
            public ResultPoint To { get; private set; }
            public int Transitions { get; private set; }
            public ResultPoint StartTimingPattern { get; private set; }
            public ResultPoint EndTimingPattern { get; private set; }
            public ResultPoint StartLastModule { get; private set; }

            public ResultPointsAndTransitions(ResultPoint from, ResultPoint to, int transitions,
                ResultPoint startTimingPattern, ResultPoint endTimingPattern, ResultPoint startLastModule)
            {
                From = from;
                To = to;
                Transitions = transitions;
                StartTimingPattern = startTimingPattern;
                EndTimingPattern = endTimingPattern;
                StartLastModule = startLastModule;
            }

            override public String ToString()
            {
                return From + "/" + To + '/' + Transitions + '/' + StartTimingPattern + '/' + StartLastModule + '/' + EndTimingPattern;
            }
        }

        /// <summary>
        /// Point = the point
        /// Angle = the angle from the center
        /// Distance = the distance from the center
        /// </summary>
        private sealed class ResultPointsAndRotation
        {
            public ResultPoint Point { get; private set; }
            public double Angle { get; private set; }
            public double Distance { get; private set; }

            public ResultPointsAndRotation(ResultPoint point, double angle, double distance)
            {
                Point = point;
                Angle = angle;
                Distance = distance;
            }

            override public String ToString()
            {
                return Point + "/" + Angle + '/' + Distance;
            }
        }

        /// <summary>
        /// Orders ResultPointsAndTransitions by number of transitions, ascending.
        /// </summary>
        private sealed class ResultPointsAndTransitionsComparator : IComparer<ResultPointsAndTransitions>
        {
            public int Compare(ResultPointsAndTransitions o1, ResultPointsAndTransitions o2)
            {
                return o1.Transitions - o2.Transitions;
            }
        }
    }
}