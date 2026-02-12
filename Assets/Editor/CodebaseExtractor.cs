using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

public class CodebaseExtractor
{
    private class ProjectModel
    {
        public string project;
        public List<FileModel> files = new();
    }

    private class FileModel
    {
        public string path;
        public string @namespace;
        public List<TypeModel> types = new();
    }

    private class TypeModel
    {
        public string name;
        public string kind;
        public string baseType;
        public List<string> interfaces = new();
        public List<FieldModel> fields = new();
        public List<MethodModel> methods = new();
        public List<string> dependencies = new();
    }

    private class FieldModel
    {
        public string name;
        public string type;
        public string accessibility;
        public bool serialized;
    }

    private class MethodModel
    {
        public string name;
        public string returnType;
        public string accessibility;
        public bool isStatic;
        public List<string> parameters = new();
    }

    [MenuItem("Tools/Generate AI Codebase JSON")]
    public static void GenerateJsonOverview()
    {
        string[] files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

        var project = new ProjectModel
        {
            project = Application.productName
        };

        foreach (var file in files)
        {
            if (file.Contains("/Editor/"))
                continue;

            string code = File.ReadAllText(file);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var fileModel = new FileModel
            {
                path = file.Replace(Application.dataPath, "Assets")
            };

            var namespaceNode = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            fileModel.@namespace = namespaceNode?.Name.ToString() ?? "Global";

            var typeNodes = root.DescendantNodes().OfType<TypeDeclarationSyntax>();

            foreach (var typeNode in typeNodes)
            {
                var typeModel = new TypeModel
                {
                    name = typeNode.Identifier.Text,
                    kind = typeNode.Kind().ToString()
                };

                if (typeNode.BaseList != null)
                {
                    var bases = typeNode.BaseList.Types.Select(t => t.Type.ToString()).ToList();
                    typeModel.baseType = bases.FirstOrDefault();
                    typeModel.interfaces = bases.Skip(1).ToList();
                }

                // Fields
                var fields = typeNode.Members.OfType<FieldDeclarationSyntax>();
                foreach (var field in fields)
                {
                    var isSerialized = field.AttributeLists
                        .SelectMany(a => a.Attributes)
                        .Any(a => a.Name.ToString().Contains("SerializeField"));

                    foreach (var variable in field.Declaration.Variables)
                    {
                        typeModel.fields.Add(new FieldModel
                        {
                            name = variable.Identifier.Text,
                            type = field.Declaration.Type.ToString(),
                            accessibility = field.Modifiers.ToString(),
                            serialized = isSerialized
                        });
                    }
                }

                // Methods
                var methods = typeNode.Members.OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    typeModel.methods.Add(new MethodModel
                    {
                        name = method.Identifier.Text,
                        returnType = method.ReturnType.ToString(),
                        accessibility = method.Modifiers.ToString(),
                        isStatic = method.Modifiers.Any(SyntaxKind.StaticKeyword),
                        parameters = method.ParameterList.Parameters
                            .Select(p => $"{p.Type} {p.Identifier}")
                            .ToList()
                    });
                }

                // Dependencies (type references)
                var identifiers = typeNode.DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Select(i => i.Identifier.Text)
                    .Distinct()
                    .ToList();

                typeModel.dependencies = identifiers;

                fileModel.types.Add(typeModel);
            }

            if (fileModel.types.Count > 0)
                project.files.Add(fileModel);
        }

        string json = JsonConvert.SerializeObject(project, Formatting.Indented);

        string outputPath = Path.Combine(Application.dataPath, "CodebaseOverview.json");
        File.WriteAllText(outputPath, json);

        AssetDatabase.Refresh();
        Debug.Log("AI Codebase JSON generated at: " + outputPath);
    }
}
