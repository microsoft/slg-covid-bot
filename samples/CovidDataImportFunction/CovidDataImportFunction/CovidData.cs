using System;
using System.Collections.Generic;
using System.Text;

namespace CovidDataImportFunction
{
    public class CovidData
    {
        public int? FIPS { get; set; }
        public string Admin2 { get; set; }
        public string Province_State { get; set; }
        public string Country_Region { get; set; }
        public DateTime Last_Update { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
        public int Active { get; set; }
        public string Combined_key { get; set; }
    }
}
