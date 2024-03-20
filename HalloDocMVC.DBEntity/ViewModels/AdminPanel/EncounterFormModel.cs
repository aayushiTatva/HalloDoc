using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocMVC.DBEntity.ViewModels.AdminPanel
{
    public class EncounterFormModel
    {
        public int RequestID { get; set; }
        public int RequestTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RequestDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
