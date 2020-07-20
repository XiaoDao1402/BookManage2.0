using JW.Base.Base;
using JW.Base.Lang.Attributes;
using JW.Base.Lang.Date;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace JW.Base.Lang {
    /// <summary>
    /// 对象工具类
    /// </summary>
    public static class ObjectHelper {
        #region ToString
        /// <summary>
        /// 对象转换为字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>字符串</returns>
        public static String ObjectToString(this object obj) {
            if (obj == null)
                return null;

            return obj.ToString();
        }

        /// <summary>
        /// 将对象转化为字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="objString"></param>
        /// <param name="properties">要显示的属性</param>
        /// <returns>对象字符串</returns>
        public static String ObjectToString(this object obj, string objString = null, params string[] properties) {
            if (obj == null)
                return null;

            var displayString = objString;
            if (displayString.IsNullOrEmpty()) {
                displayString = obj.GetType().Name;
                NameAttribute displayNameAttrib = obj.GetType().GetAttribute<NameAttribute>(true);
                if (displayNameAttrib != null)
                    displayString = displayNameAttrib.DisplayName;
            }

            string propertyString = null;
            foreach (var property in properties) {
                if (property == null)
                    continue;

                var value = obj.GetPropertyValue(property);
                if (value == null)
                    continue;

                if (propertyString.IsNotNullOrEmpty())
                    propertyString += ",";

                var prop = obj.GetType().GetProperty(property);
                var propNameAttr = prop.GetAttribute<NameAttribute>(true);

                propertyString += String.Format("{0}={1}", propNameAttr == null ? property : propNameAttr.DisplayName, value);
            }

            return string.Format("{0}{1}", displayString
                , IsNullOrEmpty(propertyString) ? "" : String.Format("[{0}]", propertyString));
        }
        #endregion

        #region DateTime

        /// <summary>
        /// 字符串转日期时间,空字符串返回默认时间
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, string format) {

            if (str.IsNullOrEmpty()) {
                return ConvertHelper.DEFAULT_DATE;
            }

            DateTime dt = DateTime.ParseExact(str, format, CultureInfo.CurrentCulture);

            return dt;
        }/// <summary>
         /// 是否日期相同
         /// </summary>
         /// <param name="time1"></param>
         /// <param name="time2"></param>
         /// <returns></returns>
        public static bool IsDateEqual(DateTime time1, DateTime time2) {
            return time2.Date.CompareTo(time1.Date) == 0;
        }

        /// <summary>
        /// 是否日期1大于等于日期2
        /// </summary>
        /// <param name="time1">日期1</param>
        /// <param name="time2">日期2</param>
        /// <returns></returns>
        public static bool IsDateGreaterOrEqual(DateTime time1, DateTime time2) {
            return time1.Date.CompareTo(time2.Date) >= 0;
        }

        /// <summary>
        /// 计算日期间天数
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static int CalcDays(DateTime time1, DateTime time2) {
            if (time2.CompareTo(time1) > 0) {
                return (time2.Date - time1.Date).Days;
            } else {
                return (time1.Date - time2.Date).Days;
            }
        }

        /// <summary>
        /// 计算时间差（毫秒）
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static double CalcMilliseconds(DateTime time1, DateTime time2) {
            if (time2.CompareTo(time1) > 0) {
                return (time2 - time1).TotalSeconds;
            } else {
                return (time1 - time2).TotalSeconds;
            }
        }

        /// <summary>
        /// 获取当前日期某个时间
        /// </summary>
        /// <param name="time">时间（"6:00:00"）</param>
        /// <returns></returns>
        public static DateTime CreateCurrentDateAtTime(string time) {
            var current = DateTime.Now.ToString("yyyy/MM/dd");

            var result = DateTime.Parse(string.Format("{0} {1}", current, time));

            return result;
        }

        /// <summary>
        /// 获取星期几
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Week GetWeek(DateTime date) {
            Week week = null;
            switch ((int)date.DayOfWeek) {
                case 0:
                    week = new Week { EN = "Sunday", CN = "星期天" };
                    break;
                case 1:
                    week = new Week { EN = "Monday", CN = "星期一" };
                    break;
                case 2:
                    week = new Week { EN = "Tuesday", CN = "星期二" };
                    break;
                case 3:
                    week = new Week { EN = "Wednesday", CN = "星期三" };
                    break;
                case 4:
                    week = new Week { EN = "Thursday", CN = "星期四" };
                    break;
                case 5:
                    week = new Week { EN = "Friday", CN = "星期五" };
                    break;
                default:
                    week = new Week { EN = "Saturday", CN = "星期六" };
                    break;
            }
            return week;
        }

        /// <summary>
        /// 获取月份
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Month GetMonth(DateTime date) {
            switch (date.Month) {
                case 1:
                    return new Month { EN = "JAN", CN = "一月" };
                case 2:
                    return new Month { EN = "FEB", CN = "二月" };
                case 3:
                    return new Month { EN = "MAR", CN = "三月" };
                case 4:
                    return new Month { EN = "APR", CN = "四月" };
                case 5:
                    return new Month { EN = "MAY", CN = "五月" };
                case 6:
                    return new Month { EN = "JUNE", CN = "六月" };
                case 7:
                    return new Month { EN = "JULY", CN = "七月" };
                case 8:
                    return new Month { EN = "AUG", CN = "八月" };
                case 9:
                    return new Month { EN = "SEPT", CN = "九月" };
                case 10:
                    return new Month { EN = "OCT", CN = "十月" };
                case 11:
                    return new Month { EN = "NOV", CN = "十一月" };
                case 12:
                    return new Month { EN = "DEC", CN = "十二月" };
                default:
                    return new Month { EN = "JAN", CN = "一月" };
            }
        }

        #endregion

        #region Enum
        /// <summary>
        /// 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string ToDisplayText(this Enum en) {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0) {

                object[] attrs = memInfo[0].GetCustomAttributes(
                                              typeof(DisplayTextAttribute),

                                              false);

                if (attrs != null && attrs.Length > 0)

                    return ((DisplayTextAttribute)attrs[0]).Text;

            }

            return en.ToString();
        }
        #endregion

        #region Is null or empty
        /// <summary>
        /// 如果为空则返回另外一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="testObject">测试对象</param>
        /// <param name="anotherObject">另外一个对象</param>
        /// <returns>如果为空则返回另外一个对象</returns>
        public static T IfNull<T>(this T testObject, T anotherObject) {
            if (((object)testObject).IsNull())
                return anotherObject;

            return testObject;
        }
        /// <summary>
        /// 对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <returns>如果对象为DBNull或为null则返回true；否则返回false</returns>
        public static bool IsNull(this object testObject) {
            if (testObject == null || (object)testObject == Convert.DBNull)
                return true;

            return false;
        }
        /// <summary>
        /// 对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <returns>如果对象为DBNull或为null或者为空字符串或者为空数组，则返回true；否则返回false</returns>
        public static bool IsNullOrEmpty(this object testObject) {
            if (testObject == null || (object)testObject == Convert.DBNull)
                return true;

            if (testObject is string) {
                return String.IsNullOrEmpty(((string)testObject));
            } else if (testObject is ICollection) {
                if (((ICollection)testObject).Count == 0)
                    return true;
            } else if (testObject is Array) {
                if (((Array)testObject).Length == 0)
                    return true;
            } else if (testObject is DataTable) {
                if (((DataTable)testObject).Rows.Count == 0)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 对象是否不为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <returns>如果对象为DBNull或为null或者为空字符串或者为空数组，则返回false；否则返回true</returns>
        public static bool IsNotNullOrEmpty(this object testObject) {
            return !IsNullOrEmpty(testObject);
        }
        #endregion

        #region Check true or false
        /// <summary>
        /// 检查布尔变量是否为True
        /// </summary>
        /// <param name="boolValue">布尔变量</param>
        /// <param name="message">如果不为True，抛出的异常信息</param>
        public static void CheckTrue(this bool boolValue, string message) {
            if (!boolValue)
                throw new Exception(message);
        }

        /// <summary>
        /// 检查布尔变量是否为False
        /// </summary>
        /// <param name="boolValue">布尔变量</param>
        /// <param name="message">如果不为False，抛出的异常信息</param>
        public static void CheckFalse(this bool boolValue, string message) {
            CheckTrue(!boolValue, message);
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        public static void CheckNullOrEmpty(this object testObject) {
            CheckNullOrEmpty(testObject, "Object is null or empty.");
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <param name="message">如果对象为空，抛出的异常信息</param>
        public static void CheckNullOrEmpty(this object testObject, string message) {
            if (!IsNullOrEmpty(testObject))
                throw new Exception(message);
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        public static void CheckNull(this object testObject) {
            CheckNull(testObject, "Object is null.");
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <param name="message">如果对象为空，抛出的异常信息</param>
        public static void CheckNull(this object testObject, string message) {
            if (!testObject.IsNull())
                throw new Exception(message);
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <returns>如果对象为DBNull或者为null则返回true,否则返回false</returns>
        public static void CheckNotNullOrEmpty(this object testObject) {
            CheckNotNullOrEmpty(testObject, "Object is null or empty.");
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <returns>如果对象为DBNull或者为null则返回true,否则返回false</returns>
        /// <param name="message">如果对象不为空，抛出的异常信息</param>
        public static void CheckNotNullOrEmpty(this object testObject, string message) {
            if (IsNullOrEmpty(testObject))
                throw new Exception(message);
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <returns>如果对象为DBNull或者为null则返回true,否则返回false</returns>
        public static void CheckNotNull(this object testObject) {
            CheckNotNull(testObject, "Object is null.");
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="testObject">测试对象</param>
        /// <returns>如果对象为DBNull或者为null则返回true,否则返回false</returns>
        /// <param name="message">如果对象不为空，抛出的异常信息</param>
        public static void CheckNotNull(this object testObject, string message) {
            if (testObject.IsNull())
                throw new Exception(message);
        }
        #endregion

        #region Check Byte Size

        /// <summary>
        /// 检查文本大小不超过最大字节
        /// </summary>
        /// <param name="content">待检测文本</param>
        /// <param name="maxSize">最大字节</param>
        /// <returns></returns>
        public static bool CheckMaxSize(this string content, int maxSize) {
            int len = System.Text.Encoding.Unicode.GetByteCount(content);
            if (len > maxSize) {
                return false;
            }
            return true;
        }

        #endregion

        #region 数据比对
        /// <summary>
        /// 判断对象是否相等
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="src">源对象</param>
        /// <param name="dest">目标对象</param>
        /// <returns>如果对象相等，返回True，否则返回False</returns>
        public static bool IsEqual<T>(this T src, T dest)
            where T : IEquatable<T> {
            if (src == null) {
                if (dest == null)
                    return true;
                else
                    return false;
            }

            if (src is IEquatable<T>) {
                return ((IEquatable<T>)src).Equals(dest);
            }

            return (object)src == (object)dest;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">对象类型能够</typeparam>
        /// <param name="sourceObject">待转换的对象</param>
        /// <param name="nullValue">如果对象为空，返回的值</param>
        /// <param name="castErrorMessage">转换异常信息</param>
        /// <returns>类型转换后的对象</returns>
        public static T ObjectCast<T>(this object sourceObject, T nullValue, string castErrorMessage) {
            if (sourceObject == null) {
                return nullValue;
            }

            try {
                return (T)sourceObject;
            } catch {
                throw new Exception(castErrorMessage);
            }
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="sourceObject">待转换的对象</param>
        /// <param name="nullValue">如果对象为空，返回的值</param>
        /// <returns>类型转换后的对象</returns>
        public static T ObjectCast<T>(this object sourceObject, T nullValue) {
            return ObjectCast(sourceObject, nullValue, "Object cannot cast to new type.");
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="sourceObject">待转换的对象</param>
        /// <returns>类型转换后的对象</returns>
        public static T ObjectCast<T>(this object sourceObject) {
            return ObjectCast(sourceObject, default(T));
        }
        #endregion

        #region 初始化简化方法
        /// <summary>
        /// 添加数据细节
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T AddDetail<T>(this T exception, string key, object value) where T : Exception {
            exception.Data.Add(key, value);
            return exception;
        }

        /// <summary>
        /// 添加元素到字典
        /// </summary>
        /// <param name="dict">字典</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>字典</returns>
        public static Dictionary<K, V> Append<K, V>(this Dictionary<K, V> dict, K key, V value) {
            dict.Add(key, value);
            return dict;
        }

        /// <summary>
        /// 添加元素到字典
        /// </summary>
        /// <param name="dict">字典</param>
        /// <param name="elements">元素</param>
        /// <returns>字典</returns>
        public static Dictionary<K, V> Append<K, V>(this Dictionary<K, V> dict, IEnumerable<KeyValuePair<K, V>> elements) {
            foreach (KeyValuePair<K, V> element in elements) {
                if (!dict.ContainsKey(element.Key)) {
                    dict.Add(element.Key, element.Value);
                }
            }
            return dict;
        }

        /// <summary>
        /// 添加元素到列表
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="elements">元素</param>
        /// <returns>列表</returns>
        public static List<T> Append<T>(this List<T> list, params T[] elements) {
            foreach (T element in elements) {
                list.Add(element);
            }
            return list;
        }

        /// <summary>
        /// 修改列表元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="source">修改源</param>
        /// <param name="change">修改值</param>
        /// <returns>列表</returns>
        public static List<T> Change<T>(this List<T> list, T source, T change) {
            if (source.IsNullOrEmpty() || change.IsNullOrEmpty())
                return list;
            // 获取列表中的修改源
            T temp = list.Where(t => {
                return t.Equals(source);
            }).FirstOrDefault();
            // 获取修改源的列表序号
            int index = list.IndexOf(temp);
            // 存在序号修改
            if (index >= 0) {
                list[index] = change;
            }
            return list;
        }


        /// <summary>
        /// 添加元素到列表
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="elements">元素</param>
        /// <returns>列表</returns>
        public static List<T> Append<T>(this List<T> list, IEnumerable<T> elements) {
            if (elements.IsNullOrEmpty())
                return list;

            foreach (T element in elements) {
                list.Add(element);
            }
            return list;
        }
        #endregion

        #region 获取/设置属性
        /// <summary>
        /// 获取属性值，如果能找到相应属性，返回属性值，否则返回null
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性</param>
        /// <returns>属性值</returns>
        public static object GetPropertyValue(this object obj, string propertyName) {
            if (obj == null || propertyName.IsNullOrEmpty())
                return null;

            Type objType = obj.GetType();

            var property = objType.GetProperty(propertyName);
            if (property != null) {
                try {
                    return property.GetValue(obj, null);
                } catch {
                    return null;
                }
            } else {
                var field = objType.GetField(propertyName);
                if (field != null) {
                    try {
                        return field.GetValue(obj);
                    } catch {
                        return null;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns>返回obj</returns>
        public static T SetPropertyValue<T>(this T obj, string propertyName, object propertyValue) {
            if (obj == null || propertyName.IsNullOrEmpty())
                return obj;

            Type objType = obj.GetType();

            var property = objType.GetProperty(propertyName);
            if (property != null) {
                property.SetValue(obj, propertyValue, null);

                return obj;
            } else {
                var field = objType.GetField(propertyName);
                if (field != null) {
                    field.SetValue(obj, propertyValue);
                }
                return obj;
            }
        }
        #endregion

        #region 新增截取数组中指定长度数据和异或运算
        /// <summary>
        /// 截取数组中指定长度数据
        /// </summary>
        /// <param name="src">目标数组</param>
        /// <param name="startIndex">数组下标</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取后数据</returns>
        public static byte[] GetSubArray(byte[] src, int startIndex, int length) {
            if (startIndex + length > src.Length)
                return null;
            byte[] ret = new byte[length];
            for (int i = 0; i < length; i++) {
                ret[i] = src[startIndex + i];
            }
            return ret;
        }

        /// <summary>
        /// 异或运算
        /// </summary>
        /// <param name="byte1"></param>
        /// <param name="byte2"></param>
        /// <returns></returns>
        public static bool Xor(byte byte1, byte byte2) {
            bool falg = false;
            if ((byte1 & byte2) == byte2)
                falg = true;
            return falg;
        }
        #endregion

        #region 解构扩展

        /// <summary>
        /// 键值对解构
        /// </summary>
        /// <typeparam name="K">键类型</typeparam>
        /// <typeparam name="V">值类型</typeparam>
        /// <param name="keyValue">键值对</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Deconstruct<K, V>(this KeyValuePair<K, V> keyValue, out K key, out V value) {
            key = keyValue.Key;
            value = keyValue.Value;
        }

        #endregion

        #region Color

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="htmlColor"></param>
        /// <returns></returns>
        public static Color FromHtml(this Color color, string htmlColor) {
            return ColorTranslator.FromHtml(htmlColor);
        }

        #endregion
    }
}
