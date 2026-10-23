using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Grabbable : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    [SerializeField] private float requiredStrength;

    private TargetJoint2D activeGrabJoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>Begins a physics-driven hold at the supplied world-space target.</summary>
    public bool TryBeginGrab(Vector2 target, float maxForce, float frequency, float dampingRatio)
    {
        if (activeGrabJoint != null || rb == null || rb.bodyType != RigidbodyType2D.Dynamic)
        {
            return false;
        }

        activeGrabJoint = rb.gameObject.AddComponent<TargetJoint2D>();
        activeGrabJoint.autoConfigureTarget = false;
        activeGrabJoint.anchor = rb.transform.InverseTransformPoint(target);
        activeGrabJoint.target = target;
        activeGrabJoint.maxForce = maxForce;
        activeGrabJoint.frequency = frequency;
        activeGrabJoint.dampingRatio = dampingRatio;
        return true;
    }

    /// <summary>Updates the held body's world-space physics target.</summary>
    public void UpdateGrabTarget(Vector2 target)
    {
        if (activeGrabJoint != null)
        {
            activeGrabJoint.target = target;
        }
    }

    /// <summary>Removes the temporary physics joint if one is active.</summary>
    public void Release()
    {
        if (activeGrabJoint == null)
        {
            return;
        }

        Destroy(activeGrabJoint);
        activeGrabJoint = null;
    }
}
