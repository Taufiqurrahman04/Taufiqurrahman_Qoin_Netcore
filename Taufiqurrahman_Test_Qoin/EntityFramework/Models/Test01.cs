

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taufiqurrahman_Test_Qoin.EntityFramework.Models
{
    public class Test01
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nama { get; set; }
        public byte Status { get; set; }
        public DateTime Created { get;set; }
        public DateTime? Updated { get;set; }

    }
}
