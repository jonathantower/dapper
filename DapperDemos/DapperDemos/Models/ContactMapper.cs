using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace DapperDemos.Models
{
	public class ContactMapper : ClassMapper<Contact>
	{
		public ContactMapper()
		{
			//use a custom schema
			Schema("dbo");

			Table("Contacts");

			//have a custom primary key
			Map(x => x.Id).Key(KeyType.Identity);

			//Use a different name property from database column
			Map(x => x.FirstName).Column("FirstName");

			//Ignore this property entirely
			Map(x => x.FullName).Ignore();

			//optional, map all other columns
			AutoMap();
		}
	}
}
