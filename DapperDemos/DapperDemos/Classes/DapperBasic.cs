using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperDemos.Models;

namespace DapperDemos.Classes
{
	public class DapperBasic
	{
		private static readonly string CONNECTION_STRING = ConfigurationManager
			.ConnectionStrings["default"]
			.ConnectionString;

		public static void RunAll()
		{
			SimpleQuery();
			ParameterizedQuery();
			DynamicMapping();
			ExecuteWithNoResults();
			BulkInsert();
			InListSupport();
			MultiMapping();
			MultiResults();
			StoredProcedure();
		}

		private static void SimpleQuery()
		{
			// create connection
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				var contact = cn.Query<Contact>(
					"SELECT * FROM Contacts WHERE ID = 1")
					.First();

				Console.WriteLine(
					"{0} {1} (simple query)",
					contact.FirstName,
					contact.LastName);

				Console.ReadKey();
			}
		}

		private static void ParameterizedQuery()
		{
			// create connection
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				// open connection
				cn.Open();

				// execute query
				var contact = cn.Query<Contact>(
					"SELECT * FROM Contacts WHERE ID = @ID",
					new { ID = 1 })
					.First();

				// write out first record
				Console.WriteLine(
					"{0} {1} (parameterized query)",
					contact.FirstName,
					contact.LastName);

				// wait for keypress
				Console.ReadKey();
			}
		}

		private static void DynamicMapping()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				var contacts = cn.Query("select * from contacts");

				Console.WriteLine("{0} (dynamic)", contacts.First().FirstName);
			}

			Console.ReadKey();
		}

		private static void ExecuteWithNoResults()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				cn.Execute(
					@"IF NOT OBJECT_ID('dbo.Products', 'U') IS NULL 
					  DROP TABLE dbo.Products");

				Console.WriteLine("Removed Products table");
			}

			Console.ReadKey();
		}

		private static void BulkInsert()
		{
			var contacts = new[]
			{
				new Contact {FirstName = "Test", LastName = "User 1", Email = "user1@test.com"},
				new Contact {FirstName = "Test", LastName = "User 2", Email = "user2@test.com"},
				new Contact {FirstName = "Test", LastName = "User 3", Email = "user3@test.com"},
			};

			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				var rowCount = cn.Execute(
					@"insert Contacts(FirstName, LastName, Email) 
					  values (@FirstName, @LastName, @Email)",
					contacts);

				Console.WriteLine("Inserted {0} rows", rowCount);
			}

			Console.ReadKey();


		}

		private static void InListSupport()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				// "SELECT * FROM Contacts WHERE ID IN(1, 2, 3)"
				var count = cn.Query(
					"SELECT Count(*) as [Count] FROM Contacts WHERE ID in @IDs",
					new { IDs = new[] { 1, 2, 3 } }).First().Count;

				Console.WriteLine("Found {0} rows with IN() query", count);
			}
		}

		private static void MultiMapping()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				var o = cn.Query<Order, Contact, Order>(
					@"SELECT * 
                      FROM Orders o
					  JOIN Contacts c on c.Id = o.ContactID
					  WHERE c.ID = 1",
					 (order, contact) => { order.Contact = contact; return order; })
					.First();

				Console.WriteLine(
					"Order: {0} with contact {1} {2}",
					o.Id,
					o.Contact.FirstName,
					o.Contact.LastName);
			}

			Console.ReadKey();
		}

		private static void MultiResults()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				var multiResults = cn.QueryMultiple(
					@"
						SELECT * FROM Contacts WHERE ID = 1
						SELECT * FROM Orders WHERE ID = 1
					",
					new { id = 1 });

				var contact = multiResults.Read<Contact>().First();
				var order = multiResults.Read<Order>().First();

				Console.WriteLine("Found contact id {0}", contact.Id);
				Console.WriteLine("Found order id {0}", order.Id);
			}

			Console.ReadKey();
		}

		private static void StoredProcedure()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				var parameters = new DynamicParameters();
				parameters.Add("@contactID", 1);

				var c = cn.Query<Contact>(
						"spMagicProc",
						parameters,
						commandType: CommandType.StoredProcedure)
					.First();

				Console.WriteLine("Found {0} {1}", c.FirstName, c.LastName);
			}

			Console.ReadKey();




		}
	}
}
