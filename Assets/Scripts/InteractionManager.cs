using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject handContainer;
    public GameObject leftHand;
    public GameObject wrench;
    public GameObject bolt;

    public bool wrenchAffixedToHand = false;
    public bool handConstrainedByWrench = false;

    private bool havePlayedPickUpSFX = false;
    private bool havePlayedWrenchTurnSFX = false;
    private bool haveBegunWrenchManipulation = false;

    private RaycastHit hit;
    private RaycastHit handHit;

    private Vector3 forward;

    private Vector3 newWrenchBoxColliderPosition = new Vector3(5, 0, 0);
    private Vector3 newWrenchBoxColliderSize = new Vector3(40, 60, 15);

    private float boltTurnIncrement = 0.008f;
    private float maxBoltZPosition = 9.58f;

    private float handShiftX = 4.0f;
    private float handShiftY = 1.6f;

    private bool havePerformedOutsideGrab = false;
    private bool haveRotatedWrench = false;
    private int wrenchRotationYAmount = -90;

    void Update()
    {
            MoveHandWithCursor();

            if (!haveBegunWrenchManipulation)
            {
                TriggerGrabOnClick();
            }
           
            PerformGrabOnWrench();
            AffixWrenchToHand();
            ConstrainHandByWrench();
    }

    //If hand is not yet locked into wrench turning position, move hand with mouse cursor.
    private void MoveHandWithCursor()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (!handConstrainedByWrench)
            {
                leftHand.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 1);
            }
        }
    }

    //If raycast has not targeted wrench, mouse click to see hand closing animation
    private void TriggerGrabOnClick()
    {
        if (Input.GetMouseButtonDown(0) && !wrenchAffixedToHand)
        {
     
            if (!havePerformedOutsideGrab && handHit.collider.gameObject.tag != "Wrench")
            {
                handContainer.GetComponent<Animator>().SetTrigger("PerformGrab");
                havePerformedOutsideGrab = true;

                StartCoroutine("ResetOutsideGrab");
            }
        }
    }

    //Reset hand animation after closing
    private IEnumerator ResetOutsideGrab()
    {
        yield return new WaitForSeconds(0.1f);
        havePerformedOutsideGrab = false;
    }

    //Shoot raycast forward from hand. If raycast from hits wrench and mouse clicked, hand locks onto wrench
    private void PerformGrabOnWrench()
    {
        int raycastLength = 10;

        forward = handContainer.transform.TransformDirection(Vector3.forward) * raycastLength;

        if (Physics.Raycast(leftHand.transform.GetChild(0).transform.position, forward, out handHit))
        {

           if (handHit.collider.gameObject.tag == "Wrench")
            {
                if (Input.GetMouseButtonDown(0) && !wrenchAffixedToHand && !haveBegunWrenchManipulation)
                {
                    handContainer.GetComponent<Animator>().SetTrigger("GrabWrench");
                    wrenchAffixedToHand = true;
                    haveBegunWrenchManipulation = true;
                }
            }
        }
    }

    //Attaches the wrench to the hand
    private void AffixWrenchToHand()
    {
        if (wrenchAffixedToHand)
        {
            if (!haveRotatedWrench)
            {
                AlignWrenchMovementWithHand();
            }
            wrench.transform.position = new Vector3(leftHand.transform.position.x + handShiftX, leftHand.transform.position.y + handShiftY, leftHand.transform.position.z);

            if (!havePlayedPickUpSFX)
            {
                leftHand.GetComponent<AudioSource>().Play();
                havePlayedPickUpSFX = true;
            }
         
        }
    }

    //Makes hand and wrench rotate around bolt
    private void ConstrainHandByWrench()
    {
        if (handConstrainedByWrench)
        {
            leftHand.transform.RotateAround(bolt.transform.position, Vector3.back, Input.GetAxis("Mouse X") * 4);
            wrench.transform.RotateAround(bolt.transform.position, Vector3.back, Input.GetAxis("Mouse X") * 4);

            TurnBolt();
        }
    }

    //Screws in bolt according to force applied when moving mouse right.
    private void TurnBolt()
    {
        if (Input.GetAxis("Mouse X") > 0)
        {
            if (bolt.transform.position.z <= maxBoltZPosition)
            {
                bolt.transform.position += new Vector3(0, 0, boltTurnIncrement);
                bolt.transform.RotateAround(bolt.transform.position, Vector3.back, Input.GetAxis("Mouse X") * 4);

                if (!havePlayedWrenchTurnSFX)
                {
                    this.GetComponent<AudioSource>().Play();
                    havePlayedWrenchTurnSFX = true;
                }
            } else
            {
                StopWrenchAfterBoltTurningComplete();
            }
        }
        else
        {
            havePlayedWrenchTurnSFX = false;
        }
    }

    private void StopWrenchAfterBoltTurningComplete()
    {
        wrench.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    //Flips wrench up to correct position when inside hand, removes box collider, then
    //creates a new, smaller one for interaction with the bolt.
    private void AlignWrenchMovementWithHand()
    {
        Destroy(wrench.GetComponent<Collider>());
        wrench.transform.Rotate(wrenchRotationYAmount, 0, 0, Space.World);
        BoxCollider collider = wrench.AddComponent<BoxCollider>();
        collider.center = newWrenchBoxColliderPosition;
        collider.size = newWrenchBoxColliderSize;
        haveRotatedWrench = true;
    }
}
