using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotData
{
    [Table("taskData")]
    public class TaskData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Column]
        [StringLength(50)]
        public string TaskName { get; set; }

        [Column]
        [StringLength(50)]
        public string? TaskStatus { get; set; }

    }
}
