using System;
using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Api.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The adddress is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The phone is required")]
        public string Phone { get; set; }

        public string UserType { get; set; }
        
        public string Money { get; set; }

        public UserModel(string Name, string Email, string Phone, string Address, string UserType, string Money)
        {
            this.Name = Name;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.UserType = UserType;
            this.Money = Money;
        }

        public UserModel()
        {
            this.UserType = "Normal";
            this.Money = "0";
        }
    }
}
