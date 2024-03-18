using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HalloDocMVC.DBEntity.ViewModels.AdminPanel.ViewUploadModel;
namespace HalloDocMVC.DBEntity.ViewModels.AdminPanel
{
    public class CloseCaseModel
    {
        public int RequestID { get; set; }
        public int RequestClientID { get; set; }
        public int RequestWiseFileID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RC_FirstName { get; set; }
        public string RC_LastName { get; set; }
        public DateTime RC_DateOfBirth { get; set; }
        public string RC_PhoneNumber { get; set; }
        public string RC_Email { get; set; }
        public List<Documents> documents { get; set; } = null;
    }
}
