using Solver.Enums;

namespace BinaryPuzzleSolver;

public class FieldReader
{
    private FileInfo _fileInfo;

    public FieldReader(string fileName)
    {
        _fileInfo = new FileInfo(fileName);
    }

    public FieldReader(FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }

    /// <summary>
    /// Returns a 2d array with the origin in the top-left growing downwards 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public FieldValues[] ReadFile()
    {
        if (!_fileInfo.Exists)
            throw new FileNotFoundException($"The given `{_fileInfo.FullName} file doesn't exist");

        using FileStream stream = _fileInfo.OpenRead();
        using StreamReader reader = new StreamReader(stream);

        List<string> lines = [];

        while (reader.ReadLine() is { } line)
            lines.Add(line);

        FieldValues[] values = new FieldValues[lines.Count * lines.Count];
        for (var i = 0; i < lines.Count; i++)
        {
            var currentLine = lines[i];

            for (var j = 0; j < currentLine.Length; j++)
            {
                var idx = i * lines.Count + j;
                var character = currentLine[j];

                if (character == '-')
                {
                    values[idx] = FieldValues.Open;
                    continue;
                }

                values[idx] = (FieldValues)(character - '0');
            }
        }

        return values;
    }
}