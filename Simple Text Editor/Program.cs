string[] commands =
{
    "0. End program",
    "1. Append text symbols to the end",
    "2. Start the new line",
    "3. Use files to loading/saving the information",
    "4. Print the current text to console",
    "5. Insert the text by line and symbol index",
    "6. Search",
    "7. Clearing the console"
};

int ROWS = 1;
string[,] savedText = new string[ROWS, 100];

while (true)
{
    Console.WriteLine("Write -help to see the commands");
    Console.WriteLine("Choose the command:");
    string? userCommand = Console.ReadLine();
    if (userCommand == "-help")
    {
        ShowCommands();
    }

    if (userCommand == "0")
    {
        break;
    }

    
}

void ShowCommands()
{
    foreach (var command in commands)
    {
        Console.WriteLine(command);
    }
}


