using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotData
{
    [Table("usersDataEntry")]
    public class UsersDataEntry
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsersDataId { get; set; }

        [Column]
        [StringLength(12)]
        public string? NumberPhone { get; set; }

        [Column]
        public long TelegramId { get; set; }

        [Column]
        [StringLength(50)]
        public string? LoginBitrix { get; set; }

        [Column]
        [StringLength(50)]
        public string? LoginYandex { get; set; }

        [Column]
        [StringLength(50)]
        public string? LoginMail { get; set; }

        [Column]
        [StringLength(50)]
        public string? LoginServer { get; set; }

        [Column]
        [StringLength(200)]
        public string? PasswordBitrix { get; set; }

        [Column]
        [StringLength(200)]
        public string? PasswordYandex { get; set; }

        [Column]
        [StringLength(50)]
        public string? UserINN { get; set; }

        [Column]
        [StringLength(200)]
        public string? PasswordMail { get; set; }

        [Column]
        [StringLength(200)]
        public string? PasswordServer { get; set; }
    }
}
