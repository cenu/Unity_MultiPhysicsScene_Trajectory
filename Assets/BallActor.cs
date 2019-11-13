
using UnityEngine;

public class BallActor : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private bool _play;
    private float _potential;

    public Vector3 position { get { return _rigidBody.position; } }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.isKinematic = true;
    }

    public void SetRenderable(bool renderable)
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var ren in renderers)
            ren.enabled = renderable;
    }

    public void Warp(Vector3 pos)
    {
        _rigidBody.position = pos;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
    }

    public void Play(float potential)
    {
        _rigidBody.isKinematic = false;
        _play = true;
        _potential = potential;

        _dbg_Log = 0;
    }

    public void Stop()
    {
        _rigidBody.isKinematic = true;
        _play = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    int _dbg_Log;
    public void Simulate(float fixedDeltaTime)
    {
        if (_play && 0 < _potential)
        {
            _rigidBody.AddForce(new Vector3(0, 0, _potential), ForceMode.Force);
            _potential -= fixedDeltaTime;
        }
    }
}
