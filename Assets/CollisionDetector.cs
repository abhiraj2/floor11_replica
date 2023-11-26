using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CollisionDetector : MonoBehaviour
{

    public GameObject canvas;
    public GameObject switchPrefab;
    GameObject UIObj;
    bool canSwitch = false;
    bool state = true;
    bool fanOn = false;
    public GameObject SwitchObject;
    public GameObject Camera;
    public GameObject Fan;
    public GameObject[] lights = new GameObject[15];
    float speed = 100.0f;
    private PhotonView photonView = null;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            photonView = GetComponent<PhotonView>();
        }
        catch
        {
            Debug.Log("This switch is not synced");
        }
         
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && canSwitch)
        {
            RaycastHit hitObject;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitObject, 100))
            {
                
                if (hitObject.collider.CompareTag("LightSwitch"))
                {
                    SwitchObject.transform.Rotate(SwitchObject.transform.forward * 180);
                    state = !state;
                    try
                    {
                        foreach (GameObject bulb in lights)
                        {
                            Light light = bulb.GetComponent<Light>();
                            light.enabled = !light.enabled;
                        }
                    }
                    catch
                    {
                        Debug.Log("All lights done");
                    }

                    if (photonView != null)
                    {
                        photonView.RPC("SwitchLights", RpcTarget.Others);
                    }
                }
                else if (hitObject.collider.CompareTag("FanSwitch") && Fan != null)
                {
                    SwitchObject.transform.Rotate(SwitchObject.transform.forward * 180);
                    fanOn = !fanOn;
                    if (photonView != null)
                    {
                        photonView.RPC("SwitchFan", RpcTarget.Others);
                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.L) && canSwitch)
        {
            SwitchObject.transform.Rotate(SwitchObject.transform.forward * 180);
            
            state = !state;
            try
            {
                foreach (GameObject bulb in lights)
                {
                    Light light = bulb.GetComponent<Light>();
                    light.enabled = !light.enabled;
                }
            }
            catch {
                Debug.Log("All lights done");
            }

            if(photonView != null)
            {
                photonView.RPC("SwitchLights", RpcTarget.Others);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.F) && canSwitch && Fan != null)
        {
            fanOn = !fanOn;
            if (photonView != null)
            {
                photonView.RPC("SwitchFan", RpcTarget.Others);
            }
        }
        if (fanOn)
        {
            Fan.transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }
    }

    [PunRPC]
    void SwitchFan()
    {
        fanOn = !fanOn;
    }

    [PunRPC]
    void SwitchLights()
    {
        Debug.Log("Got RPC");
        state = !state;
        try
        {
            foreach (GameObject bulb in lights)
            {
                Light light = bulb.GetComponent<Light>();
                light.enabled = !light.enabled;
            }
        }
        catch
        {
            Debug.Log("All lights done");
        }
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("Done");
        if (col.gameObject.name == "Player")
        {
            canSwitch = false;
            //Destroy(UIObj);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Collided");
        if (col.gameObject.name == "Player")
        {
            /*GameObject newUi;
            newUi = Instantiate(switchPrefab, new Vector3(-0f, -0f, 0f), Quaternion.identity);
            newUi.transform.parent = canvas.transform;
            newUi.transform.position = new Vector3(0f, 0f, 0f);
            newUi.transform.position = new Vector3(300f, 100f, 0f);*/
            canSwitch = true;
            //UIObj = newUi;
        }
    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log("Collided " + col.gameObject.name);
        if (col.gameObject.name == "Player")
        {
            Debug.Log("Collided With Player");
        }
    }

}
