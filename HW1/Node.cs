using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Node
{
    //data members----------------------------------------------------------------------------
    private int data;
    private Node? left;
    private Node? right;

    //public methods--------------------------------------------------------------------------


    //Constructor - Initializes Values
    public Node(int newData = 0)
    {
        data = newData;
        left = null;
        right = null;
    }


    //getLeft() - returns reference to left node
    public Node? getLeft()
    {
        return left;
    }

    //getRight() - returns reference to right node
    public Node? getRight()
    {
        return right;
    }

    //getData() - returns data held in node
    public int? getData()
    {
        return data;
    }

    //setLeft() - Updates node's left node
    public void setLeft(Node nleft)
    {
        left = nleft;
    }

    //setRight() - Updates node's right node
    public void setRight(Node nright)
    {
        right = nright;
    }


    //setData() - updates node's data
    public void setData(int newData)
    {
        data = newData;
    }

}

