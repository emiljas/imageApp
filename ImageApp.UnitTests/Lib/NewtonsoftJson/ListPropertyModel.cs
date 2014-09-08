using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.UnitTests.Lib.NewtonsoftJson
{
    public class ListPropertyModel
    {
        private List<string> list;
        public List<string> List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }
    }
}
