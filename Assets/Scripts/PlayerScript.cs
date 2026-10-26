using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private GameObject deathParticles;

    [HideInInspector] public GameObject colliderObject;
    [HideInInspector] public TextMeshProUGUI scoreText;
    [HideInInspector] public bool hit = false;

    private SpriteRenderer fingerSprite;

    private int currHealth;

    private void Awake() => fingerSprite = transform.Find("finger").GetComponent<SpriteRenderer>();

    private void Start() => currHealth = maxHealth;

    //player got hit
    public void Hit()
    {
        Debug.Log("player hit");

        if (!hit)
        {
            currHealth--;
            if (currHealth <= 0)
            {
                Death();
                currHealth = maxHealth;
            }
            else
                StartCoroutine(knockback());
        }
    }

    private IEnumerator knockback()
    {
        hit = true;

        //turn red
        float ElapsedTime = 0;
        float TotalTime = 0.05f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            fingerSprite.color = Color.Lerp(fingerSprite.color, Color.red, ElapsedTime / TotalTime);
            if (transform.eulerAngles.z == 0)
                transform.position = Vector3.Lerp(transform.localPosition, new Vector2(transform.localPosition.x, transform.localPosition.y - 0.3f), ElapsedTime / TotalTime);
            else
                transform.position = Vector3.Lerp(transform.localPosition, new Vector2(transform.localPosition.x, transform.localPosition.y + 0.3f), ElapsedTime / TotalTime);
            yield return null;
        }

        ElapsedTime = 0;
        TotalTime = 0.1f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            fingerSprite.color = Color.Lerp(fingerSprite.color, new Color(255, 220, 220), ElapsedTime / TotalTime);
            if (transform.eulerAngles.z == 0)
                transform.position = Vector3.Lerp(transform.localPosition, new Vector2(transform.localPosition.x, transform.localPosition.y - 0.04f), ElapsedTime / TotalTime);
            else
                transform.position = Vector3.Lerp(transform.localPosition, new Vector2(transform.localPosition.x, transform.localPosition.y + 0.04f), ElapsedTime / TotalTime);
            yield return null;
        }

        //turn white
        ElapsedTime = 0;
        TotalTime = 0.05f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            fingerSprite.color = Color.Lerp(fingerSprite.color, Color.white, ElapsedTime / TotalTime);
            if (transform.eulerAngles.z == 0)
                transform.position = Vector3.Lerp(transform.localPosition, new Vector2(transform.localPosition.x, transform.localPosition.y - 0.01f), ElapsedTime / TotalTime);
            else
                transform.position = Vector3.Lerp(transform.localPosition, new Vector2(transform.localPosition.x, transform.localPosition.y + 0.01f), ElapsedTime / TotalTime);
            yield return null;
        }

        hit = false;
    }

    //player died
    private void Death()
    {
        Debug.Log("player died");

        GameObject p = Instantiate(deathParticles);
        p.transform.position = transform.position;

        scoreText.text = (int.Parse(scoreText.text) + 1).ToString();

        if (int.Parse(scoreText.text) >= 3)
            Camera.main.GetComponent<GameScript>().EndGame(gameObject.name);

        Destroy(colliderObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "sword" && collision.gameObject != colliderObject)
            Hit();
    }
}
