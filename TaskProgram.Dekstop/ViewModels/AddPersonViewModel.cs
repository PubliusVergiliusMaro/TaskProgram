using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using TaskProgram.Common;
using TaskProgram.Database.Models;
using TaskProgram.Dekstop.Commands;
using TaskProgram.Dekstop.Services.NavigationServices;
using TaskProgram.Services.PersonServices;

namespace TaskProgram.Dekstop.ViewModels
{
	public class AddPersonViewModel : ViewModelBase
	{
		private readonly INavigationService _personListMavigationService;
		private readonly IPersonService _personService;

		public AddPersonViewModel(IPersonService personService,
			INavigationService personListMavigationService)
		{
			_personListMavigationService = personListMavigationService;
			_personService = personService;
			CancelCommand = new DelegateCommand(this.Cancel);
			SaveCommand = new DelegateCommand(Save, CanSave);
				
			LoadFromFileCommand = new DelegateCommand(LoadFromFile);

		}
		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }
		public ICommand LoadFromFileCommand { get; }
		private string firstName;
		public string FirstName
		{
			get { return firstName; }
			set
			{
				firstName = value;
				OnPropertyChanged(nameof(FirstName));
			}
		}
		private string lastName;
		public string LastName
		{
			get { return lastName; }
			set
			{
				lastName = value;
				OnPropertyChanged(nameof(LastName));
			}
		}
		private string gender;
		public string Gender
		{
			get { return gender; }
			set
			{
				gender = value;
				OnPropertyChanged(nameof(Gender));
			}
		}
		private int age;
		public int Age
		{
			get { return age; }
			set
			{
				age = value;
				OnPropertyChanged(nameof(Age));
			}
		}
		private string phoneNumber;
		public string PhoneNumber
		{
			get { return phoneNumber; }
			set
			{
				phoneNumber = value;
				OnPropertyChanged(nameof(PhoneNumber));
			}
		}
		private string streetAddress;
		public string StreetAddress
		{
			get { return streetAddress; }
			set
			{
				streetAddress = value;
				OnPropertyChanged(nameof(StreetAddress));
			}
		}
		private string city;
		public string City
		{
			get { return city; }
			set
			{
				city = value;
				OnPropertyChanged(nameof(City));
			}
		}
		private string state;
		public string State
		{
			get { return state; }
			set
			{
				state = value;
				OnPropertyChanged(nameof(State));
			}
		}
		private string postalCode;
		public string PostalCode
		{
			get { return postalCode; }
			set
			{
				postalCode = value;
				OnPropertyChanged(nameof(PostalCode));
			}
		}
		private bool efIsChekced;
		public bool EfIsChecked
		{
			get => efIsChekced;
			set
			{ 
				efIsChekced = value;
				OnPropertyChanged(nameof(EfIsChecked));
			}
		}
		private bool adoIsChekced;
		public bool AdoIsChecked
		{
			get => adoIsChekced;
			set
			{ 
				adoIsChekced = value;
				OnPropertyChanged(nameof(AdoIsChecked));
			}
		}
		private bool fileIsChekced;
		public bool FileIsChecked
		{
			get => fileIsChekced;
			set
			{
				fileIsChekced = value;
				OnPropertyChanged(nameof(FileIsChecked));
			}
		}
		private void Cancel()
		{
			_personListMavigationService.Navigate();
		}
		private void Save()
		{
			var person = new Person(FirstName, LastName, Gender, Age, PhoneNumber,
				new Address(StreetAddress, City, State, PostalCode));
			try
			{
				if (FileIsChecked)
				{
					List<Person> people = _personService.Deserialize(Constants.FILE_PATH);
					if(people != null)
					{
						people.Add(person);
						_personService.Serialize(people,Constants.FILE_PATH);
					}
					else
					{
                      _personService.Serialize(new List<Person>() { person},Constants.FILE_PATH);
					}
				}
				else if(AdoIsChecked) 
				{ 
					_personService.CreateADO(person);
				}
				else if(EfIsChecked)
				{
					_personService.CreateEF(person);
				}
			
				MessageBox.Show("Successfully create new Person.", "Success",
				  MessageBoxButton.OK, MessageBoxImage.Information);
				Cancel();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		//private void SaveToFile()
		//{
		//	var person = new Person(FirstName, LastName, Gender, Age, PhoneNumber,
		//		new Address(StreetAddress, City, State, PostalCode));
		//	try
		//	{
		//		List<Person> allPeople = _personService.Deserialize(Constants.FILE_PATH);
		//		if(allPeople != null)
		//		{
		//	       	allPeople.Add(person);
		//		   _personService.Serialize(allPeople,Constants.FILE_PATH);
		//		}
		//		else
		//		{
		//			_personService.Serialize(new List<Person> {person},Constants.FILE_PATH);
		//		}
		//		MessageBox.Show("Successfully create new Person.", "Success",
		//	    MessageBoxButton.OK, MessageBoxImage.Information);
		//    	Cancel();
		//	}
		//	catch(Exception ex)
		//	{
		//		MessageBox.Show(ex.Message);
		//	}
		
		//}

		private void LoadFromFile()
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Multiselect = false;
			fileDialog.Filter = "Json files (*.json)|*.json";
			fileDialog.ShowDialog();
			if (!fileDialog.CheckFileExists || string.IsNullOrEmpty(fileDialog.FileName))
			{
				//MessageBox.Show("File is not found or empty");
				return;
			}
			try
			{
		        List<Person> people = _personService.Deserialize(fileDialog.FileName);
				foreach(Person personItem in people)
				{
					_personService.CreateADO(personItem);
				}
				MessageBox.Show("Successfully add Person to database.", "Success",
				MessageBoxButton.OK, MessageBoxImage.Information);
				Cancel();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}
		private bool CanSave()
		{
			return !string.IsNullOrEmpty(this.FirstName)
				&& !string.IsNullOrEmpty(this.LastName)
				&& !string.IsNullOrEmpty(this.Gender)
				&& !string.IsNullOrEmpty(this.PhoneNumber)
				&& !string.IsNullOrEmpty(this.StreetAddress)
				&& !string.IsNullOrEmpty(this.City)
				&& !string.IsNullOrEmpty(this.State)
				&& !string.IsNullOrEmpty(this.PostalCode)
				&& (EfIsChecked || AdoIsChecked || FileIsChecked);
		}
	}
}
