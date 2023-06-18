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
        public string? UserLogin { get; set; }

        [Column]
        [StringLength(50)]
        public string? UserPassword { get; set; }

        [Column]
        public DateTime ExpDate { get; set; }

    }
}
