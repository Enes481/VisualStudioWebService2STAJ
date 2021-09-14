
using EDMTAPIAuthentication7.Models;
using EDMTAPIAuthentication7.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EDMTAPIAuthentication7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class registerController : ControllerBase
    {
        NetCoreAuthenticationContext dbContext = new NetCoreAuthenticationContext();

        // POST api/<ValuesController1>
        [HttpPost]
        public String Post([FromBody] TblUser value)
        {
            // First we need check user have existing in database 
            if (!dbContext.TblUsers.Any(user => user.UserName.Equals(value.UserName)))
            {
                TblUser user = new TblUser();
                user.UserName = value.UserName;
                user.Password=value.Password;//assign value from post to user
                user.Salt = Convert.ToBase64String(common.GetRandomSalt(16));

                user.Password = Convert.ToBase64String(common.SaltHashPassword(
                    Encoding.ASCII.GetBytes(value.Password),
                    Convert.FromBase64String(user.Salt)));

                //add to database

                try
                {
                    dbContext.Add(user);
                    dbContext.SaveChanges();
                    return JsonConvert.SerializeObject("Kayıt başarılı");

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(ex.Message);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("kullanıcı veri tabanında mevcut");
            }
        }


    }
}
