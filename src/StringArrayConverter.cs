using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Hompus.YamlScalarOrSequence
{
    public class ScalarOrSequenceConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(IEnumerable<string>).IsAssignableFrom(type);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume<Scalar>(out var scalar))
            {
                return new List<string> { scalar.Value };
            }

            if (parser.TryConsume<SequenceStart>(out var _))
            {
                var items = new List<string>();

                while (parser.TryConsume<Scalar>(out var scalarItem))
                {
                    items.Add(scalarItem.Value);
                }

                parser.Consume<SequenceEnd>();

                return items;
            }

            return Enumerable.Empty<string>();
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var sequence = (IEnumerable<string>)value;
            if (sequence.Count() == 1)
            {
                emitter.Emit(new Scalar(default, sequence.First()));
            }
            else
            {
                emitter.Emit(new SequenceStart(default, default, false, SequenceStyle.Any));

                foreach (var item in sequence)
                {
                    emitter.Emit(new Scalar(default, item));
                }

                emitter.Emit(new SequenceEnd());
            }
        }
    }
}
