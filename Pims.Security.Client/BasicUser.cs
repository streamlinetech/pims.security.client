using System;
using Streamline.Pims.Security.Client.Dtos;

namespace Streamline.Pims.Security.Client
{
    public class BasicUser : IBasicUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public dynamic StinvUser { get; set; }
    }
}