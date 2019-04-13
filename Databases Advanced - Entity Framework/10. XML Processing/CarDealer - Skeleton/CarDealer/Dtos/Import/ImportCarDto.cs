using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Car")]
    public class ImportCarDto
    {
        //Cars>
        //<Car>
        //<make>Opel</make>
        //<model>Omega</model>
        //<TraveledDistance>176664996</TraveledDistance>
        //<parts>
        //<partId id = "38" />
        //    < partId id="102"/>

        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public ImportPartIdDto[] Parts { get; set; }
    }
}
