using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Behaviour of the 3d dice roll over the 2d UGUI
/// </summary>
public class Die : MonoBehaviour
{
    public float UnitVel = 1000;
    public float MaxMagnitude = 20000;
    public float MinMagnitude = 1000;
    public AudioClip BounceClip;
    public AudioClip ShakingClip;
    public AudioClip FinishedClip;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioPlayer = GetComponent<AudioSource>();
        _startPos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Reset();
            return;
        }

        // when we first let go, we apply a force, but it can take a frame to make it affect velocities
        if (_letGo)
        {
            _letGo = false;
            return;
        }

        if (_stopped)
            return;

        if (AutoRotate())
            return;

        TestStationary();
    }

    public void Roll(Action<int> finish)
    {
        gameObject.SetActive(true);
        Reset();
        _finished = finish;
    }

    public void Reset()
    {
        _rigidBody.isKinematic = true;
        _rigidBody.freezeRotation = false;
        _rigidBody.velocity = Vector3.zero;

        _audioPlayer.loop = true;
        _audioPlayer.clip = ShakingClip;
        _audioPlayer.Play();

        transform.position = _startPos;
        transform.rotation = Random.rotationUniform;

        _rolling = false;
        _stopped = false;
        _letGo = false;

        NewRotationTarget();
    }

    void NewRotationTarget()
    {
        _rotationSeek = Random.rotationUniform;
        _rotationSeekRate = Random.Range(1.5f, 3.5f);
        _rotationSeekTime = Random.Range(1, 2);
    }

    private bool AutoRotate()
    {
        if (_rolling)
            return false;

        _rotationSeekTime -= Time.deltaTime;
        if (_rotationSeekTime < 0)
            NewRotationTarget();

        transform.rotation = Quaternion.Slerp(transform.rotation, _rotationSeek, _rotationSeekRate * Time.deltaTime);
        return true;
    }

    private void TestStationary()
    {
        var v = _rigidBody.velocity.magnitude;
        var r = _rigidBody.angularVelocity.magnitude;

        // I'm leaving this here as a sign to not always rely on ReSharper for clarity
        if (!(v < 0.1f) || !(r < 0.1f))
            return;

        RollFinished();
    }

    private void RollFinished()
    {
        Debug.Log("Stopped roll");
        _rolling = false;
        _stopped = true;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.freezeRotation = true;
        _rigidBody.isKinematic = true;

        // results aren't really used yet. It's just an indication to the players.
        int result = 3;
        _audioPlayer.Stop();
        _audioPlayer.PlayOneShot(FinishedClip);
        StartCoroutine(Completed(result));
    }

    IEnumerator Completed(int result)
    {
        yield return null;
        yield return new WaitForSeconds(1);
        if (_finished != null)
            _finished(result);
    }

    void OnMouseDown()
    {
        if (!CanInteract)
            return;

        _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        _startMouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (!CanInteract)
            return;

        _lastScreenPoint = _screenPoint;
        _screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
        _velocity = _screenPoint - _lastScreenPoint;
        Vector3 world = Camera.main.ScreenToWorldPoint(_screenPoint) + _startMouseOffset;
        transform.position = world;
    }

    void OnMouseUp()
    {
        if (!CanInteract)
            return;

        var force = UnitVel * _velocity;
        force.z *= -1.5f;
        Debug.Log("Initial force: " + force);
        var mag = force.magnitude;
        if (mag > MaxMagnitude)
        {
            Debug.Log("Clipped, was " + mag);
            force = force / mag * MaxMagnitude;
        }
        else if (mag < MinMagnitude)
        {
            Debug.Log("Boosted, was " + mag);
            if (mag < 0.01f)
            {
                force = Random.onUnitSphere;
                mag = 1;
            }
            force = force / mag * MinMagnitude;
        }
        Debug.Log("Final force: " + force);

        _rigidBody.isKinematic = false;
        _rigidBody.AddForce(force, ForceMode.Force);
        _rolling = true;
        _letGo = true;
    }

    void OnCollisionEnter(Collision col)
    {
        float now = Time.time;
        if (now - _lastBounce < _minBounceInterval)
            return;
        _lastBounce = now;
        _audioPlayer.PlayOneShot(BounceClip);
    }

    private bool CanInteract { get { return !_rolling && !_stopped; } }

    private Rigidbody _rigidBody;
    private AudioSource _audioPlayer;
    Action<int> _finished;          // invoked when the roll has finished
    private float _minBounceInterval = 0.3f;
    private float _lastBounce = 0;  // time of last bounce, to avoid playing too many bounce sfx

    private Vector3 _startPos;
    private Vector3 _startMouseOffset;
    private Vector3 _screenPoint;
    private Vector3 _lastScreenPoint;
    private Vector3 _velocity;      // velocity at release time. has to be clamped to avoid 1. flying through colliders and 2. just dropping to a 6 or something.

    // yes, these should be an enum
    private bool _rolling = false;  // dice is currently rolling
    private bool _stopped = false;  // dice has stopped moving
    private bool _letGo = false;    // player has let go of the dice.

    // auto-rotate dice before player lets go
    private float _rotationSeekTime;
    private float _rotationSeekRate;
    private Quaternion _rotationSeek;
}
