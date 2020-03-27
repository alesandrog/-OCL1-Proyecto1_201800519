using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OCL1_Proyecto1_201800519
{
    public class AFN
    {

        private Automaton afn;
        private LinkedList<string> regex;
        private LinkedList<Token> token_regex;
        private string id = "";
        public AFN(LinkedList<string> regex)

        {
            this.regex = regex;
        }

        public AFN(LinkedList<Token> token_regex, string id)
        {
            this.token_regex = token_regex;
            this.id = id;
        }


        public void thompson2()
        {


            LinkedList<string> alphabet = new LinkedList<string>();
            Stack<Object> thompson_stack = new Stack<Object>();

            foreach (Token token in token_regex)
            {

                switch (token.getTipo())
                {


                    case Token.Tipo.OPERADOR_OR:

                        Automaton a1_or = (Automaton)thompson_stack.Pop();
                        Automaton a2_or = (Automaton)thompson_stack.Pop();
                        Automaton or = this.or(a1_or, a2_or);
                        thompson_stack.Push(or);


                        this.afn = or;

                        break;

                    case Token.Tipo.OPERADOR_CONCAT:

                        Automaton a1_and = (Automaton)thompson_stack.Pop();
                        Automaton a2_and = (Automaton)thompson_stack.Pop();
                        Automaton and = this.and(a1_and, a2_and);
                        thompson_stack.Push(and);
                        this.afn = and;
                        break;

                    case Token.Tipo.OPERADOR_KLEENE:

                        Automaton a1_kleene = (Automaton)thompson_stack.Pop();
                        Automaton kleene = cerradura_kleene(a1_kleene);
                        thompson_stack.Push(kleene);
                        this.afn = kleene;

                        break;

                    case Token.Tipo.OPERADOR_POSITIVA:

                        Automaton c1 = (Automaton)thompson_stack.Pop();
                        Automaton c4 = (Automaton)this.cerradura_positiva(c1);
                        Automaton c2 = (Automaton)this.cerradura_kleene(c1);
                        Automaton c3 = this.and(c2, c4);
                        thompson_stack.Push(c3);
                        this.afn = c3;

                        break;

                    case Token.Tipo.OPERADOR_INTE:

                        Automaton interrogation_simbol = new Automaton();
                        Automaton a1_inte = this.simbol(Automaton.EPSILON);
                        Automaton a2_inte = (Automaton)thompson_stack.Pop();
                        interrogation_simbol = this.or(a1_inte, a2_inte);
                        thompson_stack.Push(interrogation_simbol);
                        this.afn = interrogation_simbol;
                        break;

                    default:
                        Automaton simbol = this.simbol(token.Getval());
                        thompson_stack.Push(simbol);
                        this.afn = simbol;
                        if (!alphabet.Contains(token.Getval()))
                        {
                            alphabet.AddLast(token.Getval());
                        }

                        break;
                }


            }
            this.afn.setType("AFN");
            thompson_stack.Clear();
            Graphviz g = new Graphviz();
            g.graph_automaton(generarDOT(id + " : AFN", this.afn));
            AFD afd = new AFD(this.afn, alphabet, id);
            afd.construir_afd();


        


    

        }



        /*
        public void thompson()
        {

            Stack<Object> thompson_stack = new Stack<Object>();

            foreach (string token in regex)
            {

                switch (token)
                {


                    case "|":

                        Automaton a1_or = (Automaton)thompson_stack.Pop();
                        Automaton a2_or = (Automaton)thompson_stack.Pop();
                        Automaton or = this.or(a1_or, a2_or);
                        thompson_stack.Push(or);


                        this.afn = or;

                        break;

                    case ".":

                        Automaton a1_and = (Automaton)thompson_stack.Pop();
                        Automaton a2_and = (Automaton)thompson_stack.Pop();
                        Automaton and = this.and(a1_and, a2_and);
                        thompson_stack.Push(and);
                        this.afn = and;
                        break;

                    case "*":

                        Automaton a1_kleene = (Automaton)thompson_stack.Pop();
                        Automaton kleene = cerradura_kleene(a1_kleene);
                        thompson_stack.Push(kleene);
                        this.afn = kleene;

                        break;

                    case "+":

                        Automaton c1 = (Automaton)thompson_stack.Pop();
                        Automaton c4 = (Automaton)this.cerradura_positiva(c1);
                        Automaton c2 = (Automaton)this.cerradura_kleene(c1);
                        Automaton c3 = this.and(c4, c2);
                        thompson_stack.Push(c3);
                        this.afn = c3;

                        break;

                    case "?":

                        Automaton interrogation_simbol = new Automaton();
                        Automaton a1_inte = this.simbol(Automaton.EPSILON);
                        Automaton a2_inte = (Automaton)thompson_stack.Pop();
                        interrogation_simbol = this.or(a1_inte, a2_inte);
                        thompson_stack.Push(interrogation_simbol);
                        this.afn = interrogation_simbol;
                        break;

                    default:
                        Automaton simbol = this.simbol(token);
                        thompson_stack.Push(simbol);
                        this.afn = simbol;
                        this.alphabet.AddLast(token);
                        break;
                }


            }

            this.afn.setType("AFN");
            thompson_stack.Clear();
            Graphviz g = new Graphviz();
            g.graph_automaton(generarDOT(id + " : AFN", this.afn));
            AFD afd = new AFD(this.afn, this.alphabet, id);
            afd.construir_afd();
            
      //      transicionesE();


        }*/


        public Automaton simbol(Object t_simbol)
        {

            //Definir un nuevo automata
            Automaton afn = new Automaton();

            Estado begin = new Estado(0);

            Estado end = new Estado(1);

            //Establecer las transiciones para el estado inicial
            Transition tran = new Transition(begin, end, t_simbol);
            begin.setTransitions(tran);

            //Agregar los estados a la lista de estados del automata
            afn.addEstados(begin);
            afn.addEstados(end);

            //Definir el estado inicial y final del automata
            afn.setBegin(begin);
            afn.addEstadosAceptacion(end);
            return afn;

        }


        public Automaton cerradura_kleene(Automaton afn)
        {

            //Definir un nuevo automata con su estado inicial 
            Automaton kleene = new Automaton();

            /*Se marca el estado inicial como "actual" debido a  que el automata recibido por parametro tambien
            posee sus estados inicial y final
             */
            Estado actual_begin = new Estado(0);
            kleene.addEstados(actual_begin);
            kleene.setBegin(actual_begin);


            //Actualizar los indicadores de estado del afn recibido
            for (int i = 0; i < afn.getEstados().Count(); i++)
            {
                Estado aux = afn.getEstados().ElementAt(i);
                for (int j = 0; j < aux.getTransitions().Count(); j++)
                {

                    aux.getTransitions().ElementAt(j).getBegin().setId(aux.getTransitions().ElementAt(j).getBegin().getId() + 1);
                }
                aux.setId(i + 1);
                kleene.addEstados(aux);
            }



            Estado actual_end = new Estado(afn.getEstados().Count() + 1);
            kleene.addEstados(actual_end);
            kleene.addEstadosAceptacion(actual_end);

            Estado last_begin = afn.getBegin();

            LinkedList<Estado> last_end = afn.getEstadosAceptacion();

            actual_begin.getTransitions().AddLast(new Transition(actual_begin, last_begin, Automaton.EPSILON));
            actual_begin.getTransitions().AddLast(new Transition(actual_begin, actual_end, Automaton.EPSILON));


            for (int i = 0; i < last_end.Count(); i++)
            {
                last_end.ElementAt(i).getTransitions().AddLast(new Transition(last_end.ElementAt(i), last_begin, Automaton.EPSILON));
                last_end.ElementAt(i).getTransitions().AddLast(new Transition(last_end.ElementAt(i), actual_end, Automaton.EPSILON));
            }

            return kleene;
        }


        public Automaton and(Automaton left, Automaton right)
        {

            Automaton afn = new Automaton();
            int i = 0;
            for (i = 0; i < right.getEstados().Count(); i++)
            {
                Estado aux = right.getEstados().ElementAt(i);
                aux.setId(i);

                if (i == 0)
                {
                    afn.setBegin(aux);
                }

                if (i == right.getEstados().Count() - 1)
                {
                    for (int j = 0; j < right.getEstadosAceptacion().Count(); j++)
                    {
                        //aux.setTransitions(new Transition((Estado)right.getEstadosAceptacion().ElementAt(j), left.getBegin(), Automaton.EPSILON));
                        foreach (Transition t in left.getBegin().getTransitions()) {
                            //t.setBegin(aux);
                            aux.setTransitions(new Transition((Estado)right.getEstadosAceptacion().ElementAt(j), t.getEnd(), t.getSimbol()));                         
                        }

                    }
                }
                afn.addEstados(aux);

            }


            for (int j = 1; j < left.getEstados().Count(); j++)
            {
                Estado aux = left.getEstados().ElementAt(j);
                aux.setId(i);

                if (j == left.getEstados().Count() - 1)
                    afn.addEstadosAceptacion(aux);
                afn.addEstados(aux);
                i++;
            }
            return afn;
        }


        public Automaton or(Automaton left, Automaton right)
        {
            Automaton afn = new Automaton();


            Estado actual_begin = new Estado(0);

            actual_begin.setTransitions(new Transition(actual_begin, right.getBegin(), Automaton.EPSILON));

            afn.addEstados(actual_begin);
            afn.setBegin(actual_begin);
            int i = 0;

            for (i = 0; i < left.getEstados().Count(); i++)
            {
                Estado aux = left.getEstados().ElementAt(i);
                aux.setId(i + 1);
                afn.addEstados(aux);
            }

            for (int j = 0; j < right.getEstados().Count(); j++)
            {
                Estado aux = right.getEstados().ElementAt(j);
                aux.setId(i + 1);
                afn.addEstados(aux);
                i++;
            }

            Estado actual_end = new Estado(left.getEstados().Count() + right.getEstados().Count() + 1);
            afn.addEstados(actual_end);
            afn.addEstadosAceptacion(actual_end);


            Estado last_begin = left.getBegin();
            LinkedList<Estado> last_end_left = left.getEstadosAceptacion();
            LinkedList<Estado> last_end_right = right.getEstadosAceptacion();

            actual_begin.getTransitions().AddLast(new Transition(actual_begin, last_begin, Automaton.EPSILON));


            for (int k = 0; k < last_end_left.Count(); k++)
                last_end_left.ElementAt(k).getTransitions().AddLast(new Transition(last_end_left.ElementAt(k), actual_end, Automaton.EPSILON));

            for (int k = 0; k < last_end_left.Count(); k++)
                last_end_right.ElementAt(k).getTransitions().AddLast(new Transition(last_end_right.ElementAt(k), actual_end, Automaton.EPSILON));

            return afn;
        }


        public Automaton cerradura_positiva(Automaton afn_pos)
        {

            Automaton afn = new Automaton();


            int i = 0;

            //Recorrer el automata actual
            for (i = 0; i < afn_pos.getEstados().Count(); i++)
            {

                //Clonar cada estado (sin sus transiciones)
                Estado aux = afn_pos.getEstados().ElementAt(i);
                Estado nuevo_estado = (Estado) aux.Clone();

                if (i == 0)
                {
                    afn.setBegin(nuevo_estado);
                }
                afn.addEstados(nuevo_estado);
            }


            //Recorrer el nuevo automata
            for (int j = 0; j < afn.getEstados().Count(); j++) {

                //Estado auxiliar del automata nuevo
                Estado e = afn.getEstados().ElementAt(j);

                //Estado auxiliar del automata actual
                Estado e2 = afn_pos.getEstados().ElementAt(j);
                foreach (Transition t in e2.getTransitions())
                {
                    //Agregar las transiciones del nuevo automata
                    Transition aux = new Transition(search(afn.getEstados(), e2.getId()), search(afn.getEstados(), t.getEnd().getId()), t.getSimbol());
                    e.setTransitions(aux);
                }

            }

            for (int j = 0; j < afn_pos.getEstadosAceptacion().Count(); j++)
            {

                Estado aux = afn_pos.getEstadosAceptacion().ElementAt(j);
                Estado n = search(afn.getEstados(), aux.getId());
                afn.addEstadosAceptacion(n);
            }

            return afn;

        }

        public Estado search(LinkedList<Estado> estados, int param) {

            Estado aux = null;

            foreach (Estado e in estados) {

                if (e.getId() == param) {

                    aux = e;
                    return aux; 
                }
            }

            return aux;

        }




        public String generarDOT(String nombreArchivo, Automaton automataFinito)
        {
            String texto = "digraph automata_finito {\n";

            texto += "\trankdir=LR;" + "\n";

            texto += "\tgraph [label=\"" + nombreArchivo + "\", labelloc=t, fontsize=20]; \n";
            texto += "\tnode [shape=doublecircle, style = filled,color = lightblue];";
            //mediumseagreen
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


        public void transicionesE()
        {
            LinkedList<int> listaEstados = new LinkedList<int>();

            foreach (Estado t in this.afn.getEstados())
            {
                int i = 0;
                foreach (Transition tran in t.getTransitions())
                {

                    if ((string)tran.getSimbol() == Automaton.EPSILON)
                    {

                        i++;
                        listaEstados.AddLast(tran.getEnd().getId());
                    }
                    Console.WriteLine("El estado " + t.getId() + " tiene " + i + " transiciones con epsilon " + tran.getBegin().getId() + " -> " + tran.getEnd().getId());
                }

            }

            Console.WriteLine("*********************************************************");

            foreach (int n in listaEstados)
            {

                Console.WriteLine(n);

            }

        }



        public Automaton getAfn()
        {
            return this.afn;
        }

        public void setAfn(Automaton afn)
        {
            this.afn = afn;
        }


    }
}
