using System;
using System.IO;
using UnityEngine;

namespace SpaceTravel.Game
{
    public static class SaveService
    {
        private const string FileName = "save.json";

        public static string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, FileName);
        }

        public static void Save(GameState state)
        {
            if (state == null)
                return;

            try
            {
                var json = JsonUtility.ToJson(state, prettyPrint: false);
                var path = GetSavePath();
                File.WriteAllText(path, json);
#if UNITY_EDITOR
                Debug.Log($"[SaveService] Saved game to {path}");
#endif
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError($"[SaveService] Failed to save game: {ex}");
#endif
            }
        }

        public static GameState Load()
        {
            try
            {
                var path = GetSavePath();
                if (!File.Exists(path))
                    return null;

                var json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var state = JsonUtility.FromJson<GameState>(json);
#if UNITY_EDITOR
                Debug.Log($"[SaveService] Loaded game from {path}");
#endif
                return state;
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError($"[SaveService] Failed to load game: {ex}");
#endif
                return null;
            }
        }

        public static bool HasSave()
        {
            return File.Exists(GetSavePath());
        }
        
        public static void DeleteSave()
        {
            try
            {
                var path = GetSavePath();
                if (File.Exists(path))
                {
                    File.Delete(path);
#if UNITY_EDITOR
                    Debug.Log($"[SaveService] Save deleted: {path}");
#endif
                }
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError($"[SaveService] Failed to delete save: {ex}");
#endif
            }
        }

    }
}