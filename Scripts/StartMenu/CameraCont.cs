using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCont : MonoBehaviour
{
    [SerializeField] GameObject rotator;
    public float rotateMult;
    void Update()
    {
        rotateMult = rotator.transform.rotation.eulerAngles.y;
        rotateMult = rotateMult - Time.deltaTime * 10;
        rotator.transform.rotation = Quaternion.Euler(0, rotateMult, 0);

        if (rotateMult <= -360)
        rotateMult = 0;
    }
}
