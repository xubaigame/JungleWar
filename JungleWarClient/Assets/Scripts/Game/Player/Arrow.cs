using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class Arrow : MonoBehaviour {

    public int speed = 5;
    public GameObject explosionEffect;
    public bool isLocal = false;
    private Rigidbody rgd;
    public RoleType roleType;
    // Use this for initialization
    void Start () {
        rgd = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        rgd.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(isLocal)
            {
                bool local = other.GetComponent<PlayerInfo>().isLocal;
                if(isLocal!= local)
                {
                    GameFacade.Instance.SendAttack(Random.Range(10,20));
                }
            }
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_ShootPerson);
        }
        else
        {
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Miss);
        }
        GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
