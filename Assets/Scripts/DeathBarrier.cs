using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public Animator animator;
    private bool died;
    private bool respawn;
    [SerializeField] Transform player;

    public CharacterController2D Control;

    //fix this************************
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform checkpointSpawn;

    [SerializeField] Rigidbody2D m_Rigidbody2D;

    //private int respawnPoint;
    private bool checkPointHit;

    private void OnTriggerEnter2D(Collider2D collision)
	{
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        died = true;
        animator.SetBool("died", died);
        Invoke("SetBoolBack", .5f);
    }


	private void SetBoolBack()
    {
        //Destroy(GameObject.Find("Player"));
        if (checkPointHit)
        {
            player.transform.position = checkpointSpawn.position;
		}
		else
		{
            player.transform.position = spawnPoint.position;
        }

        died = false;
        animator.SetBool("died", died);

        respawn = true;
        animator.SetBool("respawn", respawn);

        Invoke("SetBoolBack2", .2f);
    }

    private void SetBoolBack2()
	{
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        respawn = false;
        animator.SetBool("respawn", respawn);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
    {
        checkPointHit = Control.getCheckPointHit();
    }
}