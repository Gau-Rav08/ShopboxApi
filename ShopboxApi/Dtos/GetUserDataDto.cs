using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopboxApi.Dtos
{
    public class GetUserDataDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
