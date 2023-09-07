using Microsoft.Data.SqlClient;
using TelegramBotApi.Entityes;
using TelegramBotApi.Services;

namespace TelegramBotApi.Interfaces
{
    public interface ISetToDataBase

    {
      

        bool FirstCallContact(long idContact);

        bool SetTaskToDataBase(string TaskYandex, string status);

        void SetOrUpdateUserYandexToDataBase(string userid, string email);

        void AddNewTaskToDatabase(string TaskYandex, string status, SqlConnection sqlConnection);

        void UpdateStatusTaskDataBase(string TaskYandex, string status, SqlConnection sqlConnection);

        string CheckTaskToDatabase(string TaskYandex, SqlConnection sqlConnection);

        void AddPostToDataBase(string Post, string TgId);

        void SetNummerPhoneToDataBase(string username, string phone, long Tgid, string lastname, string firstname);

        bool CheckPostToDataBase(string Post, long TgId);

        bool SetPostToDataBase(string Post, long TgId);

        void SetTaskToTaskUser(string TaskYandex,string NumberPhoneUser, string status);

        public bool CheckNumberPhoneToDataBase(string NumberPhoneUser,  Serilog.ILogger _logger);

        public void SetTelegramIdToDataBase(long TgId, string NumberPhoneUser,  Serilog.ILogger _logger);

        void SetUnregistredUserToDataBase(string username, string phone, long Tgid, string lastname, string firstname);

        List<UnRegisteredUser> GetAllUnregisteredUser();

        List<UnRegisteredUser> GetAllRegisteredUser();

        bool RegistrationNewUserToDataBase(UnRegisteredUser user);

        bool SetLoginPasswordForUser(UnRegisteredUser user, string UserInn, string ServerLogin, string ServerPassvord,string YandexLogin, string YandexPassword, string BitrixLogin, string BitrixPassword, string MailLogin, string MailPassword, bool isAdmin);

        bool DeleteUnregisteredUser(UnRegisteredUser unregisteredUser);

        bool SetUserLogin(UnRegisteredUser unregisteredUser, string UserInn, string ServerLogin, string ServerPassvord, string YandexLogin, string YandexPassword, string BitrixLogin, string BitrixPassword, string MailLogin, string MailPassword, bool isAdmin);

        int CheckUserLogin(UnRegisteredUser unregisteredUser);

        UserDatas GetUserDatas(string UserInn);

        RegisteredUser GetRegisteredUser(string NumberPhone);

        bool IsRegister(string numberphone);

        bool isAdmin(long tgId);
    }

}
