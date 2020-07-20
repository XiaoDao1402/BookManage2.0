using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Base.Lang {
    /// <summary>
    /// 显示文本
    /// </summary>
    public class DisplayTextAttribute : Attribute {
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public DisplayTextAttribute(string text) {
            Text = text;
        }
    }
}
