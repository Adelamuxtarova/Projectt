using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Entities
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        private Decimal _grossPay { get; set; }
        public Decimal GrossPay { get; set; }
        public Decimal Hourly { get; set; }
        public Decimal Hours { get; set; }
        public string FilePath { get; set; }

    }
}
