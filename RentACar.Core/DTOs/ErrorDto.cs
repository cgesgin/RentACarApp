using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class ErrorDto
    {
        public List<string> Errors { get; set; } =  new List<string>();
    }
}
