string str;
input:
Console.WriteLine("Enter Numbers: ");
str = Console.ReadLine();
Tree tree = new Tree();

if (str is not null)
{
    char delim = ' ';
    string[] ints = str.Split(delim);
    int number = 0;

    foreach (string s in ints)
    {
        if (int.TryParse(s, out number))
        {
            tree.insert(number);
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