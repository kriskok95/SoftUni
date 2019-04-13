﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Castle.DynamicProxy.Generators.Emitters;

namespace CarDealer.Dtos.Export
{
    [XmlType("part")]
    public class ExportPartDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlAttribute("price")]
        public decimal Price { get; set; }

    }
}
