using System.Collections.Generic;
using GameLabGraz.ImmersiveAnalytics;
using UnityEngine;

public class PlayerTrack : MonoBehaviour
{
    [SerializeField] private ObjectTrack head;
    [SerializeField] private ObjectTrack rightHand;
    [SerializeField] private ObjectTrack leftHand;
    
    private readonly List<ObjectTrack> _objectTracks = new List<ObjectTrack>();

    private void Start()
    {
        _objectTracks.AddRange(new []
        {
            head, rightHand, leftHand
        });
    }

    public void PlayTrack()
    {
        foreach (var track in _objectTracks)
            track.PlayTrack();
    }

    public void StopTrack()
    {
        foreach (var track in _objectTracks)
            track.StopTrack();
    }

    public void RevertTrack()
    {
        foreach (var track in _objectTracks)
            track.RevertTrack();
    }
}
