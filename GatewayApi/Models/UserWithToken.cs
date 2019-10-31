﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayApi.Models
{
    public class UserWithToken
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
