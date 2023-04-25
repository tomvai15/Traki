﻿namespace Traki.Api.Contracts.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string? UserIconBase64 { get; set; }
    }
}
