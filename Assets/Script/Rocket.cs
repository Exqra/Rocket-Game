using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource sound;

    [SerializeField] float rcsThrust = 175f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathClip;
    [SerializeField] AudioClip nextLevelClip;

    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem nextLevelParticles;

    enum State {Alive, Dead, Transcending }
    State state;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
        QualitySettings.shadows = ShadowQuality.Disable;
        setStateAlive();
        print("State is: " + state);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
            RocketMove();
    }
    private void RocketMove()
    {
        float rotateOffset = rcsThrust * Time.deltaTime;
        float mainOffset = mainThrust * Time.deltaTime;

        rigidbody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotateOffset);
        }
        else if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * rotateOffset);
        }
        else if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainOffset);
        }

        rigidbody.freezeRotation = false;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainOffset);
            if( state == State.Alive && !sound.isPlaying)
            {
                sound.PlayOneShot(mainEngine);
                thrustParticles.Play();
            }
                
        }
        else
        {
            if( sound.isPlaying)
                sound.Stop();
            thrustParticles.Stop();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
            return;

        const string friendly = "Friendly";
        const string finish = "Finish";
        switch (collision.gameObject.tag)
        {
            case friendly:
                // Do nothing
                break;
            case finish:
                nextLevelLogic();
                break;
            default:
                restartLevelLogic();
                break;
        }
    }
    private void nextLevelLogic()
    {
        print("Hit Obstacle");
        setStateTranscending();
        Invoke("loadNextLevel", 1f);
        sound.Stop();
        sound.PlayOneShot(nextLevelClip);
        thrustParticles.Stop();
        nextLevelParticles.Play();
    }
    private void restartLevelLogic()
    {
        print("Dead");
        setStateDead();
        Invoke("loadCurrentLevel", 1f);
        sound.Stop();
        sound.PlayOneShot(deathClip);
        thrustParticles.Stop();
        deathParticles.Play();
    }
    private void loadCurrentLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void loadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
    private void setStateAlive()
    {
        state = State.Alive;
    }
    private void setStateDead()
    {
        state = State.Dead;
    }
    private void setStateTranscending()
    {
        state = State.Transcending;
    }
}

