using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLabGraz.ImmersiveAnalytics
{
    public class ObjectTrack : MonoBehaviour
    {
        [SerializeField] private int playerId;

        [SerializeField] private int objectId;

        private DateTime _starTime;
        private DateTime _endTime;

        private bool _play;
        private int _recordIndex = 1;
        private DateTime _lastRecordTime;

        private Vector3 _startPosition;
        private Vector3 _startRotation;

        public List<TrackEntry> Records { get; set; } = new List<TrackEntry>();

        private void Awake()
        {
            Records = TrackManager.Instance.GetTrackRecords(playerId, objectId);
            _startPosition = Records.First().Position;
            _startRotation = Records.First().Rotation;
            _starTime = Records.First().Time;
            _endTime = Records.Last().Time;

            _lastRecordTime = _starTime;
            transform.position = _startPosition;
            transform.rotation = Quaternion.Euler(_startRotation);
        }

        private IEnumerator Play()
        {
            while (_play && _recordIndex < Records.Count)
            {
                var timeToWait = (float)(Records[_recordIndex].Time - _lastRecordTime).TotalSeconds;
                yield return new WaitForSeconds(timeToWait);

                _lastRecordTime = Records[_recordIndex].Time;

                transform.position = Records[_recordIndex].Position;
                transform.rotation = Quaternion.Euler(Records[_recordIndex].Rotation);
                _recordIndex++;
            }
        }

        public void PlayTrack()
        {
            if (_play) return;

            _play = true;
            StartCoroutine(Play());
        }

        public void StopTrack()
        {
            _play = false;
        }

        public void RevertTrack()
        {
            _play = false;
            _recordIndex = 0;
            _lastRecordTime = _starTime;

            transform.position = _startPosition;
            transform.rotation = Quaternion.Euler(_startRotation);
        }
    }
}
