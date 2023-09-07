using Microsoft.Data.SqlClient;
using Serilog;
using System.Diagnostics.Metrics;
using TelegramBotApi.Entityes;
using TelegramBotApi.Interfaces;
using TelegramBotData;


namespace TelegramBotApi.Services
{
        public class WriteToDataBase : ISetToDataBase
        {
        private string _connectionString;
        private static Serilog.ILogger _logger;
        public WriteToDataBase(Serilog.ILogger logger )
            {
                _connectionString = "data source=localhost\\SQLEXPRESS;initial catalog=TelegramBotData;User Id=TelegramBotUser;Password=1587panda;MultipleActiveResultSets=True;trustServerCertificate=true;App=EntityFramework";
                _logger = logger;
            }
        public void AddNewTaskToDatabase(string TaskYandex, string status, SqlConnection sqlConnection)
        {

            try
            {
                using (var sqlCommand = new SqlCommand(String.Format("  INSERT INTO [TelegramBotData].[dbo].[taskData] (TaskName,TaskStatus) VALUES ({0}, {1}) ", "'" + TaskYandex + "'", "'" + status + "'"), sqlConnection))
                {
                    var reader = sqlCommand.ExecuteReader();
                    _logger.Information("Добавлена новая задача {0} в статусе {1} " + DateTime.Now, TaskYandex, status);
                    
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message +" "+ DateTime.Now);

            }

        }
        public void AddPostToDataBase(string Post, string tgId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT COUNT(*) AS CountUser FROM [TelegramBotData].[dbo].[userData] WHERE TelegramId = {0}", ""), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            int countsql = Convert.ToInt32(reader["CountUser"].ToString());
                            if (countsql == 0)
                            {

                            }
                            _logger.Information("Пользователь с id {0} присутствует в базе данных" + DateTime.Now, "");
                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + " "+ DateTime.Now);
                }
            }
        }
        public bool CheckPostToDataBase(string Post, long TgId)
        {
            bool PostToDataBase = false;
            string postUser = "";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT [UserPost] FROM [TelegramBotData].[dbo].[userData] WHERE TelegramId = {0}", "'" + TgId.ToString()) + "'", sqlConnection))
                    {

                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            postUser = reader["UserPost"].ToString();
                        }
                        if (postUser != "")
                        {
                            _logger.Information("Пользователь с id {0} имеет почту в базе данных: {1}", TgId, Post);
                            PostToDataBase = true;
                        }
                        else
                        {
                            _logger.Information("Для пользователя с id {0} нет почты в базе данных", TgId, Post);
                            PostToDataBase = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + " " + DateTime.Now);
            }

            return PostToDataBase;
        }
        public string CheckTaskToDatabase(string TaskYandex, SqlConnection sqlConnection)
        {

            string countsql = "";
            try
            {
                using (var sqlCommand = new SqlCommand(String.Format("  SELECT COUNT([TaskName]) AS Countstring FROM [TelegramBotData].[dbo].[taskData] WHERE TaskName = {0} ", "'" + TaskYandex + "'"), sqlConnection))
                {
                    var reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        countsql = reader["Countstring"].ToString();
                        _logger.Information("Получена задача {0} из базы данных " + DateTime.Now, TaskYandex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message +" "+ DateTime.Now);
            }
            return countsql;

        }
        public bool FirstCallContact(long idContact)
        {
            bool addcontacttodatabase = false;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT COUNT(*) AS CountUser FROM [TelegramBotData].[dbo].[userData] WHERE TelegramId = {0}", idContact), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            int countsql = Convert.ToInt32(reader["CountUser"].ToString());
                            if (countsql == 0)
                            {
                                addcontacttodatabase = true;
                            }
                            _logger.Information("Пользователь с id {0} присутствует в базе данных " + DateTime.Now, idContact);
                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + DateTime.Now);
                }

            }
            return addcontacttodatabase;
        }
        public void SetNummerPhoneToDataBase(string username, string phone, long Tgid, string lastname, string firstname)   
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  INSERT INTO [TelegramBotData].[dbo].[userData] (NumberPhone, TelegramId, UserName, ExpDate, FirstName, LastName) VALUES ({0},{1},{2},{3},{4},{5}) ", "'" + phone + "'", "'" + Tgid + "'", "'" + username + "'", "'" + DateTime.Now.ToString() + "'", "'" + firstname + "'", "'" + lastname + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                    }
                    _logger.Information("Пользователь {0} с id {1} записан в базу данных", firstname, Tgid + " " + DateTime.Now);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + DateTime.Now);
                }
            }
        }
        public bool SetPostToDataBase(string Post, long TgId)
        {
            bool setsuccess;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  UPDATE  [TelegramBotData].[dbo].[usersDataEntry] SET LoginServer ={0} WHERE TgId = {1}", "'" + Post + "'", "'" + TgId.ToString()) + "'", sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        _logger.Information("Записана почта {0} для пользователя с id:{1}" + " " + DateTime.Now);
                        setsuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + " " + DateTime.Now);
                setsuccess = false;
            }
            return setsuccess;
        }
        public bool SetTaskToDataBase(string TaskYandex, string status)
        {
            bool taskiswrite = true;
            TaskDatas taskData = new();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    int counttobase = Convert.ToInt32(CheckTaskToDatabase(TaskYandex, sqlConnection));
                    if (counttobase == 0)
                    {
                        AddNewTaskToDatabase(TaskYandex, status, sqlConnection);
                        taskiswrite = true;
                    }
                    else if (counttobase > 0)
                    {
                        using (var sqlCommand = new SqlCommand(String.Format("  SELECT [TaskName], [TaskStatus] FROM [TelegramBotData].[dbo].[taskData] WHERE TaskName = {0} ", "'" + TaskYandex + "'"), sqlConnection))
                        {
                            var reader = sqlCommand.ExecuteReader();
                            while (reader.Read())
                            {
                                taskData.TaskName = reader["TaskName"].ToString();
                                taskData.TaskStatus = reader["TaskStatus"].ToString();
                                _logger.Information("Получена задача {0} со статусом {1} ", TaskYandex, status + " " + DateTime.Now);
                                if (taskData.TaskStatus != status)
                                {
                                    UpdateStatusTaskDataBase(TaskYandex, status, sqlConnection);
                                    taskiswrite = true;
                                }
                                else
                                {
                                    taskiswrite = false;
                                    return taskiswrite;
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + DateTime.Now);
                }
            }

            return taskiswrite;
        }
        public void SetTaskToTaskUser(string TaskYandex, string NumberPhoneUser, string status)
        {
            throw new NotImplementedException();
        }
        public void SetOrUpdateUserYandexToDataBase(string userid, string email)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(String.Format("  SELECT COUNT(*) AS CountUser FROM [TelegramBotData].[dbo].[userYandex] WHERE IdYandex = {0} ", "'" + userid + "'"), sqlConnection))
                {
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        int countsql = Convert.ToInt32(reader["CountUser"].ToString());
                        if (countsql == 0)
                        {

                        }
                        _logger.Information("Пользователь с id {0} присутствует в базе данных" + DateTime.Now, userid);
                    }
                }
            }




        }
        public void UpdateStatusTaskDataBase(string TaskYandex, string status, SqlConnection sqlConnection)
        {

            try
            {
                using (var sqlCommand = new SqlCommand(String.Format("  UPDATE [TelegramBotData].[dbo].[taskData] SET TaskStatus = {1} WHERE TaskName = {0} ", "'" + TaskYandex + "'", "'" + status + "'"), sqlConnection))
                {
                    var reader = sqlCommand.ExecuteReader();
                    _logger.Information("Задача {0} была обновлена в статус {1} ", TaskYandex, status);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + DateTime.Now);
            }

        }
        public bool CheckNumberPhoneToDataBase(string NumberPhoneUser, Serilog.ILogger _logger)
        {
            bool numberisbase = false;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT COUNT(*) AS CountUser FROM [TelegramBotData].[dbo].[userData] WHERE NumberPhone = {0}", NumberPhoneUser), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            int countsql = Convert.ToInt32(reader["CountUser"].ToString());
                            if (!(countsql == 0))
                            {
                                numberisbase = true;
                            }
                            _logger.Information("Пользователь с номером {0} присутствует в базе данных" + DateTime.Now, NumberPhoneUser);
                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + DateTime.Now);
                }
            }
            return numberisbase;
        }
        public void SetTelegramIdToDataBase(long TgId, string NumberPhoneUser, Serilog.ILogger _logger)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    using (var sqlCommand = new SqlCommand(String.Format("  UPDATE [TelegramBotData].[dbo].[taskData] SET TelegramId = {1} WHERE NumberPhone = {0} ", "'" + NumberPhoneUser + "'", "'" + TgId + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        _logger.Information("Записан телеграмм id {0} пользователя с номером телефона {1} " + " " + DateTime.Now);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + DateTime.Now);
                }
            }
        }
        public void SetUnregistredUserToDataBase(string username, string phone, long Tgid, string lastname, string firstname)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  INSERT INTO [TelegramBotData].[dbo].[unRegisteredUser] (NumberPhone, TelegramId, UserName, ExpDate, FirstName, LastName) VALUES ({0},{1},{2},{3},{4},{5}) ", "'" + phone + "'", "'" + Tgid + "'", "'" + username + "'", "'" + DateTime.Now.ToString() + "'", "'" + firstname + "'", "'" + lastname + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                    }
                    _logger.Information("Пользователь {0} с id {1} записан в базу данных", firstname, Tgid + " " + DateTime.Now);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + DateTime.Now);
                }
            }
        }
        public List<Entityes.UnRegisteredUser> GetAllUnregisteredUser()
        {
            List<Entityes.UnRegisteredUser> unregistredUsers = new List<Entityes.UnRegisteredUser>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT * FROM [TelegramBotData].[dbo].[unRegisteredUser] "), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            Entityes.UnRegisteredUser unregistredUser = new Entityes.UnRegisteredUser();
                            unregistredUser.UserName = reader["UserName"].ToString();
                            unregistredUser.LastName = reader["LastName"].ToString();
                            unregistredUser.FirstName = reader["FirstName"].ToString();
                            unregistredUser.NumberPhone = reader["NumberPhone"].ToString();
                            unregistredUser.TelegramId = reader["TelegramId"].ToString();
                            unregistredUser.ExpDate = reader["LastName"].ToString();
                            unregistredUser.UserId = reader["UserId"].ToString();
                            unregistredUsers.Add(unregistredUser);
                        }
                        _logger.Information("Получены незарегистрированный пользователи - " + unregistredUsers.Count + " " + DateTime.Now);
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString() + " " + DateTime.Now);
                }
                return unregistredUsers;
            }
        }
        public bool RegistrationNewUserToDataBase(Entityes.UnRegisteredUser user)
        {
            bool taskiswrite;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  INSERT INTO [TelegramBotData].[dbo].[userData] (NumberPhone, TelegramId, UserName, ExpDate, FirstName, LastName) VALUES ({0},{1},{2},{3},{4},{5}) ", "'" + user.NumberPhone + "'", "'" + user.TelegramId + "'", "'" + user.UserName + "'", "'" + DateTime.Now.ToString() + "'", "'" + user.FirstName + "'", "'" + user.LastName + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        taskiswrite = true;
                        _logger.Information("Зарегистрирован пользователь " + user.FirstName + " " + user.LastName + " " + DateTime.Now);
                    }
                    using (var sqlCommand = new SqlCommand(String.Format("  INSERT INTO [TelegramBotData].[dbo].[usersDataEntry] (NumberPhone, TelegramId) VALUES ({0},{1}) ", "'" + user.NumberPhone + "'", "'" + user.TelegramId + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        taskiswrite = true;
                        _logger.Information("Пользователю "+ user.FirstName + " " + user.LastName + "добавлены данные: " + user.NumberPhone + " " + user.TelegramId + " " + DateTime.Now);
                    }


                }
                catch (Exception ex)
                {
                    taskiswrite = false;
                    _logger.Error(ex.Message + " " + DateTime.Now);
                }
            }
            return taskiswrite;
        }
        public List<Entityes.UnRegisteredUser> GetAllRegisteredUser()
        {
            List<Entityes.UnRegisteredUser> unregistredUsers = new List<Entityes.UnRegisteredUser>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT * FROM [TelegramBotData].[dbo].[userData] "), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            Entityes.UnRegisteredUser unregistredUser = new Entityes.UnRegisteredUser();
                            unregistredUser.UserName = reader["UserName"].ToString();
                            unregistredUser.LastName = reader["LastName"].ToString();
                            unregistredUser.FirstName = reader["FirstName"].ToString();
                            unregistredUser.NumberPhone = reader["NumberPhone"].ToString();
                            unregistredUser.CodeZup = reader["CodeZup"].ToString();
                            unregistredUser.TelegramId = reader["TelegramId"].ToString();
                            unregistredUser.ExpDate = reader["LastName"].ToString();
                            unregistredUser.UserId = reader["UserId"].ToString();
                            unregistredUser.isAdmin = Convert.ToInt32(reader["isAdmin"]);
                            unregistredUsers.Add(unregistredUser);
                        }
                        _logger.Information("Получены зарегистрированные пользователи - "+ unregistredUsers.Count + " "+ DateTime.Now);
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString() + " " + DateTime.Now);
                }
                return unregistredUsers;
            }
        }
        public bool DeleteUnregisteredUser(Entityes.UnRegisteredUser unregisteredUser)
        {
            bool issuccess;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  DELETE FROM [TelegramBotData].[dbo].[unRegisteredUser] WHERE UserId= {0} ", unregisteredUser.UserId), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        issuccess = true;
                    }
                    _logger.Information("Удален пользователь " + unregisteredUser.FirstName + " " +unregisteredUser.LastName+ " " + DateTime.Now);

                }
                catch (Exception ex)
                {
                    issuccess = false;
                    _logger.Error(ex.ToString()+ " " + DateTime.Now);
                }
            }
            return issuccess;
        }
        public bool SetUserLogin(Entityes.UnRegisteredUser unregisteredUser, string UserInn, string ServerLogin, string ServerPassvord, string YandexLogin, string YandexPassword, string BitrixLogin, string BitrixPassword, string MailLogin, string MailPassword, bool isAdmin)
        {
            int admin = 0;
            if (isAdmin)
            {
                admin = 1;
            }
            bool taskiswrite;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  INSERT INTO [TelegramBotData].[dbo].[usersDataEntry] (NumberPhone, TelegramId, LoginBitrix, LoginYandex, LoginMail, LoginServer, PasswordBitrix, PasswordYandex, PasswordMail, PasswordServer, UserINN) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) ", "'" + unregisteredUser.NumberPhone + "'", "'" + unregisteredUser.TelegramId + "'", "'" + BitrixLogin + "'", "'" + YandexLogin + "'", "'" + MailLogin + "'", "'" + ServerLogin + "'", "'" + BitrixPassword + "'", "'" + YandexPassword + "'", "'" + MailPassword + "'", "'" + ServerPassvord + "'", "'" + UserInn + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        taskiswrite = true;
                        _logger.Information("Добавлены пользовательские данные для пользователя " + unregisteredUser.FirstName + " " +  unregisteredUser.LastName + " " + DateTime.Now);
                    }
                    using (var sqlCommand = new SqlCommand(String.Format("  UPDATE [TelegramBotData].[dbo].[userData] SET isAdmin = {0}, UserINN ={2} WHERE TelegramId = {1} ", admin, "'" + unregisteredUser.TelegramId + "'", "'" + UserInn + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        taskiswrite = true;
                        _logger.Information("Обновлены пользовательские данные для пользователя " + unregisteredUser.FirstName + " " + unregisteredUser.LastName + " " + DateTime.Now);
                    }

                }
                catch (Exception ex)
                {
                    taskiswrite = false;
                    _logger?.Error(ex.Message + DateTime.Now);
                }
            }
            return taskiswrite;
        }
        public int CheckUserLogin(Entityes.UnRegisteredUser unregisteredUser)
        {
            int countuser = 0;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT COUNT(*) AS CountUser FROM [TelegramBotData].[dbo].[userData] WHERE TelegramId= {0} ", "'" + unregisteredUser.TelegramId + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            countuser = Convert.ToInt32(reader["CountUser"]);
                            if(countuser > 0)
                            {
                                _logger.Information("Пользователь " + unregisteredUser.FirstName + " " + unregisteredUser.LastName + "есть в базе данных " + DateTime.Now);
                            }
                            else
                            {
                                _logger.Information("Пользователь " + unregisteredUser.FirstName + " " + unregisteredUser.LastName + "отсутствует в базе данных " + DateTime.Now);
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message +" "+ DateTime.Now);
                }
            }
            return countuser;
        }
        public bool SetLoginPasswordForUser(Entityes.UnRegisteredUser user, string UserInn, string ServerLogin, string ServerPassvord, string YandexLogin, string YandexPassword, string BitrixLogin, string BitrixPassword, string MailLogin, string MailPassword, bool isAdmin)
        {
            bool taskiswrite = false;
            int admin = 0;
            if (isAdmin)
            {
                admin = 1;
            }
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                if (CheckUserLogin(user) > 0)
                {
                    try
                    {
                        sqlConnection.Open();
                        using (var sqlCommand = new SqlCommand(String.Format("  UPDATE [TelegramBotData].[dbo].[usersDataEntry] SET PasswordServer = {0}, LoginServer = {1}, LoginBitrix = {3}, PasswordBitrix = {4}, LoginYandex ={5}, PasswordYandex = {6}, LoginMail = {7}, PasswordMail = {8}, UserINN ={9} WHERE TelegramId = {2} ", "'" + ServerPassvord + "'", "'" + ServerLogin + "'", "'" + user.TelegramId + "'", "'" + BitrixLogin + "'", "'" + BitrixPassword + "'", "'" + YandexLogin + "'", "'" + YandexPassword + "'", "'" + MailLogin + "'", "'" + MailPassword + "'", "'" + UserInn + "'"), sqlConnection))
                        {
                            var reader = sqlCommand.ExecuteReader();
                            taskiswrite = true;
                            _logger.Information("Обновлены пользовательские данные для пользователя " + user.FirstName + " " + user.LastName+ " "+DateTime.Now);
                        }
                        using (var sqlCommand = new SqlCommand(String.Format("  UPDATE [TelegramBotData].[dbo].[userData] SET isAdmin = {0}, UserINN ={2} WHERE TelegramId = {1} ", admin, "'" + user.TelegramId + "'", "'" + UserInn + "'"), sqlConnection))
                        {
                            var reader = sqlCommand.ExecuteReader();
                            taskiswrite = true;
                            _logger.Information("Обновлены статусы администрирования для пользователя " + user.FirstName + " " + user.LastName+ " " + DateTime.Now);
                        }

                    }
                    catch (Exception ex)
                    {
                        taskiswrite = false;
                        _logger?.Error(ex.Message + " " +DateTime.Now);
                    }
                }
                else
                {
                    taskiswrite = SetUserLogin(user, UserInn, ServerLogin, ServerPassvord, YandexLogin, YandexPassword, BitrixLogin, BitrixPassword, MailLogin, MailPassword, isAdmin);
                }


            }
            return taskiswrite;
        }
        public bool isAdmin(long tgId)
        {
            bool isAdmin = false;
            int countadmin = 0;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT [isAdmin] FROM [TelegramBotData].[dbo].[userData] WHERE TelegramId= {0} ", "'" + tgId + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            countadmin = Convert.ToInt32(reader["isAdmin"]);
                            _logger.Information("Получен статус администрирования для пользователя c телеграмм id " + tgId +" " + DateTime.Now);
                        }

                    }
                    if (countadmin > 0)
                    {
                        isAdmin = true;
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + " " + DateTime.Now);
                }
            }
            return isAdmin;
        }
        public UserDatas GetUserDatas(string UserInn)
        {
            UserDatas DataUsers = new UserDatas();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT * FROM [TelegramBotData].[dbo].[usersDataEntry] WHERE UserINN= {0} ", "'" + UserInn + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            DataUsers.NumberPhone = reader["NumberPhone"].ToString();
                            DataUsers.LoginMail = reader["LoginMail"].ToString();
                            DataUsers.PasswordMail = reader["PasswordMail"].ToString();
                            DataUsers.LoginYandex = reader["LoginYandex"].ToString();
                            DataUsers.PasswordYandex = reader["PasswordYandex"].ToString();
                            DataUsers.LoginBitrix = reader["LoginBitrix"].ToString();
                            DataUsers.PasswordBitrix = reader["PasswordBitrix"].ToString();
                            DataUsers.LoginServer = reader["LoginServer"].ToString();
                            DataUsers.PasswordServer = reader["PasswordServer"].ToString();
                            DataUsers.UserINN = reader["UserINN"].ToString();
                            _logger.Information("Получены данные пользователя с номером телефона - " + DataUsers.UserINN + " " + DateTime.Now);
                        }

                    }
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT [FirstName], [LastName] FROM [TelegramBotData].[dbo].[userData] WHERE UserINN= {0} ", "'" + UserInn + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            DataUsers.UserFirtsName = reader["FirstName"].ToString();
                            DataUsers.UserLastName = reader["LastName"].ToString();
                            _logger.Information(" Получен пользователь " + DataUsers.UserFirtsName + " " + DataUsers.UserLastName + DateTime.Now);
                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + " " + DateTime.Now);
                }
            }
            return DataUsers;
        }
        public RegisteredUser GetRegisteredUser(string NumberPhone)
        {
            RegisteredUser registeredUser = new RegisteredUser();
            
               
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        using (var sqlCommand = new SqlCommand(String.Format("  SELECT * FROM [TelegramBotData].[dbo].[userData] WHERE NumberPhone= {0} ", "'" + NumberPhone + "'"), sqlConnection))
                        {
                            var reader = sqlCommand.ExecuteReader();
                            while (reader.Read())
                            {
                                registeredUser.UserName = reader["UserName"].ToString();
                                registeredUser.FirstName = reader["FirstName"].ToString();
                                registeredUser.LastName = reader["LastName"].ToString();
                                registeredUser.UserINN = reader["UserINN"].ToString();
                                _logger.Information("Получен пользователь с номером телефона " + NumberPhone + " " + DateTime.Now.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message +" "+  DateTime.Now);
                    }
                }
            return registeredUser;
        }
        public bool IsRegister(string numberphone)
        {
            bool isRegister = false;
            int count = 0;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT COUNT (*) AS CountUser FROM [TelegramBotData].[dbo].[userData] WHERE NumberPhone= {0} ", "'" + numberphone + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            count = Convert.ToInt32(reader["CountUser"]);
                        }
                        if (count > 0)
                        {
                            isRegister = true;
                            _logger.Information("Пользователь с номером телефона " + numberphone +" зарегистрирован " + DateTime.Now);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message+" "+ DateTime.Now);
                }
            }
            return isRegister;
        }
    }
}
