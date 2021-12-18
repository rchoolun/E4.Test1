using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using Test1.Model;

namespace Test1.Business
{
    public class Business
    {
        XmlDocument data;

        public Business()
        {
            data = new XmlDocument();
            data.Load("App_Data/Data.xml");
        }

        /// <summary>
        /// Returns a JSON string
        /// </summary>
        /// <returns></returns>
        public UserCollection GetUsers()
        {
            try
            {
                var userList = DeserializeToObject<UserCollection>("App_Data/Data.xml");

                //var json = JsonConvert.SerializeXmlNode(data, Newtonsoft.Json.Formatting.None, true);
                return userList;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Status AddUser(User newUser)
        {
            try
            {
                XmlNode root1 = data.DocumentElement;

                XmlElement XEle = data.CreateElement("User");
                XEle.SetAttribute("Id", Guid.NewGuid().ToString());
                XEle.SetAttribute("Name", newUser.Name);
                XEle.SetAttribute("Surname", newUser.Surname);
                XEle.SetAttribute("Phone", newUser.Phone);
                data.DocumentElement.AppendChild(XEle.Clone());
                data.Save("App_Data/Data.xml");

                return new Status { IsSuccess = true };
            }
            catch(Exception ex)
            {
                return new Status { IsSuccess = false, Message = ex.Message };
            }
        }

        public Status UpdateUser(User userToUpdate)
        {
            try
            {
                XmlNode nodeToUpdate = data.SelectSingleNode("/Users/User[@Id='" + userToUpdate.Id + "']");

                if (nodeToUpdate != null)
                {
                    nodeToUpdate.Attributes["Name"].Value = userToUpdate.Name;
                    nodeToUpdate.Attributes["Surname"].Value = userToUpdate.Surname;
                    nodeToUpdate.Attributes["Phone"].Value = userToUpdate.Phone;
                }
                data.Save("App_Data/Data.xml");

                return new Status { IsSuccess = true };
            }
            catch(Exception ex)
            {
                return new Status { IsSuccess = false, Message = ex.Message };
            }
            
        }

        public Status RemoveUser(string Id)
        {
            try
            {
                XmlNode nodeToDelete = data.SelectSingleNode("/Users/User[@Id='" + Id + "']");

                if (nodeToDelete != null)
                {
                    nodeToDelete.ParentNode.RemoveChild(nodeToDelete);
                }
                data.Save("App_Data/Data.xml");

                return new Status { IsSuccess = true };
            }
            catch(Exception ex)
            {
                return new Status { IsSuccess = false, Message = ex.Message };
            }
        }

        public T DeserializeToObject<T>(string filepath) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(filepath))
            {
                return (T)ser.Deserialize(sr);
            }
        }

    }
}