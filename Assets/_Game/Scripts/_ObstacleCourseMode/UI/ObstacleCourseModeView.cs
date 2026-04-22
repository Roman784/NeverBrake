using TMPro;
using UnityEngine;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _deathCountView;

        public void DisplayDeathCount(int count) => _deathCountView.text = count.ToString();
    }
}
