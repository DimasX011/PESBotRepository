using Microsoft.Data.SqlClient;
using TelegramBotApi.Interfaces;

namespace TelegramBotApi.Services
{
    public class WriteToDataBase : ISetToDataBase
    {

        public void AddNewTaskToDatabase(string TaskYandex, string status, SqlConnection sqlConnection)
        {
            try
            {
                using (var sqlCommand = new SqlCommand(String.Format("  INSERT INTO [TelegramBotData].[dbo].[taskData] (TaskName,TaskStatus) VALUES ({0}, {1}) ", "'" + TaskYandex + "'", "'" + status + "'"), sqlConnection))
                {
                    var reader = sqlCommand.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return countsql;
        }

        public bool FirstCallContact(long idContact, string ConnectionString)
        {
            bool addcontacttodatabase = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
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
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return addcontacttodatabase;
        }

        public UserEntityLogin GetUserData(string Username, string ConnectionString)
        {
            UserEntityLogin userEntity = new();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT [UserLogin], [UserPassword] FROM [TelegramBotData].[dbo].[userData] WHERE UserName = {0}", Username), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            userEntity.UserLogin = reader["UserLogin"].ToString();
                            userEntity.UserPassword = reader["UserPassword"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
            return userEntity;
        }

        public void SetNummerPhoneToDataBase(string username, string phone, long Tgid, string lastname, string firstname, string ConnectionString)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  INSERT INTO [TelegramBotData].[dbo].[userData] (NumberPhone, TelegramId, UserName, ExpDate, FirstName, LastName) VALUES ({0},{1},{2},{3},{4},{5}) ", "'" + phone + "'", "'" + Tgid + "'", "'" + username + "'", "'" + DateTime.Now.ToString() + "'", "'" + firstname + "'", "'" + lastname + "'"), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public bool SetTaskToDataBase(string TaskYandex, string status, string ConnectionString)
        {
            bool taskiswrite = true;
            TaskDatas taskData = new();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
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
                    Console.WriteLine(ex.Message);
                }
            }

            return taskiswrite;
        }

        public void UpdateStatusTaskDataBase(string TaskYandex, string status, SqlConnection sqlConnection)
        {
            try
            {
                using (var sqlCommand = new SqlCommand(String.Format("  UPDATE [TelegramBotData].[dbo].[taskData] SET TaskStatus = {1} WHERE TaskName = {0} ", "'" + TaskYandex + "'", "'" + status + "'"), sqlConnection))
                {
                    var reader = sqlCommand.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
