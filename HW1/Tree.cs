using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Tree
{
    private Node? root;


    public Tree()
    {
        root = null;
    }

    public Node? getRoot()
    {
        return root;
    }

    public void setRoot(Node? nroot)
    {
        root = nroot;
    }


    public bool insert(int newData)
    {
        return insert(newData, root);
    }

    private bool insert(int newData, Node? node)
    {
        if (node == null)
        {
            node = new Node(newData);
            return true;
        }
        else
        {
            if (node.getData() < newData)
            {
                if (node.getRight() == null)
                {
                    node.setRight(new Node(newData));
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


    public void print()
    {
        print(root);
    }

    private void print(Node? node)
    {
        if (node != null)
        {
            print(node.getLeft());
            Console.WriteLine(node.getData());
            print(node.getRight());
        }
    }


}

