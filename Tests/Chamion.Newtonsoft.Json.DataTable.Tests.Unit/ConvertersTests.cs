using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chamion.Newtonsoft.Json.DataTable.Converters;
using Chamion.Newtonsoft.Json.DataTable.Services;
using Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Comparers;
using Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Enums;
using Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Models;
using Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Models.Car;
using Chamion.Newtonsoft.Json.DataTable.Tests.Unit.Models.Price;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Chamion.Newtonsoft.Json.DataTable.Tests.Unit
{
    [TestFixture]
    public class ConvertersTests
    {
        private System.Data.DataTable _dataTable;
        private MyBigModel _model;
        private readonly DataTableComparer _comparer = new DataTableComparer();

        [SetUp]
        public void Setup()
        {
            _dataTable = new System.Data.DataTable();

            var engine1 = new Engine
            {
                Name = "Engine1",
                Material = new Material { Id = 1, TextureCode = 0.5 },
                Id = 1
            }; 
            
            var engine2 = new Engine
            {
                Name = "Engine2",
                Material = new Material { Id = 2, TextureCode = 0.6 },
                Id = 2
            };
            
            var cars = new List<Car>
            {
                new Car { Id = 1, Name = "Car1", Engine = engine1, Type = CarType.Cheap },
                new Car { Id = 2, Name = "Car2", Engine = engine2, Type = CarType.Elite }
            };
            
            var price = new Price
            {
                Id = 1,
                Value = 100
            };
            
            var price2 = new Price
            {
                Id = 2,
                Value = 200
            };
            
            var price3 = new Price
            {
                Id = 3,
                Value = 300
            };

            _dataTable.Columns.Add("Id", typeof(int));
            _dataTable.Columns.Add("Name", typeof(string));
            _dataTable.Columns.Add("Data", typeof( List<Car>)); 
            _dataTable.Columns.Add("Type", typeof( CarType)); 
            _dataTable.Columns.Add("Prices", typeof( ObservableCollection<Price>)); 
            
            _dataTable.Rows.Add();
            _dataTable.Rows.Add(1, "Коллеция", cars, CarType.Cheap, new ObservableCollection<Price> { price, price2, price3 });
            _dataTable.Rows.Add(2, "Мерседес", cars, CarType.Elite);
            _dataTable.Rows.Add(3, "Лада", DBNull.Value, CarType.Cheap);
            _dataTable.Rows.Add(DBNull.Value, DBNull.Value,  new List<Car>(), DBNull.Value);
            
            _model = new MyBigModel
            {
                DataTable = _dataTable,
                Type = CarType.Cheap,
                Id = 1
            };
        }
        
        [Test]
        public void DataTableJsonConvert_SuccessfulConvert()
        {
            var structDatService = new StructureDataConverter();
            
            var objects = structDatService.ToAnonymousObjects(_model.DataTable);
            var jsonString = JsonConvert.SerializeObject(objects);
            var toDto = structDatService.MapAnonymousObjects<RowsObject>(objects);
            
            var deserializeObjects = JsonConvert.DeserializeObject<List<object>>(jsonString);
            
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new DataTableConverter<RowsObject>()
                }
            };
            
            var serializeObject = JsonConvert.SerializeObject(_model, settings);
            var deserializeObject = JsonConvert.DeserializeObject<MyBigModel>(serializeObject,settings);
            var serializeObjectNew = JsonConvert.SerializeObject(_model, settings);
            
            var isColumnTypeAndNameAndRowsCountEquals = _comparer.AreEqual(_model.DataTable, deserializeObject.DataTable);
            var isContentJsonIsEquals = serializeObject.Equals(serializeObjectNew);
            
            Assert.IsTrue(isContentJsonIsEquals);
            Assert.IsTrue(isColumnTypeAndNameAndRowsCountEquals);
        }
    }
}