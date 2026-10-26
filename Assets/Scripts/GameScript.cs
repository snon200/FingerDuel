using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameScript : MonoBehaviour
{
    [SerializeField] private GameObject annoucerText;

    private void Start() => Application.targetFrameRate = 60;

    public void EndGame(string loser) => StartCoroutine(EndCoroutine(loser));

    private IEnumerator EndCoroutine(string loser)
    {
        GetComponent<MultipleTouch>().enabled = false;

        annoucerText.GetComponent<TextMeshProUGUI>().fontSize = 150;
        if (loser.Contains("Green"))
        {
            annoucerText.GetComponent<TextMeshProUGUI>().text = "Red Player Won!";
            annoucerText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            annoucerText.GetComponent<TextMeshProUGUI>().text = "Green Player Won!";
            annoucerText.GetComponent<TextMeshProUGUI>().color = new Color32(0, 235, 0, 255);
        }
        annoucerText.GetComponent<Animation>().Play();

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Player"))
            if (!o.name.Equals(loser))
                o.GetComponent<Animation>().Play();

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(0);
    }
}
