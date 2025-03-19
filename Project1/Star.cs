using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Star
    {
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public string? Description { get; set; }
        public bool? Male { get; set; }
        public string? Nationality { get; set; }

        public override string ToString()
        {
            return $"{Name} {Dob?.ToString("MM/dd/yyyy") ?? ""} {Description ?? ""} {Male.ToString()} {Nationality ?? ""}";
        }
    }
}
