﻿using System.ComponentModel.DataAnnotations;

namespace TUQA_Shop.DTOs
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }


    }
}
