using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TelegramBotApi.Entityes
{
    public class UserDatas
    {      
        public string? NumberPhone { get; set; }
        public long TelegramId { get; set; }   
        public string? LoginBitrix { get; set; }
        public string? LoginYandex { get; set; }    
        public string? LoginMail { get; set; }
        public string? LoginServer { get; set; }     
        public string? PasswordBitrix { get; set; }
        public string? PasswordYandex { get; set; }    
        public string? UserINN { get; set; }
        public string? PasswordMail { get; set; }
        public string? PasswordServer { get; set; }
        public string? UserFirtsName { get; set;}
        public string? UserLastName { get; set;}
    }
}
