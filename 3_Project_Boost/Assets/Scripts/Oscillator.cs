using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10, 0, 0);
    [SerializeField] float period = 2f;
   

    private Vector3 startingPos;

    //todo remove from inspector later

    float movementFactor; //0 for not moved, 1 for fully moved.

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        //set movementFactor automatically
        if(period <= Mathf.Epsilon) { return ; }
        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2; // 2pi 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + .05f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
