﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OWM.Domain.Entities;

namespace OWM.Application.Services.Dtos
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Occupation is Required")]
        public int? OccupationId { get; set; }

        [Required(ErrorMessage = "Ethnicity is Required")]
        public int? EthnicityId { get; set; }



        [Required(ErrorMessage = "City is Required")]
        public int? CityId { get; set; }


        [Required(ErrorMessage = "Gender is Required")]
        public int? Gender { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is Required")]
        public string Surname { get; set; }

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "City is Required")]
        public string CityName { get; set; }

        public string Interest { get; set; }

        public List<Interest> Interests => string.IsNullOrEmpty(Interest)
            ? new List<Interest>()
            : Interest.Split(',').Select(x => new Interest {Name = x}).ToList();

        [Required(AllowEmptyStrings = false,ErrorMessage = "Birthday is required")]
        public DateTime? DateOfBirth => string.IsNullOrEmpty(Birthday) ? (DateTime?) null : DateTime.ParseExact(Birthday.Replace("-", "/"), Utils.Constants.DateFormat, null);

        [Required(ErrorMessage = "Birthday is required")]
        public string Birthday { get; set; }

        public string ProfileImageAddress { get; set; }
        public bool  VerifiedEmail { get; set; }

        public string HowDidYouHearUs { get; set; }
    }
}