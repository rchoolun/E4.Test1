using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Test1.Model;

namespace Test1
{
    public partial class AjaxProcess : System.Web.UI.Page
    {
        private int currentPageIndex;

        private string order = string.Empty;

        private string orderBy = string.Empty;

        private int limit;

        private string sort = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            var method = HttpContext.Current.Request["method"];

            currentPageIndex = (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["page"])) ? Convert.ToInt32(HttpContext.Current.Request.Form["page"]) : 1;
            limit = (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["limit"])) ? Convert.ToInt32(HttpContext.Current.Request.Form["limit"]) : 10;
            sort = (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["sortorder"])) ? HttpContext.Current.Request.Form["sortorder"] : "";
            order = (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["sortname"])) ? HttpContext.Current.Request.Form["sortname"] : "";
            orderBy = string.Concat(order, "_", sort);

            if (method.Equals("GetAllUsers"))
            {
                GetUsers();
            }

            if (method.Equals("CreateUser"))
            {
                var sr = new StreamReader(Request.InputStream);
                var jsonObject = JObject.Parse(sr.ReadLine());

                var name = jsonObject["Name"].ToString();
                var surname = jsonObject["Surname"].ToString();
                var phone = jsonObject["Phone"].ToString();

                var newUser = new User { Id = Guid.NewGuid(), Name = name, Surname = surname, Phone = phone };

                Response.Write(JsonConvert.SerializeObject(CreateUser(newUser)));
            }

            if (method.Equals("DeleteUser"))
            {
                var sr = new StreamReader(Request.InputStream);
                var jsonObject = JObject.Parse(sr.ReadLine());

                var id = jsonObject["Id"].ToString();

                Response.Write(JsonConvert.SerializeObject(DeleteUser(id)));
            }

            if (method.Equals("UpdateUser"))
            {
                var sr = new StreamReader(Request.InputStream);
                var jsonObject = JObject.Parse(sr.ReadLine());

                var id = jsonObject["Id"].ToString();
                var name = jsonObject["Name"].ToString();
                var surname = jsonObject["Surname"].ToString();
                var phone = jsonObject["Phone"].ToString();

                var newUser = new User { Id = new Guid(id), Name = name, Surname = surname, Phone = phone };

                Response.Write(JsonConvert.SerializeObject(UpdateUser(newUser)));
            }
        }

        /// <summary>
        /// Get all users in the XML file
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

        /// <summary>
        /// Adds a new user in the XML file
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="isBlank"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string CreateUser(User newUser)
        {
            var business = new Business.Business();

            var status = business.AddUser(newUser);

            if(status.IsSuccess)
            {
                return "The user has been added successfully";
            }

            return status.Message;
        }

        /// <summary>
        /// Adds a new user in the XML file
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="isBlank"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string DeleteUser(string Id)
        {
            var business = new Business.Business();

            var status = business.RemoveUser(Id);

            if (status.IsSuccess)
            {
                return "The user has been deleted successfully";
            }

            return status.Message;
        }

        /// <summary>
        /// Update the required user
        /// </summary>
        /// <returns></returns>
        protected string UpdateUser(User newUser)
        {
            var business = new Business.Business();

            var status = business.UpdateUser(newUser);

            if (status.IsSuccess)
            {
                return "The user has been updated successfully";
            }

            return status.Message;
        }
    }
}