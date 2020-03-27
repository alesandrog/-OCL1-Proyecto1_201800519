using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    public class Automaton:ICloneable
    {

        public static string EPSILON = "ε";

        public static char EPSILON_CHAR = EPSILON.ElementAt(0);

        private string id;

        private Estado begin;

        private LinkedList<Estado> end;
        
        private LinkedList<Estado> estados;
        public LinkedList<string> alphabet = new LinkedList<string>();

        private string type;

        public Automaton()
        {
            this.estados = new LinkedList<Estado>();
            this.end = new LinkedList<Estado>();

        }

        public Automaton(LinkedList<Estado> estados, LinkedList<Estado> end , Estado begin, string type) {

            this.estados = estados;
            this.end = end;
            this.begin = begin;
            this.type = type;

        }

        public Estado getBegin()
        {
            return begin;
        }

        public void setBegin(Estado begin)
        {
            this.begin = begin;
        }

        public LinkedList<Estado> getEstadosAceptacion()
        {
            return end;
        }

        public void addEstadosAceptacion(Estado end)
        {
            this.end.AddLast(end);
        }


        public LinkedList<Estado> getEstados()
        {
            return estados;
        }

        public Estado getEstados(int index)
        {
            return estados.ElementAt(index);
        }


        public void addEstados(Estado estado)
        {
            this.estados.AddLast(estado);
        }

        public void setType(String type)
        {
            this.type = type;
        }

        public String getType()
        {
            return this.type;
        }

        public LinkedList<string> getAlphabet() { return this.alphabet; }
        public void setAlphabet(LinkedList<string> alphabet) { this.alphabet = alphabet; }

        public object Clone()
        {

            Automaton clon = new Automaton();
            LinkedList<Estado> estados_clon = new LinkedList<Estado>();
            LinkedList<Estado> end_clon = new LinkedList<Estado>();

            foreach (Estado e in this.getEstados()) {

                    Estado cln = (Estado) e.Clone();
                    estados_clon.AddLast(cln);
                }



                foreach (Estado e2 in this.end) {

                    Estado cln2 = (Estado)e2.Clone();
                    end_clon.AddLast(cln2);

                }

                clon.begin = (Estado)begin.Clone();
                clon.estados = estados_clon;
                clon.end = end_clon;
                clon.type = type; 
            return clon;
        }

        public string getId() { return this.id; }
        public void setId(string id) { this.id = id; }
    }
}
