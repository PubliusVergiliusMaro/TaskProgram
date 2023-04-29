using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
		private static ObservableCollection<PersonViewModel> _people;
		private readonly IPersonService _personService;
		private static int CurrentPerson;
		public static IEnumerable<PersonViewModel> People => _people;
		public AddPersonViewModel(IPersonService personService,
			INavigationService personListMavigationService)
		{
			_personListMavigationService = personListMavigationService;
			_personService = personService;
			_people = new ObservableCollection<PersonViewModel>();
			CurrentPerson = 0;

			List<Person> people = _personService.GetAllEF();
			if (people != null && people.Count != 0)
			{
				foreach (Person person in _personService.GetAllEF())
				{
					var personViewModel = new PersonViewModel(person);
					_people.Add(personViewModel);
				}
			}


			CancelCommand = new DelegateCommand(this.Cancel);
			SaveCommand = new DelegateCommand(Save, CanSave);
			LoadFromFileCommand = new DelegateCommand(LoadFromFile);
			NextPersonCommand = new DelegateCommand(NextCurrentPerson,CanGoNext);
			PreviousPersonCommand = new DelegateCommand(PreviousCurrentPerson,CanGoPrevious);
			UpdateCommand = new DelegateCommand(Update,CanUpdate);
			LoadCurrentPerson();
		}

		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }
		public ICommand LoadFromFileCommand { get; }
		public ICommand PreviousPersonCommand { get; }
		public ICommand NextPersonCommand { get; }
		public ICommand UpdateCommand { get; }


		public string personNumberInfo;
		public string PersonNumberInfo
		{
			get => personNumberInfo;
			set
			{
				personNumberInfo = value;
				OnPropertyChanged(nameof(PersonNumberInfo));
			}
		}
		/// Properties for Displaying and updating exists People
		private string firstName;
		public string FirstName
		{
			get => firstName;
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
		/// Properties for adding new Person
		private string newfirstName;
		public string NewFirstName
		{
			get => newfirstName;
			set
			{
				newfirstName = value;
				OnPropertyChanged(nameof(NewFirstName));
			}
		}
		private string newlastName;
		public string NewLastName
		{
			get { return newlastName; }
			set
			{
				newlastName = value;
				OnPropertyChanged(nameof(NewLastName));
			}
		}
		private string newgender;
		public string NewGender
		{
			get { return newgender; }
			set
			{
				newgender = value;
				OnPropertyChanged(nameof(NewGender));
			}
		}
		private int newage;
		public int NewAge
		{
			get { return newage; }
			set
			{
				newage = value;
				OnPropertyChanged(nameof(NewAge));
			}
		}
		private string newphoneNumber;
		public string NewPhoneNumber
		{
			get { return newphoneNumber; }
			set
			{
				newphoneNumber = value;
				OnPropertyChanged(nameof(NewPhoneNumber));
			}
		}
		private string newstreetAddress;
		public string NewStreetAddress
		{
			get { return newstreetAddress; }
			set
			{
				newstreetAddress = value;
				OnPropertyChanged(nameof(NewStreetAddress));
			}
		}
		private string newcity;
		public string NewCity
		{
			get { return newcity; }
			set
			{
				newcity = value;
				OnPropertyChanged(nameof(NewCity));
			}
		}
		private string newstate;
		public string NewState
		{
			get { return newstate; }
			set
			{
				newstate = value;
				OnPropertyChanged(nameof(NewState));
			}
		}
		private string newpostalCode;
		public string NewPostalCode
		{
			get { return newpostalCode; }
			set
			{
				newpostalCode = value;
				OnPropertyChanged(nameof(NewPostalCode));
			}
		}
		private void Cancel()
		{
			_personListMavigationService.Navigate();
		}
		private void Save()
		{
			var person = new Person(NewFirstName, NewLastName, NewGender, NewAge, NewPhoneNumber,
				new Address(NewStreetAddress, NewCity, NewState, NewPostalCode));
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
		private void Update()
		{
			try
			{
				var person = new Person(FirstName, LastName, Gender, Age, PhoneNumber,
					new Address(StreetAddress, City, State, PostalCode));
				if (_personService.UpdateEF(person))
				{
					MessageBox.Show("Successfully updated Person.", "Success",
					  MessageBoxButton.OK, MessageBoxImage.Information);
					Cancel();
				}
				else
				{
					MessageBox.Show("Error with update service");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}
		private void LoadCurrentPerson()
		{
			FirstName = _people[CurrentPerson].FirstName;
			LastName = _people[CurrentPerson].LastName;
			Gender = _people[CurrentPerson].Gender;
			Age = int.Parse(_people[CurrentPerson].Age);
			PhoneNumber = _people[CurrentPerson].PhoneNumber;
			StreetAddress = _people[CurrentPerson].StreetAddress;
			City = _people[CurrentPerson].City;
			State = _people[CurrentPerson].State;
			PostalCode = _people[CurrentPerson].PostalCode;

			PersonNumberInfo = $"{CurrentPerson+1}/{_people.Count}";
		}
		private void LoadFromFile()
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Multiselect = false;
			fileDialog.Filter = "Json files (*.json)|*.json";
			fileDialog.ShowDialog();
			if (!fileDialog.CheckFileExists || string.IsNullOrEmpty(fileDialog.FileName))
			{
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
		private void PreviousCurrentPerson()
		{
			CurrentPerson--;
			LoadCurrentPerson();
		}

		private void NextCurrentPerson()
		{
			CurrentPerson++;
			LoadCurrentPerson();
		}
		private bool CanGoPrevious()
		{
			return CurrentPerson != 0;
		}

		private bool CanGoNext()
		{
			return CurrentPerson != _people.Count - 1;
		}
		private bool CanSave()
		{
			return !string.IsNullOrEmpty(this.NewFirstName)
				&& !string.IsNullOrEmpty(this.NewLastName)
				&& !string.IsNullOrEmpty(this.NewGender)
				&& !string.IsNullOrEmpty(this.NewPhoneNumber)
				&& !string.IsNullOrEmpty(this.NewStreetAddress)
				&& !string.IsNullOrEmpty(this.NewCity)
				&& !string.IsNullOrEmpty(this.NewState)
				&& !string.IsNullOrEmpty(this.NewPostalCode)
				&& (EfIsChecked || AdoIsChecked || FileIsChecked);
		}
		private bool CanUpdate()
		{
			return !string.IsNullOrEmpty(this.FirstName)
				&& !string.IsNullOrEmpty(this.LastName)
				&& !string.IsNullOrEmpty(this.Gender)
				&& !string.IsNullOrEmpty(this.PhoneNumber)
				&& !string.IsNullOrEmpty(this.StreetAddress)
				&& !string.IsNullOrEmpty(this.City)
				&& !string.IsNullOrEmpty(this.State)
				&& !string.IsNullOrEmpty(this.PostalCode);
		}
	}
}
