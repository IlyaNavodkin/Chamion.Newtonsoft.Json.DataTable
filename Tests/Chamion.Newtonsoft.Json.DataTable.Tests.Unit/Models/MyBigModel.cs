using Chamion.Newtonsoft.Json.DataTable.Converters;
using Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Enums;
using Newtonsoft.Json;

namespace Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Models
{
    public class MyBigModel
    {
        public int Id { get; set; }
        
        public CarType Type { get; set; }
        
        [JsonConverter(typeof(DataTableConverter<RowsObject>))]
        public System.Data.DataTable DataTable { get; set; }
    }
}