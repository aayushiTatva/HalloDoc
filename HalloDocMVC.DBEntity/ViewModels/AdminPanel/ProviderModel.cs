using HalloDocMVC.DBEntity.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocMVC.DBEntity.ViewModels.AdminPanel
{
    public class ProviderModel
    {
        public int? AdminId { get; set; }
        public int? PhysicianId { get; set; }
        public string? PhysicianName { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set;}
        public int? RegionId { get; set; }
        public int? NotificationId { get; set; }
        public BitArray? Notification { get; set; }
        public string? AspnetuserId { get; set; }
        public string? UserName { get; set; }
        public string Password { get; set; }
        public string? RegionsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? MedicalLicence { get; set; }
        public string? Photo { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public string? AdminNotes { get; set; }
        public bool Isagreementdoc { get; set; }
        public bool Isbackgrounddoc { get; set; }
        public bool Istrainingdoc { get; set; }
        public bool Isnondisclosuredoc { get; set; }
        public bool Islicencedoc { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? AltPhoneNumber { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get;set; }
        public short? Status { get; set; }
        public string BusinessName { get; set; } = null!;
        public string? BusinessWebsite { get; set; } = null!;
        public BitArray? Isdeleted { get; set; }
        public string? Npinumber { get; set; }
        public string? Signature { get; set; }
        public IFormFile? SignatureFile { get; set; }
        public BitArray? Iscredentialdoc { get; set; }
        public BitArray? Istokengenerate { get; set; }
        public string? Syncmailaddredss { get; set; }
        public IFormFile? Agreementdoc { get; set; }
        public IFormFile? NonDisclosuredoc { get; set; }
        public IFormFile? Trainingdoc { get; set; }
        public IFormFile? BackGrounddoc { get; set; }
        public IFormFile? Licencedoc { get; set; }
        public List<Region>? Regionids { get; set; }
        public class Regions
        {
            public int? RegionId { get; set; }
            public string? RegionName { get; set; }
        }
    }
}
