using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
   public class Estado:ICloneable
    {


        private int id;
        private LinkedList<Transition> transitions = new LinkedList<Transition>();
        private LinkedList<Estado> transition = new LinkedList<Estado>();
        private bool marcado = false;

        public Estado(int id, LinkedList<Transition> transitions)
        {
            this.id = id;
            this.transitions = transitions;
        }

        public Estado(int id)
        {
            this.id = id;

        }


        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public LinkedList<Transition> getTransitions()
        {

            return transitions;
        }

        public void setTransitions(Transition transition)
        {
            this.transitions.AddLast(transition);
        }

        public object Clone()
        {

            Estado clon = new Estado(id);
            return clon;
        }

        public void marcar() {
            this.marcado = true;
        }

        public bool getMarca() {
            return marcado;
        }
    }
}
