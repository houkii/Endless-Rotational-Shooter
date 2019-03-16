using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayBullet : MonoBehaviour
{
    public float range = 20f;
    public int damage = 1;
    public float lifeTime = 0.15f;

    private Ray ray;
    private RaycastHit hit;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private int shootableMask;
    
    void Awake()
    {
        // gun muzzle makes sure orientation is correct...
        ray.origin = transform.position;
        ray.direction = transform.forward;
        line.SetPosition(0, transform.position);

        if(Physics.Raycast(ray, out hit, range))
        {
            if(hit.collider.gameObject.tag == "Enemy")
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                enemy.OnEnemyHit?.Invoke(this.damage);
            }

            // set line end at enemy pos
            line.SetPosition(1, hit.point);
        }
        else
        {
            // set line end at max range
            line.SetPosition(1, ray.origin + ray.direction * range);
        }
        Destroy(this.gameObject, lifeTime);
    }
}
