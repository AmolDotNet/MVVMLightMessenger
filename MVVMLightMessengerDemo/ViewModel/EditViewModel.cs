using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMLightMessengerDemo.Helper;
using MVVMLightMessengerDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMLightMessengerDemo.ViewModel
{
    public class EditViewModel : INotifyPropertyChanged
    {
        Amol_LearningEntities context = new Amol_LearningEntities();
        User _userInfo;

        #region Public Property Members

        public string _selectedCountry;
        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (value != null)
                {
                    _selectedCountry = value;
                    GetCity(SelectedCountry.ToString());
                    RaisePropertyChanged("SelectedCountry");
                }
            }
        }
        public RelayCommand<User> SendUserCommand { get; set; }
        public RelayCommand<User> DeleteCommand { get; set; }
        public RelayCommand<User> UpdateCommand { get; set; }
        public RelayCommand<string> CancelCommand { get; set; }

        private string _buttonContent;
        public string ButtonContent
        {
            get { return _buttonContent; }
            set
            {
                _buttonContent = value;
                RaisePropertyChanged("ButtonContent");
            }
        }

        private User _userGender;
        public User UserGender
        {
            get { return _userGender; }
            set
            {
                _userGender = value;
                RaisePropertyChanged("UserGender");
            }
        }
        public User UserInfo
        {
            get { return _userInfo; }
            set
            {
                if (value != null)
                {
                    _userInfo = value;
                    SetGender(value);
                    RaisePropertyChanged("UserInfo");
                }

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

        #endregion

        public EditViewModel()
        {
            try
            {
                if (UserInfo == null)
                    UserInfo = new User();
                SelectedCountry = string.Empty;
                MCountry = new ObservableCollection<string>(
                    from c in context.Users
                    group c by c.Country into g
                    select g.Key);

                UpdateCommand = new RelayCommand<User>(UpdateUser, UpdateCanExecute);
                CancelCommand = new RelayCommand<string>(CancelUpdating);
                ReceiveUserInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region private Member Vars

        private void CancelUpdating(string obj)
        {
            var msg = new GoToMessagePage() { PageName = "HomeView", ButtonContentMsg = obj };
            SelectedCountry = string.Empty;
            UserInfo = new User();
            Messenger.Default.Send<GoToMessagePage>(msg);
        }

        private void GetCity(string selectedCountry)
        {
            UserInfo.Country = selectedCountry;
            MCity = new ObservableCollection<string>(
                                from c in context.Users
                                where c.Country == selectedCountry
                                group c by c.City into g
                                select g.Key
                );
        }

        private void ReceiveUserInfo()
        {
            if (UserInfo == null)
                UserInfo = new User();
            Messenger.Default.Register<GoToMessagePage>(this, (msg) =>
            {
                if (msg.UserObj != null)
                    AssignUserData(msg);
            });

        }

        private void AssignUserData(GoToMessagePage msg)
        {
            this.ButtonContent = msg.ButtonContentMsg;
            User userObj = msg.UserObj;
            User userData = new User();
            userData.Id = userObj.Id;
            userData.UserName = userObj.UserName;
            userData.LastName = userObj.LastName;
            userData.City = userObj.City;
            userData.Country = userObj.Country;
            userData.Address = userObj.Address;
            userData.email = userObj.email;
            userData.Gender = userObj.Gender;
            this.UserInfo = userData;
            if (userObj.Country != null)
            {
                SelectedCountry = userObj.Country;
            }
        }

        private void SetGender(User value)
        {
            UserGender = new User();
            UserGender.Gender = (value.Gender) ? false : true;
        }

        bool UpdateCanExecute(User p_objUser)
        {
            Regex regxEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            bool isUserName = String.IsNullOrEmpty(p_objUser.UserName) ? false : true;
            bool isCountry = String.IsNullOrEmpty(SelectedCountry) ? false : true;
            bool isCity = String.IsNullOrEmpty(p_objUser.City) ? false : true;
            bool isAddress = String.IsNullOrEmpty(p_objUser.Address) ? false : true;
            bool isEmail = String.IsNullOrEmpty(p_objUser.email) ? false : (regxEmail.Match(p_objUser.email) == Match.Empty) ? false : true;

            if (isUserName && isCountry && isCity && isAddress && isEmail)
            {
                return true;
            }
            return false;
        }

        private void UpdateUser(User obj)
        {
            try
            {
                if (ButtonContent == "Update")
                {
                    User u = (from c in context.Users
                              where c.Id == obj.Id
                              select c).FirstOrDefault();

                    u.LastName = obj.LastName;
                    u.UserName = obj.UserName;
                    u.Country = obj.Country;
                    u.City = obj.City;
                    u.Address = obj.Address;
                    u.email = obj.email;
                    u.Gender = obj.Gender;
                    context.SaveChanges();

                    var msg = new GoToMessagePage() { PageName = "HomeView" };
                    Messenger.Default.Send<GoToMessagePage>(msg);
                    MessageBox.Show("Data Updated Sucessfully");
                }
                else if (ButtonContent == "Create")
                {
                    obj.LastName = "Test";
                    obj.Password = "password";
                    context.Users.Add(obj);
                    context.SaveChanges();

                    var msg = new GoToMessagePage() { PageName = "HomeView" };
                    Messenger.Default.Send<GoToMessagePage>(msg, "Home");
                    MessageBox.Show("New User Created Sucessfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion


        #region Property Region

        private void RaisePropertyChanged(string proertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(proertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
