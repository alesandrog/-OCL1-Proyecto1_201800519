using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    public class Conjunto
    {
        private string id;
        private LinkedList<string> valores;

        public Conjunto(string id, LinkedList<string> valores)
        {
            this.id = id;
            this.valores = valores;
        }

        public string getId() { return this.id; }
        public LinkedList<string> getValores() { return this.valores; }
    }
}
