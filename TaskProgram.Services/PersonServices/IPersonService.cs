using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProgram.Database.Models;

namespace TaskProgram.Services.PersonServices
{
	public interface IPersonService
	{
		void Create(Person person);
		bool Delete(int id);
		List<Person> GetAllFromDb();
		Person GetById(int id);
		bool Update(Person person);
		List<Person> GetAll();
		List<Person> Deserialize();
		void Serialize(List<Person> people);
	}
}
