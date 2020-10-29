using UnityEngine;

public class NavigationArrow : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    void Update()
    {
        RotateArrow();
    }

    private void RotateArrow()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = objectToFollow.position - transform.position;

        // Remap the direction, so the arrow doesn't rotate in vertical axis
        Vector3 remapWithoutY = new Vector3(targetDirection.x, 0, targetDirection.z);

        transform.rotation = Quaternion.LookRotation(remapWithoutY);
    }
}
