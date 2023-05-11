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
    public class AdminTheme : IAdminTheme
    {
        private readonly CiDbContext _db;

        public AdminTheme(CiDbContext db)
        {
            _db = db;
        }

        public List<MissionTheme> ThemeList()
        {
            return _db.MissionThemes.Where(theme => theme.DeletedAt == null).ToList();
        }

        public string ThemeAdd(AdminThemeViewModel themevm)
        {
            try
            {
                MissionTheme DoesThemeExists = _db.MissionThemes.FirstOrDefault(theme => theme.Title!.Trim().ToLower() == themevm.ThemeName!.Trim().ToLower())!;
                if (DoesThemeExists == null)
                {
                    MissionTheme theme = new()
                    {
                        Title = themevm.ThemeName,
                        Status = themevm.Status,
                        CreatedAt = DateTime.Now,
                    };

                    _db.MissionThemes.Add(theme);
                    _db.SaveChanges();
                    return "Added";
                }
                else
                {
                    if (DoesThemeExists != null && DoesThemeExists.DeletedAt == null)
                    {
                        return "Exists";
                    }
                    else
                    {
                        DoesThemeExists.DeletedAt = null!;
                        DoesThemeExists.UpdatedAt = DateTime.Now;
                        _db.Update(DoesThemeExists);
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

        public string ThemeDelete(long ThemeId)
        {
            try
            {
                MissionTheme theme = _db.MissionThemes.Find(ThemeId)!;
                if (theme != null)
                {
                    if (_db.Missions.Any(m => m.MissionThemeId == ThemeId && m.Status == true && m.DeletedAt == null))
                    {
                        // The theme is already used by a mission, so it cannot be deleted
                        return "Exists";
                    }
                    theme.DeletedAt = DateTime.Now;
                    _db.Update(theme);
                    _db.SaveChanges();
                    return "Deleted";
                }
                else
                {
                    return "error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public AdminThemeViewModel GetThemeData(long ThemeId)
        {
            MissionTheme theme = _db.MissionThemes.Find(ThemeId)!;
            AdminThemeViewModel themevm = new AdminThemeViewModel(theme);
            return themevm;
        }

        public string EditTheme(AdminThemeViewModel themevm)
        {
            try
            {
                if (_db.MissionThemes.FirstOrDefault(missiontheme => missiontheme.Title!.Trim().ToLower() == themevm.ThemeName!.Trim().ToLower() && missiontheme.MissionThemeId != themevm.ThemeId) != null)
                {
                    return "Exists";
                }
                MissionTheme theme = _db.MissionThemes.Find(themevm.ThemeId)!;
                if (theme != null)
                {
                    theme.Title = themevm.ThemeName;
                    theme.Status = themevm.Status;
                    theme.UpdatedAt = DateTime.Now;
                    _db.MissionThemes.Update(theme);
                    _db.SaveChanges();
                    return "Updated";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
