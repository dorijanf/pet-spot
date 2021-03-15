${
    using Typewriter.Extensions.Types;
    using System.Text.RegularExpressions;
    using System.Diagnostics;

    string ToKebabCase(string typeName){
        return  Regex.Replace(typeName, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])","-$1", RegexOptions.Compiled)
                     .Trim().ToLower();
    }

    string CleanupName(string propertyName, bool? removeArray = true){
        if (removeArray.HasValue && removeArray.Value) {
            propertyName = propertyName.Replace("[]","");
        }
        return propertyName.Replace("Model","");
    }

    Template(Settings settings)
    {
        settings.OutputFilenameFactory = (file) => {
            if (file.Classes.Any()){
                var className = file.Classes.First().Name;
                className = CleanupName(className);
                className = ToKebabCase(className);
                return $"..\\..\\client-app\\src\\app\\generated-models\\{className}.ts";
            }
            if (file.Enums.Any()){
                var className = file.Enums.First().Name;
                className = ToKebabCase(className);
                return $"..\\..\\client-app\\src\\app\\generated-models\\{className}.ts";
            }
            return file.Name;
        };
    }

    string CustomProperties(Class c) => c.Properties
                                        .Select(p=> $"\tpublic {p.name}: {CleanupName(p.Type.Name, false)};")
                                        .Aggregate("", (all,prop) => $"{all}{prop}\r\n")
                                        .TrimEnd();

    string ClassName(Class c) => c.Name;

    string Imports(Class c)
    {
        IEnumerable<Type> types = c.Properties
            .Select(p => p.Type)
            .Where(t => !t.IsPrimitive || t.IsEnum)
            .Select(t => t.IsGeneric ? t.TypeArguments.First() : t)
            .Where(t => t.Name != c.Name && t.Name != "")
            .Distinct();
        return string.Join(Environment.NewLine, types.Select(t => $"import {{ {t.Name} }} from './{ToKebabCase(t.Name)}';").Distinct());
    }
}
// $Classes/Enums/Interfaces(filter)[template][separator]
// filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
// template: The template to repeat for each matched item
// separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

// More info: http://frhagn.github.io/Typewriter/

$Classes(*Dto)[
$Imports
export class $ClassName   {
    $CustomProperties
    }]$Enums(*)[export enum $Name { $Values[
        $name = $Value][,]
}]

$Classes(*Bm)[
export class $ClassName   {
    $CustomProperties
    }]$Enums(*)[export enum $Name { $Values[
        $name = $Value][,]
}]