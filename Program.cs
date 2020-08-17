using System;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hompus.YamlScalarOrSequence
{
    public class Program
    {
        public static void Main()
        {
            // Deserializer
            var deserializer = new DeserializerBuilder()
                .WithTypeConverter(new ScalarOrSequenceConverter())
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var stage = deserializer.Deserialize<Stage>("dependsOn: [ previousStage1, previousStage2 ]");
            Console.WriteLine(stage.DependsOn.Count());

            stage = deserializer.Deserialize<Stage>("dependsOn: previousStage1");
            Console.WriteLine(stage.DependsOn.Count());

            // Serializer
            var serializer = new SerializerBuilder()
                .WithTypeConverter(new ScalarOrSequenceConverter())
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            stage = new Stage { DependsOn = new[] { "previousStage1" } };
            serializer.Serialize(Console.Out, stage);
            // dependsOn: previousStage1

            stage = new Stage { DependsOn = new[] { "previousStage1", "previousStage2" } };
            serializer.Serialize(Console.Out, stage);
            // dependsOn:
            // - previousStage1
            // - previousStage2

            stage = new Stage { DependsOn = new string[0] };
            serializer.Serialize(Console.Out, stage);
            // dependsOn: []
        }
    }
}
