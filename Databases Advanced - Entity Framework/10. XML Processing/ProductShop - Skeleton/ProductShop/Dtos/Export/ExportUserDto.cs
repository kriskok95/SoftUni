using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Castle.DynamicProxy.Contributors;

namespace ProductShop.Dtos.Export
{
    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public ExportSoldProductsWithCountDto SoldProducts { get; set; }
    }
}
