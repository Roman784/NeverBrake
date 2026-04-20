using Audio;
using CMS;
using Currency;
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
        public static Wallet Wallet;
        public static PopUpsProvider PopUpsProvider;
        public static PopUpFactory PopUpFactory;

        // ========== GAMEPLAY ==========

        public static Gameplay.GameplayCamera Camera;

        // ========== PROPERTIES ==========

        public static RootCMS RootCMS => CMSProvider.RootCMS;
    }
}