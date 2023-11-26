using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviour
{
    public Transform body;
    private Transform player;
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            body.gameObject.SetActive(false);
            body.position = new Vector3(player.position.x, body.position.y, player.position.z);
            body.rotation = player.rotation;
        }
    }
}
