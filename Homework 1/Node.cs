
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Node
{

    private int data;
    private Node? left;
    private Node? right;

    public Node(int newData = 0)
    {
        data = newData;
        left = null;
        right = null;
    }



    public Node? getLeft()
    {
        return left;
    }

    public Node? getRight()
    {
        return right;
    }

    public int? getData()
    {
        return data;
    }

    public void setLeft(Node nleft)
    {
        left = nleft;
    }

    public void setRight(Node nright)
    {
        right = nright;
    }

    public void setdata(int newData)
    {
        data = newData;
    }

}

