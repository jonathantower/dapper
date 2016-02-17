using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper.Contrib.Extensions;
using DapperDemos.Models;

namespace DapperDemos.Classes
{
	public class DapperContrib
	{
		private static readonly string CONNECTION_STRING = ConfigurationManager
			.ConnectionStrings["default"]
			.ConnectionString;

		public static void RunAll()
		{
			GetMethods();
			Insert();
			UpdatesWithChangeTracking();
		}

		public static void GetMethods()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				var contact = cn.Get<Contact>(1);
				var contacts = cn.GetAll<Contact>();

				Console.WriteLine(
					"Found contact {0} {1} and {2} total contacts",
					contact.FirstName,
					contact.LastName,
					contacts.Count());
			}

			Console.ReadKey();
		}

		public static void Insert()
		{
			var contact = new Contact
			{
				FirstName = "New",
				LastName = "Contact",
				Email = "new@contact.com"
			};

			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				var id = cn.Insert(contact);
				Console.WriteLine("Inserted record ID: {0}", id);
			}

			Console.ReadKey();
		}

		public static void UpdatesWithChangeTracking()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				var contact = cn.Get<IContact>(1);
				Console.WriteLine("Update occurred: {0}", cn.Update(contact));
				
				contact.FirstName = "J.";
				Console.WriteLine("Update occurred: {0}", cn.Update(contact));
			}

			Console.ReadKey();
		}
	}
}