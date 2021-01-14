using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] float boostForce = 1f;
    [SerializeField] float rotationForce = 1f;
    [SerializeField] float loadDelay = 1f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem mainEnginePS;
    [SerializeField] ParticleSystem explosionPS;
    [SerializeField] ParticleSystem successPS;

    // Cached Variables
    private float horizontalRotation;

    // Cached References
    Rigidbody rigidBody;
    AudioSource audioSource;
    SceneManager sceneManager;

    // Cached States
    private bool inPlayMode = true;
    private bool isInvencible = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        inPlayMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        ForceInvincibilityToRocket();

        if (inPlayMode)
        {
            GetThrustInput();
            GetRotationInput();
        }
    }

    private void GetThrustInput()
    {
        if (Input.GetButton("Fire1"))
        {
            // Relative Force adds the thrust in Local coordinates.
            rigidBody.AddRelativeForce(Vector3.up * boostForce * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                PlayOneShotAudio(mainEngineSound);
                mainEnginePS.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainEnginePS.Stop();
        }
    }

    private void GetRotationInput()
    {
        rigidBody.angularVelocity = Vector3.zero;
        horizontalRotation = Input.GetAxisRaw("Horizontal");
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
        if (!inPlayMode || isInvencible) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                SuspendPlayer();
                Invoke("FinishLevel", successSound.length);
                break;
            default:
                KillPlayer();
                Invoke("ReloadLevel", explosionSound.length) ;
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
        PlayOneShotAudio(explosionSound);
        explosionPS.Play();
        inPlayMode = false;
        UnpackRocket();
    }

    private void SuspendPlayer()
    {
        PlayOneShotAudio(successSound);
        successPS.Play();
        inPlayMode = false;
    }

    private void PlayOneShotAudio(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip);
    }

    private void UnpackRocket()
    {
        var _childCount = gameObject.transform.childCount;
        for (int i = 0; i < _childCount; i++)
        {
            var _child = gameObject.transform.GetChild(i);
            _child.gameObject.AddComponent<Rigidbody>();
        }
        gameObject.transform.DetachChildren();
    }

    private void ForceInvincibilityToRocket()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                isInvencible = !isInvencible;
            }
        }
    }
}
