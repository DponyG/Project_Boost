using UnityEngine.SceneManagement;
using UnityEngine;


public class Movement : MonoBehaviour {

    [SerializeField] float rcsThrust = 100.0f;
    [SerializeField] float mainThrust = 100.0f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip finish;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem finishParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;


    enum State { Alive, Dying, Trancending};
    State state = State.Alive;


	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
      
        if (state == State.Alive) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
	}


    void OnCollisionEnter(Collision collision) {
        if(state != State.Alive) { return; }
        switch (collision.gameObject.tag) {
            case "Friendly":
                break;

            case "Finish":
                StartFinish();
                break;
            default:
                StartDeath();
                break;
        }
    }

    private void StartFinish() {
        state = State.Trancending;
        audioSource.Stop();
        audioSource.PlayOneShot(finish);
        finishParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay); //parameterise time
    }

    private void StartDeath() {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene(1); 
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0); 
    }

    private void RespondToThrustInput() {

        if (Input.GetKey(KeyCode.Space)) {

            ApplyThrust();
           
        } else {
            audioSource.Stop();
          //  mainEngineParticles.Stop(); // TODO NOT WORKING FIX 
        }
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust* Time.deltaTime );
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }

      //  mainEngineParticles.Play();


    }

    private void RespondToRotateInput() {

        rigidBody.freezeRotation = true; //take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;
    
   
        if (Input.GetKey(KeyCode.A)) {

            transform.Rotate(Vector3.forward* rotationThisFrame);
        } 
        else if (Input.GetKey(KeyCode.D)) {
   
            transform.Rotate(-Vector3.forward* rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume rotation
    }

}
