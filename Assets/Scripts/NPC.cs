using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float minTimeWait;
    [SerializeField] private float maxTimeWait;
    [SerializeField] private float maxDistance;

    [Header("Detections")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask isObstacle;

    private Vector3 targetPosition;
    private Vector3 startPosition;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        StartCoroutine(GoToTargetPositionAndWait());
    }

    private IEnumerator GoToTargetPositionAndWait() {
        while (true) {
            CalculateTargetPosition();
            while (transform.position != targetPosition) {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(minTimeWait, maxTimeWait));
        }
    }

    private void CalculateTargetPosition() {
        Vector3[] directions = new [] {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right
        };
        bool isWalkable = false;
        while (!isWalkable) {
            int randomDirection = Random.Range(0, directions.Length);
            targetPosition = transform.position + directions[randomDirection];

            isWalkable = TileIsWalkable();
        } 
    }

    private bool TileIsWalkable() {
        if (Vector3.Distance(targetPosition, startPosition) > maxDistance) {
            return false;
        } else {
            return !Physics2D.OverlapCircle(targetPosition, detectionRadius, isObstacle);
        }
    }

}
