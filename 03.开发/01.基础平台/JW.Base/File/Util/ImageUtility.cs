using JW.Base.Http;
using JW.Base.Http.Enum;
using JW.Base.Http.Static;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;

namespace JW.Base.File.Util {
    /// <summary>
    /// 辅助类：图片
    /// </summary>
    public class ImageUtility {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Image FromUrl(string url) {
            try {
                HttpHelper http = new HttpHelper();
                var result = http.GetHtml(new HttpItem {
                    URL = url,
                    UserAgent = UserAgent.Chrome,
                    ResultType = ResultType.Byte
                });
                Image image = null;
                if (result.StatusCode == HttpStatusCode.OK) {
                    MemoryStream stream = new MemoryStream(result.ResultByte);
                    image = Image.FromStream(stream);
                }
                return image;
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 生成文字图片（ClearType）
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="font">字体</param>
        /// <param name="rect">返回图片大小</param>
        /// <param name="fontColor">字体颜色</param>
        /// <param name="backColor">背景色</param>
        /// <returns></returns>
        public static Bitmap CreateFontImage(string text, Font font, Rectangle rect, Color fontColor, Color backColor) {
            Graphics g;
            Bitmap bmp;
            SolidBrush brush = new SolidBrush(fontColor);
            StringFormat format = new StringFormat(StringFormatFlags.LineLimit);
            if (rect == Rectangle.Empty) {
                bmp = new Bitmap(1, 1);
                g = Graphics.FromImage(bmp);
                //计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
                SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);

                int width = (int)(sizef.Width + 1);
                int height = (int)(sizef.Height + 1);
                rect = new Rectangle(0, 0, width, height);
                bmp.Dispose();

                bmp = new Bitmap(width, height);
            } else {
                bmp = new Bitmap(rect.Width, rect.Height);
            }

            g = Graphics.FromImage(bmp);

            //使用ClearType字体功能
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillRectangle(new SolidBrush(backColor), rect);
            g.DrawString(text, font, brush, rect, format);
            return bmp;
        }

        ///// <summary>  
        ///// 合并图片  
        ///// </summary>  
        ///// <param name="imgBack"></param>  
        ///// <param name="img"></param>  
        ///// <returns></returns>  
        //public static Bitmap CombinImage(Image imgBack, Image img, int xDeviation = 0, int yDeviation = 0) {

        //    Bitmap bmp = new Bitmap(imgBack.Width, imgBack.Height);

        //    Graphics g = Graphics.FromImage(bmp);
        //    g.Clear(Color.White);
        //    g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

        //    //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

        //    //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    

        //    g.DrawImage(img, xDeviation, yDeviation, img.Width, img.Height);
        //    GC.Collect();
        //    return bmp;
        //}

