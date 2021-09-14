
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
    public class LoginController : ControllerBase
    {

        NetCoreAuthenticationContext dbContext = new NetCoreAuthenticationContext();



        // POST api/<LoginController>
        [HttpPost]
        public String Post([FromBody] TblUser value)
        {
            //check exist
            //First , we need check user have existing in database ?

            if (dbContext.TblUsers.Any(user => user.UserName.Equals(value.UserName)))
            {
                TblUser user = dbContext.TblUsers.Where(u => u.UserName.Equals(value.UserName)).First();
                //calculate hash password from data of client and compare with hash in server with salt
                var client_post_hash_password = Convert.ToBase64String(
                    common.SaltHashPassword(
                        Encoding.ASCII.GetBytes(value.Password),
                        Convert.FromBase64String(user.Salt)));

                if (/*client_post_hash_password*/value.Password.Equals(user.Password))
                    return JsonConvert.SerializeObject(user);
                else
                    return JsonConvert.SerializeObject("Yanlış parola");
            }
            else
            {
                return JsonConvert.SerializeObject("Kullanıcı veri tabanında bulunamadı.");
            }

        }


    }
}
