using System;
using System.Collections;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace YukApiCSharp {
    public class Config {
        static readonly string ConfigFile = "config.yaml";
        public static ConfigNode OpenFileNode() {
            return new ConfigNode(new StreamReader(ConfigFile));
        }
        
        public class ConfigNode : IDisposable {
            private YamlNode node;
            private StreamReader reader;
            public ConfigNode(YamlNode node) {
                this.node = node;
                reader = null;
            }
            public ConfigNode(StreamReader reader) {
                this.reader = reader;
                YamlStream yaml = new YamlStream();
                yaml.Load(new StreamReader(ConfigFile, System.Text.Encoding.UTF8));
                node = yaml.Documents[0].RootNode;
            }
            public ConfigNode this[int i] {
                get {
                    return new ConfigNode(((YamlSequenceNode)node).Children[i]);
                }
            }
            public ConfigNode this[string key] {
                get {
                    return new ConfigNode(((YamlMappingNode)node).Children[new YamlScalarNode(key)]);
                }
            }
            override public string ToString() {
                return node.ToString();
            }

            public string StringValue() {
                return node.ToString();
            }
            public int IntValue() {
                return int.Parse(node.ToString());
            }

            public void Dispose() {
                if(reader!=null) {
                    reader.Close();
                    reader.Dispose();
                }
            }
            ~ConfigNode() {
                Dispose();
            }
        }
    }
    
}
