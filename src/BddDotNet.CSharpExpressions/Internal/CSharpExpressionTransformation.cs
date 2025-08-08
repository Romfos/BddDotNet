using BddDotNet.Extensibility;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace BddDotNet.CSharpExpressions.Internal;

internal sealed class CSharpExpressionTransformation<TGlobals>(TGlobals globals, ScriptOptions scriptOptions) : IArgumentTransformation
{
    public async ValueTask<object?> TransformAsync(object? input, Type targetType)
    {
        if (input is string text)
        {
            return await TransformAsync(text);
        }

        if (input is string[][] dataTable)
        {
            return await TransformAsync(dataTable);
        }

        return input;
    }

    private async ValueTask<string[][]> TransformAsync(string[][] originalTable)
    {
        var resultTable = new string[originalTable.Length][];

        for (var row = 0; row < originalTable.Length; row++)
        {
            resultTable[row] = new string[originalTable[row].Length];

            for (var column = 0; column < originalTable[row].Length; column++)
            {
                resultTable[row][column] = await TransformAsync(originalTable[row][column]);
            }
        }

        return resultTable;
    }

    private async ValueTask<string> TransformAsync(string text)
    {
        var trimmedText = text.AsSpan().TrimStart();

        if (trimmedText.Length > 0 && trimmedText[0] == '@')
        {
            var code = trimmedText.Slice(1).ToString();
            return await CSharpScript.EvaluateAsync<string>(code, scriptOptions, globals);
        }

        return text;
    }
}
