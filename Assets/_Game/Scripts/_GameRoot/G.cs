using Audio;
using CMS;
using GameState;
using UI;
using UnityEngine;

namespace GameRoot
{
    public static class G
    {
        public static ICMSProvider CMSProvider;
        public static Repository Repository;
        public static UIRoot UIRoot;
        public static SceneProvider SceneProvider;
        public static AudioProvider AudioProvider;

        public static RootCMS RootCMS => CMSProvider.RootCMS;
    }
}