using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JW.Base.File {
    /// <summary>
    /// 辅助类：Excel文件
    /// </summary>
    public class Excel {
        /// <summary>
        /// 实体列表导出EXCEL
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="sheetName">Sheet名</param>
        /// <param name="tableTitle">表头名称</param>
        /// <param name="action">数据动作</param>
        /// <returns></returns>
        public static byte[] ExportExcelByList<T>(List<T> list, string sheetName, string[] tableTitle, Action<T, IRow> action) where T : class, new() {
            IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow Title = null;
            IRow rows = null;
            int row = 0;

            //表头
            if (tableTitle.Length > 0) {
                row++;
                Title = sheet.CreateRow(0);
                for (int k = 0; k < tableTitle.Length; k++) {
                    Title.CreateCell(k).SetCellValue(tableTitle[k]);
                }
            }

            list.ForEach(it => {
                rows = sheet.CreateRow(row);
                action(it, rows);
                row++;
            });

            byte[] buffer = new byte[1024 * 5];
            using (MemoryStream ms = new MemoryStream()) {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
                ms.Close();
            }
            return buffer;
        }
    }
}
