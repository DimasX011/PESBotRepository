using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TelegramBotApi.Entityes
{
    public class UnRegisteredUser
    {       
        public string UserINN { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string? LastName { get; set; }   
        public string? FirstName { get; set; }   
        public string? NumberPhone { get; set; }
        public string? CodeZup { get; set; }       
        public string TelegramId { get; set; }
        public string ExpDate { get; set; }
        public int isAdmin { get; set; }
    }
}
