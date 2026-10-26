using UnityEngine;

public class TouchLocation
{
    public int touchId;
    public GameObject finger;
    public Rope rope;
    public PlayerScript playerScript;

    public TouchLocation(int newTouchId, GameObject newFinger)
    {
        touchId = newTouchId;
        finger = newFinger;
        rope = finger.transform.Find("Rope").GetComponent<Rope>();
        playerScript = finger.GetComponent<PlayerScript>();
    }
}