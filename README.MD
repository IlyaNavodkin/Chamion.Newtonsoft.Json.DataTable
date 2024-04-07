# Preface

Simple DataTable converter for Newtonsoft.Json seriliaze lib.

# Get starting

Create dto for DataTable row represent

```c#
public class RowsObject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Car.Car> Data { get; set; }

    public CarType Type { get; set; }

    public ObservableCollection<Price.Price> Prices { get; set; }
}
```

Create model class with DataTable and add JsonConverter attribute

```c#
public class MyBigModel
{
    public int Id { get; set; }

    public CarType Type { get; set; }

    [JsonConverter(typeof(DataTableConverter<RowsObject>))]  /// add this with Row dto class type
    public System.Data.DataTable DataTable { get; set; }
}
```

Use json lib, Newtonsoft.Json can do all work for u :)

```c#
var serializeObject = JsonConvert.SerializeObject(_model);
var deserializeObject = JsonConvert.DeserializeObject<MyBigModel>(serializeObject);
var serializeObjectNew = JsonConvert.SerializeObject(_model);

var isColumnTypeAndNameAndRowsCountEquals = _comparer.AreEqual(_model.DataTable, deserializeObject.DataTable);
var isContentJsonIsEquals = serializeObject.Equals(serializeObjectNew);
```

# License

This project uses the following libraries with MIT licenses:

-   Newtonsoft.Json (https://www.newtonsoft.com/json)
    Author: James Newton-King
    License: MIT License (https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)

-   NUnit (https://nunit.org/)
    Author: Charlie Poole, Rob Prouse
    License: MIT License (https://github.com/nunit/nunit/blob/master/LICENSE.txt)
