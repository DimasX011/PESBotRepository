using Microsoft.Data.SqlClient;

namespace TelegramBotApi.Interfaces
{
    public interface ISetToDataBase
    {
        bool FirstCallContact(long idContact, string ConnectionString);

        bool SetTaskToDataBase(string TaskYandex, string status, string ConnectionString);

        void AddNewTaskToDatabase(string TaskYandex, string status, SqlConnection sqlConnection);

        void UpdateStatusTaskDataBase(string TaskYandex, string status, SqlConnection sqlConnection);

        string CheckTaskToDatabase(string TaskYandex, SqlConnection sqlConnection);

        void SetNummerPhoneToDataBase(string username, string phone, long Tgid, string lastname, string firstname, string ConnectionString);

        UserEntityLogin GetUserData(string Username, string ConnectionString);
    }
}
