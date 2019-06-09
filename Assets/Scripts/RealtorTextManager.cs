using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtorTextManager : MonoBehaviour
{ 

    public GameObject descriptionText;

    //Can't make these into a single function because the  

    void MakeDescriptionTextAppear()
    {
        descriptionText.SetActive(true);
    }
    void MakeDescriptionTextDisappear()
    {
        descriptionText.SetActive(false);
    }
}
