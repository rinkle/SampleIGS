using System;

namespace IGS.Models.KeyLessModels
{
    /// <summary>
    /// DTO for results returned by stored procedure: GetCommonListing
    /// </summary>
    public class GetCommonListing_Result
    {
        public int Id { get; set; }
        public int Fk_PageId { get; set; }

        public string? Section { get; set; }
        public string? Heading { get; set; }
        public string? SubHeading { get; set; }
        public string? Description { get; set; }
        public string? AdditionalSubHeading { get; set; }
        public string? UploadedData { get; set; }

        public string? AdditionalImage1 { get; set; }
        public string? AdditionalImage2 { get; set; }
        public string? AdditionalImage3 { get; set; }

        public string? ImageLabel { get; set; }
        public string? ReferanceUrl { get; set; }

        public int? Fk_PortId { get; set; }
        public int? Fk_CaseStudyId { get; set; }
        public bool? IsActive { get; set; }
        public decimal? DisplayOrder { get; set; }

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
