using System.Collections.Generic;

namespace Pause
{
    public class PauseProvider
    {
        private List<IPaused> _pauseds = new();

        public void Register(IPaused paused) => _pauseds.Add(paused); 

        public void Pause()
        {
            if (_pauseds.Count == 0) return;

            foreach (var paused in _pauseds)
            {
                paused.Pause();
            }
        }

        public void Unpause()
        {
            if (_pauseds.Count == 0) return;

            foreach (var paused in _pauseds)
            {
                paused.Unpause();
            }
        }
    }
}