        /// <summary> 
        /// 合并图片 
        /// </summary> 
        /// <param name="imgBack"></param> 
        /// <param name="img"></param> 
        /// <param name="xDeviation"></param>
        /// <param name="yDeviation"></param>
        /// <returns></returns> 
        public static Bitmap CombinImage(Image imgBack, Image img, int xDeviation = 0, int yDeviation = 0) {
            Bitmap bmp = new Bitmap(imgBack.Width, imgBack.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
            //白色边框
            // g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 + xDeviation - 1, imgBack.Height / 2 - img.Height / 2 + yDeviation - 1, img.Width + 2, img.Height + 2);
            //填充图片

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.Clear(Color.Transparent);

            //防止出现渐变
            var imgAtt = new ImageAttributes();
            imgAtt.SetWrapMode(WrapMode.TileFlipXY);

            var x = imgBack.Width / 2 - img.Width / 2 + xDeviation;
            var y = imgBack.Height / 2 - img.Height / 2 + yDeviation;
            var width = img.Width;
            var height = img.Height;
            // g.DrawImage(img, imgBack.Width / 2 - img.Width / 2 + xDeviation, imgBack.Height / 2 - img.Height / 2 + yDeviation,img.Width , img.Height);
            g.DrawImage(img, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel, imgAtt);

            GC.Collect();
            return bmp;
        }

        /// <summary>
        /// 图片转成圆角方法二
        /// </summary>
        public static Bitmap WayTwo(Bitmap bitmap) {
            using (Image i = bitmap) {
                Bitmap b = new Bitmap(i.Width, i.Height);
                using (Graphics g = Graphics.FromImage(b)) {
                    g.DrawImage(i, 0, 0, b.Width, b.Height);
                    int r = Math.Min(b.Width, b.Height) / 2;
                    PointF c = new PointF(b.Width / 2.0F, b.Height / 2.0F);
                    for (int h = 0; h < b.Height; h++)
                        for (int w = 0; w < b.Width; w++)
                            if ((int)Math.Pow(r, 2) < ((int)Math.Pow(w * 1.0 - c.X, 2) + (int)Math.Pow(h * 1.0 - c.Y, 2))) {
                                b.SetPixel(w, h, Color.Transparent);
                            }
                }
                return b;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image Circular(Image img, Size size) {
            Bitmap b = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(b)) {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(img, 0, 0, b.Width, b.Height);
                int r = Math.Min(b.Width, b.Height) / 2;
                PointF c = new PointF(b.Width / 2.0F, b.Height / 2.0F);
                for (int h = 0; h < b.Height; h++)
                    for (int w = 0; w < b.Width; w++)
                        if ((int)Math.Pow(r, 2) < ((int)Math.Pow(w * 1.0 - c.X, 2) + (int)Math.Pow(h * 1.0 - c.Y, 2))) {
                            b.SetPixel(w, h, Color.Transparent);
                        }
                // 画背景色圆
                using (Pen p = new Pen(SystemColors.Control))
                    g.DrawEllipse(p, 0, 0, b.Width, b.Height);
            }
            return b;
        }

        /// <summary>
        /// 从大图中截取一部分图片
        /// </summary>
        /// <param name="fromImage">来源图片</param>       
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns></returns>
        public Image ResizeImage(Image fromImage, int offsetX, int offsetY, int width, int height) {
            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            //从作图区生成新图
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());

            return saveImage;
        }

        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        public static Image ResizeImage(Image bmp, int newW, int newH, int mode = 0) {
            try {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height),
                            GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            } catch {
                return null;
            }
        }

