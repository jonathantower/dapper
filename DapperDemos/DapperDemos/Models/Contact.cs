using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DapperDemos.Models
{
	//[Table("Contacts")]
	public class Contact //: IContact
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		public string FullName {
			get { return FirstName + " " + LastName; }
		}
	}
}
