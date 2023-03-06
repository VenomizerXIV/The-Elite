using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // subscribe to event when game object enabled
    private void OnEnable()
    {
        PlayerMovement.superPowerInfo += superPowerActivated;
    }
    // unsubscribe from event when game object disabled
    private void OnDisable()
    {
        PlayerMovement.superPowerInfo -= superPowerActivated;
    }

    // function activated when superPower activated
    private void superPowerActivated()
    {
        Debug.Log("destory your self");
        Destroy(gameObject);
    }
}
