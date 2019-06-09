using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchManipulation : MonoBehaviour
{
    public GameObject bolt;
    public GameObject cam;
    private bool attachWrenchToBolt = false;


    public void Update()
    {
        if (attachWrenchToBolt)
        {
            this.transform.position = bolt.transform.position;
        }

        Debug.Log(this.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bolt")
        {
            cam.GetComponent<InteractionManager>().wrenchAffixedToHand = false;
            attachWrenchToBolt = true;

        }
    }
}
