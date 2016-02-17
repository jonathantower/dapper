using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using DapperDemos.Models;
using DapperExtensions;

namespace DapperDemos.Classes
{
	public class DapperRainbow
	{
		private static readonly string CONNECTION_STRING = ConfigurationManager
			.ConnectionStrings["default"]
			.ConnectionString;

		public static void RunAll()
		{
			Update();
		}

		public class CrmDatabase : Database<CrmDatabase>
		{
			public Table<Contact> Contacts { get; set; }
			public Table<Order> Orders { get; set; }
		}

		public static void Update()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();
				var db = CrmDatabase.Init(cn, commandTimeout: 2);

				var c = db.Contacts.Get(1);
				c.FirstName = "Jonathan";
				var count = db.Contacts.Update(1, c);

				Console.WriteLine("Updated {0} rows", count);
			}

			Console.ReadKey();
		}
	}
}