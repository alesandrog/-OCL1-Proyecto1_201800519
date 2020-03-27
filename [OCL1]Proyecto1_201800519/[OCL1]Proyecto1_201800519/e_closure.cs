using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    public class e_closure
    {

        private Estado id;
        private LinkedList<Estado> op_closure = new LinkedList<Estado>();


        public e_closure(Estado id) {
            this.id = id;
        }

        public e_closure(Estado id, LinkedList<Estado> closures)
        {

            this.id = id;
            this.op_closure = closures;
        }

        public Estado getID() { return this.id; }
        public void setID(Estado id) { this.id = id; }
        public LinkedList<Estado> getClosures() { return this.op_closure; }
        public void addClosure(Estado e) { op_closure.AddLast(e); }
    }
}
