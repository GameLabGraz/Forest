using UnityEngine;

namespace GameLabGraz.ImmersiveAnalytics
{
    public class TrackableObject : MonoBehaviour
    {
        public int Id => GetInstanceID();

        private void OnEnable()
        {
            TrackManager.Instance.RegisterTrackableObject(this);
        }

        private void OnDisable()
        {
            TrackManager.Instance.UnregisterTrackableObject(this);
        }
    }
}
