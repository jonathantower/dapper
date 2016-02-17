using System;
using System.Configuration;
using System.Data.SqlClient;
using DapperDemos.Models;
using DapperExtensions;
using DapperExtensions.Mapper;

namespace DapperDemos.Classes
{
	public class DapperExtension
	{
		private static readonly string CONNECTION_STRING = ConfigurationManager
			.ConnectionStrings["default"]
			.ConnectionString;

		public static void RunAll()
		{
			Update();
		}

		public static void Update()
		{
			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();

				var c = cn.Get<Contact>(1);
				c.FirstName = "Jonathan";

				var updated = cn.Update(c);

				Console.WriteLine("Updated {0}", updated);
			}

			Console.ReadKey();
		}

	}
}