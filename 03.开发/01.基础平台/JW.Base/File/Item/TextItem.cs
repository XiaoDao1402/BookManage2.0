using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace JW.Base.File.Item {
    /// <summary>
    /// 
    /// </summary>
    public class TextItem : BaseItem {
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Font Font { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsBold { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TextAlign TextAlign { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TextAlign {
        /// <summary>
        /// 
        /// </summary>
        public static string Right = "right";
        /// <summary>
        /// 
        /// </summary>
        public static string Left = "Left";
    }
}
