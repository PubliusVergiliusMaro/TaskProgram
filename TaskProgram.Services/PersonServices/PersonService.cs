using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using TaskProgram.Common;
using TaskProgram.Database.Configurations;
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

			if (CheckDatabaseExists(Constants.DATABASE_NAME))
			{
				try
				{
					using (var connection = new SqlConnection(DbConfiguration.CONNECTION_STRING))
					{
						connection.Open();
						using (var command = new SqlCommand($@"INSERT INTO People (FirstName, LastName, Gender, Age, PhoneNumber) 
                VALUES ('{person.FirstName}', '{person.LastName}', '{person.Gender}', {person.Age}, '{person.PhoneNumber}')", connection))
						{
							var result = command.ExecuteNonQuery();
							if (result > 0)
							{
								command.CommandText = "SELECT SCOPE_IDENTITY()";
								int newId = Convert.ToInt32(command.ExecuteScalar());
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
			else
			{
				try
				{
					List<Person> personList = Deserialize();
					personList.Add(person);
					Serialize(personList);

				}
				catch (Exception ex)
				{
					return;
				}
			}

		}
		public bool Delete(int id)
		{
			if (CheckDatabaseExists(Constants.DATABASE_NAME))
			{
				try
				{
					using (var connection = new SqlConnection(DbConfiguration.CONNECTION_STRING))
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
			else
			{
				try
				{
					List<Person> personList = Deserialize();
					personList.Remove(personList.Where(person => person.Id == id).FirstOrDefault());
					Serialize(personList);
					return true;
				}
				catch (Exception ex)
				{
					return false;
				}
			}
		}
		public List<Person> GetAllFromDb()
		{
			try
			{
				using (var connection = new SqlConnection(DbConfiguration.CONNECTION_STRING))
				{
					connection.Open();
					using (var command = new SqlCommand(@"SELECT 
People.Id,
People.FirstName,
People.LastName,
People.Gender,
People.Age,
People.PhoneNumber,
Addresses.Id,
Addresses.StreetAddress,
Addresses.City,
Addresses.State,
Addresses.PostalCode
FROM People
INNER JOIN Addresses ON People.Id = Addresses.Person_FK;", connection))
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
										Age = reader.GetInt32(4),
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
			if (CheckDatabaseExists(Constants.DATABASE_NAME))
			{
				return GetAllFromDb();
			}
			else if (CheckIfJsonFileExistsAndNotEmpty("People.json"))
			{
				return Deserialize();
			}
			else return null;
		}
		private bool CheckDatabaseExists(string dataBase)
		{
			string cmdText = String.Format("SELECT * FROM sys.databases where Name='{0}'", dataBase);
			bool isExist = false;
			using (SqlConnection connection = new SqlConnection(DbConfiguration.CONNECTION_STRING))
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
			string json = JsonConvert.SerializeObject(people, Formatting.Indented, new JsonSerializerSettings
			{
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			});
			File.WriteAllText("People.json", json);
		}
	}
}
