using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaskProgram.Common;
using TaskProgram.Database.Configurations;
using TaskProgram.Database.Models;
using TaskProgram.Database.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Runtime.ExceptionServices;
using System.Reflection;

namespace TaskProgram.Services.PersonServices
{
	public class PersonService : IPersonService
	{
		private readonly IGenericRepository<Person> _personRepository;

		public PersonService(IGenericRepository<Person> personRepository)
		{
			_personRepository = personRepository;
		}
		public void CreateADO(Person person)
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
									command.CommandText = $@"INSERT INTO Address (StreetAddress, City, State, PostalCode, Person_FK) 
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
		public void CreateEF(Person person)
		{
			_personRepository.Create(person);

		}
		public bool DeleteADO(int id)
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
		public bool DeleteEF(int id)
		{
			Person dbRecord = _personRepository.Table
				.FirstOrDefault(p => p.Id == id);
			if (dbRecord == null)
			{
				return false;
			}
			_personRepository.Remove(dbRecord);
			return true;
		}
		public List<Person> GetAllADO()
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
Address.Id,
Address.StreetAddress,
Address.City,
Address.State,
Address.PostalCode
FROM People
INNER JOIN Address ON People.Id = Address.Person_FK;", connection))
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
		public List<Person> GetAllEF()
		{
			List<Person> dbRecord = _personRepository.Table
				.Include(p => p.Address)
				.ToList();
			if(dbRecord == null)
			{
				return null;
			}
			return dbRecord;
		}
		public Person GetByIdADO(int id)
		{
			try
			{
				using (var connection = new SqlConnection(DbConfiguration.CONNECTION_STRING))
				{
					connection.Open();
					using (var command = new SqlCommand($@"SELECT 
People.Id,
People.FirstName,
People.LastName,
People.Gender,
People.Age,
People.PhoneNumber,
Address.Id,
Address.StreetAddress,
Address.City,
Address.State,
Address.PostalCode
FROM People
INNER JOIN Address ON People.Id = Address.Person_FK
Where People.Id = {id};", connection))
					{
						using (var reader = command.ExecuteReader())
						{
							Person person = new Person
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
										}};
							 return person;
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public Person GetByIdEF(int id)
		{
			Person dbRecord = _personRepository.Table
					 .FirstOrDefault(book => book.Id == id);
			if (dbRecord == null)
			{
				return null;
			}
			return dbRecord;
		}
		public bool UpdateADO(Person newPerson)
		{
			try
			{
				using (var connection = new SqlConnection(DbConfiguration.CONNECTION_STRING))
				{
					connection.Open();
					using (var command = new SqlCommand($@"
                UPDATE People
                SET
                    FirstName = @firstName,
                    LastName = @lastName,
                    Gender = @gender,
                    Age = @age,
                    PhoneNumber = @phoneNumber
                WHERE Id = @id;
                UPDATE Address
                SET
                    StreetAddress = @streetAddress,
                    City = @city,
                    State = @state,
                    PostalCode = @postalCode
                WHERE Person_FK = @id;
            ", connection))
					{
						command.Parameters.AddWithValue("@id", newPerson.Id);
						command.Parameters.AddWithValue("@firstName", newPerson.FirstName);
						command.Parameters.AddWithValue("@lastName", newPerson.LastName);
						command.Parameters.AddWithValue("@gender", newPerson.Gender);
						command.Parameters.AddWithValue("@age", newPerson.Age);
						command.Parameters.AddWithValue("@phoneNumber", newPerson.PhoneNumber);
						command.Parameters.AddWithValue("@streetAddress", newPerson.Address.StreetAddress);
						command.Parameters.AddWithValue("@city", newPerson.Address.City);
						command.Parameters.AddWithValue("@state", newPerson.Address.State);
						command.Parameters.AddWithValue("@postalCode", newPerson.Address.PostalCode);
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
		public bool UpdateEF(Person person)
		{
			try
			{
				Person dbRecord = _personRepository.Table.Where(p => p.Id == person.Id)
					.Include(p => p.Address)
					.FirstOrDefault();
				if(dbRecord != null)
				{
					dbRecord.FirstName = person.FirstName;
					dbRecord.LastName = person.LastName;
					dbRecord.Gender = person.Gender;
					dbRecord.Age = person.Age;
					dbRecord.PhoneNumber = person.PhoneNumber;
					dbRecord.Address.StreetAddress = person.Address.StreetAddress;
					dbRecord.Address.City = person.Address.City;
					dbRecord.Address.State = person.Address.State;
					dbRecord.Address.PostalCode = person.Address.PostalCode;

					_personRepository.SaveChanges();
                  
				} 
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
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

		public List<Person> Deserialize(string path)
		{
			List<Person> people = new List<Person>();
			string json = File.ReadAllText(path);
			people = JsonConvert.DeserializeObject<List<Person>>(json);
			return people;
		}
		public void Serialize(List<Person> people,string path)
		{
			string json = JsonConvert.SerializeObject(people, Formatting.Indented, new JsonSerializerSettings
			{
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			});
			File.WriteAllText(path, json);
		}
	}
}
