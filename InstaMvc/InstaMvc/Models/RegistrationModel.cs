﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace InstaMvc.Models
{
    public class RegistrationModel
    {
        [Required]
        [Display(Name = "Логин")]
        [EmailAddress(ErrorMessage = "Не тот адрес ты ввел")]
        public string LoginName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MembershipPassword(
            MinRequiredNonAlphanumericCharacters = 1,
            MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
            ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).",
            MinRequiredPasswordLength = 6
        )]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "No equal")]
        [DataType(DataType.Password)]
        public string RetryPassword { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public System.DateTime Birtdate { get; set; }
        [Required]
        public bool SharedProfile { get; set; }

    }
}