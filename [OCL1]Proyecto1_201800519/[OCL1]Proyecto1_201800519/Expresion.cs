using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    public class Expresion
    {
        private string id;
        private LinkedList<Token> tokens;

        public Expresion(string id, LinkedList<Token> valores)
        {
            this.id = id;
            this.tokens = valores;
        }

        public string getId() { return this.id; }
        public LinkedList<Token> getValores() { return this.tokens; }
    }
}
