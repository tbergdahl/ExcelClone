

string str;
input:
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

    tree.print();
}