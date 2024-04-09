using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using Chamion.Newtonsoft.Json.DataTable.Services.Contracts;

namespace Chamion.Newtonsoft.Json.DataTable.Services
{
    /// <inheritdoc />
    public class StructureDataConverter : IStructureDataConverter
    {
        /// <inheritdoc />
        public IList<T> ToObjects<T>(System.Data.DataTable dataTable) 
            where T : new()
        {
            var resultList = new List<T>();

            var properties = typeof(T).GetProperties();

            foreach (DataRow row in dataTable.Rows)
            {
                var obj = new T();

                foreach (var property in properties)
                {
                    var column = dataTable.Columns[property.Name];
                    
                    if (column == null) continue;
                    
                    var columnDataType = column.DataType;
                        
                    var rowValue = row[property.Name];

                    if (rowValue == DBNull.Value) continue;
                    
                    if (property.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.Parse(property.PropertyType, rowValue.ToString());
                        property.SetValue(obj, enumValue);
                    }
                    else
                    {
                        var value = Convert.ChangeType(rowValue, columnDataType);
                        property.SetValue(obj, value);
                    }
                }
                
                resultList.Add(obj);
            }

            return resultList;
        }
        
        public IList<object> ToAnonymousObjects(System.Data.DataTable dataTable)
        {
            var resultList = new List<object>();

            foreach (DataRow row in dataTable.Rows)
            {
                var obj = new ExpandoObject() as IDictionary<string, object>;

                foreach (DataColumn column in dataTable.Columns)
                {
                    var columnName = column.ColumnName;
                    var rowValue = row[columnName];

                    obj[columnName] = rowValue != DBNull.Value ? rowValue : null;
                }

                resultList.Add(obj);
            }

            return resultList;
        }
        
        public IList<T> MapAnonymousObjects<T>(IList<object> anonymousObjects) where T : new()
        {
            var resultList = new List<T>();

            foreach (var anonymousObject in anonymousObjects)
            {
                var resultObject = new T();
                var propertyValues = (IDictionary<string, object>)anonymousObject;

                foreach (var property in typeof(T).GetProperties())
                {
                    if (propertyValues.TryGetValue(property.Name, out var value))
                    {
                        property.SetValue(resultObject, value);
                    }
                }

                resultList.Add(resultObject);
            }

            return resultList;
        }
        
        /// <inheritdoc />
        public System.Data.DataTable ToDataTable<T>(IEnumerable<T> objects)
        {
            var dataTable = new System.Data.DataTable();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                dataTable.Columns.Add(property.Name, 
                    Nullable.GetUnderlyingType(property.PropertyType) ??
                    property.PropertyType);
            }

            foreach (var obj in objects)
            {
                var newRow = dataTable.NewRow();
                foreach (var property in properties)
                {
                    var value = property.GetValue(obj);

                    newRow[property.Name] = value ?? DBNull.Value;
                }
                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }
    }
}