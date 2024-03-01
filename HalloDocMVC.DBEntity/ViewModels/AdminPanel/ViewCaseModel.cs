﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocMVC.DBEntity.ViewModels.AdminPanel
{
    public class ViewCaseModel
    {
        public int RequestId { get; set; }
        public int RequestTypeId { get; set; }
        public int Status { get; set; }
        public string? Notes { get; set; }
        public string? ConfirmationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Region { get; set; }
        public string? Room { get; set; }
    }
}
