var rows = 1;
var freeSpace = 0;
var savedText = new List<string[]>();
var savedLine = new string[] { };

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
    var userCommand = Console.ReadLine();
    if (userCommand == "-help") ShowCommands();

    if (userCommand == "0") break;

    switch (userCommand)
    {
        case "1":
        {
            Console.WriteLine("Enter text to append:");
            var userInput = Console.ReadLine();
            AddText(userInput);
            break;
        }
        case "2":
            rows++;
            freeSpace = 0;
            Console.WriteLine("New line started. Enter text to append:");
            var userInput2 = Console.ReadLine();
            savedText.Add(new string[] { });
            savedLine = new string[userInput2.Length];
            AddText(userInput2);
            break;
        case "3":
        {
            Console.WriteLine("Enter the file name for saving:");
            var fileName = Console.ReadLine();
            if (fileName != null && fileName.EndsWith(".txt"))
            {
                await using StreamWriter file = new(@$"D:\C#\Simple Text Editor\Simple Text Editor\{fileName}");
                await file.WriteLineAsync(GetText(savedText));
            }

            break;
        }
        case "4":
            Console.WriteLine("Enter the file name for loading:");
            var fileNameRead = Console.ReadLine();
            if (fileNameRead != null && fileNameRead.EndsWith(".txt"))
            {
                var lines = File.ReadAllLines(@$"D:\C#\Simple Text Editor\Simple Text Editor\{fileNameRead}");
                savedText = new List<string[]>();
                freeSpace = 0;
                LoadToMemory(lines);
                rows = lines.Length;
                freeSpace = lines[rows - 1].Length;
            }

            break;
        case "5":
            Console.WriteLine(GetText(savedText));
            break;
        case "6":
            Console.WriteLine("Choose line and index:");
            var userInput3 = Console.ReadLine().Split(' ');
            Console.WriteLine("Enter text to insert:");
            var userInput4 = Console.ReadLine();
            var line = int.Parse(userInput3[0]);
            var index = int.Parse(userInput3[1]);
            AddTextInside(userInput4, line, index);
            break;
        case "7":
            Console.WriteLine("Choose world to search");
            var userInput5 = Console.ReadLine();
            Console.WriteLine($"Founded '{userInput5}' {SearchSubstring(savedText, userInput5)}");
            break;
        case "8":
            savedText = new List<string[]>();
            break;
    }
}

void ShowCommands()
{
    foreach (var command in commands) Console.WriteLine(command);
}

void AddText(string? input)
{
    var wordLenght = 0;
    if (input != null)
        while (input.Length + freeSpace > savedLine.Length)
            savedLine = ExpandArray(savedLine, input.Length + freeSpace + savedLine.Length);

    while (input != null && wordLenght != input.Length)
        for (var i = freeSpace; i < input.Length + freeSpace; i++)
        {
            savedLine[i] = input[wordLenght].ToString();
            wordLenght++;
        }

    freeSpace += input.Length;
    if (savedText.Count == 0)
        savedText.Add(savedLine);
    else
        savedText[rows - 1] = savedLine;
}

void AddTextInside(string? input, int line, int column)
{
    while (line > rows)
    {
        savedText.Add(new string[] { });
        rows++;
        freeSpace = 0;
    }

    var text = GetLine(savedText, line - 1);
    savedText[line - 1] = ExpandArray(savedText[line - 1], input.Length + savedText[line - 1].Length);
    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
    if (savedText[line - 1][column] == null)
    {
        var wordLenght = 0;
        while (wordLenght != input.Length)
            for (var i = column; i < input.Length; i++)
            {
                savedText[line - 1][i] = input[wordLenght].ToString();
                wordLenght++;
                if (wordLenght == input.Length) break;
            }
    }
    else
    {
        var substrings = SplitAt(text, column);
        if (input != null)
        {
            var wordLenght = 0;
            while (wordLenght != input.Length)
                for (var i = column; i < column + input.Length; i++)
                {
                    savedText[line - 1][i] = input[wordLenght].ToString();
                    wordLenght++;
                    if (wordLenght == input.Length) break;
                }

            var counter1 = 0;
            for (var i = 0; i < column; i++)
            {
                if (counter1 > substrings[0].Length) break;
                savedText[line - 1][i] = substrings[0][counter1].ToString();
                counter1++;
            }

            var counter2 = 0;
            for (var i = column + input.Length; i < savedText[line - 1].Length; i++)
            {
                if (counter2 >= substrings[1].Length) break;
                savedText[line - 1][i] = substrings[1][counter2].ToString();
                counter2++;
            }
        }
    }
}

string SearchSubstring(List<string[]> text, string? substring)
{
    var sameLetters = "";
    var substringFound = "";
    var occurrence = 0;
    var counter = 0;
    var substringArray = substring.ToArray();
    foreach (var line in text)
        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == substringArray[counter].ToString() && counter < substring.Length)
            {
                counter++;
                sameLetters += line[i];
            }

            if (counter != 0 && counter <= substring.Length - 1)
                if (line[i + 1] != substringArray[counter].ToString())
                {
                    sameLetters = "";
                    counter = 0;
                }

            if (i + 1 >= line.Length && counter == 1)
            {
                substringFound += $"[{savedText.IndexOf(line) - 1}] ";
                counter = 0;
                occurrence++;
                sameLetters = "";
            }

            if (counter > substring.Length - 1 && i + 1 < line.Length)
                if (line[i + 1] != substringArray[substring.Length - 1].ToString())
                {
                    counter = 0;
                    if (sameLetters == substring)
                    {
                        substringFound += $"[{savedText.IndexOf(line)}] ";
                        occurrence++;
                        sameLetters = "";
                    }
                }
        }

    return $"{occurrence} times at line: {substringFound}.";
}

string GetText(List<string[]> dynamicArray)
{
    var text = "";
    for (var i = 0; i < dynamicArray.Count; i++)
    {
        if (i != 0) text += "\n";
        for (var j = 0; j < dynamicArray[i].Length; j++) text += dynamicArray[i][j];
    }

    return text;
}

string GetLine(List<string[]> array, int line)
{
    var text = "";
    for (var j = 0; j < array[line].Length; j++) text += array[line][j];

    return text;
}

string[] ExpandArray(string[] original, int cols)
{
    var newArray = new string[cols];
    for (var i = 0; i < original.Length; i++) newArray[i] = original[i];

    return newArray;
}

void LoadToMemory(string[] array)
{
    var counter = 0;
    rows = 1;
    while (counter != array.Length)
    {
        savedText.Add(new string[] { });
        savedLine = new string[array[counter].Length];
        AddText(array[counter]);
        counter++;
        freeSpace = 0;
        rows++;
    }
}

string[] SplitAt(string source, params int[] index)
{
    index = index.Distinct().OrderBy(x => x).ToArray();
    var output = new string[index.Length + 1];
    var pos = 0;

    for (var i = 0; i < index.Length; pos = index[i++]) output[i] = source.Substring(pos, index[i] - pos);

    output[index.Length] = source.Substring(pos);
    return output;
}