using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioClip _click;
    void Update()
    {
        if ((SceneManager.GetActiveScene().name.Equals("How To Play") && Input.anyKey) 
            || Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<AudioSource>().PlayOneShot(_click);
            SceneManager.LoadScene("Prototype 1");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("How To Play");
        }
    }
}
