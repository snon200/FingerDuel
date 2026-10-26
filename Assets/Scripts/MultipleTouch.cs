using UnityEngine;
using TMPro;

public class MultipleTouch : MonoBehaviour
{
    [SerializeField] private Vector2 touchOffset;
    [SerializeField] private GameObject fingerPrefab;
    [SerializeField] private TextMeshProUGUI player1Score;
    [SerializeField] private TextMeshProUGUI player2Score;

    private TouchLocation player1; //top player
    private TouchLocation player2; //bot player

    private Vector2 ropeSize;
    private Vector2 fingerSize;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;

        ropeSize = new Vector2(fingerPrefab.transform.GetChild(0).GetComponent<Rope>().lineWidth, fingerPrefab.transform.GetChild(0).GetComponent<Rope>().segmentLength);
        fingerSize = fingerPrefab.transform.localScale;
    }

    private void Update()
    {
        if ((Time.time - startTime) < 1f)
            return;

        for (int i=0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
            {
                Debug.Log("touch began");

                if (t.position.y >= (Screen.height / 2))
                {
                    if (player1 == null || player1.finger == null)
                        player1 = new TouchLocation(t.fingerId, CreateFinger(t));
                    else if (!player1.playerScript.hit)
                        player1.finger.transform.position = GetTouchPosition(t.position);

                    Enlarge(player1);
                }
                else
                {
                    if (player2 == null || player2.finger == null)
                        player2 = new TouchLocation(t.fingerId, CreateFinger(t));
                    else if (!player2.playerScript.hit)
                        player2.finger.transform.position = GetTouchPosition(t.position);

                    Enlarge(player2);
                }
            }
            else if (t.phase == TouchPhase.Moved)
            {
                Debug.Log("touch moving");

                if (player1 != null && player1.finger != null && !player1.playerScript.hit && t.position.y >= (Screen.height / 2))
                {
                    player1.finger.transform.position = GetTouchPosition(t.position);
                    Enlarge(player1);
                }
                else if (player2 != null && player2.finger != null && !player2.playerScript.hit && t.position.y <= (Screen.height / 2))
                {
                    player2.finger.transform.position = GetTouchPosition(t.position);
                    Enlarge(player2);
                }
            }
            else if (t.phase == TouchPhase.Ended)
            {
                Debug.Log("touch ended");

                if (player1 != null && player1.finger != null && t.position.y >= (Screen.height / 2))
                {
                    player1.finger.transform.localScale = fingerSize;
                    player1.rope.lineWidth = ropeSize.x;
                    player1.rope.segmentLength = ropeSize.y;
                }
                else if (player2 != null && player2.finger != null && t.position.y <= (Screen.height / 2))
                {
                    player2.finger.transform.localScale = fingerSize;
                    player2.rope.lineWidth = ropeSize.x;
                    player2.rope.segmentLength = ropeSize.y;
                }
            }
        }
    }

    private void Enlarge(TouchLocation player)
    {
        player.finger.transform.localScale = fingerSize * 1.3f;
        player.rope.lineWidth = ropeSize.x * 1.3f;
        player.rope.segmentLength = ropeSize.y * 1.3f;
    }

    private Vector2 GetTouchPosition(Vector2 touchPosition)
    {
        if (touchPosition.y >= (Screen.height / 2))
            return GetComponent<Camera>().ScreenToWorldPoint(new Vector3(touchPosition.x + touchOffset.x, touchPosition.y - touchOffset.y, transform.position.z));
        else
            return GetComponent<Camera>().ScreenToWorldPoint(new Vector3(touchPosition.x + touchOffset.x, touchPosition.y + touchOffset.y, transform.position.z));
    }

    private GameObject CreateFinger(Touch t)
    {
        GameObject c = Instantiate(fingerPrefab);

        if (t.position.y >= (Screen.height / 2)) //top player
        {
            c.name = "GreenTouch" + t.fingerId;

            c.transform.GetChild(0).GetComponent<LineRenderer>().material.color = new Color32(0, 235, 0, 255); //green
            c.transform.Rotate(new Vector3(0, 0, 180));
            c.transform.GetChild(0).GetComponent<Rope>().gravity *= -1f;
            c.GetComponent<PlayerScript>().scoreText = player2Score;
        }
        else //bottom player
        {
            c.name = "RedTouch" + t.fingerId;

            c.transform.GetChild(0).GetComponent<LineRenderer>().material.color = new Color32(255, 0, 0, 255); //red
            c.GetComponent<PlayerScript>().scoreText = player1Score;
        }

        c.transform.position = GetTouchPosition(t.position);
        return c;
    }
}