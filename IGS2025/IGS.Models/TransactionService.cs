using IGS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TransactionServices")] // maps class → DB table
public class TransactionService
{
    [Key] // marks primary key
    public int Id { get; set; }
    public string? AreasofFocusHeading { get; set; }
    public string? AreasofFocusDescription { get; set; }
    public string? IndustryExpertiseHeading { get; set; }
    public string? IndustryExpertiseSubHeading { get; set; }
    public string? IndustryExpertiseDescription { get; set; }
    public string? RecentProjectHeading { get; set; }
    public string? RecentProjectDescription { get; set; }
    public string? InsightHeading { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}
