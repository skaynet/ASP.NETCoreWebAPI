using ASP.NETCoreWebApi.Utility;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASP.NETCoreWebApi.Models
{
    public class FinancialTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public float Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime Date { get; set; }

        [Required]
        public int TypeId { get; set; }

        public virtual OperationType? Type { get; set; }
    }
}