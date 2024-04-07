using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Enums;

namespace Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Models
{
    public class RowsObject
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public List<Car.Car> Data { get; set; }
        
        public CarType Type { get; set; }
        
        public ObservableCollection<Price.Price> Prices { get; set; }
    }
}