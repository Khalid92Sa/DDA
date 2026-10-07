using DDA.ViewModels.Campaigns;
using OfficeOpenXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace DDA.Web.Helpers
{
    public static class ExcelCampaignReader
    {
        /// <summary>
        /// Reads column A = CIF, column B = Balance from the first worksheet.
        /// A header row ("CIF" in A1) is skipped. Blank rows are ignored; invalid rows are counted in skipped.
        /// </summary>
        public static List<CampaignExcelRow> Read(Stream stream, out int skipped)
        {
            var rows = new List<CampaignExcelRow>();
            skipped = 0;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (var package = new ExcelPackage(stream))
            {
                if (package.Workbook.Worksheets.Count == 0) return rows;
                var ws = package.Workbook.Worksheets[1];
                if (ws.Dimension == null) return rows;

                var firstRow = ws.Dimension.Start.Row;
                var lastRow = ws.Dimension.End.Row;

                var a1 = (ws.Cells[firstRow, 1].Text ?? string.Empty).Trim();
                if (a1.Equals("CIF", System.StringComparison.OrdinalIgnoreCase)) firstRow++;

                for (var r = firstRow; r <= lastRow; r++)
                {
                    var cif = (ws.Cells[r, 1].Text ?? string.Empty).Trim();
                    var balanceCell = ws.Cells[r, 2];

                    if (cif.Length == 0 && string.IsNullOrWhiteSpace(balanceCell.Text)) continue; // blank row

                    decimal balance;
                    if (cif.Length == 0 || !TryGetBalance(balanceCell, out balance) || balance < 0 || cif.Length > 50)
                    {
                        skipped++;
                        continue;
                    }

                    rows.Add(new CampaignExcelRow { Cif = cif, Balance = balance });
                }
            }
            return rows;
        }

        private static bool TryGetBalance(ExcelRange cell, out decimal balance)
        {
            balance = 0;
            if (cell.Value is double) { balance = (decimal)(double)cell.Value; return true; }

            var text = (cell.Text ?? string.Empty).Replace(",", string.Empty).Trim();
            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out balance);
        }
    }
}
