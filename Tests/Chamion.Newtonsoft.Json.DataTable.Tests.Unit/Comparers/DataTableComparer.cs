using System;

namespace Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Comparers
{
    public class DataTableComparer
    {
        public bool AreEqual(System.Data.DataTable dt1, System.Data.DataTable dt2)
        {
            if (dt1.Rows.Count != dt2.Rows.Count || dt1.Columns.Count != dt2.Columns.Count)
                return false;

            for (var i = 0; i < dt1.Columns.Count; i++)
            {
                if (dt1.Columns[i].ColumnName != dt2.Columns[i].ColumnName ||
                    dt1.Columns[i].DataType != dt2.Columns[i].DataType)
                    return false;
            }
            
            return true;
        }

        private bool IsEqualDbNull(object objA, object objB)
        {
            if (objA is DBNull)
            {
                return IsDbNullEquivalent(objB);
            }

            if (objB is DBNull)
            {
                return IsDbNullEquivalent(objA);
            }
            return false;
        }

        private bool IsDbNullEquivalent(object obj)
        {
            if (obj is int || obj is double || obj is decimal)
            {
                return Convert.ToDecimal(obj) == 0;
            }

            if (obj is string || obj is char)
            {
                return string.IsNullOrEmpty(obj.ToString());
            }
            if (obj is bool b)
            {
                return !b;
            }
            return true;
        }
        
    }
}