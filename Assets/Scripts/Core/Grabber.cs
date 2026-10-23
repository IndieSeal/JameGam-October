using UnityEngine;
using UnityEngine.InputSystem;

public class Grabber : MonoBehaviour
{
    private const string INTERACT_ACTION_NAME = "Player/Interact";

    [SerializeField] private Transform grabPoint;
    [SerializeField] private float reachRadius = 0.5f;
    [SerializeField] private LayerMask grabbableLayer;
    [SerializeField] private float maxForce = 1000f;
    [SerializeField] private float frequency = 5f;
    [SerializeField] private float dampingRatio = 0.7f;

    private Grabbable inReach;
    private Grabbable heldObject;
    private bool hasWarnedMissingSetup;

    public Grabbable InReach => inReach;
    public Grabbable HeldObject => heldObject;

    private void OnEnable()
    {
        if (grabPoint == null)
        {
            WarnMissingSetup("Grabber requires a hand-point Transform.");
        }

        UserInput.OnInteractStart += OnInteractStarted;
        UserInput.OnInteractStop += OnInteractCanceled;
    }

    private void Update()
    {
        Grabbable nearest = FindNearest();

        if (nearest != inReach)
        {
            inReach = nearest;
            //Debug.log(inReach != null ? $"In reach: {inReach.name}" : "Nothing in reach");
        }
    }

    private void FixedUpdate()
    {
        if (heldObject == null || heldObject.rb == null || grabPoint == null)
        {
            if (heldObject != null)
            {
                ReleaseHeldObject();
            }

            return;
        }

        heldObject.UpdateGrabTarget(grabPoint.position);
    }

    private Grabbable FindNearest()
    {
        if (grabPoint == null)
        {
            return null;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(grabPoint.position, reachRadius, grabbableLayer);

        Grabbable best = null;
        float bestDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            if (!hit.TryGetComponent(out Grabbable candidate))
            {
                continue;
            }

            float distance = ((Vector2)grabPoint.position - hit.ClosestPoint(grabPoint.position)).sqrMagnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = candidate;
            }
        }

        return best;
    }

    private void OnInteractStarted()
    {
        ////Debug.log($"Interact started. held={heldObject}, inReach={inReach}");

        if (heldObject != null || inReach == null || grabPoint == null)
        {
            return;
        }

        if (inReach.TryBeginGrab(grabPoint.position, maxForce, frequency, dampingRatio))
        {
            heldObject = inReach;
            //Debug.log($"Grabbed {heldObject.name}");
        }
        else
        {
            //Debug.logWarning($"TryBeginGrab failed for {inReach.name} (already grabbed, missing Rigidbody2D, or not Dynamic).", inReach);
        }
    }

    private void OnInteractCanceled()
    {
        //Debug.log("Interact canceled, releasing.");
        ReleaseHeldObject();
    }

    private void ReleaseHeldObject()
    {
        if (heldObject != null)
        {
            heldObject.Release();
        }

        heldObject = null;
    }

    private void OnDisable()
    {
        UserInput.OnInteractStart -= OnInteractStarted;
        UserInput.OnInteractStop -= OnInteractCanceled;

        ReleaseHeldObject();
    }

    private void WarnMissingSetup(string message)
    {
        if (hasWarnedMissingSetup)
        {
            return;
        }

        hasWarnedMissingSetup = true;
        //Debug.logWarning(message, this);
    }

    private void OnDrawGizmosSelected()
    {
        if (grabPoint == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(grabPoint.position, reachRadius);
    }
}
