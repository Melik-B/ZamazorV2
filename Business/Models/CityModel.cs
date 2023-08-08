using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CityModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("City Name")]
        public string Name { get; set; }

        public int CountryId { get; set; }

        [DisplayName("Country Name")]
        public string CountryNameDisplay { get; set; }
    }
}