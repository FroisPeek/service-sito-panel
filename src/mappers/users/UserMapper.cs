using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ServiceSitoPanel.src.dtos.users;
using ServiceSitoPanel.src.helpers;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.mappers.users
{
    public class UserMapper
    {
        public Users MappUserDto(CreateUserDto user, int tenant_id)
        {
            return new Users
            {
                name = user.name,
                password = user.password,
                profile = user.profile,
                active = true,
                created_at = DateTime.Now.NowInBrasilia(),
                tenant_id = tenant_id
            };
        }
    }
}