using Project1.Data;

IEnumerable<int> valuesEnum = Enumerable.Range(0, 5);
var valuesEnumOver22 = valuesEnum.Where(x => x >= 3);
Console.WriteLine($"Print an IEnumerable<int>.ToList().Count: {valuesEnumOver22.ToList().Count}");



//IEnumerableVSIQueryable
//On the frist we request data from a DataBase. If we apply a filter (where id==3 for example)
//All the data is sent from de DB to the client side and the filter is applied in memmory. On the
//other hand, a IQueryable item filters everthing on the server. The main difference is where the
//filter logic is applied.




