# ZXing-CSharp-MicroQR-Detector
Micro QR Code detection for ZXing .NET

Referenced Projects:
ZXing "Zebra Corssing" for Java
https://github.com/zxing/zxing

ZXing "Zebra Corssing" for .Net
http://zxingnet.codeplex.com/

Micro QR Code Encoder/Decoder for ZXing by TUPUNCO
https://github.com/tupunco/ZXing-CSharp

The projects above provided the bases for the Micro QR Code Detector feature enhancement.

Micro QR Code Detection:

The Micro QR Code Encoder/Decoder for ZXing by TUPUNCO was branched from Zebra Crossing .NET in 2007.  This branch was no longer compatible with the latest set of code from ZXing .NET.  These two projects have been merged together so that the latest branch of ZXing Crossing .NET is used as the bases and the Micro QR Code Encoder/Decoder from TUPUNCO is merged into it.

I consider the merge performed imperfect at present.  I have not yet had a chance to switch over everything to use the newest methods of ZXing .NET however this task will be completed as I continue with this project.  The merge as imperfect as it is works correctly so that all the features of Micro QR code Encoder/Decoder work as they worked in the previous version and all of the previous features of ZXing .NET continue to work.

The feature I developed on top of TUPUNCO Micro QR Code Encoder/Decoder is a Micro QR Code detector.  This was a fun challenges to design a method to get the correct bits from the barcode based on the single finder pattern that is returned.  A lot of the existing code for both QR Code and Data Matrix were refactored to provide the methods needed to detect a Micro QR code.  There are still a lot of optimizations that can be added to this code however I wanted to push out this feature into "the community" so that it could be extended/refactored if desired.

QR Code is trademarked by Denso Wave, inc.

http://www.qrcode.com/en/codes/microqr.html

