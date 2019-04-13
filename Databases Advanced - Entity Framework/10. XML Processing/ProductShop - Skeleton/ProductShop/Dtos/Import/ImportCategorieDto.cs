using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType("Category")]
    public class ImportCategorieDto
    {
        [Required]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
