using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TelegramBotApi.Entityes
{
    public class RegisteredUser
    {

        public string UserName { get; set; }
        public string? UserINN { get; set; }       
        public string? LastName { get; set; }
        public string? FirstName { get; set; }       
        public string? NumberPhone { get; set; }
        public string? CodeZup { get; set; }
        public long TelegramId { get; set; }
        public bool isAdmin { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
