using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchManipulation : MonoBehaviour
{
    public GameObject bolt;
    public GameObject cam;
    public GameObject hand;


    private float wrenchShiftX = 0.2f;
    private float wrenchShiftZ = 0.6f;

    private bool attachWrenchToBolt = false;

    public void Update()
    {
        AttachWrenchToBolt();
    }

    //Snaps the wrench to the bolt
    private void AttachWrenchToBolt()
    {
        if (attachWrenchToBolt)
        {
            this.transform.position = new Vector3(bolt.transform.position.x + wrenchShiftX, bolt.transform.position.y, bolt.transform.position.z - wrenchShiftZ);
        }
    }

    //Sets specific constraints on the wrench's rigidbody to keep it from reacting after collision with bolt
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bolt")
        {
            cam.GetComponent<InteractionManager>().wrenchAffixedToHand = false;
            attachWrenchToBolt = true;
            cam.GetComponent<InteractionManager>().handConstrainedByWrench = true;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
            this.GetComponent<AudioSource>().Play();
        }
    }
}
