using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    public float speed = 5.0f;
    float forwardInput;
    private GameObject focalPoint;
    public bool hasPowerup = false;
    private float powerupStr = 15.0f;
    public GameObject powerupIndicator;
    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        //Hiyerarþide Focal Point dediðimiz GameObject'i buluyor.
        focalPoint = GameObject.Find("Focal Point");
    }
    private void Update()
    {
        PlayerMove();
        powerupIndicator.gameObject.transform.position = gameObject.transform.position + new Vector3(0, -0.5f, 0);
    }
    public void PlayerMove()
    {
        forwardInput = Input.GetAxis("Vertical");
        playerRB.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            enemyRB.AddForce(awayFromPlayer * powerupStr, ForceMode.Impulse);
        }
    }
}
