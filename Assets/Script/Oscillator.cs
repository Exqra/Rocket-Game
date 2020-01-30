using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [Range(0, 1)] [SerializeField] float movementFactor;
    private float period = 2f;

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        movementVector = new Vector3(10f, 10f, 10f);
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float sinValue = Mathf.Sin(tau * cycles);
        movementFactor = sinValue / 2 + 0.5f;
        transform.position = startingPos + (movementVector * movementFactor);
        
    }
}
