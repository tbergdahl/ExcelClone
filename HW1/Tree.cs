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
   

    /// Constructor - Initializes Values
    public Tree()
    {
        root = null;
        node_count = 0;
    }
    
    //getRoot() - returns reference to root
    public Node? getRoot()
    {
        return root;
    }

    //setRoot() - changes root to be the input
    public void setRoot(Node? nroot)
    {
        root = nroot;
    }

    //nodeCount() - returns number of nodes stored in member node_count
    public int nodeCount()
    {
        return node_count;
    }

    //insert() - calls private method insert()
    public bool insert(int newData)
    {
        return insert(newData, root);
    }

    //print() - calls private method print()
    public void print() 
    {
        print(root);
    }

    //levels() - calls private method levels()
    public int levels()
    {
        return levels(root);
    }

    //minLevels() - returns minimum number of levels possible given the internal node count of the tree
    public int minLevels()
    {
        return (int) Math.Ceiling(Math.Log2(node_count + 1)) - 1;
    }

    //private methods--------------------------------------------------------------------------

    //levels() - private method that recursively calculates the number of levels in the tree -
    //helper function for public method levels()
    private int levels(Node? node)
    {
        if(node == null) // until we reach the end of the tree,
        {
            return 0;
        }
        else
        {
            int left = 1 + levels(node.getLeft()); // count the number of levels in the left subtree, 
            int right = 1 + levels(node.getRight()); // count the number of levels in the right subtree,
            return Math.Max(left, right); // and return which subtree has more levels
        }   
    }


    //insert() - private method that inserts data into the tree -
    //helper function for public method insert()
    private bool insert(int newData, Node? node) 
    {
        if (node == null) // empty tree
        {
            root = new Node(newData); // make a new root node with newData
            node_count++;
            return true;
        }
        else
        {
            if (node.getData() < newData) // if our current node has data that is smaller than our newData
            {
                //we need to traverse right subtree


                if (node.getRight() == null) // if there is no more nodes in the right subtree,
                                             // that means our newData is the new largest value
                {
                    node.setRight(new Node(newData)); //make node with new data
                    node_count++;
                    return true;
                }
                else
                {
                    return insert(newData, node.getRight()); // right subtree exists, move along until we find a empty spot to insert
                }
            }
            else if (node.getData() > newData)
            {
                //same idea as above, but we need to go in the left subtree since our data is smaller than current node

                if (node.getLeft() == null)// if there is no more nodes in the left subtree,
                                           // that means our newData is the new smallest value
                {
                    node.setLeft(new Node(newData)); 
                    node_count++;
                    return true;
                }
                else
                {
                    return insert(newData, node.getLeft()); // there are nodes in the left subtree, keep going until we find a spot to insert
                }
            }
            else { return false; } // if newData ever matches data in a node, stop (no dupes allowed)

        }
    }


   
    //print() - private method that recursively prints the contents of the tree using in-order traversal - 
    //helper function for public method print()
    private void print(Node? node) 
    {
        if (node != null)
        {
            print(node.getLeft()); //go as far left as we can (smallest value)
            Console.WriteLine(node.getData()); //print smallest values
            print(node.getRight()); //keep going from smallest to largest
        }
    }


}

