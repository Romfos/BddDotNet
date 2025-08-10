using BddDotNet.Extensibility;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BddDotNet.Gherkin.Models.Internal;

internal sealed class ModelTransformation<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TModel>() : IArgumentTransformation
{
    private static readonly Type type = typeof(TModel);
    private static readonly ParameterInfo[] firstConstructorParameters = type.GetConstructors().First().GetParameters();
    private static readonly PropertyInfo[] properties = type.GetProperties();
    private static readonly FieldInfo[] fields = type.GetFields();

    public ValueTask<object?> TransformAsync(object? input, Type targetType)
    {
        if (input is string[][] dataTable && targetType == typeof(TModel))
        {
            var modelData = GetModelData(dataTable);
            var model = Transform(modelData);
            return new(model);
        }

        return new(input);
    }

    private Dictionary<string, string> GetModelData(string[][] dataTable)
    {
        if (!(dataTable.Length > 1
            && dataTable.All(x => x.Length == 2)
            && dataTable[0][0] == "Name"
            && dataTable[0][1] == "Value"))
        {
            throw new Exception("Invalid table format. Name-Value table is expected");
        }

        if (dataTable[0].GroupBy(x => x[0]).Any(x => x.Count() > 1))
        {
            throw new Exception("Invalid table format. Table cannot have duplicate name rows");
        }

        return dataTable.Skip(1).ToDictionary(x => x[0].ToLower(), x => x[1], StringComparer.OrdinalIgnoreCase);
    }

    private TModel Transform(Dictionary<string, string> modelData)
    {
        var modelConstructorParameters = new object?[firstConstructorParameters.Length];

        for (var i = 0; i < firstConstructorParameters.Length; i++)
        {
            var constructorParameterName = firstConstructorParameters[i].Name!;
            var constructorParameterType = firstConstructorParameters[i].ParameterType;

            var value = modelData[constructorParameterName];
            modelData.Remove(constructorParameterName);

            modelConstructorParameters[i] = Convert.ChangeType(value, constructorParameterType);
        }

        var model = (TModel)Activator.CreateInstance(typeof(TModel), modelConstructorParameters)!;

        foreach (var modelDataRow in modelData)
        {
            var property = properties.FirstOrDefault(x => string.Equals(x.Name, modelDataRow.Key, StringComparison.OrdinalIgnoreCase));
            if (property != null)
            {
                if (Nullable.GetUnderlyingType(property.PropertyType) is Type underlyingFieldType)
                {
                    property.SetValue(model, Convert.ChangeType(modelDataRow.Value, underlyingFieldType));
                }
                else
                {
                    property.SetValue(model, Convert.ChangeType(modelDataRow.Value, property.PropertyType));
                }
            }

            var field = fields.FirstOrDefault(x => string.Equals(x.Name, modelDataRow.Key, StringComparison.OrdinalIgnoreCase));
            if (field != null)
            {
                if (Nullable.GetUnderlyingType(field.FieldType) is Type underlyingFieldType)
                {
                    field.SetValue(model, Convert.ChangeType(modelDataRow.Value, underlyingFieldType));
                }
                else
                {
                    field.SetValue(model, Convert.ChangeType(modelDataRow.Value, field.FieldType));
                }
            }

            if (property == null && field == null)
            {
                throw new Exception($"Model does not have property or field with name '{modelDataRow.Key}'");
            }
        }

        return model;
    }
}
