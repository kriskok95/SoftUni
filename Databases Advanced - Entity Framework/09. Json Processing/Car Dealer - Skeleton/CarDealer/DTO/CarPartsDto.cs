using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CarDealer.Models;
using Newtonsoft.Json;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;

namespace CarDealer.DTO
{
    public class CarPartsDto
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public int TravelledDistance { get; set; }

        public int[] PartsId { get; set; }
    }
}
