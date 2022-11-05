using System;
namespace Sat.Recruitment.Services.Implementations
{
    public partial class UserService
    {
        private string GetNormalizedEmail(string email)
        {
            string[] userDomain = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            int idxPlus = userDomain[0].IndexOf('+', StringComparison.Ordinal);
            string userNamePart = idxPlus >= 0 ? userDomain[0].Remove(idxPlus) : userDomain[0];
            return $"{userNamePart.Replace(".", "").ToLower()}@{userDomain[1].ToLower()}";
        }

        private decimal GetCalculatedMoney(decimal money, string userType)
        {
            decimal gifPercentage = 0;

            switch (userType)
            {
                case "Normal":
                    {
                        if (money > 100)
                        {
                            gifPercentage = Convert.ToDecimal(0.12);
                        }
                        else if (money <= 100 && money > 10)
                        {
                            gifPercentage = Convert.ToDecimal(0.8);
                        }
                        break;
                    }
                case "SuperUser":
                    {
                        if (money > 100)
                        {
                            gifPercentage = Convert.ToDecimal(0.20);
                        }
                        break;
                    }
                case "Premium":
                    {
                        if (money > 100)
                        {
                            gifPercentage = Convert.ToDecimal(2);
                        }
                        break;
                    }
            }

            return money * (1 + gifPercentage);
        }
    }
}
