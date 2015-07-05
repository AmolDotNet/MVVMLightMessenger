using System.Windows;
using MVVMLightMessengerDemo.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using MVVMLightMessengerDemo.Helper;
using MVVMLightMessengerDemo.View;

namespace MVVMLightMessengerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<GoToMessagePage>(this, (action) => ReceiveMessage(action));
        }

        private object ReceiveMessage(GoToMessagePage action)
        {
            switch (action.PageName)
            {
                case "EditView":
                    this.contentView2.Content = this.EditView;
                    break;
                case "HomeView":
                    this.contentView1.Content = this.HomeView;
                    this.contentView2.Content = string.Empty;
                    break;
                case "NewView":
                    this.contentView2.Content = this.EditView;
                    break;
                default:
                    break;
            }
            return null;
        }


        #region Public Property

        private EditView _editView;
        public EditView EditView
        {
            get
            {
                if (_editView == null)
                    _editView = new EditView();
                return _editView;
            }
        }

        private HomeView _homeView;
        public HomeView HomeView
        {
            get
            {
                if (_homeView == null)
                    _homeView = new HomeView();
                return _homeView;
            }
        }

        #endregion

    }
}