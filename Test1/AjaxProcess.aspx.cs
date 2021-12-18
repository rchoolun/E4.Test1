using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using Test1.Model;

namespace Test1
{
    public partial class AjaxProcess : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var method = HttpContext.Current.Request["method"];

            if(method != null)
            {
                if (method.Equals("GetAllUsers"))
                {
                    GetUsers();
                }
            }          
        }

        /// <summary>
        /// Gets all users from the XML file
        /// </summary>
        protected void GetUsers()
        {
            var business = new Business.Business();

            //Serialize the list of user object to JSON
            var users = JsonConvert.SerializeObject(business.GetUsers());

            //Select the require token from the JSON 
            var obj = JObject.Parse(users);
            var json = obj.SelectToken("Users");

            Response.ContentType = "application/json";
            Response.Write(json);
        }


        [WebMethod]
        /// <summary>
        /// Update/Add the required user
        /// </summary>
        /// <returns></returns>
        public static string SaveUser(string Id, string Name, string Surname, string Phone)
        {
            //User validation
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Phone))
            {
                return "Please make sure all the information are filled IN";
            }

            if (!Regex.Match(Name, "^[A-Z][a-zA-Z]*$").Success)
            {
                return "Name is incorrect";
            }

            if (!Regex.Match(Surname, "^[A-Z][a-zA-Z]*$").Success)
            {
                return "Surname is incorrect";
            }

            if (!Regex.Match(Phone, "^[0-9]*$").Success)
            {
                return "Phone number is incorrect";
            }

            User user;
            var business = new Business.Business();

            Status status;

            if (string.IsNullOrEmpty(Id))
            {
                user = new User { Id = Guid.NewGuid(), Name = Name, Surname = Surname, Phone = Phone };

                status = business.AddUser(user);
            }
            else
            {
                user = new User { Id = new Guid(Id), Name = Name, Surname = Surname, Phone = Phone };

                status = business.UpdateUser(user);
            }

            if (status.IsSuccess)
            {
                return "The user data has been updated successfully";
            }

            return status.Message;
        }

        /// <summary>
        /// Deletes the user from XML
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [WebMethod]
        public static string DeleteUser(string Id)
        {
            var business = new Business.Business();

            var status = business.RemoveUser(Id);

            if (status.IsSuccess)
            {
                return "The user has been deleted successfully";
            }

            return status.Message;
        }
    }
}