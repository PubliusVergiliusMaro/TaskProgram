using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using TaskProgram.Common;
using TaskProgram.Database.Models;
using TaskProgram.Database.Repository;

namespace TaskProgram.Services.PersonServices
{
	public class PersonService : IPersonService
	{
		private readonly IGenericRepository<Person> _personRepository;

		public PersonService(IGenericRepository<Person> personRepository)
		{
			_personRepository = personRepository;
		}
		public void Create(Person person)
		{
			try
			{
				using (var connection = new SqlConnection(Constants.CONNECTION_STRING))
				{
					connection.Open();
					using (var command = new SqlCommand($@"INSERT INTO Persons (FirstName, LastName, Gender, Age, PhoneNumber) 
                VALUES ('{person.FirstName}', '{person.LastName}', '{person.Gender}', {person.Age}, '{person.PhoneNumber}')", connection))
					{
						var result = command.ExecuteNonQuery();
						if (result > 0)
						{
							// Get the ID of the newly inserted person
							command.CommandText = "SELECT SCOPE_IDENTITY()";
							int newId = Convert.ToInt32(command.ExecuteScalar());

							// Insert the address of the person
							if (person.Address != null)
							{
								command.CommandText = $@"INSERT INTO Addresses (StreetAddress, City, State, PostalCode, Person_FK) 
                            VALUES ('{person.Address.StreetAddress}', '{person.Address.City}', '{person.Address.State}', '{person.Address.PostalCode}', {newId})";
								command.ExecuteNonQuery();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return;
			}
		}
		public bool Delete(int id)
		{
			try
			{
				using (var connection = new SqlConnection(Constants.CONNECTION_STRING))
				{
					connection.Open();
					using (var command = new SqlCommand($@"DELETE FROM People WHERE id = {id}", connection))
					{
						var res = command.ExecuteNonQuery();
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public List<Person> GetAllFromDb()
		{
			try
			{
				using (var connection = new SqlConnection(Constants.CONNECTION_STRING))
				{
					connection.Open();
					using (var command = new SqlCommand("" +
						 "SELECT Id," +
				" FirstName," +
				" LastName," +
				" Gender," +
				" Age," +
				" PhoneNumber," +
				" StreetAddress," +
				" City," +
				" State," +
				" PostalCode" +
				" FROM People " +
				" INNER JOIN Addresses ON People.AddressId = Addresses.Id", connection))
					{
						using (var reader = command.ExecuteReader())
						{
							List<Person> people = new List<Person>();
							while (reader.Read())
							{
								people.Add(
									new Person
									{
										Id = reader.GetInt32(0),
										FirstName = reader.GetString(1),
										LastName = reader.GetString(2),
										Gender = reader.GetString(3),
										Age = int.Parse(reader.GetString(4)),
										PhoneNumber = reader.GetString(5),
										Address =
										new Address()
										{
											Id = reader.GetInt32(6),
											StreetAddress = reader.GetString(7),
											City = reader.GetString(8),
											State = reader.GetString(9),
											PostalCode = reader.GetString(10),
										}
									});
							}
							return people;
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public Person GetById(int id)
		{
			Person dbRecord = _personRepository.Table
					 .FirstOrDefault(book => book.Id == id);
			if (dbRecord == null)
			{
				return null;
			}
			return dbRecord;
		}
		public bool Update(Person person)
		{
			try
			{
				_personRepository.Table.Update(person);
				_personRepository.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public List<Person> GetAll()
		{
			if (CheckDatabaseExists("EFCorePeopleDB"))
			{
				var books = _personRepository.Table.ToList();
				if (books.Count == 0)
				{
					if (CheckIfJsonFileExistsAndNotEmpty(Constants.FILE_PATH))
					{
						var bookes = Deserialize();
						if (bookes.Count != 0) return Deserialize();
						else return null;
					}
					else return null;
				}
				else return GetAllFromDb();
			}
			else if (CheckIfJsonFileExistsAndNotEmpty(Constants.FILE_PATH))
			{
				var books = Deserialize();
				if (books.Count != 0)
				{
					return Deserialize();
				}
				else return null;
			}
			else return null;
		}
		private bool CheckDatabaseExists(string dataBase)
		{
			string cmdText = String.Format("SELECT * FROM sys.databases where Name='{0}'", dataBase);
			bool isExist = false;
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING))
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(cmdText, connection))
				{
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						isExist = reader.HasRows;
					}
				}
				connection.Close();
			}
			return isExist;
		}
		private bool CheckIfJsonFileExistsAndNotEmpty(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return false;
			}
			string fileContents = File.ReadAllText(filePath);
			if (string.IsNullOrWhiteSpace(fileContents))
			{
				return false;
			}
			try
			{
				var jsonObject = JsonConvert.DeserializeObject(fileContents);
				return true;
			}
			catch (JsonReaderException)
			{
				return false;
			}
		}
		public List<Person> Deserialize()
		{
			List<Person> people = new List<Person>();
			string json = File.ReadAllText("People.json");
			people = JsonConvert.DeserializeObject<List<Person>>(json);
			return people;
		}
		public void Serialize(List<Person> people)
		{
			string json = JsonConvert.SerializeObject(people);
			File.WriteAllText("People.json", json);
		}
	}
}
