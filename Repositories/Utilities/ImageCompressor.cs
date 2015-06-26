using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Utilities
{
    public class ImageCompressor
    {
        public string CompressImage(string image)
        {

            string[] imageString = image.Split(new char[] { ',' });

            Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(imageString[1]));
            System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
            
            var bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));
            var targetX = 500;
            var targetY = (int) (bitImage.Height * targetX / bitImage.Width);
            
            var thumbnail = new Bitmap(targetX, targetY);
            using (Graphics graphicsHandle = Graphics.FromImage(thumbnail))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(bitImage, 0, 0, targetX, targetY);
            }
            var thumbnailMS = new MemoryStream();
            thumbnail.Save(thumbnailMS, ImageFormat.Png);

            return Convert.ToBase64String(ImageToByteArray(thumbnail));
        }

        private string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }

        public  byte[] ImageToByteArray(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }
    }
}
