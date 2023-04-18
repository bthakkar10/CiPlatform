﻿using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class AdminUser : IAdminUser
    {
        private readonly CiDbContext _db;
        public AdminUser(CiDbContext db)
        {
            _db = db;
        }

        public List<User> users()
        {
            return _db.Users.Where(U => U.DeletedAt == null).ToList();
        }


        public string IsUserAdded(AdminUserViewModel vm)
        {
            try
            {
                User DoesUserExists = _db.Users.Where(u => u.Email == vm.Email).FirstOrDefault();
                if(DoesUserExists == null)
                {
                    User user = new User()
                    {
                        FirstName = vm.FirstName,
                        LastName = vm.Surname,
                        Email = vm.Email,
                        PhoneNumber = vm.PhoneNumber,
                        EmployeeId = vm.EmployeeId,
                        Department = vm.Department,
                        CreatedAt = DateTime.Now,
                        CountryId = vm.CountryId,
                        CityId = vm.CityId,
                        Password = vm.Password,
                        Availability = vm.Availibility,
                        Status = vm.Status, 
                    };
                    _db.Users.Add(user);    
                    _db.SaveChanges();  
                    return "Added";
                }
                else
                {
                    return "Exists";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public bool IsUserDeleted(long UserId)
        {
            try
            {
                User user = _db.Users.FirstOrDefault(u => u.UserId == UserId);
                if (user != null)
                {
                    user.DeletedAt = DateTime.Now;
                    _db.Update(user);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception )
            {
                return false;
            }
        }

        public AdminUserViewModel GetDataOnEdit(long UserId)
        {
            User user = _db.Users.Find(UserId);
            AdminUserViewModel vm = new AdminUserViewModel(user);
            return vm;
        }
        public bool EditUser( AdminUserViewModel vm )
        {
            User user = _db.Users.Find(vm.UserId);
            if(user != null)
            {
                user.FirstName = vm.FirstName;
                user.LastName = vm.Surname;
                user.Email = vm.Email;
                user.PhoneNumber = vm.PhoneNumber;
                user.EmployeeId = vm.EmployeeId;
                user.Department = vm.Department;
                user.UpdatedAt = DateTime.Now;
                user.CountryId = vm.CountryId;
                user.CityId = vm.CityId;
                user.Password = vm.Password;
                user.Availability = vm.Availibility;
                user.Status = vm.Status;        
                _db.Users.Update(user);
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