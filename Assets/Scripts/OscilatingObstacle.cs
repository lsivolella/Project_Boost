using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class OscilatingObstacle : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(0f, 0f, 0f);
    [SerializeField] float movementPeriod = 1f;

    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ControllObjectOscilation();
    }

    private void ControllObjectOscilation()
    {
        if (movementPeriod <= Mathf.Epsilon) { return; } // Protects against Period == 0.
        float _movementCycles = Time.time / movementPeriod; // Grows continually from 0.

        const float _tau = Mathf.PI * 2;
        float _rawSinWave = Mathf.Sin(_movementCycles * _tau); // Goes from -1 to 1.

        float _movementMeter = _rawSinWave / 2f + 0.5f;
        // Because _rawSinWave / 2f equals -0.5f and 0.5f at its peaks, we need to add 0.5f
        // to make the movementMeter oscilate between 0 and 1.

        Vector3 _offset = movementVector * _movementMeter;
        transform.position = startingPosition + _offset;
    }
}
