namespace E4UsersMVCWebApp.Models
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;

    public class UserModel : IValidatableObject
    {
        private const int _currentYear = 2022;

        public UserModel()
        {
            this.Id = 0;
            this.FirstName = "";
            this.LastName = "";
            this.DoB = DateTime.Now;
            this.Cellphone = "";
            this.Email = "";
            this.ImagePath = "";
        }
        public UserModel(int id, string firstName, string lastName, DateTime doB, string cellphone, string email, string imagePath)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DoB = doB;
            Cellphone = cellphone;
            Email = email;
            ImagePath = imagePath;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Remote(action: "VerifyName", controller: "Users", AdditionalFields = nameof(LastName))]
        [Display(Name = "First Name(s)")] 
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [Remote(action: "VerifyName", controller: "Users", AdditionalFields = nameof(FirstName))]
        [Display(Name = "Surname")] 
        public string LastName { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DoB { get; set; }

        [Phone] 
        public string Cellphone { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address")] 
        public string Email { get; set; }

        public string ImagePath { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DoB.Year > _currentYear)
            {
                yield return new ValidationResult(
                    $"Date of birth cannot be later than {_currentYear}.",new[] { nameof(DoB) });
            }
        }
    }
}
