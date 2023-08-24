using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PesBotData
{
    [Table("userData")]
    public class AccountData
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