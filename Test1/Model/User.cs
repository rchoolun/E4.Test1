using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Test1.Model
{
    [XmlRoot(ElementName = "Users")]
    public class UserCollection
    {
        [XmlElement(ElementName = "User")]
        public List<User> Users { get; set; }
    }

    [Serializable]
    public class User
    {
        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        [XmlAttribute("Name")]
        [Required]
        public string Name { get; set; }

        [XmlAttribute("Surname")]
        [Required]
        public string Surname { get; set; }

        [XmlAttribute("Phone")]
        [Required]
        public string Phone { get; set; }
    }
}