#if UNITY_EDITOR
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SpaceTravel.Game.Editor
{
    public static class ModuleDefinitionAutoGenerator
    {
        const string IconsRoot = "Assets/SpaceTravel/Art/Modules";
        const string OutputRoot = "Assets/SpaceTravel/Data/Modules";

        [MenuItem("SpaceTravel/Generate/Module Definitions From Icons")]
        public static void GenerateFromIcons()
        {
            if (!AssetDatabase.IsValidFolder(IconsRoot))
            {
                Debug.LogError("Icons root folder not found: " + IconsRoot);
                return;
            }

            if (!AssetDatabase.IsValidFolder(OutputRoot))
                CreateFolderRecursive(OutputRoot);

            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { IconsRoot });
            int created = 0;
            int updated = 0;

            for (int i = 0; i < guids.Length; i++)
            {
                string texturePath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                if (texture == null)
                    continue;

                string rawName = texture.name;
                string categoryName;
                string coreName;
                SplitName(rawName, out categoryName, out coreName);

                string id = coreName.ToLowerInvariant();
                var category = ParseCategory(categoryName);

                var sprite = AssetDatabase
                    .LoadAllAssetsAtPath(texturePath)
                    .OfType<Sprite>()
                    .FirstOrDefault();

                string defPath = Path.Combine(OutputRoot, id + ".asset").Replace("\\", "/");
                var def = AssetDatabase.LoadAssetAtPath<ModuleDefinition>(defPath);

                if (def == null)
                {
                    def = ScriptableObject.CreateInstance<ModuleDefinition>();
                    def.Id = id;
                    def.DisplayName = ToTitle(coreName);
                    def.Category = category;
                    def.Icon = sprite;

                    AssetDatabase.CreateAsset(def, defPath);
                    created++;
                }
                else
                {
                    def.Category = category;
                    if (sprite != null)
                        def.Icon = sprite;

                    if (string.IsNullOrEmpty(def.Id))
                        def.Id = id;
                    if (string.IsNullOrEmpty(def.DisplayName))
                        def.DisplayName = ToTitle(coreName);

                    EditorUtility.SetDirty(def);
                    updated++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"ModuleDefinition generation finished. Created: {created}, Updated: {updated}");
        }

        static void SplitName(string raw, out string category, out string core)
        {
            if (string.IsNullOrEmpty(raw))
            {
                category = string.Empty;
                core = string.Empty;
                return;
            }

            var parts = raw.Split('_');
            if (parts.Length <= 1)
            {
                category = string.Empty;
                core = raw;
                return;
            }

            category = parts[0];
            core = string.Join("_", parts.Skip(1));
        }

        static ModuleCategory ParseCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return ModuleCategory.None;

            if (System.Enum.TryParse(categoryName, ignoreCase: true, out ModuleCategory result))
                return result;

            return ModuleCategory.None;
        }

        static string ToTitle(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                return string.Empty;

            string s = raw.Replace("_", " ").Trim();
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }

        static void CreateFolderRecursive(string path)
        {
            string[] parts = path.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
#endif