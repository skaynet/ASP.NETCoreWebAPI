using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASP.NETCoreWebApi.Models
{
    public class OperationType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public ICollection<FinancialTransaction>? FinancialTransactions { get; set; }
    }
}