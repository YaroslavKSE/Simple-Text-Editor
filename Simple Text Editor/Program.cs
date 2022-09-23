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

    switch (userCommand)
    {
        case "1":
        {
            string? userInput = Console.ReadLine();
            AddText(userInput);
            break;
        }
        case "2":
            break;
        case "4":
            Console.WriteLine(GetLine(savedText));
            break;
        case "5":
            break;
        case "6":
            break;
        case "7":
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

void AddText(string? input)
{
    if (input != null)
        for (int i = 0; i < input.Length; i++)
        {
            savedText[ROWS - 1, i] = input[i].ToString();
        }
}

string GetLine(string[,] array)
{
    string text = "";
    foreach (var character in array)
    {
        text += character;
    }

    return text;
}
