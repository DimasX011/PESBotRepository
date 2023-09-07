using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBotData
{
    [Table("userData")]
    public class UserData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Column]
        [StringLength(50)]
        public string UserName { get; set; }

        [Column]
        [StringLength(50)]
        public string? UserINN { get; set; }

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
        [StringLength(50)]
        public string? CodeZup { get; set; }

        [Column]
        public long TelegramId { get; set; }

        [Column]
        public bool isAdmin { get; set; }

        [Column]
        public DateTime ExpDate { get; set; }

    }
}
