using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotData
{
    
    [Table("taskUsers")]
    public class TaskUsers
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskUsersId { get; set; }

        [Column]
        [StringLength(100)]
        public string TaskName { get; set; }

        [Column]
        [StringLength(12)]
        public string? TaskNumberPhone { get; set; }
    }
}
