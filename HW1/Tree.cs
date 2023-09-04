using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


internal class Tree
{


    //data members-----------------------------------------------------------------------------
    private Node? root;
    private int node_count;



    //public methods---------------------------------------------------------------------------
    public Tree()
    {
        root = null;
        node_count = 0;
    }

    public Node? getRoot()
    {
        return root;
    }

    public void setRoot(Node? nroot)
    {
        root = nroot;
    }

    public int nodeCount()
    {
        return node_count;
    }

    public bool insert(int newData)
    {
        return insert(newData, root);
    }

    public void print() 
    {
        print(root);
    }

    public int levels()
    {
        return levels(root);
    }

    public int minLevels()
    {
        return (int) Math.Ceiling(Math.Log2(node_count + 1)) - 1;
    }

    //private methods--------------------------------------------------------------------------

    private int levels(Node? node)
    {
        if(node == null)
        {
            return 0;
        }
        else
        {
            int left = 1 + levels(node.getLeft());
            int right = 1 + levels(node.getRight());
            return Math.Max(left, right);
        }   
    }



    private bool insert(int newData, Node? node) //helper function for public insert()
    {
        if (node == null)
        {
            root = new Node(newData);
            node_count++;
            return true;
        }
        else
        {
            if (node.getData() < newData)
            {
                if (node.getRight() == null)
                {
                    node.setRight(new Node(newData));
                    node_count++;
                    return true;
                }
                else
                {
                    return insert(newData, node.getRight());
                }
            }
            else if (node.getData() > newData)
            {
                if (node.getLeft() == null)
                {
                    node.setLeft(new Node(newData));
                    node_count++;
                    return true;
                }
                else
                {
                    return insert(newData, node.getLeft());
                }
            }
            else { return false; }

        }
    }


   

    private void print(Node? node) //helper function for public print()
    {
        if (node != null)
        {
            print(node.getLeft());
            Console.WriteLine(node.getData());
            print(node.getRight());
        }
    }


}

