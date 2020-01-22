using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource sound;
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
        rigidbody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward);
        }
        else if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward);
        }
        else if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            rigidbody.AddRelativeForce(Vector3.up);
        }

        rigidbody.freezeRotation = false;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up);
            if( !sound.isPlaying)
                sound.Play();
        }
        else
        {
            if( sound.isPlaying)
                sound.Stop();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        const string friendly = "Friendly";
        const string finish = "Finish";
        switch (collision.gameObject.tag)
        {
            case friendly:
                // Do nothing
                break;
            case finish:
                print("Hit Obstacle");
                Invoke("loadNextLevel", 1f);
                setStateTranscending();
                break;
            default:
                print("Dead");
                Invoke("loadCurrentLevel", 1f);
                setStateDead();
                break;
        }
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

