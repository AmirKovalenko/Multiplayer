using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{

    [SerializeField] private float speed = 10;

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * (Time.deltaTime * speed));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * (Time.deltaTime * speed));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * (Time.deltaTime * speed));
        }

    }
}
