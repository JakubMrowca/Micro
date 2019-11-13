using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GatewayApi.Models;
using User.Domain.Entity;
using User.Shared.Dto;

namespace User.Services.Repositories.Read
{
    public class UsersReadRepository
    {
        private List<UserVm> Users = new List<UserVm>
        {
            new UserVm
            {
                Name = "jmrowca", Description = "Jakub Mrowca", CompanyName = "Abis", Email = "jmrowca@abis.krakow.pl",
                Id = 1, Role = "Worker"
            },
            new UserVm
            {
                Name = "jengel", Description = "Jerzy Engel", CompanyName = "Engel solution", Email = "", Id = 2,
                Role = "User"
            },
            new UserVm
            {
                Name = "admin", Description = "Administrator", CompanyName = "", Email = "", Id = 3,
                Role = "Administrator"
            },
        };

        public UserVm GetUser(string name)
        {
            return Users.Select( x => new UserVm
            {
                CompanyName = x.CompanyName,Name = x.Name
                ,Description = x.Description,Role = x.Role,Email = x.Email,Id = x.Id
            }).FirstOrDefault(x => x.Name == name);
        }

        public List<UserVm> GetAll()
        {
            return Users;
        }
    }
}
