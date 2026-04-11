using Audio;
using CMS;
using Gameplay;
using GameState;
using UI;
using UnityEngine;

namespace GameRoot
{
    public static class G
    {
        // ========== GLOBAL ==========
        
        public static ICMSProvider CMSProvider;
        public static Repository Repository;
        public static UIRoot UIRoot;
        public static SceneProvider SceneProvider;
        public static AudioProvider AudioProvider;

        // ========== GAMEPLAY ==========



        // ========== PROPERTIES ==========

        public static RootCMS RootCMS => CMSProvider.RootCMS;
    }
}