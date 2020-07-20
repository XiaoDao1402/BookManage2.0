using JW.Base.File.Item;
using JW.Base.File.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace JW.Base.File {
    /// <summary>
    /// 画布
    /// </summary>
    public class Canvas {
        /// <summary>
        /// 
        /// </summary>
        public List<ImageItem> Images { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TextItem> Texts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// 合并
        /// </summary>
        /// <returns></returns>
        public Bitmap Combin() {
            // 初始化画布(最终的拼图画布)并设置宽高
            Bitmap bitmap = new Bitmap(Size.Width, Size.Height);
            // 初始化画板
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            // 将画布涂为白色(底部颜色可自行设置)
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, Size.Width, Size.Height));
            Images.Sort((item1, item2) => item1.ZIndex.CompareTo(item2.ZIndex));
            Texts.Sort((item1, item2) => item1.ZIndex.CompareTo(item2.ZIndex));

            List<ImageItem> images = Images;
            List<TextItem> texts = Texts;

            foreach (ImageItem item in images) {
                if (item.FillRadius) {
                    item.Image = ImageUtility.Circular(item.Image, new Size { Width = item.Width, Height = item.Height });
                } else {
                    item.Image = ImageUtility.ResizeImage(item.Image, item.Width, item.Height);
                }
                using (Bitmap imageMap = new Bitmap(item.Image)) {
                    g.DrawImage(imageMap, item.X, item.Y, item.Width, item.Height);
                }
            }

            foreach (TextItem item in texts) {
                using (Bitmap bitmapText = new Bitmap(ImageUtility.CreateFontImage(item.Text, item.Font, new Rectangle(Point.Empty, new Size(item.Width, item.Height)), item.Color, Color.Transparent))) {
                    g.DrawImage(bitmapText, item.X, item.Y, bitmapText.Width, bitmapText.Height);
                }
            }

            return bitmap;
        }
    }
}
