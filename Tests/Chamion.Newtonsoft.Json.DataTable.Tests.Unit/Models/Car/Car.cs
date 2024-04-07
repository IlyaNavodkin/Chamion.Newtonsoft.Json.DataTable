using Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Enums;

namespace Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Models.Car
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CarType Type { get; set; }
        public Engine Engine { get; set; }
    }
}