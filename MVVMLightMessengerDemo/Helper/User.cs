using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVVMLightMessengerDemo.Model
{

    public partial class User : INotifyPropertyChanged, IDataErrorInfo
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get { return GetErrorForProperty(columnName); }
        }

        private string GetErrorForProperty(string propertyName)
        {
            string strResult = String.Empty;
            switch (propertyName)
            {
                case "UserName":
                    if (string.IsNullOrEmpty(UserName))
                        strResult = "Enter User Name";
                    //else
                    //{
                    //    if (UserName.Length > 7)
                    //        strResult = "Name cannot be more than 7 characters long.";
                    //}
                    break;

                case "LastName":
                    if (string.IsNullOrEmpty(LastName))
                        strResult = "Enter Last Name";
                    else
                    {
                        if (UserName.Length > 7)
                            strResult = "Name cannot be more than 7 characters long.";
                    }
                    break;

                case "email":
                   Regex regxEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    if (string.IsNullOrEmpty(email))
                        strResult = "Email can't be blank";
                    else
                    {
                        if (regxEmail.Match(email) == Match.Empty)
                            strResult = "Invalid Email format";
                    }
                    break;
                case "Address":
                    if (string.IsNullOrEmpty(Address))
                        strResult = "Address can't be blank";
                    break;
                case "Country":
                    if (string.IsNullOrEmpty(Country))
                        strResult = "Select Country";
                    break;
                case "City":
                    if (string.IsNullOrEmpty(City))
                        strResult = "Select City";
                    break;

            }
            return strResult;
        }
    }
}
