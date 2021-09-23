using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    private static Vector3 deltaPosition = new Vector3(0, 20, -20);

    void LateUpdate()
    {
        transform.position = player.transform.position + deltaPosition;
    }
}
