using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;


namespace DapperDemos.Models
{
	//[Table("Contacts")]
	public class Contact : IContact
	{
		//[Key]
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		[Computed]
		public string FullName {
			get { return FirstName + " " + LastName; }
		}
	}
}
