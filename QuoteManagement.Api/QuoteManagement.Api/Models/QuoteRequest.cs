using QuoteManagement.Api.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteManagement.Api.Models
{
    public class QuoteRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = Constants.Messages.NameExceedsCharacters)]
        public string Name { get; set; }

        [Required]
        [PossibleValues(FieldName = "Sex", 
            ExpectedValues = new string[] { Constants.Sex.Male, Constants.Sex.Female })]
        public string Sex { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateRequired(ErrorMessage = Constants.Messages.PastDateOfBirthRequired)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = Constants.Messages.MobileNumberIsInvalid)]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(25)]
        [PossibleValues(FieldName = "PensionPlan", 
            ExpectedValues = new string[] {Constants.PensionPlan.Silver.Name,
            Constants.PensionPlan.Gold.Name, Constants.PensionPlan.Platinum.Name })]
        public string PensionPlan { get; set; }

        [Required]        
        [DataType(DataType.Currency)]
        [MinimumAmountByPlan]
        public decimal? InvestmentAmount { get; set; }

        [Required]
        [Range(60, 75, ErrorMessage = Constants.Messages.RetirementAgeRangeIsInvalid)]
        [MinimumRetirementAgeByPlan]
        public int? RetirementAge { get; set; }

        [DataType(DataType.Currency)]
        public decimal MaturityAmount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime QuoteDate { get; set; }
    }
}
