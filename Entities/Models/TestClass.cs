using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class TestClass
    {

        public Guid Id { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}
