using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class AdminCms : IAdminCms
    {

        private readonly CiDbContext _db;
        public AdminCms(CiDbContext db)
        {
            _db = db;
        }

        public List<CmsPage> CmsList()
        {
            return _db.CmsPages.Where(cms => cms.DeletedAt == null).ToList();
        }

        public bool CmsAdd(AdminCmsViewModel cmsvm)
        {
            try
            {
                CmsPage cms = new CmsPage()
                {
                    Title= cmsvm.Title,    
                    Description = cmsvm.CmsDescription,
                    Slug = cmsvm.Slug, 
                    Status= cmsvm.Status,  
                    CreatedAt = DateTime.Now,   
                };

                _db.CmsPages.Add(cms);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CmsDelete(long CmsId)
        {
            try
            {
                CmsPage cms = _db.CmsPages.Find(CmsId);
                if (cms != null)
                {
                    cms.DeletedAt = DateTime.Now;
                    _db.Update(cms);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AdminCmsViewModel GetCmsData(long CmsId)
        {
            CmsPage cms = _db.CmsPages.Find(CmsId);
            AdminCmsViewModel cmsvm = new AdminCmsViewModel(cms);
            return cmsvm;
        }

        public bool EditCms(AdminCmsViewModel cmsvm)
        {
            CmsPage cms = _db.CmsPages.Find(cmsvm.CmsId);
            if (cms != null)
            {
                cms.Title= cmsvm.Title;
                cms.Description = cmsvm.CmsDescription;
                cms.Slug= cmsvm.Slug;
                cms.Status = cmsvm.Status;
                cms.UpdatedAt= DateTime.Now;
               _db.CmsPages.Update(cms);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
