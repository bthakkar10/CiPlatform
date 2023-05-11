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
        //for admin side page
        public List<CmsPage> CmsList()
        {
            return _db.CmsPages.Where(cms => cms.DeletedAt == null).ToList();
        }

        //to display on user side 
        public List<CmsPage> CmsListUser()
        {
            return _db.CmsPages.Where(cms => cms.DeletedAt == null && cms.Status == true).ToList();
        }

        public string CmsAdd(AdminCmsViewModel cmsvm)
        {
            try
            {
                CmsPage? DoesCmsExist = _db.CmsPages.FirstOrDefault(cms => cms.Title!.Trim().ToLower() == cmsvm.Title.Trim().ToLower());
                if(DoesCmsExist== null)
                {
                    CmsPage cms = new()
                    {
                        Title = cmsvm.Title,
                        Description = cmsvm.CmsDescription,
                        Slug = cmsvm.Slug,
                        Status = cmsvm.Status,
                        CreatedAt = DateTime.Now,
                    };

                    _db.CmsPages.Add(cms);
                    _db.SaveChanges();
                    return "Added";
                }
                else
                {
                    if (DoesCmsExist != null && DoesCmsExist.DeletedAt == null)
                    {
                        return "Exists";
                    }
                    else
                    {
                        DoesCmsExist.DeletedAt = null!;
                        DoesCmsExist.UpdatedAt = DateTime.Now;
                        _db.Update(DoesCmsExist);
                        _db.SaveChanges();
                        return "Added";
                    }
                }
               
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool CmsDelete(long CmsId)
        {
            try
            {
                CmsPage cms = _db.CmsPages.Find(CmsId)!;
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
            CmsPage cms = _db.CmsPages.Find(CmsId)!;
            AdminCmsViewModel cmsvm = new AdminCmsViewModel(cms);
            return cmsvm;
        }

        public string EditCms(AdminCmsViewModel cmsvm)
        {
            if(_db.CmsPages.FirstOrDefault(cms => cms.Title!.Trim().ToLower() == cmsvm.Title!.Trim().ToLower() && cms.CmsPageId != cmsvm.CmsId) != null)
            {
                return "Exists";
            }
            CmsPage cms = _db.CmsPages.Find(cmsvm.CmsId)!;
            if (cms != null)
            {
                cms.Title= cmsvm.Title;
                cms.Description = cmsvm.CmsDescription;
                cms.Slug= cmsvm.Slug;
                cms.Status = cmsvm.Status;
                cms.UpdatedAt= DateTime.Now;
               _db.CmsPages.Update(cms);
                _db.SaveChanges();
                return "Updated";
            }
            else
            {
                return "Error";
            }

        }
    }
}
