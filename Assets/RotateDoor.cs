using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RotateDoor : MonoBehaviour
{
    public GameObject ParentDoor;
    public GameObject doorPrefab;
    public GameObject canvas;

    public Transform Camera;

    private GameObject UIObj;
    private bool isOpen = false;
    private bool canToggle = false;
    private bool startRotation = false;
    private int dir = 0;
    private PhotonView photonView;
    float speed = 75.0f;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canToggle)
        {
            startRotation = true;
            if (photonView != null)
            {
                photonView.RPC("DoorRotate", RpcTarget.Others);
            }
        }

        if (Input.GetMouseButtonDown(0) && canToggle)
        {
            RaycastHit hitObject;
            if(Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitObject, 100))
            {
                if (hitObject.collider.CompareTag("Door"))
                {
                    startRotation = true;
                    if (photonView != null)
                    {
                        photonView.RPC("DoorRotate", RpcTarget.Others);
                    }
                }
            }
        }

        if (startRotation)
        {
            Debug.Log(ParentDoor.transform.localEulerAngles);
            if (ParentDoor.transform.localEulerAngles.y < 90 && dir == 0)
            {
                ParentDoor.transform.Rotate(Vector3.up * speed * Time.deltaTime);
                if(ParentDoor.transform.localEulerAngles.y > 90)
                {
                    dir = 1;
                    startRotation = false;
                }
            }
            else
            {
                ParentDoor.transform.Rotate(Vector3.up * -1*speed * Time.deltaTime);
                if (ParentDoor.transform.localEulerAngles.y < 0 || ParentDoor.transform.localEulerAngles.y > 350) 
                {
                    ParentDoor.transform.localEulerAngles = new Vector3(0, 0, 0);
                    dir = 0;
                    startRotation = false;
                }
            }

        }
    }

    [PunRPC]
    void DoorRotate()
    {
        startRotation = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Player")
        {
            /*GameObject newUi;
            newUi = Instantiate(doorPrefab, new Vector3(-0f, -0f, 0f), Quaternion.identity);
            newUi.transform.parent = canvas.transform;
            newUi.transform.position = new Vector3(0f, 0f, 0f);
            newUi.transform.position = new Vector3(300f, 50f, 0f);*/
            canToggle = true;
            //UIObj = newUi;
        }

    }

    void OnTriggerExit(Collider col)
    {
        this.canToggle = false;
        //Destroy(UIObj);
    }
  
}
