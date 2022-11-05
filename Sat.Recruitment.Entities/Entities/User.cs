using System;
using System.Linq;
using CsvHelper.Configuration.Attributes;

namespace Sat.Recruitment.Domain.Entities
{
    public class User
    {
        [Index(0)]
        public int Id { get; set; }
        [Index(1)]
        public string Name { get; set; }
        [Index(2)]
        public string Email { get; set; }
        [Index(3)]
        public string NormalizedEmail { get; set; }
        [Index(4)]
        public string Phone { get; set; }
        [Index(5)]
        public string Address { get; set; }
        [Index(6)]
        public string UserType { get; set; }
        [Index(7)]
        public decimal Money { get; set; }
        [Index(8)]
        public decimal CalculatedMoney { get; set; }

        public User()
        {
            Id = 0;
            Name = "";
            Email = "";
            NormalizedEmail = "";
            Phone = "";
            Address = "";
            UserType = "Normal";
            Money = 0;
            CalculatedMoney = 0;
        }
        
        public override string ToString()
        {
            return $"{Id},{Name},{Email},{NormalizedEmail},{Phone},{Address},{UserType},{Money},{CalculatedMoney}";
        }
    }
}
