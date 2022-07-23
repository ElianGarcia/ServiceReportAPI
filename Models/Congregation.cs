using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceReportAPI.Models
{
    public class Congregation
    {
        public Int64 CongregationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(8)]
        public int Code { get; set; }

        public Congregation()
        {
            CongregationId = 0;
            Name = String.Empty;
            Code = 0;
        }

        public Congregation(long id, string name, int code)
        {
            CongregationId = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code;
        }
    }
}
