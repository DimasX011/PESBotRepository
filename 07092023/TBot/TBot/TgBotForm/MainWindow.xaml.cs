using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot.Types;
using TelegramBotApi.Entityes;
using TelegramBotApi.Interfaces;
using TelegramBotApi.Services;
using Serilog.Events;

namespace TgBotForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Serilog.ILogger logger;
        private static ISetToDataBase _dataBase = new WriteToDataBase(logger);

        List<UnRegisteredUser> _unregistredUsers = new List<UnRegisteredUser>();
        List<UnRegisteredUser> _registredUsers = new List<UnRegisteredUser>();

        private static UnRegisteredUser currentUser = new();

        public MainWindow()
        {
            logger = Log.Logger;
            logger = new LoggerConfiguration()
            .WriteTo.Conditional(
                evt => evt.Level == LogEventLevel.Information,
                wt => wt.File("E:\\Development\\TBot\\TelegramBotApi\\Logs\\log_write_todb.txt", rollingInterval: RollingInterval.Day))
            .WriteTo.Conditional(
                evt => evt.Level == LogEventLevel.Debug,
                wt => wt.File("E:\\Development\\TBot\\TelegramBotApi\\Logs\\log-log_technical_write_todb.txt", rollingInterval: RollingInterval.Day))
            .WriteTo.Conditional(
                evt => evt.Level == LogEventLevel.Error,
                wt => wt.File("E:\\Development\\TBot\\TelegramBotApi\\Logs\\log-log_error_write_todb.txt", rollingInterval: RollingInterval.Day))
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

            

            _dataBase = new WriteToDataBase(logger);
            InitializeComponent();
        }

        public void UpdateDataUser_Click(object sender, RoutedEventArgs e)
        {
            _unregistredUsers.Clear();
            _unregistredUsers = _dataBase.GetAllUnregisteredUser();
            UsersAdd.Children.Clear();
            foreach (var user in _unregistredUsers)
            {
                Button button = new Button();
                button.MinHeight = 25;
                button.Content =String.Format( "Сотрудник {0} {1} телефон {2}",user.FirstName, user.LastName, user.NumberPhone);
                button.Click += (s, ev) => SetDataUser(s, ev, user);
                button.Background = Brushes.Black;
                button.Foreground = Brushes.White;
                button.BorderThickness = new Thickness(2);
                button.BorderBrush = Brushes.White;
                button.Padding = new Thickness(0);
                button.FontSize = 10;
                button.FontWeight = FontWeights.Bold;
                button.Margin = new Thickness(10);

                UsersAdd.Children.Add(button);
            }
        }

        public void UpdateDataUserRegister_Click(object sender, RoutedEventArgs e)
        {
            _registredUsers.Clear();
            _registredUsers = _dataBase.GetAllRegisteredUser();
            UsersRegistred.Children.Clear();
            foreach (var user in _registredUsers)
            {
                Button button = new Button();
                button.MinHeight = 25;
                button.Content = String.Format("Сотрудник {0} {1} телефон {2}", user.FirstName, user.LastName, user.NumberPhone);
                button.Background = Brushes.Black;
                button.Foreground = Brushes.White;
                button.BorderThickness = new Thickness(2);
                button.BorderBrush = Brushes.White;
                button.Padding = new Thickness(0);
                button.FontSize = 10;
                button.FontWeight = FontWeights.Bold;
                button.Margin = new Thickness(0);
                button.Click += (s, ev) => SetLoginsPassword(s, ev, user);
                UsersRegistred.Children.Add(button);
            }
        }

        private void SetDataUser(object sender, RoutedEventArgs e, UnRegisteredUser user)
        {
            if (MessageBox.Show("Зарегистрировать пользователя " + user.FirstName + " " + user.LastName + "?",
                    "Регистрация пользователя",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool ansertodatabase;
                ansertodatabase = _dataBase.RegistrationNewUserToDataBase(user);
                if (ansertodatabase)
                {
                    MessageBox.Show("Пользователь успешно зарегистрирован",
                    "Успешная регистрация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                   
                }
                else
                {
                    MessageBox.Show("Произошла ошибка",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                }
                bool deleteregostration;
                deleteregostration = _dataBase.DeleteUnregisteredUser(user);
                if (!deleteregostration)
                {
                    MessageBox.Show("Произошла ошибка у2",
                   "Ошибка",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }

                
            }
            UpdateDataUser_Click(null, null);
            UpdateDataUserRegister_Click(null, null);
        }

        public void SetLoginsPassword(object sender, RoutedEventArgs e, UnRegisteredUser user)
        {
            WindowRegistration windowRegistration = new WindowRegistration(user, logger);
            windowRegistration.Owner = this;
            windowRegistration.Show();
            UpdateDataUser_Click(null, null);
            UpdateDataUserRegister_Click(null, null);
        }


        private void TextBox_SearchLine_TextChanged_1(object sender, TextChangedEventArgs e)
        {

            List<UnRegisteredUser> lines = _registredUsers;
            string searchText = SortRegistered.Text.ToLower();
           
            List<UnRegisteredUser> sortedLines = _registredUsers.Where(line => line.FirstName.ToLower().Contains(searchText) || line.LastName.ToLower().Contains(searchText)).ToList();
            UsersRegistred.Children.Clear();
            foreach (var user in sortedLines)
            {
                Button button = new Button();
                button.MinHeight = 25;
                button.Content = String.Format("Сотрудник {0} {1} телефон {2}", user.FirstName, user.LastName, user.NumberPhone);
                button.Click += (s, ev) => SetDataUser(s, ev, user);
                button.Background = Brushes.Black;
                button.Foreground = Brushes.White;
                button.BorderThickness = new Thickness(2);
                button.BorderBrush = Brushes.White;
                button.Padding = new Thickness(0);
                button.FontSize = 10;
                button.FontWeight = FontWeights.Bold;
                button.Margin = new Thickness(0);
                UsersRegistred.Children.Add(button);
            }
         
        }
        
        private void TextBox_SearchLine_PreviewKeyDown_3(object sender, KeyEventArgs e)
        {

        }

        private void SortUnRegistered_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<UnRegisteredUser> lines = _unregistredUsers;
            string searchText = SortRegistered.Text.ToLower();

            List<UnRegisteredUser> sortedLines = _registredUsers.Where(line => line.FirstName.ToLower().Contains(searchText) || line.LastName.ToLower().Contains(searchText)).ToList();
            UsersAdd.Children.Clear();
            foreach (var user in sortedLines)
            {
                Button button = new Button();
                button.MinHeight = 25;
                button.Content = String.Format("Сотрудник {0} {1} телефон {2}", user.FirstName, user.LastName, user.NumberPhone);

                button.Background = Brushes.Black;
                button.Foreground = Brushes.White;
                button.BorderThickness = new Thickness(2);
                button.BorderBrush = Brushes.White;
                button.Padding = new Thickness(0);
                button.FontSize = 10;
                button.FontWeight = FontWeights.Bold;
                button.Margin = new Thickness(0);

                button.Click += (s, ev) => SetDataUser(s, ev, user);

                UsersAdd.Children.Add(button);
            }
        }

        private void SortUnRegistered_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
