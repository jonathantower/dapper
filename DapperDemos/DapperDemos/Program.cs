using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperDemos.Classes;
using DapperDemos.Models;

namespace DapperDemos
{
	class Program
	{
		static void Main(string[] args)
		{
			DapperBasic.HelloWorld();
			//DapperBasic.RunAll();
			//DapperContrib.RunAll();
			//DapperSQLBuilder.RunAll();
			//DapperRainbow.RunAll(); //doesn't work
			//DapperExtension.RunAll();
		}

	}
}
