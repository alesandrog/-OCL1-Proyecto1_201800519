using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    class Node
    {

        public static LinkedList<string> token_output = new LinkedList<string>();
        public static LinkedList<Token> token_out = new LinkedList<Token>();
        public enum Tipo
        {

            NODO,
            OPERADOR

        }


        private Object value;
        private Node left;
        private Node right;
        private Node.Tipo type;
        //Initializing root 
        public Node(Object value)
        {
            this.value = value;
            left = right = null;
        }

        //Initializing with subtrees
        public Node(Node left, Object value, Node right)
        {
            this.value = value;
            this.left = left;
            this.right = right;
        }
        
        // Access Operations

        public Object getValue() { return value; }
        public Node leftTree() { return left; }
        public Node rightTree() { return right; }
        public void setValue(Object v) { value = v; }
        public void setLeft(Node n) { left = n; }
        public void setRight(Node n) { right = n; }
        public Node.Tipo getType() { return type; }
        public void setType(Node.Tipo type) { this.type = type; }

        public void visit()
        {
           // Token t = (Token)value;
            Console.WriteLine( value.ToString() );

        }

        public string visit_post()
        {

            token_output.AddLast(value.ToString());
            return value.ToString();

        }

        public string visit_postt()
        {

            Token aux = (Token)value;
            token_out.AddLast(aux);
            return aux.Getval();

        }
    }
}
