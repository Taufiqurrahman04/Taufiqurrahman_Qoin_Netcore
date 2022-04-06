

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taufiqurrahman_Test_Qoin.ServiceWorker.Models
{
    public class Test01
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nama { get; set; }
        public byte Status { get; set; }
        public DateTime Created { get;set; }=DateTime.Now;
        public DateTime? Updated { get;set; }

    }
}
