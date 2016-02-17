using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DapperDemos.Models;

namespace DapperDemos.Classes
{
	public class DapperSQLBuilder
	{
		private static readonly string CONNECTION_STRING = ConfigurationManager
			.ConnectionStrings["default"]
			.ConnectionString;

		public static void RunAll()
		{
			BuildSQL("test");
		}

		private static void BuildSQL(string firstName = null, string lastName = null)
		{
			var builder = new SqlBuilder();
			var template = builder.AddTemplate("Select * from Contacts /**where**/ ");
		
			if (firstName != null)
				builder.Where("FirstName LIKE '%' + @FirstName + '%'", new { FirstName = firstName });

			if (lastName != null)
				builder.Where("LastName LIKE '%' + @LastName + '%'", new { LastName = lastName });

			using (var cn = new SqlConnection(CONNECTION_STRING))
			{
				cn.Open();
				var c = cn.Query<Contact>(template.RawSql, template.Parameters);
				Console.WriteLine("Found {0} contacts", c.Count());
			}


			Console.ReadKey();
		}
	}
}