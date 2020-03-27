using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    class BinaryTree
    {


        protected Node root;
        public BinaryTree()
        {
            root = null;
        }
        public BinaryTree(Node root)
        {
            this.root = root;
        }
        public Node getRoot()
        {
            return root;
        }
        // Comprueba el estatus del árbol
        bool isEmpty()
        {
            return root == null;

        }

        public static Node newTree(Node left, Object value, Node right)
        {
            return new Node( left,  value, right);
        }

        public  void postorder(Node r)
        {
            if (r != null)
            {
                postorder(r.leftTree());
                postorder(r.rightTree());
                r.visit();
            }
        }

        public void postorder_regex(Node r) {


            if (r != null)
            {
                postorder_regex(r.leftTree());
                postorder_regex(r.rightTree());
                r.visit_post();
            }

        }

        public void postordertoken_regex(Node r)
        {


            if (r != null)
            {
                postordertoken_regex(r.leftTree());
                postordertoken_regex(r.rightTree());
                r.visit_postt();
            }

        }



    }
}

