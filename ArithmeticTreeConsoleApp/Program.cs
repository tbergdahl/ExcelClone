// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

int option;
bool expEntered = false;
string? current = string.Empty, name;
Console.WriteLine("Menu: Current Expression - " + current + "\n(1) Enter New Expression\n(2) Set Variable Value\n(3) Evaluate Tree\n(4) Exit\n\n");
if (int.TryParse(Console.ReadLine(), out option))
{
    // Spreadsheet_Engine.EvaluationTree tree = new Spreadsheet_Engine.EvaluationTree("0 + 0");
    while (option != 4)
    {
        switch (option)
        {
            case 1:
                if (!expEntered)
                {
                    Console.WriteLine("Enter Expression: ");
                    current = Console.ReadLine();
                    if (current != null)
                    {
                        // tree = new Spreadsheet_Engine.EvaluationTree(current);
                    }

                    expEntered = true;
                }
                else
                {
                    Console.WriteLine("You Already Have Enetered An Expression.\n");
                }

                break;

            case 2:
                Console.WriteLine("Enter Variable Name:");
                name = Console.ReadLine();
                Console.WriteLine("Enter Variable Value:");

                // _ = double.TryParse(Console.ReadLine(), out double value);
                if (name != null)
                {
                    // tree.SetVariable(name, value);
                }

                break;

            case 3:
                // Console.WriteLine(current + " = " + tree.Evaluate() + '\n');
                break;
        }

        Console.WriteLine("Menu: Current Expression - " + current + "\n(1) Enter New Expression\n(2) Set Variable Value\n(3) Evaluate Tree\n(4) Exit\n\n");
        _ = int.TryParse(Console.ReadLine(), out option);
    }

    Console.WriteLine("Exited Successfully.\n");
}