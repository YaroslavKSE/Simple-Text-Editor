int ROWS = 1;
int COLS = 30;
int freeSpace = 0;
var savedText = new List<string[]>();
string[] savedLine = new string[COLS];

string[] commands =
{
    "0. End program",
    "1. Append text symbols to the end",
    "2. Start the new line",
    "3. Saving the information",
    "4. Load the information",
    "5. Print the current text to console",
    "6. Insert the text by line and symbol index",
    "7. Search",
    "8. Clearing the console"
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
            freeSpace = 0;
            Console.WriteLine("New line started. Enter text to append:");
            string? userInput2 = Console.ReadLine();
            savedText.Add(new string[] { });
            savedLine = new string[userInput2.Length];
            AddText(userInput2);
            break;
        case "3":
        {
            Console.WriteLine("Enter the file name for saving:");
            string? fileName = Console.ReadLine();
            if (fileName != null && fileName.EndsWith(".txt"))
            {
                await using StreamWriter file = new(@$"D:\C#\Simple Text Editor\Simple Text Editor\{fileName}");
                await file.WriteLineAsync(GetText(savedText));
            }

            break;
        }
        case "4":
            Console.WriteLine("Enter the file name for loading:");
            string? fileNameRead = Console.ReadLine();
            if (fileNameRead != null && fileNameRead.EndsWith(".txt"))
            {
                string[] lines = File.ReadAllLines(@$"D:\C#\Simple Text Editor\Simple Text Editor\{fileNameRead}");
                LoadToMemory(lines);
                ROWS = lines.Length;
                freeSpace = lines[ROWS - 1].Length;
            }

            break;
        case "5":
            Console.WriteLine(GetText(savedText));
            break;
        case "6":
            Console.WriteLine("Choose line and index:");
            string[] userInput3 = Console.ReadLine().Split(' ');
            Console.WriteLine("Enter text to insert:");
            string? userInput4 = Console.ReadLine();
            var line = int.Parse(userInput3[0]);
            var index = int.Parse(userInput3[1]);
            AddTextInside(userInput4, line, index);
            break;
        case "7":
            Console.WriteLine("Choose world to search");
            string? userInput5 = Console.ReadLine();
            
            break;
        case "8":
            savedText = new List<string[]>();
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
        while (input.Length + freeSpace > savedLine.Length)
        {
            COLS *= 2;
            savedLine = ExpandArray(savedLine, COLS);
        }

    while (input != null && wordLenght != input.Length)
    {
        for (int i = freeSpace; i < input.Length + freeSpace; i++)
        {
            savedLine[i] = input[wordLenght].ToString();
            wordLenght++;
        }
    }

    freeSpace += input.Length;
    if (savedText.Count == 0)
    {
        savedText.Add(savedLine);
    }
    else
    {
        savedText[ROWS - 1] = savedLine;
    }
}

void AddTextInside(string? input, int line, int column)
{
    while (line > ROWS)
    {
        savedText.Add(new String[] { });
        ROWS++;
        freeSpace = 0;
    }

    var text = GetLine(savedText, line - 1);
    savedText[line - 1] = ExpandArray(savedText[line - 1], input.Length + savedText[line - 1].Length);
    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
    if (savedText[line - 1][column] == null)
    {
        var wordLenght = 0;
        while (wordLenght != input.Length)
        {
            for (int i = column; i < COLS; i++)
            {
                savedText[line - 1][i] = input[wordLenght].ToString();
                wordLenght++;
                if (wordLenght == input.Length)
                {
                    break;
                }
            }
        }
    }
    else
    {
        var substrings = SplitAt(text, column);
        if (input != null)
        {
            var wordLenght = 0;
            while (wordLenght != input.Length)
            {
                for (int i = column; i < column + input.Length; i++)
                {
                    savedText[line - 1][i] = input[wordLenght].ToString();
                    wordLenght++;
                    if (wordLenght == input.Length)
                    {
                        break;
                    }
                }
            }

            var counter1 = 0;
            for (int i = 0; i < column; i++)
            {
                if (counter1 > substrings[0].Length) break;
                savedText[line - 1][i] = substrings[0][counter1].ToString();
                counter1++;
            }

            var counter2 = 0;
            for (int i = column + input.Length; i < savedText[line - 1].Length; i++)
            {
                if (counter2 >= substrings[1].Length) break;
                savedText[line - 1][i] = substrings[1][counter2].ToString();
                counter2++;
            }
        }
    }
}

// string SearchSubstring(List<string[]> text, string substring)
// {
//     string substringFound = "";
//     int occurrence = 0;
//     foreach (var line in text)
//     {
//         for (int i = 0; i < line.Length; i++)
//         {
//             if (line[i] == substring[i].ToString())
//             {
//                
//             }
//         }
//     }
// }

string GetText(List<string[]> dynamicArray)
{
    string text = "";
    for (int i = 0; i < dynamicArray.Count; i++)
    {
        if (i != 0) text += "\n";
        for (int j = 0; j < dynamicArray[i].Length; j++)
        {
            text += dynamicArray[i][j];
        }
    }

    return text;
}

string GetLine(List<string[]> array, int line)
{
    string text = "";
    for (int j = 0; j < array[line].Length && j != null; j++)
    {
        text += array[line][j];
    }

    return text;
}

string[] ExpandArray(string[] original, int cols)
{
    var newArray = new String[cols];
    for (int i = 0; i < original.Length; i++)
    {
        newArray[i] = original[i];
    }

    return newArray;
}

void LoadToMemory(string[] array)
{
    int counter = 0;
    while (counter != array.Length)
    {
        savedText.Add(new string[] { });
        savedLine = new string[array[counter].Length];
        AddText(array[counter]);
        counter++;
        freeSpace = 0;
        ROWS++;
    }
}

string[] SplitAt(string source, params int[] index)
{
    index = index.Distinct().OrderBy(x => x).ToArray();
    string[] output = new string[index.Length + 1];
    int pos = 0;

    for (int i = 0; i < index.Length; pos = index[i++])
    {
        output[i] = source.Substring(pos, index[i] - pos);
    }

    output[index.Length] = source.Substring(pos);
    return output;
}