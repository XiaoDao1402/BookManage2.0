using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Base.File.Item {
    /// <summary>
    /// 
    /// </summary>
    public class BaseItem {
        /// <summary>
        /// X坐标
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 层级，越大越高
        /// </summary>
        public int ZIndex { get; set; }
    }
}
