using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Sat.Recruitment.DataAccess.Contracts;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IList<User> users;
        private readonly IFileHelper fileHelper;
        private string path;

        public UserRepository(string path, IFileHelper fileHelper)
        {
            this.path = path;
            this.fileHelper = fileHelper;

            var res = ReadUsersFromFile();
            res.Wait();

            users = res.Result; 
        }

        private async Task<IList<User>> ReadUsersFromFile()
        {
            IList<User> _users = new List<User>();
            
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
                using (MemoryStream ms = fileHelper.GetFileData(path))
                using (StreamReader reader = new StreamReader(ms))
                using (var csv = new CsvReader(reader, config))
                {
                    await foreach(var rec in csv.GetRecordsAsync<User>())
                    {
                        _users.Add(rec);
                    }
                }
            }
            catch
            {
                _users = new List<User>();
            }

            return _users;
        }

        public IList<User> GetUsers()
        {
            return users;
        }

        public async Task<User> Add(User user)
        {
            User result = null;

            try
            {
                if (user == null)
                {
                    throw new Exception("User is null.");
                }
                user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
                users.Add(user);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };

                using (MemoryStream ms = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(ms))
                using (CsvWriter csvWriter = new CsvWriter(writer, config))
                {
                    await csvWriter.WriteRecordsAsync<User>(users);
                    writer.Flush();
                    fileHelper.SaveFileData(path, ms);
                }
                
                
                result = user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<bool> Edit(int id, User user)
        {
            bool result = false;

            try
            {
                if (user == null)
                {
                    throw new Exception("User is null.");
                }
                int idx = users.ToList().FindIndex(u => u.Id == id);
                if (idx < 0) { throw new Exception("The user does not exist."); }

                user.Id = id;
                users[idx] = user;

                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
                using (MemoryStream ms = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(path))
                using(CsvWriter csvWriter = new CsvWriter(writer, config))
                {
                    await csvWriter.WriteRecordsAsync<User>(users);
                    fileHelper.SaveFileData(path, ms);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            bool result = false;            

            try
            {
                int idx = users.ToList().FindIndex(u => u.Id == id);
                if (idx < 0) { throw new Exception("The user does not exist."); }
                users.RemoveAt(idx);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
                using (MemoryStream ms = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(path))
                using (CsvWriter csvWriter = new CsvWriter(writer, config))
                {
                    await csvWriter.WriteRecordsAsync<User>(users);
                    fileHelper.SaveFileData(path, ms);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
