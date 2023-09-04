string str;
input:
Console.WriteLine("Enter Numbers: ");
str = Console.ReadLine();
Tree tree = new Tree();

if (str is not null)
{
    char delim = ' '; // delimiter used to split string
    string[] ints = str.Split(delim); // spilt every int in the string into a substring, store in array of strings
    int number = 0;

    foreach (string s in ints) //go through every string in array of strings
    {
        if (int.TryParse(s, out number)) //try to convert string into an integer, if it fails, retry input
        {
            tree.insert(number); //if conversion is successful, insert to tree
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto input;
        }
    }
    Console.WriteLine("Tree Contents:\n----------------\n");
    tree.print();
    Console.WriteLine("\n----------------\n");
    Console.WriteLine("Tree Statistics:\n\tNumber of Nodes: " + tree.nodeCount() + "\n\tNumber of Levels: " + tree.levels() + "\n\tMinimum Number of Levels That a Tree With " + tree.nodeCount() + " Nodes Could Have = " + tree.minLevels() + "\nDone");

}