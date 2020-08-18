using UnityEngine;

namespace GameLabGraz.ImmersiveAnalytics
{
    public class PlayerTracker : MonoBehaviour
    {
        public long Id { get; set; }

        private void OnEnable()
        {   
            TrackManager.Instance.RegisterPlayer(this);
        }

        private void OnDisable()
        {
            TrackManager.Instance.UnregisterPlayer();
        }
    }
}
