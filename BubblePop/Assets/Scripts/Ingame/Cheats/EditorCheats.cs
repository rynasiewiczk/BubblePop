using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace Ingame.Cheats
{
    public static class EditorCheats
    {
        private const string MENU_SCENE_NAME = "MenuScene";

        public static bool GameStartedFromIngame()
        {
            var result = !SceneManager.GetSceneByName(MENU_SCENE_NAME).isLoaded;
            return result;
        }
    }
}