using TMPro;
using UnityEngine;
using Utils;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _deathCountView;
        [SerializeField] private TMP_Text _bestTimeView;
        [SerializeField] private TMP_Text _currentTimeView;

        public void DisplayDeathCount(int count) => _deathCountView.text = count.ToString();
        public void DisplayBestTime(int time) => _bestTimeView.text = time.ToTimeFormat();
        public void DisplayCurrentTime(int time) => _currentTimeView.text = time.ToTimeFormat();
    }
}
