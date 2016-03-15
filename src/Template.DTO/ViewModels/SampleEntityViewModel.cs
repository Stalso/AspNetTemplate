using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.DTO.ViewModels
{
    public class SampleEntityViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class SampleEntityViewModelA:SampleEntityViewModel
    {
        public string ADesc { get; set; }
    }
}
