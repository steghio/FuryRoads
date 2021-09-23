using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float turnSpeed = 25.0f;
    private float horizontalInput;
    private float forwardInput;
    private bool isTank;
    private int score;
    private int highscore;
    private static Vector3 deltaScorePosition = new Vector3(0, 10, 10);
    private static Vector3 deltaTimePosition = new Vector3(5, 10, 10);
    private static Vector3 deltaHighscorePosition = new Vector3(5, 13, 10);

    private MeshCollider _meshCollider;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    [SerializeField] private GameObject _tank;
    [SerializeField] private GameObject _car;
    [SerializeField] private GameObject _score;
    [SerializeField] private GameObject _time;
    [SerializeField] private GameObject _highscore;
    [SerializeField] private AudioClip _dieAudio;
    [SerializeField] private AudioClip _finishAudio;

    private void Awake()
    {

        _meshCollider = GetComponent<MeshCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        _rigidbody.mass = 100f;

        _tank.GetComponent<MeshCollider>().enabled = false;
        _car.GetComponent<MeshCollider>().enabled = false;

        _tank.SetActive(true);
        _car.SetActive(false);

        isTank = true;
        score = 0;
        highscore = PlayerPrefs.GetInt("highscore", 0);

        Time.timeScale = 1;
    }

    private void LateUpdate()
    {
        if(Time.timeScale == 0)
        {
            return;
        }
        _score.transform.position = transform.position + deltaScorePosition;
        _score.GetComponent<TextMesh>().text = score.ToString();

        _time.transform.position = transform.position + deltaTimePosition;
        _time.GetComponent<TextMesh>().text = Time.timeSinceLevelLoad.ToString("0.00");

        _highscore.transform.position = transform.position + deltaHighscorePosition;
        _highscore.GetComponent<TextMesh>().text = highscore.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if ((_tank.activeSelf && (_tank.transform.position.x <= -10 || _tank.transform.position.x >= 10)) 
            || (_car.activeSelf && (_car.transform.position.x <= -10 || _car.transform.position.x >= 10)))
        {
            _audioSource.PlayOneShot(_dieAudio);
        }

        if (Input.GetKeyDown(KeyCode.R) 
            || (_tank.activeSelf && _tank.transform.position.y <= -5) 
            || (_car.activeSelf && _car.transform.position.y <= -5))
        {
            SceneManager.LoadScene("Prototype 1");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTank = !isTank;

            if (isTank)
            {
                _tank.SetActive(true);
                _car.SetActive(false);
                _meshCollider.sharedMesh = _tank.GetComponent<MeshCollider>().sharedMesh;
                speed = 5.0f;
                turnSpeed = 25.0f;
                _rigidbody.mass = 100f;
            }
            else
            {
                _tank.SetActive(false);
                _car.SetActive(true);
                _meshCollider.sharedMesh = _car.GetComponent<MeshCollider>().sharedMesh;
                speed = 20.0f;
                turnSpeed = 45.0f;
                _rigidbody.mass = 10f;
            }
        }

        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            score++;
        }

        if (collision.gameObject.CompareTag("boulder"))
        {
            score -= 5;
        }

        if (collision.gameObject.CompareTag("finish"))
        {
            _audioSource.PlayOneShot(_finishAudio);
            score += 60 - (int)Math.Ceiling(Time.timeSinceLevelLoad);
            PlayerPrefs.SetInt("highscore", Math.Max(score, highscore));
            Time.timeScale = 0;
            _time.GetComponent<TextMesh>().text = "";
            _score.GetComponent<TextMesh>().text = "SCORE: " + score.ToString("0.00");
        }
    }
}
