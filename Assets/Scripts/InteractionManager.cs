using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject handContainer;
    public GameObject leftHand;
    public GameObject wrench;

    private RaycastHit hit;
    private RaycastHit handHit;

    private Vector3 forward;

    private bool havePerformedOutsideGrab = false;
    private bool wrenchInTargetPosition = false;
    private bool haveRotatedWrench = false;
    public bool wrenchAffixedToHand = false;
    private int wrenchRotationYAmount = -90;

    void Update()
    {
        MoveHandWithCursor();
        TriggerGrabOnClick();


        forward = handContainer.transform.TransformDirection(Vector3.forward) * 20;
        if (Physics.Raycast(leftHand.transform.position, forward, out handHit)) {
            wrenchInTargetPosition = true;

            if (Input.GetMouseButtonDown(0) && !wrenchAffixedToHand)
            {
                handContainer.GetComponent<Animator>().SetTrigger("GrabWrench");
                wrenchAffixedToHand = true;
            }
        }

        if (wrenchAffixedToHand)
        {

            if (!haveRotatedWrench)
            {
                AlignWrenchWithHand();
            }
                wrench.transform.position = new Vector3(leftHand.transform.position.x + 4, leftHand.transform.position.y + 1.6f, leftHand.transform.position.z);
        }
    }










    private void TriggerGrabOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!havePerformedOutsideGrab)
            {
                PerformGrabAnimation();
                havePerformedOutsideGrab = true;
                StartCoroutine("ResetOutsideGrab");
            }
        }
    }

    private IEnumerator ResetOutsideGrab()
    {
        yield return new WaitForSeconds(0.1f);
        havePerformedOutsideGrab = false;
    }

    private void MoveHandWithCursor()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            leftHand.transform.position = hit.point;
        }
    }

    private void PerformGrabAnimation()
    {
        handContainer.GetComponent<Animator>().SetTrigger("PerformGrab");
    }

    private void AlignWrenchWithHand()
    {
        Destroy(wrench.GetComponent<Collider>());
        wrench.transform.Rotate(wrenchRotationYAmount, 0, 0, Space.World);
        BoxCollider collider = wrench.AddComponent<BoxCollider>();
        collider.center = new Vector3(5, 0, 0);
        collider.size = new Vector3(40, 60, 15);
        haveRotatedWrench = true;
    }
}
