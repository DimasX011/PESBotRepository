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
using System.Windows.Shapes;
using TelegramBotApi;
using TelegramBotApi.Entityes;
using TelegramBotApi.Interfaces;
using TelegramBotApi.Services;
using Serilog.Events;

namespace TgBotForm
{
    /// <summary>
    /// Логика взаимодействия для WindowRegistration.xaml
    /// </summary>
    public partial class WindowRegistration : Window
    {
        private static UnRegisteredUser currentUser = new();
        private static RegisteredUser _registeredUser = new RegisteredUser();
        private static Serilog.ILogger _logger;
        private static ISetToDataBase _dataBase = new WriteToDataBase(_logger);
        private static UserDatas _currentuserDatas = new UserDatas();

        

        public WindowRegistration(UnRegisteredUser unRegisteredUser, Serilog.ILogger logger)
        {

            _logger = logger; 
            _dataBase = new WriteToDataBase(_logger);
            InitializeComponent();

            bool admin = false;
            if (unRegisteredUser.isAdmin == 1)
            {
                admin = true;
            }
            if(_dataBase.IsRegister(unRegisteredUser.NumberPhone))
            {
                _registeredUser = _dataBase.GetRegisteredUser(unRegisteredUser.NumberPhone);
                string inn = _registeredUser.UserINN;
                _currentuserDatas = _dataBase.GetUserDatas(inn);
                UserLogin_Server.Text = _currentuserDatas.LoginServer;
                UserPassword_Server.Text = Decrypter.DecryptValue(_currentuserDatas.PasswordServer);
                UserLogin_Yandex.Text = _currentuserDatas.LoginYandex;
                UserPassword_Yandex.Text = Decrypter.DecryptValue(_currentuserDatas.PasswordYandex);
                UserLogin_Bitrix.Text = _currentuserDatas.LoginBitrix;
                UserPassword_Bitrix.Text = Decrypter.DecryptValue(_currentuserDatas.PasswordBitrix);
                UserLogin_Mail.Text = _currentuserDatas.LoginMail;
                UserPassword_Mail.Text = Decrypter.DecryptValue(_currentuserDatas.PasswordMail);
                UserINN.Text = _currentuserDatas.UserINN;
            }
            currentUser = unRegisteredUser;
            isAdmin.IsChecked = admin; 
          

            UserFirstName.Text = UserFirstName.Text + ": " + currentUser.FirstName;

            UserLastName.Text = UserLastName.Text + ": " + currentUser.LastName;

            UserPhone.Text = UserPhone.Text + ": " + currentUser.NumberPhone;

            TelegramId.Text = TelegramId.Text + ": " + currentUser.TelegramId;
        }

       

        private void Registration(object sender, RoutedEventArgs e)
        {
            bool answerTodataBase;

            string passwordserver = "";
            string passwordyandex = "";
            string passwordmail = "";
            string passwordbitrix = "";
            bool admin = isAdmin.IsChecked?? false;
            if(UserPassword_Server.Text != "")
            {
                var resultpasswordserver = Decrypter.CreatePasswordHash(UserPassword_Server.Text);
                passwordserver = $"{resultpasswordserver.passwordEncrypted}~{resultpasswordserver.passwordSalt}";
            }

            if(UserPassword_Yandex.Text != "")
            {
                var resultpasswordyandex = Decrypter.CreatePasswordHash(UserLogin_Yandex.Text);
                passwordyandex = $"{resultpasswordyandex.passwordEncrypted}~{resultpasswordyandex.passwordSalt}";

            }

            if(UserPassword_Mail.Text != "")
            {
                var resultpasswordbitrix = Decrypter.CreatePasswordHash(UserPassword_Bitrix.Text);
                passwordbitrix = $"{resultpasswordbitrix.passwordEncrypted}~{resultpasswordbitrix.passwordSalt}";
            }

            if(UserPassword_Mail.Text != null)
            {
                var resultpasswordmail = Decrypter.CreatePasswordHash(UserPassword_Mail.Text);
                passwordmail = $"{resultpasswordmail.passwordEncrypted}~{resultpasswordmail.passwordSalt}";

            }
                
                answerTodataBase = _dataBase.SetLoginPasswordForUser(currentUser, UserINN.Text, UserLogin_Server.Text, passwordserver ,UserLogin_Yandex.Text, passwordyandex ,UserLogin_Bitrix.Text, passwordbitrix, UserLogin_Mail.Text, passwordmail, admin);

                MainWindow mainWindow = this.Owner as MainWindow;

                if (answerTodataBase)
                {
                    MessageBox.Show("Пользователь успешно зарегистрирован!", "Регистрация пользователя", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                    mainWindow.UpdateDataUserRegister_Click(null, null);
                    mainWindow.UpdateDataUser_Click(null, null);

                }
                else
                {
                    MessageBox.Show("Произошла ошибка, обращайтесь к Диме, пусть разбирается!", "Регистрация пользователя", MessageBoxButton.OK, MessageBoxImage.Error);
                    mainWindow.UpdateDataUserRegister_Click(null, null);
                    mainWindow.UpdateDataUser_Click(null, null);
                }
            
           
        }
    }
}
