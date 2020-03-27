using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    public class Lexema
    {
        private string id;
        private string lexema;

        public Lexema(string id, string lexema) {

            this.id = id;
            this.lexema = lexema;
        }

        public string getId() { return this.id;  }
        public string getLex() { return this.lexema; }
    }
}