        /// <summary>
        /// 按照指定大小缩放图片，但是为了保证图片宽高比自动截取
        /// </summary>
        /// <param name="srcPath">原图片路径</param>
        /// <param name="destWidth">目标图片宽</param>
        /// <param name="destHeight">目标图片高</param>
        /// <returns></returns>
        public static Bitmap SizeImageWithOldPercent(Bitmap srcPath, int destWidth, int destHeight) {
            Image srcImage = srcPath;
            try {
                // 要截取图片的宽度（原始宽度）
                int srcWidth = srcImage.Width;
                // 要截取图片的高度（原始高度）
                int srcHeight = srcImage.Height;
                // 截取开始横坐标
                int newX = 0;
                // 截取开始纵坐标
                int newY = 0;

                // 截取比例
                double whPercent = ((double)destWidth / (double)destHeight) * ((double)srcImage.Height / (double)srcImage.Width);
                if (whPercent > 1) {
                    // 当前图片宽度对于要截取比例过大时
                    srcWidth = int.Parse(Math.Round(srcImage.Width / whPercent).ToString());
                } else if (whPercent < 1) {
                    // 当前图片高度对于要截取比例过大时
                    srcHeight = int.Parse(Math.Round(srcImage.Height * whPercent).ToString());
                }

                if (srcWidth != srcImage.Width) {
                    // 宽度有变化时，调整开始截取的横坐标
                    newX = Math.Abs(int.Parse(Math.Round(((double)srcImage.Width - srcWidth) / 2).ToString()));
                } else if (srcHeight == srcImage.Height) {
                    // 高度有变化时，调整开始截取的纵坐标
                    newY = Math.Abs(int.Parse(Math.Round(((double)srcImage.Height - (double)srcHeight) / 2).ToString()));
                }

                // 将原始图片画到目标画布上，返回目标图片
                Bitmap cutedImage = CutImage(srcImage, newX, newY, srcWidth, srcHeight);

                // 在创建一个新的画布
                Bitmap bmp = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage(bmp);

                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clear(Color.Transparent);

                //防止出现渐变
                var imgAtt = new ImageAttributes();
                imgAtt.SetWrapMode(WrapMode.TileFlipXY);

                // g.DrawImage(cutedImage, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, cutedImage.Width, cutedImage.Height), GraphicsUnit.Pixel);
                // 在画板的指定位置画图
                g.DrawImage(cutedImage, new Rectangle(0, 0, destWidth, destHeight), 0, 0, cutedImage.Width, cutedImage.Height, GraphicsUnit.Pixel, imgAtt);
                // g.Dispose();

                return bmp;
            } catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// 剪裁 -- 用GDI+
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="StartX">开始坐标X</param>
        /// <param name="StartY">开始坐标Y</param>
        /// <param name="destWidth">目标图片宽度</param>
        /// <param name="destHeight">目标图片高度高度</param>
        /// <returns>剪裁后的Bitmap</returns>
        public static Bitmap CutImage(Image image, int StartX, int StartY, int destWidth, int destHeight) {
            int srcWidth = image.Width;
            int srcHeight = image.Height;
            if (StartX >= srcWidth || StartY >= srcHeight) {
                // 开始截取坐标过大时，结束处理
                return null;
            }

            if (StartX + destWidth > srcWidth) {
                // 宽度过大时只截取到最大大小
                destWidth = srcWidth - StartX;
            }

            if (StartY + destHeight > srcHeight) {
                // 高度过大时只截取到最大大小
                destHeight = srcHeight - StartY;
            }

            try {
                // 根据目标图片的大小，实例化一个画布
                Bitmap bmpOut = new Bitmap(destWidth, destHeight);
                // 实例化一个画笔
                Graphics g = Graphics.FromImage(bmpOut);

                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clear(Color.Transparent);

                // 将原始图片画到目标画布上
                // g.DrawImage(image,new Rectangle(0, 0, destWidth, destHeight), new Rectangle(StartX, StartY, destWidth, destHeight), GraphicsUnit.Pixel);

                //防止出现渐变(生成高质量不模糊的图片)
                var imgAtt = new ImageAttributes();
                imgAtt.SetWrapMode(WrapMode.TileFlipXY);
                // 在画板的指定位置画图
                g.DrawImage(image, new Rectangle(0, 0, destWidth, destHeight), StartX, StartY, destWidth, destHeight, GraphicsUnit.Pixel, imgAtt);

                g.Dispose();

                return bmpOut;
            } catch {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="destHeight"></param>
        /// <param name="destWidth"></param>
        /// <returns></returns>
        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth) {
            Image imgSource = b;
            ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放          
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth) {
                if ((sWidth * destHeight) > (sHeight * destWidth)) {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                } else {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            } else {
                return b;
                //sW = sWidth;
                //sH = sHeight;
            }

            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.Clear(Color.Transparent);

            //防止出现渐变
            var imgAtt = new ImageAttributes();
            imgAtt.SetWrapMode(WrapMode.TileFlipXY);
            //在画板的指定位置画图
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0,
                imgSource.Width, imgSource.Height, GraphicsUnit.Pixel, imgAtt);

            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量    
            //EncoderParameters encoderParams = new EncoderParameters();
            //long[] quality = new long[1];
            //quality[0] = 100;
            //EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //encoderParams.Param[0] = encoderParam;

            imgSource.Dispose();
            return outBmp;
        }
    }
}
