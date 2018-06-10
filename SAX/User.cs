using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAX
{
    class User
    {
        private string name;
        private string id;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public HashSet<string> skills = new HashSet<string>();
    }
}
