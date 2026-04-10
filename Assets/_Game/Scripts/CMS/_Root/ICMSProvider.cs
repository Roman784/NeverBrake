using Cysharp.Threading.Tasks;
using R3;

namespace CMS
{
    public interface ICMSProvider
    {
        public RootCMS RootCMS { get; }
        public UniTask<bool> LoadRootCMS();
    }
}
