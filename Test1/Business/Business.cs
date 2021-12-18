using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            data.Load(ConfigurationManager.AppSettings["Datasource"]);
        }

        /// <summary>
        /// Gets all users from XML and converts to a list of users
        /// </summary>
        /// <returns></returns>
        public UserCollection GetUsers()
        {
            try
            {
                var userList = DeserializeToObject<UserCollection>(ConfigurationManager.AppSettings["Datasource"]);
                return userList;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Created a new user in the XML
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public Status AddUser(User newUser)
        {
            try
            {
                XmlNode root1 = data.DocumentElement;

                XmlElement XEle = data.CreateElement("User");
                XEle.SetAttribute("Id", newUser.Id.ToString());
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

        /// <summary>
        /// Update existing user in XML
        /// </summary>
        /// <param name="userToUpdate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes the selected user from the XML file
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Generic method to deserialize xml to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filepath"></param>
        /// <returns></returns>
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