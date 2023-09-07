using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotData
{
    [Table("userYandex")]
    public class UserYandex
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserYandexIdToDataBase { get; set; }

        [Column]
        [StringLength(50)]
        public string IdYandex { get; set; }

        [Column]
        [StringLength(50)]
        public string? email { get; set; }

    }
}
