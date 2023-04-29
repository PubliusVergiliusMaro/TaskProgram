using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProgram.Database.Configurations;
using TaskProgram.Database.Models;

namespace TaskProgram.Services.PersonServices
{
	public interface IPersonService
	{
		void CreateADO(Person person);
		void CreateEF(Person person);
		bool DeleteADO(int id);
		bool DeleteEF(int id);
		List<Person> GetAllADO();
	    List<Person> GetAllEF();
		Person GetByIdADO(int id);
		Person GetByIdEF(int id);
		bool UpdateADO(Person newPerson);
		bool UpdateEF(Person person);
		List<Person> Deserialize(string path);
		void Serialize(List<Person> people, string path);
	}
}
