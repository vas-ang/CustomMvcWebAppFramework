namespace CustomFramework.Mvc
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    using Contracts;
    using DefaultViews;

    public class ViewEngine : IViewEngine
    {
        public string GetHtml(string templateHtml, object model, string user)
        {
            Type modelType = model?.GetType() ?? typeof(object);

            string modelTypeName = modelType.FullName;

            if (modelType.IsGenericType)
            {
                modelTypeName = GetGenericTypeFullName(modelType);
            }

            string preparedCSharpCode = PrepareCSharpCode(templateHtml);

            string code =
                $@"using System;
                   using System.Text;
                   using System.Linq;
                   using System.Collections.Generic;
                   
                   using CustomFramework.Mvc;
                   using CustomFramework.Mvc.Contracts;
                   
                   namespace ViewNamespace
                   {{
                        public class ViewClass : IView
                        {{
                            public string GetHtml(object model, string user)
                            {{
                                var Model = model as {modelTypeName};
                                object User = user;

                                StringBuilder html = new StringBuilder();

                                {preparedCSharpCode}

                                return html.ToString();
                            }}
                        }}
                   }}";

            IView view = GetInstanceFromCode(code, model);

            return view.GetHtml(model, user);
        }

        private string GetGenericTypeFullName(Type modelType)
        {
            int argumentCountBegining = modelType.Name
                .LastIndexOf('`');

            string genericModelTypeName = modelType.Name
                .Substring(0, argumentCountBegining);

            string genericTypeFullName = $"{modelType.Namespace}.{genericModelTypeName}";

            IEnumerable<string> genericTypeArguments = modelType.GenericTypeArguments
                .Select(GetGenericTypeArgumentFullName);

            string modelTypeName = $"{genericTypeFullName}<{string.Join(", ", genericTypeArguments)}>";

            return modelTypeName;
        }

        private string GetGenericTypeArgumentFullName(Type genericTypeArgument)
        {
            if (genericTypeArgument.IsGenericType)
            {
                return GetGenericTypeFullName(genericTypeArgument);
            }

            return genericTypeArgument.FullName;
        }

        private IView GetInstanceFromCode(string code, object model)
        {
            var compilation = CSharpCompilation.Create("ViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (var library in libraries)
            {
                compilation = compilation
                    .AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            if (model != null)
            {
                Type modelType = model.GetType();

                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(modelType.Assembly.Location));

                if (modelType.IsGenericType)
                {
                    compilation = AddGenericArgumentReferences(compilation, modelType);
                }
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using MemoryStream memoryStream = new MemoryStream();

            var compilationResult = compilation.Emit(memoryStream);
            if (!compilationResult.Success)
            {
                return new ErrorView(compilationResult.Diagnostics
                    .Where(x => x.Severity == DiagnosticSeverity.Error)
                    .Select(x => x.GetMessage()));
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            var assemblyByteArray = memoryStream.ToArray();
            var assembly = Assembly.Load(assemblyByteArray);
            var type = assembly.GetType("ViewNamespace.ViewClass");
            var instance = Activator.CreateInstance(type) as IView;
            return instance;
        }

        private static CSharpCompilation AddGenericArgumentReferences(CSharpCompilation compilation, Type modelType)
        {
            Type[] genericArguments = modelType.GetGenericArguments();

            foreach (var genericArgument in genericArguments)
            {
                var metadataReference = MetadataReference.CreateFromFile(genericArgument.Assembly.Location) as MetadataReference;

                if (compilation.References.All(mr => mr.Display != metadataReference.Display))
                {
                    compilation = compilation.AddReferences(metadataReference);
                }

                if (genericArgument.IsGenericType)
                {
                    compilation = AddGenericArgumentReferences(compilation, genericArgument);
                }
            }

            return compilation;
        }

        private string PrepareCSharpCode(string template)
        {
            const string codeRegEx = ".*?(?=<|>|\\s|$)";

            string[] supportedOperators = { "for", "foreach", "if", "else" };

            string[] lines = template.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            StringBuilder templateAsCode = new StringBuilder();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.TrimStart().StartsWith("{") ||
                    line.TrimStart().StartsWith("}"))
                {
                    templateAsCode.AppendLine(line);
                }
                else if (supportedOperators.Any(so => line.TrimStart().StartsWith("@" + so)))
                {
                    templateAsCode.AppendLine(line.Replace("@", string.Empty));
                }
                else
                {
                    templateAsCode.Append($@"html.AppendLine(");

                    while (line.Contains("@"))
                    {
                        int indexOfSpecialSign = line.IndexOf("@");

                        string html = line.Substring(0, indexOfSpecialSign);
                        int indexAfterSpecialSign = indexOfSpecialSign + 1;

                        Match codeMatched = Regex.Match(line.Substring(indexAfterSpecialSign), codeRegEx);

                        int indexAfterCSCode = indexAfterSpecialSign + codeMatched.Length;
                        line = line.Substring(indexAfterCSCode);

                        templateAsCode.Append("@\"" + html.Replace("\"", "\"\"") + '"' + $" + {codeMatched.Value} + ");
                    }

                    templateAsCode.AppendLine($"@\"{line.Replace("\"", "\"\"")}\");");
                }
            }

            return templateAsCode.ToString();
        }
    }
}
