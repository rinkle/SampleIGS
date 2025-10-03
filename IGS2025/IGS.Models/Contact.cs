using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string? Organization { get; set; }
        public string? City { get; set; }
        public string? Street_Address1 { get; set; }
        public string? Street_Address2 { get; set; }
        public string? Street_Address3 { get; set; }
        public string? Pincode { get; set; }
        public string? State { get; set; }
        public string? StateFullName { get; set; }
        public string? Country { get; set; }
        public decimal? DisplayOrder { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? GoogleMapUrl { get; set; }
        public string? GoogleMapDirectionURL { get; set; }
        public string? OfficeLogo { get; set; }
        public string? MapImage { get; set; }
        public string? LeftHeading { get; set; }
        public string? LeftSubHeading { get; set; }
        public string? Longitude { get; set; }
        public string? Lattitude { get; set; }
        public string? PinLabel { get; set; }
        public string? LinkedInUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
