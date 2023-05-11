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
    public class AdminBanner : IAdminBanner
    {

        private readonly CiDbContext _db;
        public AdminBanner(CiDbContext db)
        {
            _db = db;
        }

        public List<Banner> BannerList()
        {
            return _db.Banners.Where(banner => banner.DeletedAt == null).ToList();
        }

        public string BannerAdd(AdminBannerViewModel bannervm)
        {
            try
            {
                Banner DoesSortOrderExist = _db.Banners.FirstOrDefault(banner => banner.SortOrder ==  bannervm.sortOrder);
                if(DoesSortOrderExist == null)
                {
                    Banner banner = new()
                    {
                        Title = bannervm.Title,
                        Description = bannervm.Description,
                        SortOrder = bannervm.sortOrder,
                        CreatedAt = DateTime.Now,

                    };
                    if (bannervm.UpdatedImg != null)
                    {
                        var NewImg = bannervm.UpdatedImg.FileName;
                        var guid = Guid.NewGuid().ToString()[..8];
                        var fileName = $"{guid}_{NewImg}"; // getting filename

                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/Banner", fileName);//for updating new img

                        banner.Image = fileName;

                        using var stream = new FileStream(filePath, FileMode.Create);
                        bannervm.UpdatedImg.CopyTo(stream);
                    }
                    _db.Banners.Add(banner);
                    _db.SaveChanges();
                    return "Added";
                }
                else
                {
                    if (DoesSortOrderExist != null && DoesSortOrderExist.DeletedAt == null)
                    {
                        return "Exists";
                    }
                    else
                    {
                        DoesSortOrderExist.DeletedAt = null!;
                        DoesSortOrderExist.UpdatedAt = DateTime.Now;
                        _db.Update(DoesSortOrderExist);
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

        public bool BannerDelete(long BannerId)
        {
            try
            {
                Banner banner = _db.Banners.Find(BannerId)!;
                if (banner != null)
                {
                    banner.DeletedAt = DateTime.Now;
                    _db.Update(banner);
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

        public AdminBannerViewModel GetBannerData(long BannerId)
        {
            Banner banner = _db.Banners.Find(BannerId)!;
            AdminBannerViewModel bannervm = new(banner);
            return bannervm;
        }

        public string EditBanner(AdminBannerViewModel bannervm)
        {
            if (_db.Banners.FirstOrDefault(banner => banner.SortOrder == bannervm.sortOrder && banner.BannerId != bannervm.BannerId) != null)
            {
                return "Exists";
            }
            Banner banner = _db.Banners.Find(bannervm.BannerId)!;
            if (banner != null)
            {
                banner.Title = bannervm.Title;
                banner.Description = bannervm.Description;
                banner.SortOrder = bannervm.sortOrder;
                banner.UpdatedAt = DateTime.Now;

                if (bannervm.UpdatedImg != null)
                {
                    var NewImg = bannervm.UpdatedImg.FileName;
                    var guid = Guid.NewGuid().ToString()[..8];
                    var fileName = $"{guid}_{NewImg}"; // getting filename

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/Banner", fileName);//for updating new img

                    banner.Image = fileName;

                    using var stream = new FileStream(filePath, FileMode.Create);
                    bannervm.UpdatedImg.CopyTo(stream);
                }

                _db.Banners.Update(banner);
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
