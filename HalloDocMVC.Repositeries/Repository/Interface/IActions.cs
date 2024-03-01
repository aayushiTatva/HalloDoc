//
using HalloDocMVC.DBEntity.DataContext;
using HalloDocMVC.DBEntity.ViewModels.AdminPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocMVC.Repositories.Admin.Repository.Interface
{
    public interface IActions 
    {
        public ViewCaseModel GetRequestForViewCase(int id); //takes int id as paramter and returns viewcasemodel
        public bool EditCase(ViewCaseModel model);//takes viewcasemodel as parameter as it updates the details of viewcasemodel and returns boolean i.e. updated or not (true or false resp.)
        Task<bool> AssignProvider(int RequestId, int ProviderId, string notes);
        public bool CancelCase(int RequestID, string Note, string CaseTag);
        public bool BlockCase(int RequestID, string Note);
    }
}
