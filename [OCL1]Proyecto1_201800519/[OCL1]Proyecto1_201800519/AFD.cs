using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    public class AFD
    {

        private Automaton afn = new Automaton();
        private Automaton afd = new Automaton();
        LinkedList<e_closure> cerraduras = new LinkedList<e_closure>();
        private  LinkedList<Estado> cerraduras_temporales = new LinkedList<Estado>();
        private  LinkedList<Estado> mueves_temporales = new LinkedList<Estado>();
        private LinkedList<string> alphabet = new LinkedList<string>();
        Stack<object>  afd_stack = new Stack<object>();
        string id = "";
 
        public AFD(Automaton afn, LinkedList<string> alphabet, string id) {
            this.afn = afn;
            this.alphabet = alphabet;
            this.id = id;
        }


        public void construir_afd() {

            Estado inicio = new Estado(0);
            afd.addEstados(inicio);
            afd.setBegin(inicio);
            afd.setAlphabet(alphabet);
            HashSet<HashSet<Estado>> cerraduras = new HashSet<HashSet<Estado>>();
            Queue<HashSet<Estado>> pila = new Queue<HashSet<Estado>>();
            int identificador = 0;
            LinkedList<Estado> lista_inicial = new LinkedList<Estado>();
            lista_inicial.AddLast(afn.getBegin());
            HashSet<Estado> cerradura_inicial = e_cerradura(lista_inicial);
            foreach (Estado final in  afn.getEstadosAceptacion())
            {
                if (cerradura_inicial.Contains(final))
                    afd.addEstadosAceptacion(inicio);
            }
       
            pila.Enqueue(cerradura_inicial);

            while (pila.Count() > 0) {

                HashSet<Estado> aux = pila.Dequeue();
                foreach (string id in alphabet) {

                    LinkedList<Estado> res_mueve = mueve(aux, id);
                    HashSet<Estado> res_cerradura = new HashSet<Estado>();
                    res_cerradura = e_cerradura(res_mueve);
                    Estado anterior = afd.getEstados().ElementAt(identificador);
                    if (res_cerradura.Count() > 0) {
                        if (existe_estado(cerraduras, res_cerradura))
                        {
                            LinkedList<Estado> estados_actuales = afd.getEstados();
                            Estado actual = anterior;
                            Estado siguiente = estados_actuales.ElementAt(posicion_estado(cerraduras, res_cerradura)+1);
                            actual.setTransitions(new Transition(actual, siguiente, id));

                        }
                        else
                        {

                            cerraduras.Add(res_cerradura);
                            pila.Enqueue(res_cerradura);

                            Estado agregar = new Estado(cerraduras.Count());
                            anterior.setTransitions(new Transition(anterior, agregar, id));
                            afd.addEstados(agregar);

                            foreach (Estado e in afn.getEstadosAceptacion())
                            {
                                if (res_cerradura.Contains(e))
                                    if (!afd.getEstadosAceptacion().Contains(agregar))
                                        afd.addEstadosAceptacion(agregar);
                            }
                        }
                    }
                }
                identificador++;               
            }
            afd.setId(id);
            Form1.automatons.AddLast(afd);
            afd.setType("AFD");
            Graphviz g = new Graphviz();
            g.graph_automaton2(generarDOT(id + " : AFD", afd));
        }


        public HashSet<Estado> e_cerradura(LinkedList<Estado> estados) {

            HashSet<Estado> resultado = new HashSet<Estado>();
            foreach (Estado estado in estados) {
                Stack<Estado> pila = new Stack<Estado>();
                Estado actual = estado;
                pila.Push(actual);
                while (pila.Count() > 0)
                {

                    actual = pila.Pop();
                    foreach (Transition t in actual.getTransitions())
                    {

                        if ((string)t.getSimbol() == Automaton.EPSILON && !resultado.Contains(t.getEnd()))
                        {
                            resultado.Add(t.getEnd());
                            pila.Push(t.getEnd());
                        }
                    }
                }
                if (!resultado.Contains(estado)) {
                    resultado.Add(estado);
                }

            }

            return resultado;
        }


        public LinkedList<Estado> mueve(HashSet<Estado> estados, string simbolo)
        {
            LinkedList<Estado> resultado = new LinkedList<Estado>();

            foreach (Estado e in estados) {
                foreach (Transition t in e.getTransitions())
                {
                    //                    if ((string)t.getSimbol() == simbolo && !resultado.Contains(t.getEnd()))

                    if ((string)t.getSimbol() == simbolo && !resultado.Contains(t.getEnd()))
                    {
                        resultado.AddLast(t.getEnd());
                    }
                }
            }

            return resultado;
        }

        public bool existe_estado(HashSet<HashSet<Estado>> general, HashSet<Estado> especifica) {

            foreach (HashSet<Estado> aux in general) {
                if (aux.SetEquals(especifica))
                    return true;
            }
            return false;
        }

        public int posicion_estado(HashSet<HashSet<Estado>> general, HashSet<Estado> especifica)
        {
            int res = 999;
            for (int i = 0; i < general.Count(); i++) {
                if (general.ElementAt(i).SetEquals(especifica))
                    return i;
            }
            return res;
        }

        public String generarDOT(String nombreArchivo, Automaton automataFinito)
        {
            String texto = "digraph automata_finito {\n";

            texto += "\trankdir=LR;" + "\n";

            texto += "\tgraph [label=\"" + nombreArchivo + "\", labelloc=t, fontsize=20]; \n";
            texto += "\tnode [shape=doublecircle, style = filled,color = lightblue];";

            for (int i = 0; i < automataFinito.getEstadosAceptacion().Count(); i++)
            {
                texto += " " + automataFinito.getEstadosAceptacion().ElementAt(i).getId();
            }

            texto += ";" + "\n";
            texto += "\tnode [shape=circle];" + "\n";
            texto += "\tnode [color=lightblue,fontcolor=black];\n" + "	edge [color=lightblue];" + "\n";

            texto += "\tsecret_node [style=invis];\n" + "	secret_node -> " + automataFinito.getBegin().getId().ToString() + " [label=\"inicio\"];" + "\n";



            for (int i = 0; i < automataFinito.getEstados().Count(); i++)
            {
                LinkedList<Transition> t = automataFinito.getEstados().ElementAt(i).getTransitions();
                for (int j = 0; j < t.Count(); j++)
                {
                    texto += "\t" + t.ElementAt(j).DOT_String() + "\n";
                }

            }
            texto += "}";



            return texto;
        }
    }




}
