using Spreadsheet_Engine;

int option = 0;
string current = "", name;
Console.WriteLine("Menu: Current Expression - " + current + "\n(1) Enter New Expression\n(2) Set Variable Value\n(3) Evaluate Tree\n(4) Exit\n\n");
int.TryParse(Console.ReadLine(), out option);
EvaluationTree tree = new EvaluationTree("0 + 0");
while (option != 4)
{
    switch (option)
    {
        case 1: Console.WriteLine("Enter Expression: ");
            current = Console.ReadLine();
            tree = new EvaluationTree(current);
            break;

        case 2: Console.WriteLine("Enter Variable Name:");
            name = Console.ReadLine();
            Console.WriteLine("Enter Variable Value:") ;
            double.TryParse(Console.ReadLine(), out double value);
            tree.SetVariable(name, value);
            break;

        case 3: Console.WriteLine(current + " = " + tree.Evaluate() + '\n');
            break;
    }
    Console.WriteLine("Menu: Current Expression - " + current + "\n(1) Enter New Expression\n(2) Set Variable Value\n(3) Evaluate Tree\n(4) Exit\n\n");
    int.TryParse(Console.ReadLine(), out option);
}

Console.WriteLine("Exited Successfully.\n");