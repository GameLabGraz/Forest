using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.ImmersiveAnalytics
{


    public class PlayableTrack : MonoBehaviour
    {
        [SerializeField] private int playerId;

        [SerializeField] private int objectId;

        [SerializeField] private bool play;

        private int _recordIndex = 0;

        private DateTime _starTime;
        private DateTime _endTime;
        private List<TrackEntry> _records = new List<TrackEntry>();

        private float _time;
        private DateTime _lastRecordTime;

        private void Start()
        {
            _starTime = TrackManager.Instance.GetStartTime(playerId);
            //_endTime = TrackManager.Instance.GetEndTime(playerId);
            _lastRecordTime = _starTime;

            _records = TrackManager.Instance.GetTrackRecords(playerId, objectId);
        }

        private void Update()
        {
            if (!play || _recordIndex >= _records.Count) return;

            _time += Time.deltaTime;
            if (_time < (_records[_recordIndex].Time - _lastRecordTime).TotalSeconds) return;
            _time = 0.0f;
            _lastRecordTime = _records[_recordIndex].Time;

            transform.position = _records[_recordIndex].Position;
            transform.rotation = Quaternion.Euler(_records[_recordIndex].Rotation);
            _recordIndex++;

        }
    }
}
