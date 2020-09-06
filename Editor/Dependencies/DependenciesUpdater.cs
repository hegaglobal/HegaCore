using System.IO;
using UnityEditor;
using UnityEngine;

namespace HegaCore.Editor
{
    public sealed partial class DependenciesUpdater : MonoBehaviour
    {
        [MenuItem("Tools/Update HegaCore Dependencies", priority = -1)]
        public static void Update()
        {
            var manifestPath = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            var filePath = Path.GetFullPath(manifestPath);
            var json = File.ReadAllText(filePath);
            var manifest = new Manifest().Deserialize(SimpleJSON.Parse(json));
            Ensure(manifest);

            File.WriteAllText($"{filePath}", manifest.Serialize().ToString(2));
        }

        private static void Ensure(Manifest manifest)
        {
            var d = manifest.dependencies;

            d["com.unity.addressables"] = "1.15.1";
            d["com.unity.scriptablebuildpipeline"] = "1.7.3";
            d["jillejr.newtonsoft.json-for-unity"] = "12.0.301";

            d["com.hegaglobal.visual-novel-data"] = "https://hegaglobal:HentaiG%40mes123456@github.com/hegaglobal/VisualNovelData.git#1.4.6";

            d["com.cysharp.unitask"] = "2.0.31";
            d["com.live2d.cubism-cubismloader"] = "4.0.106";
            d["com.minhdu.uiman"] = "1.4.7";

            d["com.grashaar.uiman-textmeshpro"] = "1.3.1";
            d["com.grashaar.unity-googlespreadsheet"] = "1.0.0";
            d["com.grashaar.unity-objectpooling"] = "1.3.2";

            d["com.laicasaane.texttyper"] = "3.0.2";
            d["com.laicasaane.tinycsvparser"] = "2.6.1";
            d["com.laicasaane.unity-addressables-manager"] = "1.1.0";
            d["com.laicasaane.unity-quastatemachine"] = "1.2.0";
            d["com.laicasaane.unity-supplements"] = "2.3.4";

            d["com.littlebigfun.addressable-importer"] = "0.9.2";
            d["com.sabresaurus.playerprefseditor"] = "1.2.0";
            d["com.merlin.easyeventeditor"] = "1.0.3";
        }
    }
}