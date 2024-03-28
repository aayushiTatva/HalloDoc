using HalloDocMVC.DBEntity.ViewModels.AdminPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocMVC.Repositories.Admin.Repository.Interface
{
    public interface IContactYourProvider
    {
        Task<List<ProviderModel>> GetContacts();
        Task<List<ProviderModel>> PhysicianByRegion(int? region);
        Task<bool> ChangeNotification(Dictionary<int, bool> changeValueDict);
    }
}
