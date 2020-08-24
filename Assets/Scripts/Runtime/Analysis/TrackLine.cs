using GameLabGraz.ImmersiveAnalytics;
using UnityEngine;

[RequireComponent(typeof(ObjectTrack))]
public class TrackLine : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float width = 0.1f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    private ObjectTrack _objectTrack;
    private LineRenderer _line;

    private void Start()
    {
        _objectTrack = GetComponent<ObjectTrack>();
        SetupTrackLine();
    }

    private void SetupTrackLine()
    {
        _line = gameObject.AddComponent<LineRenderer>();
        _line.useWorldSpace = true;
        _line.material = material == null? new Material(Shader.Find("Diffuse")) : material;
        _line.startWidth = _line.endWidth = width;
        _line.positionCount = _objectTrack.Records.Count;
        
        for (var i = 0; i < _objectTrack.Records.Count; i++)
        {
            _line.SetPosition(i, _objectTrack.Records[i].Position + offset);
        }
    }
}
