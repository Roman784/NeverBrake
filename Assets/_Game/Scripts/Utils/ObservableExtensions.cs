using R3;
using System.Collections;

namespace Utils
{
    public static class ObservableExtensions
    {
        public static IEnumerator ToCoroutine<T>(this Observable<T> observable)
        {
            var isCompleted = false;
            using var _ = observable.Subscribe(
                _ => isCompleted = true);

            while (!isCompleted)
                yield return null;
        }
    }
}
