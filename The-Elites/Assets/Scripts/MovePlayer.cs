using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public KeyCode MoveL;
    public KeyCode MoveR;

    public float horizVel = 0;
    public int laneNum = 2;
    public string controLocked = "n";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(horizVel, 0, 4);

        if ((Input.GetKeyDown (MoveL)) && (laneNum>1) && (controLocked == "n"))
        {
            horizVel = -2;
            StartCoroutine(stopSlide());
            laneNum-=1;
            controLocked = "y";
        }
        if ((Input.GetKeyDown (MoveR)) && (laneNum<3)&& (controLocked == "n"))
        {
            horizVel = 2;
            StartCoroutine(stopSlide());
            laneNum+=1;
            controLocked = "y";
        }

    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "lethal")
        {
            Destroy(gameObject);
        }
    }
    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(0.5f);
        horizVel = 0;
        controLocked = "n";
    }
}
