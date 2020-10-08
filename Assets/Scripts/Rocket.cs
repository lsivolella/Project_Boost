using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] float boostForce = 1f;
    [SerializeField] float rotationForce = 1f;
    [SerializeField] float loadDelay = 1f;

    // Cached Variables
    enum State { alive, suspended, dead};
    State state;
    private float horizontalRotation;

    // Cached References
    Rigidbody rigidBody;
    AudioSource audioSource;
    SceneManager sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        state = State.alive;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.alive)
        {
            GetMouseInput();
            GetHorizontalInput();
        }
    }

    private void GetMouseInput()
    {
        if (Input.GetButton("Fire1"))
        {
            // Relative Force adds the thrust in Local coordinates.
            rigidBody.AddRelativeForce(Vector3.up * boostForce);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void GetHorizontalInput()
    {
        rigidBody.freezeRotation = true;
        horizontalRotation = Input.GetAxisRaw("Horizontal");
        rigidBody.freezeRotation = false;
    }

    private void FixedUpdate()
    {
        RotateRocket();
    }

    private void RotateRocket()
    {
        rigidBody.transform.Rotate(Vector3.forward * -horizontalRotation * rotationForce * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                SuspendPlayer();
                Invoke("FinishLevel", loadDelay);
                break;
            default:
                Debug.Log("Test");
                KillPlayer();
                Invoke("ReloadLevel", loadDelay) ;
                break;
        }
    }

    private void FinishLevel()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        sceneManager.LoadNextScene();
    }

    private void ReloadLevel()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        sceneManager.ReloadScene();
    }

    private void KillPlayer()
    {
        state = State.dead;
        audioSource.Stop();
    }

    private void SuspendPlayer()
    {
        state = State.suspended;
        audioSource.Stop();
    }
}
