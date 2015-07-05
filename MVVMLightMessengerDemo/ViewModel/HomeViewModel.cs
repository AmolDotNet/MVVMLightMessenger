using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMLightMessengerDemo.Helper;
using MVVMLightMessengerDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace MVVMLightMessengerDemo.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {

        Amol_LearningEntities context;
        private string _UserName;

        #region Public Proprty Member vars

        public RelayCommand<User> CreateCommand { get; set; }
        public RelayCommand<User> DeleteCommand { get; set; }
        public RelayCommand<User> EditCommand { get; set; }
        public RelayCommand<string> NewCommand { get; set; }
        public RelayCommand<User> SendUserCommand { get; set; }
        public RelayCommand<object> GetDetailsCommand { get; set; }

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                if (value != null)
                {
                    _UserName = value;
                    SearchEmployee(value);
                    RaisePropertyChanged("UserName");
                }
            }
        }

        private ObservableCollection<User> _user;
        public ObservableCollection<User> Users
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("Users");
            }
        }

        private ObservableCollection<string> _mCountry;
        public ObservableCollection<string> MCountry
        {
            get { return _mCountry; }
            set { _mCountry = value; RaisePropertyChanged("MCountry"); }
        }

        private ObservableCollection<string> _mCity;
        public ObservableCollection<string> MCity
        {
            get { return _mCity; }
            set
            {
                _mCity = value;
                RaisePropertyChanged("MCity");
            }
        }

        private User _userInfo;
        [Required]
        public User UserInfo
        {
            get { return _userInfo; }
            set
            {
                _userInfo = value;
                RaisePropertyChanged("UserInfo");
            }
        }

        private string _selectedCountry;
        [Required(ErrorMessage = "Field 'FirstName' is required.")]
        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                GetCity(value.ToString());
                UserInfo.Country = value.ToString();
                RaisePropertyChanged("SelectedCountry");
            }
        }

        #endregion

        public HomeViewModel()
        {
            try
            {
                if (Users == null)
                    Users = new ObservableCollection<User>();
                if (UserInfo == null)
                    UserInfo = new User();
                MCountry = new ObservableCollection<string>(
                                from c in context.Users
                                group c by c.Country into g
                                select g.Key);

                context = new Amol_LearningEntities();
                CreateCommand = new RelayCommand<User>(CreateUser, CreateCanExecute);
                DeleteCommand = new RelayCommand<User>(DeleteUser);
                EditCommand = new RelayCommand<User>(param => EditUser(param));
                NewCommand = new RelayCommand<string>(param => NewUser(param));
                ReceiveUserInfo();
                RefreshData();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Private Member Vars

        private void NewUser(string obj)
        {
            User userObj = new User();
            userObj.Id = Convert.ToInt32(context.Users.Select(x => x.Id).Max()) + 1;
            var msg = new GoToMessagePage() { UserObj = userObj, PageName = "NewView", ButtonContentMsg = obj };
            Messenger.Default.Send<GoToMessagePage>(msg);
        }

        bool CreateCanExecute(User p_objUser)
        {
            Regex regxEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            bool isUserName = String.IsNullOrEmpty(p_objUser.UserName) ? false : true;
            bool isCountry = String.IsNullOrEmpty(SelectedCountry) ? false : true;
            bool isCity = String.IsNullOrEmpty(p_objUser.City) ? false : true;
            bool isAddress = String.IsNullOrEmpty(p_objUser.Address) ? false : true;
            bool isEmail = String.IsNullOrEmpty(p_objUser.email) ? false : (regxEmail.Match(p_objUser.email) == Match.Empty) ? false : true;

            if (isUserName && isCountry && isCity && isEmail && isAddress)
            {
                return true;
            }
            return false;
        }

        private void CreateUser(User obj)
        {
            try
            {
                obj.LastName = "Test";
                obj.Password = "password";

                context.Users.Add(obj);
                context.SaveChanges();
                RefreshData();
                MessageBox.Show("New User Created Sucessfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteUser(User param)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                context.Users.Remove(param);
                context.SaveChanges();
                RefreshData();
            }
        }

        private void EditUser(User userObj)
        {
            if (userObj != null)
            {
                var msg = new GoToMessagePage() { UserObj = userObj, PageName = "NewView", ButtonContentMsg = "Update" };
                Messenger.Default.Send<GoToMessagePage>(msg);
            }
        }

        void GetCity(string selectedCountry)
        {
            MCity = new ObservableCollection<string>(
                                from c in context.Users
                                where c.Country == selectedCountry
                                group c by c.City into g
                                select g.Key
                );
        }

        private void ReceiveUserInfo()
        {
            Messenger.Default.Register<GoToMessagePage>(this, (msg) =>
            {
                RefreshData();
            });
        }

        void RefreshData()
        {
            Users.Clear();
            foreach (User e in context.Users)
            {
                Users.Add(e);
            }
        }

        private void SearchEmployee(string param)
        {
            ObservableCollection<User> res;
            Users.Clear();
            res = new ObservableCollection<User>((from u in context.Users
                                                  where u.UserName.StartsWith(param)
                                                  select u).ToList());
            foreach (User item in res)
            {
                Users.Add(item);
            }
        }

        #endregion

        private void RaisePropertyChanged(string proertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(proertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
