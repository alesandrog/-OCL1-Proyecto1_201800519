using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OCL1_Proyecto1_201800519
{
    class BinaryTreeC
    {

        public BinaryTreeC() {

        }

        public void tree_constructor3(LinkedList<Token> token_list)
        {

         

                BinaryTree tree = new BinaryTree();
                Stack<object> token_stack = new Stack<object>();

                for (int i = token_list.Count() - 1; i >= 0; i--)
                {
                    Token t = token_list.ElementAt(i);

                    switch (t.getTipo())
                    {

                        case Token.Tipo.OPERADOR_CONCAT:

                            Node a1_concat = (Node)token_stack.Pop();
                            Node a2_concat = (Node)token_stack.Pop();
                            Node a_concat = BinaryTree.newTree(a1_concat, t, a2_concat);
                            token_stack.Push(a_concat);
                            break;

                        case Token.Tipo.OPERADOR_OR:

                            Node a1_or = (Node)token_stack.Pop();
                            Node a2_or = (Node)token_stack.Pop();
                            Node a_or = BinaryTree.newTree(a1_or, t, a2_or);
                            token_stack.Push(a_or);
                            break;

                        case Token.Tipo.OPERADOR_KLEENE:

                            Node a1_kleene = (Node)token_stack.Pop();
                            Node a_kleene = BinaryTree.newTree(a1_kleene, t, null);
                            token_stack.Push(a_kleene);
                            break;

                        case Token.Tipo.OPERADOR_POSITIVA:

                            Node a1_plus = (Node)token_stack.Pop();
                            Node a_plus = BinaryTree.newTree(a1_plus, t, null);
                            token_stack.Push(a_plus);
                            break;

                        case Token.Tipo.OPERADOR_INTE:

                            Node a1_b = (Node)token_stack.Pop();
                            Node a_b = BinaryTree.newTree(a1_b, t, null);
                            token_stack.Push(a_b);
                            break;

                        default:

                            Node a1 = BinaryTree.newTree(null, t, null);
                            token_stack.Push(a1);
                            break;

                    }

                }

                Node a = (Node)token_stack.Pop();
                tree = new BinaryTree(a);
                tree.postorder(a);
                tree.postordertoken_regex(a);


            
        



        }


        public void tree_constructor2(LinkedList<Token> token_list) {

            BinaryTree tree = new BinaryTree();
            Stack<object> token_stack = new Stack<object>();

            for (int i = token_list.Count() - 1; i >= 0; i--)
            {
                Token t = token_list.ElementAt(i);

                switch (t.Getval()) {

                    case ".":

                        Node a1_concat = (Node) token_stack.Pop();
                        Node a2_concat = (Node) token_stack.Pop();
                        Node a_concat = BinaryTree.newTree(a1_concat, t.Getval(), a2_concat);
                        token_stack.Push(a_concat);
                        break;

                    case "|":

                        Node a1_or = (Node)token_stack.Pop();
                        Node a2_or = (Node)token_stack.Pop();
                        Node a_or = BinaryTree.newTree(a1_or, t.Getval(), a2_or);
                        token_stack.Push(a_or);
                        break;

                    case "*":

                        Node a1_kleene = (Node)token_stack.Pop();
                        Node a_kleene = BinaryTree.newTree(a1_kleene, t.Getval(), null);
                        token_stack.Push(a_kleene);
                        break;

                    case "+":

                        Node a1_plus = (Node)token_stack.Pop();
                        Node a_plus = BinaryTree.newTree(a1_plus, t.Getval(), null);
                        token_stack.Push(a_plus);
                        break;

                    case "?":

                        Node a1_b = (Node)token_stack.Pop();
                        Node a_b = BinaryTree.newTree(a1_b, t.Getval(), null);
                        token_stack.Push(a_b);
                        break;

                    default:

                        Node a1 = BinaryTree.newTree(null, t.Getval(), null);
                        token_stack.Push(a1);
                        break;

                }

            }

            Node a = (Node)token_stack.Pop();
            tree = new BinaryTree(a);
            tree.postorder(a);
            tree.postorder_regex(a);


        }

 
        bool isBinary(Token token) {

            if (token.Getval() == "|" || token.Getval() == ".")          
                return true;

            return false;
        }




    }
}
