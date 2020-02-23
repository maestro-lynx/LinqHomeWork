using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Data;

namespace Task
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
            var dataSource = new DataSource();


            var customers = dataSource.Customers;
            var suppliers = dataSource.Suppliers;
            var product = dataSource.Products;


            //заказы первого клиента
            var ordersFirstCustomer = customers.First().Orders;

            //сумма первого заказа у первого клиента
            var summFirstOrderOfOFirstCustomer = customers.First().Orders.First().Total;
            foreach (var item in customers)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
	}
}