using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Die :  MonoBehaviour
{
    public float UnitVel = 1000;
    public float MaxMagnitude = 20000;
    public float MinMagnitude = 1000;

	void Awake()
	{
	    _rb = GetComponent<Rigidbody>();
	    startPos = transform.position;
	    Reset();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Reset();
            return;
        }

        // when we first let go, we apply a force, but it can take a frame to make it affect velocities
        if (letGo)
        {
            letGo = false;
            return;
        }

        if (stopped)
            return;

        if (AutoRotate())
            return;

        TestStationary();
    }

    Action<int> _finished;

    public void Roll(Action<int> finish)
    {
        gameObject.SetActive(true);
        Reset();
        _finished = finish;
    }

    public void Reset()
    {
	    _rb.isKinematic = true;
        _rb.freezeRotation = false;
        _rb.velocity = Vector3.zero;

        transform.position = startPos;
	    transform.rotation = Random.rotationUniform;

	    rolling = false;
	    stopped = false;
        letGo = false;

        NewRotationTarget();
    }

    void NewRotationTarget()
    {
        rotationSeek = Random.rotationUniform;
        rotationSeekRate = Random.Range(1.5f, 3.5f);
        rotationSeekTime = Random.Range(1, 2);
    }

    private bool AutoRotate()
    {
        if (rolling)
            return false;

        rotationSeekTime -= Time.deltaTime;
        if (rotationSeekTime < 0)
            NewRotationTarget();

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationSeek, rotationSeekRate*Time.deltaTime);
        return true;
    }

    private void TestStationary()
    {
        var v = _rb.velocity.magnitude;
        var r = _rb.angularVelocity.magnitude;
        if (!(v < 0.01f) || !(r < 0.01f))
            return;

        RollFinished();
    }

    private void RollFinished()
    {
        Debug.Log("Stopped roll");
        rolling = false;
        stopped = true;
        _rb.velocity = Vector3.zero;
        _rb.freezeRotation = true;
        _rb.isKinematic = true;

        int result = 3;
        StartCoroutine(Completed(result));
    }

    IEnumerator Completed(int result)
    {
        yield return null;
        yield return new WaitForSeconds(3);
        if (_finished != null)
            _finished(result);
        gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (!CanInteract)
            return;

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (!CanInteract)
            return;

        lastScreenPoint = screenPoint;
        screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        velocity = screenPoint - lastScreenPoint;
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPoint) + offset;
        transform.position = world;
    }

    void OnMouseUp()
    {
        if (!CanInteract)
            return;

        var force = UnitVel * velocity;
        force.z *= -1.5f;
        Debug.Log("Initial force: " + force);
        var mag = force.magnitude;
        if (mag > MaxMagnitude)
        {
            Debug.Log("Clipped, was " + mag);
            force = force / mag * MaxMagnitude;
        }
        if (mag < MinMagnitude)
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

        _rb.isKinematic = false;
        _rb.AddForce(force, ForceMode.Force);
        rolling = true;
        letGo = true;
    }

    private bool CanInteract { get { return !rolling && !stopped; } }
    private Rigidbody _rb;
    private Vector3 startPos;
    private Vector3 offset;
    private Vector3 screenPoint;
    private Vector3 lastScreenPoint;
    private Vector3 velocity;
    private float rotationSeekTime;
    private float rotationSeekRate;
    private Quaternion rotationSeek;
    private bool rolling = false;
    private bool stopped = false;
    private bool letGo = false;
}
