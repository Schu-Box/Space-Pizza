using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

public class Laser : MonoBehaviour
{
    public float speed = 30f;
    [HideInInspector] public int damage;

    [FormerlySerializedAs("liveTime")]
    [SerializeField]
    private float lifeTime = 3f;

    private Rigidbody2D _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Fire(ShipLaser shipLaser)
    {
        if(_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        damage = shipLaser.laserDamage;
        
        transform.eulerAngles = shipLaser.transform.eulerAngles;

        float shipVelocity = 0f;
        if (ShipManager.Current.PlayerShip != null)
        {
            shipVelocity = ShipManager.Current.PlayerShip.Velocity.magnitude;
        }
        
        _rigidbody.velocity =  transform.up * (speed + shipVelocity);

        StartCoroutine(RunSelfDestructionSequence());
    }

    private IEnumerator RunSelfDestructionSequence()
    {
        yield return new WaitForSeconds(lifeTime);
        
        DestroyLaser();
    }

    public void DestroyLaser()
    {
        Destroy(gameObject);
    }
    
    // public void OnTriggerEnter2D(Collider2D other)
    // {
    //     Hazard hazard = other.gameObject.GetComponent<Hazard>();
    //     if (hazard != null)
    //     {
    //         Debug.Log("laser hit!");
    //         
    //         hazard.TakeDamage(_shipLaser.laserDamage);
    //     }
    // }
}
