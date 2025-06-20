using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    private float inputX;
    private float inputY;
    private bool isMoving;
    private Vector3 destinationPoint;
    private Vector3 interactionPoint;
    private Vector3 lastInput;
    private Collider2D frontCollider;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float radioInteraction;
    [SerializeField] private LayerMask isObstacle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (inputY == 0) {
            inputX = Input.GetAxisRaw("Horizontal");
        }
        if (inputX == 0) {
            inputY = Input.GetAxisRaw("Vertical");
        }

        if (!isMoving && (inputX != 0 || inputY != 0)) {
            lastInput = new Vector3(inputX, inputY, 0);
            destinationPoint = transform.position + lastInput;
            interactionPoint = destinationPoint;

            frontCollider = CheckInFront();
            if (!frontCollider) {
                StartCoroutine(Move());
            }
        }
    }

    IEnumerator Move() {
        isMoving = true;            
        
        while (transform.position != destinationPoint) {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, movementSpeed * Time.deltaTime);
            yield return null;
        }
        interactionPoint = transform.position + lastInput;
        isMoving = false; 
    }

    private Collider2D CheckInFront() {
        return Physics2D.OverlapCircle(interactionPoint, radioInteraction, isObstacle);
    }
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(interactionPoint, radioInteraction);
    }
}
