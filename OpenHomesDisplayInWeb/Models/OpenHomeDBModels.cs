using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHomesDisplayInWeb
{
    public class RegionsClass
    {
        public static List<Region> RegionsList { get; set; }
    }

    public class Region
    {
        [Key]
        public int Id { get; set; }
        public string RegionName { get; set; }
        public List<District> DistrictList { get; set; }
    }

    public class District
    {
        [Key]
        public int Id { get; set; }
        public string DistrictName { get; set; }
        public List<Suburb> SuburbList { get; set; }
        public int RegionId { get; set; }
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }
    }

    public class Suburb
    {
        [Key]
        public int Id { get; set; }
        public string SuburbName { get; set; }
        public virtual List<OpenHome> HomeList { get; set; }
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
    }

    public class OpenHome
    {
        [Key]
        public int Sl_No { get; set; }
        public string Location { get; set; }
        public string Rooms { get; set; }
        public string PropertyType { get; set; }
        public string Price { get; set; }
        public string Parking { get; set; }
        public string OpenHomeTime { get; set; }
        public int SuburbId { get; set; }
        [ForeignKey("SuburbId")]
        public virtual Suburb Suburb { get; set; }
    }
}
