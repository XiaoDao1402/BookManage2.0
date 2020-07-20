using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JW.Base.Base {
    /// <summary>
    /// 数据转换辅助类
    /// </summary>
    public class ConvertHelper {

        #region 基本数据转换

        /// <summary>
        /// 默认日期
        /// </summary>
        public static DateTime DEFAULT_DATE = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

        /// <summary>
        /// 转换成整形
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果为空或者非数字字符串的话，则返回0</returns>
        public static int ToInt(object obj) {
            int result = 0;

            if (obj == null) {
                return result;
            }

            if (int.TryParse(obj.ToString(), out result)) {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果转换的数据为空，则返回sting.Empty</returns>
        public static string ToActionString(object obj) {
            return null == obj ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// 转换成双浮点
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果转换的数据为空，则返回0.0000</returns>
        public static double ToDouble(object obj) {
            double result = 0.0000;

            if (null == obj) {
                return result;
            }

            if (double.TryParse(obj.ToString(), out result)) {
                return result;
            }


            return result;
        }

        /// <summary>
        /// 将对象转换为数值(DateTime)类型,转换失败返回1900-01-01
        /// </summary>
        /// <param name="obj">转换的对象</param>
        /// <param name="format">日期格式</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj, string format) {
            try {
                DateTime dt = DateTime.ParseExact(ToActionString(obj), format, System.Globalization.CultureInfo.CurrentCulture);
                if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                    return dt;
                return DEFAULT_DATE;
            } catch {
                return DEFAULT_DATE;
            }
        }

        /// <summary>
        /// 转成日期格式
        /// </summary>
        /// <param name="obj">日期字符串</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj) {
            string str = ToActionString(obj);
            if (null == obj || string.IsNullOrEmpty(str)) {
                return DEFAULT_DATE;
            } else {
                if (str.Contains("-")) {
                    DateTime dt = DateTime.Parse(str);
                    if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                        return dt;
                } else {
                    DateTime dt = DateTime.ParseExact(str, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                    if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                        return dt;
                }
            }

            return DEFAULT_DATE;
        }

        /// <summary>
        /// 转成单浮点数
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果转换的数据为空，则返回0.0f</returns>
        public static float ToFloat(object obj) {
            float result = 0.0f;

            if (null == obj) {
                return result;
            }

            if (float.TryParse(obj.ToString(), out result)) {
                return result;
            }


            return result;
        }

        /// <summary>
        /// 转成小数
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果转换的数据为空，则返回0</returns>
        public static decimal ToDecimal(object obj) {
            decimal result = 0M;

            if (null == obj) {
                return result;
            }

            if (decimal.TryParse(obj.ToString(), out result)) {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 转成整型long
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果；如果转换的数据为空，则返回0</returns>
        public static long ToLong(object obj) {
            long result = 0;
            if (null == obj) {
                return result;
            }

            if (long.TryParse(obj.ToString(), out result)) {
                return result;
            }

            return result;
        }
        #endregion

        #region 补足位数

        /// <summary>
        /// 往左边补足位数
        /// </summary>
        /// <param name="obj">补足的变量</param>
        /// <param name="totalLenght">总共长度</param>
        /// <param name="padSymbol">补充的符号</param>
        /// <returns></returns>
        public static string PadLeftSide(object obj, int totalLenght, char padSymbol) {
            string result = string.Empty;

            if (null == obj) {
                result = result.PadLeft(totalLenght, padSymbol);
            } else {
                result = obj.ToString();
                result = result.PadLeft(totalLenght, padSymbol);
            }

            return result;
        }

        /// <summary>
        /// 往右边补足位数
        /// </summary>
        /// <param name="obj">补足的变量</param>
        /// <param name="totalLenght">总共长度</param>
        /// <param name="padSymbol">补充的符号</param>
        /// <returns></returns>
        public static string PadRightSide(object obj, int totalLenght, char padSymbol) {
            string result = string.Empty;

            if (null == obj) {
                result = result.PadRight(totalLenght, padSymbol);
            } else {
                result = obj.ToString();
                result = result.PadRight(totalLenght, padSymbol);
            }

            return result;
        }

        #endregion

        #region 数据判断

        /// <summary>
		/// 判断对象是否为正确的日期值
		/// </summary>
		/// <param name="obj">目标数据</param>
		/// <returns>Boolean。</returns>
		public static bool IsDateTime(object obj) {
            DateTime DEFAULT_DATE = Convert.ToDateTime("1900-01-01");

            try {
                string str = ToActionString(obj);
                if (string.IsNullOrEmpty(str)) {
                    return false;
                }

                if (str.Contains("-")) {
                    DateTime dt = DateTime.Parse(str);
                    if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                        return true;
                } else {
                    DateTime dt = DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                        return true;
                }

                return false;
            } catch {
                return false;
            }
        }

        #endregion

        #region 图片

        /// <summary>
        /// 获取Image图片格式
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetImageFormat(Stream file) {
            string format = string.Empty;

            byte[] sb = new byte[2];  //这次读取的就是直接0-1的位置长度了.
            file.Read(sb, 0, sb.Length);
            //根据文件头判断
            string strFlag = sb[0].ToString() + sb[1].ToString();
            //察看格式类型
            switch (strFlag) {
                //JPG格式
                case "255216":
                    format = ".jpg"; break;
                //GIF格式
                case "7173":
                    format = ".gif"; break;
                //BMP格式
                case "6677":
                    format = ".bmp"; break;
                //PNG格式
                case "13780":
                    format = ".png"; break;
                //其他格式
                default: break;
            }

            return format;
        }

        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image) {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream()) {
                if (format.Equals(ImageFormat.Jpeg)) {
                    image.Save(ms, ImageFormat.Jpeg);
                } else if (format.Equals(ImageFormat.Png)) {
                    image.Save(ms, ImageFormat.Png);
                } else if (format.Equals(ImageFormat.Bmp)) {
                    image.Save(ms, ImageFormat.Bmp);
                } else if (format.Equals(ImageFormat.Gif)) {
                    image.Save(ms, ImageFormat.Gif);
                } else if (format.Equals(ImageFormat.Icon)) {
                    image.Save(ms, ImageFormat.Icon);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary> 
        /// 生成缩略图重载方法1，返回缩略图的Image对象 
        /// </summary>
        /// <param name="image"></param> 
        /// <param name="Width">缩略图的宽度</param> 
        /// <param name="Height">缩略图的高度</param> 
        /// <returns>缩略图的Image对象</returns> 
        public static Image GetReducedImage(Image image, int Width, int Height) {
            try {
                //用指定的大小和格式初始化Bitmap类的新实例 
                Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                //从指定的Image对象创建新Graphics对象 
                Graphics graphics = Graphics.FromImage(bitmap);
                //清除整个绘图面并以透明背景色填充 
                graphics.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片对象 
                graphics.DrawImage(image, new Rectangle(0, 0, Width, Height));
                return bitmap;
            } catch {
                return null;
            }
        }

        #endregion

        #region 经纬度

        /// <summary>
        /// 计算坐标距离
        /// </summary>
        /// <param name="lng1"></param>
        /// <param name="lat1"></param>
        /// <param name="lng2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double DistanceOfTwoPoints(double lng1, double lat1, double lng2, double lat2) {
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double a = radLat1 - radLat2;
            double b = Rad(lng1) - Rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s *= 6378245.0;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        #endregion

        #region Private
        private static double Rad(double d) {
            return d * Math.PI / 180.0;
        }

        #endregion

    }
}
