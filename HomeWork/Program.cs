using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeWork.Data;

namespace HomeWork
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataSource = new DataSource();


            var customers = dataSource.Customers;
            var suppliers = dataSource.Suppliers;
            var product = dataSource.Products;

            #region первое задание
            //Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) превосходит некоторую величину X.
            decimal x;
            Console.Write("Величина Х = ");
            x = decimal.Parse(Console.ReadLine());
            var customersOrdersSum = from c in customers
                                     where c.Orders.Select(p => p.Total).Sum() > x
                                     select new
                                     {
                                         name = c.CompanyName,
                                         SumOfOrders = c.Orders.Select(p => p.Total).Sum()
                                     };
            foreach (var customer in customersOrdersSum)
            {
                Console.WriteLine($"{customer.name} \t {customer.SumOfOrders}");
            }
            Console.ReadKey();
            #endregion
            #region второе задание 
            //Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X
            Console.Write("Величина Х = ");
            decimal x2 = decimal.Parse(Console.ReadLine());
            var customersHighOrders = from c in customers
                                      where c.Orders.Any(p => p.Total > x2)
                                      select new
                                      {
                                          Name = c.CompanyName,
                                          Orders = c.Orders.Select(p => p),
                                          Sum = c.Orders.Select(p => p.Total).Sum()
                                      };
            foreach (var item in customersHighOrders)
            {
                Console.WriteLine($"{item.Name}");
                foreach (var order in item.Orders)
                {
                    Console.WriteLine($"\t{order.OrderDate}\t{order.OrderID}\t{order.Total}");
                }
                Console.WriteLine($"Sum = {item.Sum}");
                Console.WriteLine();
            }
            Console.ReadKey();
            #endregion
            #region третье задание
            //Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. 
            var customerToSuppliers = from c in customers
                                      join s in suppliers on c.City
                                      equals s.City into spl
                                      select new
                                      {
                                          CustomerID = c.CustomerID,
                                          CustomerName = c.CompanyName,
                                          CustomerLocation = c.City,
                                          Suppliers = spl
                                      };

            foreach (var item in customerToSuppliers)
            {
                bool hasSupplier = false;
                Console.WriteLine($"{item.CustomerID}\t{item.CustomerName}\t{item.CustomerLocation}\t");
                Console.WriteLine("Suppliers: ");
                foreach (var supplier in item.Suppliers)
                {
                    hasSupplier = true;
                    Console.WriteLine($"{supplier.SupplierName}\t{supplier.Address}");
                }
                if (!hasSupplier)
                {
                    Console.WriteLine("---");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            #endregion
            #region четвертое задание
            /*
             * Укажите всех клиентов, у которых указан нецифровой почтовый код или не заполнен регион
             * или в телефоне не указан код оператора(для простоты считаем, что это равнозначно «нет круглых скобочек в начале»).
             */
            var incorrectCustomers = from c in customers
                                     where (!int.TryParse(c.PostalCode, out int result) ||
                                     String.IsNullOrEmpty(c.Region) || !c.Phone.Contains('('))
                                     select new
                                     {
                                         Name = c.CompanyName,
                                         PostalCode = (!int.TryParse(c.PostalCode, out int result)) ? "Postal code isn't correct"
                                                        : "",
                                         Region = String.IsNullOrEmpty(c.Region) ? "Region isn't correct" : "",
                                         Phone = !(c.Phone.Contains('(')) ? "Don't have a code of operator" : ""
                                     };
            foreach (var item in incorrectCustomers)
            {
                Console.WriteLine($"{item.Name}\n\t{item.PostalCode}\t{item.Region}\t{item.Phone}");
                Console.WriteLine();
            }
            Console.ReadKey();
            #endregion
            #region пятое задание
            //Сгруппируйте товары по группам «дешевые», «средняя цена», «дорогие». Границы каждой группы задайте сами
            var ranges = new[] { 30, 80,decimal.MaxValue };
            Queue<string> rangeNames = new Queue<string>();
            rangeNames.Enqueue("Cheap products");
            rangeNames.Enqueue("Average products");
            rangeNames.Enqueue("Expensive products");
            var grouppedproducts = from p in product
                                   group p by (ranges.FirstOrDefault(r => r > p.UnitPrice)) into groups
                                   select new
                                   {
                                   Name = rangeNames.Dequeue(),
                                   Count = groups.Count(),
                                   Products = from pr in groups orderby pr.UnitPrice select pr
                               }                               ;
            foreach (var item in grouppedproducts)
            {
                Console.WriteLine($"{item.Name}: \t{item.Count}");
                foreach (var pr in item.Products)
                {
                    Console.WriteLine($"\t{pr.ProductName}\n\t{pr.UnitPrice}");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            #endregion
        }
    }
}
