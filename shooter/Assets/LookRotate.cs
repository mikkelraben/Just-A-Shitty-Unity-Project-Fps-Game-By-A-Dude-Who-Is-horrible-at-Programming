using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookRotate : NetworkBehaviour {
    public float rotateX;
    public float rotateY;
    public float sensitivity;
    GameObject Child;
    Quaternion quaternion = Quaternion.identity;
	// Update is called once per frame
	void Update () {
        if(hasAuthority == false){
            return;
        }

        rotateX = (Input.GetAxis("Mouse Y") + quaternion.eulerAngles.x);
        rotateY = Input.GetAxis("Mouse X") + quaternion.eulerAngles.y;
        if(rotateX > 85 && rotateX < 180){
            rotateX = 85;
        }
        if(rotateX < 270 && rotateX > 180){
            rotateX = 270;
        }
        Child = transform.GetChild(0).gameObject;

        quaternion.eulerAngles = new Vector3(0, rotateY, 0);
        transform.SetPositionAndRotation(transform.position, quaternion);
        quaternion.eulerAngles = new Vector3(rotateX, rotateY, 0);
        Child.transform.SetPositionAndRotation(Child.transform.position, quaternion);

    }
}
