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
    public class StoreModel : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public bool IsVirtual { get; set; }

        [Range(1, 5, ErrorMessage = "{0} must be between {1} and {2}!")]
        [DisplayName("Rating")]
        public byte? Rating { get; set; }

        public string IsVirtualDisplay { get; set; }
    }
}