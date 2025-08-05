using BddDotNet.Extensibility;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace BddDotNet.CSharpExpressions.Transformations;

internal sealed class CSharpExpressionTransformation<TGlobals>(TGlobals globals, ScriptOptions scriptOptions) : IArgumentTransformation
{
    public async ValueTask<object?> TransformAsync(object? input, Type targetType)
    {
        if (input is string text)
        {
            var trimmedText = text.AsSpan().TrimStart();

            if (trimmedText.Length > 0 && trimmedText[0] == '@')
            {
                var code = trimmedText.Slice(1).ToString();
                return await CSharpScript.EvaluateAsync(code, scriptOptions, globals);
            }
        }

        return input;
    }
}
