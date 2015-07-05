using MVVMLightMessengerDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMLightMessengerDemo.Helper
{
    public class GoToMessagePage
    {
        public string PageName { get; set; }
        public string UpdateMsg { get; set; }
        public string ButtonContentMsg { get; set; }
        private User _userObj;

        public User UserObj
        {
            get { return _userObj; }
            set { _userObj = value; }
        }
        
    }
}
