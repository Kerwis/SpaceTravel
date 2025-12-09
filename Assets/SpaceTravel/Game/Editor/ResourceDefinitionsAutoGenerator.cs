#if UNITY_EDITOR
using System;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SpaceTravel.Game.Editor
{
    public static class ResourceDefinitionAutoGenerator
    {
        const string IconsRoot = "Assets/SpaceTravel/Art/Resources";
        const string OutputRoot = "Assets/SpaceTravel/Data/Resources";

        [MenuItem("SpaceTravel/Generate/Resource Definitions From Icons")]
        public static void GenerateFromIcons()
        {
            if (!AssetDatabase.IsValidFolder(IconsRoot))
            {
                Debug.LogError("Icons root folder not found: " + IconsRoot);
                return;
            }

            if (!AssetDatabase.IsValidFolder(OutputRoot))
            {
                CreateFolderRecursive(OutputRoot);
            }

            string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { IconsRoot });
            int created = 0;
            int updated = 0;

            for (int i = 0; i < guids.Length; i++)
            {
                string spritePath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (sprite == null)
                    continue;

                string rawName = sprite.name;
                ParseName(rawName, out string id, out ResourceGroup group);

                string defPath = Path.Combine(OutputRoot, id + ".asset").Replace("\\", "/");
                var def = AssetDatabase.LoadAssetAtPath<ResourceDefinition>(defPath);

                if (def == null)
                {
                    def = ScriptableObject.CreateInstance<ResourceDefinition>();
                    def.Id = id;
                    def.Group = group;
                    def.DisplayName = ToTitle(id);
                    def.Icon = sprite;

                    AssetDatabase.CreateAsset(def, defPath);
                    created++;
                }
                else
                {
                    def.Icon = sprite;
                    def.Group = group;

                    if (string.IsNullOrEmpty(def.Id))
                        def.Id = id;
                    if (string.IsNullOrEmpty(def.DisplayName))
                        def.DisplayName = ToTitle(id);

                    EditorUtility.SetDirty(def);
                    updated++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"ResourceDefinition generation finished. Created: {created}, Updated: {updated}");
        }

        static void ParseName(string rawName, out string id, out ResourceGroup group)
        {
            id = rawName.ToLowerInvariant();
            group = default;

            if (string.IsNullOrEmpty(rawName))
                return;

            string[] parts = rawName.Split('_');
            if (parts.Length == 0)
                return;

            string groupToken = parts[0];

            if (Enum.TryParse(groupToken, true, out ResourceGroup parsedGroup))
                group = parsedGroup;

            if (parts.Length > 1)
            {
                string core = string.Join("_", parts, 1, parts.Length - 1);
                if (!string.IsNullOrEmpty(core))
                    id = core.ToLowerInvariant();
            }
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