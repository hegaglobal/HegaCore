using System;
using System.Collections.Generic;

namespace HegaCore.Editor
{
    using JSONNode = SimpleJSON.JSONNode;
    using JSONObject = SimpleJSON.JSONObject;

    public sealed partial class DependenciesUpdater
    {
        [Serializable]
        private sealed class Manifest
        {
            public readonly Dictionary<string, string> dependencies = new Dictionary<string, string>();
            public readonly List<ScopedRegistry> scopedRegistries = new List<ScopedRegistry>();

            public JSONNode Serialize()
            {
                var json = new JSONObject();
                var d = json[nameof(this.dependencies)].AsObject;

                foreach (var v in this.dependencies)
                {
                    d[v.Key] = v.Value;
                }

                var s = json[nameof(this.scopedRegistries)].AsArray;

                for (var i = 0; i < this.scopedRegistries.Count; i++)
                {
                    s[i] = this.scopedRegistries[i].Serialize();
                }

                return json;
            }

            public Manifest Deserialize(JSONNode node)
            {
                this.dependencies.Clear();

                foreach (var v in node[nameof(this.dependencies)])
                {
                    this.dependencies[v.Key] = v.Value;
                }

                this.scopedRegistries.Clear();

                foreach (var v in node[nameof(this.scopedRegistries)])
                {
                    this.scopedRegistries.Add(new ScopedRegistry().Deserialize(v.Value));
                }

                return this;
            }
        }

        [Serializable]
        private sealed class ScopedRegistry
        {
            public string name;
            public string url;
            public readonly List<string> scopes = new List<string>();

            public JSONNode Serialize()
            {
                var json = new JSONObject();
                json[nameof(this.name)] = this.name;
                json[nameof(this.url)] = this.url;

                var s = json[nameof(this.scopes)].AsArray;

                for (var i = 0; i < this.scopes.Count; i++)
                {
                    s[i] = this.scopes[i];
                }

                return json;
            }

            public ScopedRegistry Deserialize(JSONNode node)
            {
                this.name = node[nameof(this.name)];
                this.url = node[nameof(this.url)];

                if (this.name == null)
                    this.name = string.Empty;

                if (this.url == null)
                    this.url = string.Empty;

                this.scopes.Clear();

                foreach (var v in node[nameof(this.scopes)])
                {
                    this.scopes.Add(v.Value);
                }

                return this;
            }
        }
    }
}