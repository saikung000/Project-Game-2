using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Boss_NetworkAnimator : NetworkBehaviour
{

    void Start()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(3, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(4, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(5, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(6, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(7, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(8, true);
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnStartLocalPlayer()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(3, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(4, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(5, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(6, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(7, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(8, true);
   

    }
    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(3, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(4, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(5, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(6, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(7, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(8, true);
       
    }
}
