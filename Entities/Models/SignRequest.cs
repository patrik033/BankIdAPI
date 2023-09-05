using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class SignRequest
    {

        [Required]
        public string endUserIp { get; set; }
        //public object? requirement { get; set; }

        [Required]
        public string userVisibleData { get; set; }

        //public string? userNonVisibleData { get; set; }
        [Required]
        public string userVisibleDataFormat { get; set; }
    }
}
