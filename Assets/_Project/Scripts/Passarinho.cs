using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passarinho : MonoBehaviour {
    [SerializeField]
    private bool m_isEnemy;
    [SerializeField]
    private AudioSource m_AudioSource; // m_-> member(private)
    public AudioClip m_JumpAudioClip;

	public Rigidbody2D myRigidbody2D;

	public Animator myAnimator;

	public float force;
    public SpriteRenderer maySpriteRendere;
    private int m_EnemyMovementDecision;

    // Use this for initialization
    void Start()
    { 
        if (m_isEnemy)
        {
            StartCoroutine(SetEnemyMovementDecision_Coroutine());
        }
    }
	
	// Update is called once per frame
	void Update () {

		ProcessMovement();

		ProcessJump();
		
	}

	private void ProcessJump()
	{
		if(!m_isEnemy && Input.GetKeyDown(KeyCode.Space) || (m_isEnemy && GetEnemyJumpDecision() ))
		{
			myRigidbody2D.velocity = Vector2.up * force;
			myAnimator.SetTrigger("Jump");
            m_AudioSource.clip = m_JumpAudioClip;
            m_AudioSource.Play();
		}
	}

	private void ProcessMovement()
	{
        float directionX = 0;
        if(m_isEnemy)
        {
            directionX = m_EnemyMovementDecision;
        }
        else
        {
         directionX = Input.GetAxis("Horizontal");
        }

		float directionY = myRigidbody2D.velocity.y;

		myRigidbody2D.velocity = new Vector2(directionX, directionY);

		myAnimator.SetFloat("VelocityX", myRigidbody2D.velocity.x);
       // bool canFlip = directionX >= 0 ? false : true;
 
        ProcessFlip(directionX);
	}
    
   
    private void ProcessFlip(float directionX)
    {
        if ((maySpriteRendere.flipX && directionX > 0) || (!maySpriteRendere.flipX && directionX < 0))
        {
            maySpriteRendere.flipX = !maySpriteRendere.flipX;
        }
       
    }
    private bool GetEnemyJumpDecision()
    {
        float rnd = Random.Range(0, 1000);

        return rnd <= 5;
    }
    private IEnumerator SetEnemyMovementDecision_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); //espera 1 segundo.

            m_EnemyMovementDecision = Random.Range(-1, 2); // nunca alcança o maximo
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "player")
        {
            Destroy(other.gameObject);
        }
    }

}
