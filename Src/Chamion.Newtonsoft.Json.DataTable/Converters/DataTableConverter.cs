using System;
using System.Collections.Generic;
using Chamion.Newtonsoft.Json.DataTable.Services;
using Chamion.Newtonsoft.Json.DataTable.Services.Contracts;
using Newtonsoft.Json;

namespace Chamion.Newtonsoft.Json.DataTable.Converters
{
    /// <inheritdoc />
    public class DataTableConverter<T> : JsonConverter where T : new()
    {
        private readonly IStructureDataConverter _structureDataConverter 
            = new StructureDataConverter();

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dataTable = (System.Data.DataTable)value;

            var result = _structureDataConverter.ToObjects<T>(dataTable);
            
            serializer.Serialize(writer, result);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var objects = serializer.Deserialize<List<T>>(reader);
            
            var result = _structureDataConverter.ToDataTable<T>(objects);
            
            return result;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(System.Data.DataTable);
        }
    }
}