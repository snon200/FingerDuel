using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public float gravity;

    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private BoxCollider2D[] colliders;

    public float segmentLength; //each segment length
    [SerializeField] private int segments; //number of segments
    public float lineWidth; //rope width

    [HideInInspector] public GameObject colliderObject;
    [SerializeField] private GameObject collidersPrefab;

    private void Awake() => lineRenderer = GetComponent<LineRenderer>();

    private void Start()
    {
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        colliderObject = Instantiate(collidersPrefab);
        transform.parent.GetComponent<PlayerScript>().colliderObject = colliderObject;
        if (transform.parent.eulerAngles.z == 0) //bottom player
            colliderObject.transform.position = new Vector2(colliderObject.transform.position.x, colliderObject.transform.position.y - 0.5f);
        else //top player
            colliderObject.transform.position = new Vector2(colliderObject.transform.position.x, colliderObject.transform.position.y + 0.5f);

        colliders = new BoxCollider2D[segments];
        for (int i = 0; i < segments; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= segmentLength;

            colliders[i] = colliderObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            colliders[i].size = new Vector2(lineWidth, segmentLength);
            colliders[i].isTrigger = true;
        }
    }

    private void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, gravity);

        for (int i = 1; i < segments; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to Parent object
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = transform.parent.position;
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < segments - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - segmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > segmentLength)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < segmentLength)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            ropePositions[i] = ropeSegments[i].posNow;

            colliders[i].offset = ropePositions[i];
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}