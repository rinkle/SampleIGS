using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("ErrorLog")]
    public class ErrorLog
    {
        [Key]
        public int Id { get; set; }   // or decimal if your DB uses decimal PK

        [MaxLength(4000)]
        public string Message { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string InnerMessage { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Source { get; set; } = string.Empty;

        public string? StackTrace { get; set; }

        public string? Details { get; set; }

        public DateTime ErrorDate { get; set; }
    }
}
