using System;
using System.Collections.Generic;

namespace MyDiet_API.Shared.Auth
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess{ get; set; }

        public string Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
