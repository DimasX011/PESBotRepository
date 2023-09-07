using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotData
{
    [Table("unRegisteredUser")]
    public class UnRegisteredUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Column]
        [StringLength(50)]
        public string UserName { get; set; }

        [Column]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Column]
        [StringLength(50)]
        public string? FirstName { get; set; }


        [Column]
        [StringLength(12)]
        public string? NumberPhone { get; set; }

        [Column]
        public long TelegramId { get; set; }

        [Column]
        public DateTime ExpDate { get; set; }
    }
}
