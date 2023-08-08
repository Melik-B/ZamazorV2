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
    public class CategoryModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(700, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Number of Products")]
        public int ProductCountDisplay { get; set; }
    }
}