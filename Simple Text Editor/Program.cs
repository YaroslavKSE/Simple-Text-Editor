int ROWS = 1;
int COLS = 100;
int freeSpace = 0;
string[,] savedText = new string[ROWS, COLS];

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
            Console.WriteLine("Enter text to append:");
            string? userInput = Console.ReadLine();
            AddText(userInput);
            break;
        }
        case "2":
            ROWS++;
            Console.WriteLine("New line started. Enter text to append:");
            string? userInput2 = Console.ReadLine();
            savedText = AddLineToArray(savedText, ROWS, COLS);
            freeSpace = 0;
            AddText(userInput2);
            break;
        case "3":
        {
            Console.WriteLine("Enter the file name for saving:");
            string? fileName = Console.ReadLine();
            if (fileName != null)
            {
                await using StreamWriter file = new(@$"D:\C#\Simple Text Editor\Simple Text Editor\{fileName}");
                await file.WriteLineAsync(GetLine(savedText));
            }

            break;
        }
        case "4":
            Console.WriteLine("Enter the file name for loading:");
            string? fileNameRead = Console.ReadLine();
            string[] lines = File.ReadAllLines(@$"D:\C#\Simple Text Editor\Simple Text Editor\{fileNameRead}");
            ROWS = lines.Length;
            string[,] array = new string[ROWS, COLS];
            savedText = array;
            for (int i = 0; i < ROWS; i++)
            {
                var word = lines[i].ToArray();
                for (int j = 0; j < lines[i].Length; j++)
                {
                    savedText[i, j] = word[j].ToString();
                }
            }

            break;
        case "5":
            Console.WriteLine(GetLine(savedText));
            break;
        case "6":
            break;
        case "7":
            savedText = new string[ROWS, COLS];
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
    var wordLenght = 0;
    if (input != null)
        if (input.Length > COLS)
        {
            COLS *= 3;
            savedText = AddLineToArray(savedText, ROWS, COLS);
        }

    while (wordLenght != input.Length)
    {
        for (int i = freeSpace; i < input.Length + freeSpace; i++)
        {
            savedText[ROWS - 1, i] = input[wordLenght].ToString();
            wordLenght++;
        }
    }

    freeSpace = input.Length;
}

string GetLine(string[,] array)
{
    string text = "";
    for (int i = 0; i < array.GetLength(0); i++)
    {
        if (i != 0) text += "\n";
        for (int j = 0; j < array.GetLength(1); j++)
        {
            text += array[i, j];
        }
    }

    return text;
}

string[,] AddLineToArray(string[,] original, int rows, int cols)
{
    var newArray = new String[rows, cols];
    for (int i = 0; i < ROWS - 1; i++)
    {
        for (int j = 0; j < COLS; j++)
        {
            newArray[i, j] = original[i, j];
        }
    }

    return newArray;
}