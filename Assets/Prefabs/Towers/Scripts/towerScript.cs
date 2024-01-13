using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TowerStats
{
    public float health;
    public float maxHealth;
    public float damage;
    public float radius;
    public float value;
    public float fireRate;
    public int numbProjectiles;
    public int numbTargets;
}
[System.Serializable]
public class TowerUpgrade
{
    public TowerStats upgrade;
    public float price;
}
[System.Serializable]
public class UpgradeBranch
{
    public TowerUpgrade[] leaves;
}
public class towerScript : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioClip fireSound;
    public SpriteRenderer sprite;
    public String TowerDesc;
    public String TowerTitle;
    public TowerStats towerStats;
    public UpgradeBranch[] upgradeTree;

    //{ { new TowerUpgrade { }, new TowerUpgrade { }, new TowerUpgrade { }, new TowerUpgrade { }, new TowerUpgrade { } }, { new TowerUpgrade { }, new TowerUpgrade { }, new TowerUpgrade { }, new TowerUpgrade { }, new TowerUpgrade { } } }
    private float timeToNextFire;
    public GameObject projectile;
    private Collider[] previous = new Collider[0];
    public bool healing = false;
    public towerContainerScript parentContainer;
    // Start is called before the first frame update
    void Start()
    {
        timeToNextFire = Time.time + towerStats.fireRate;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeToNextFire < Time.time)
        {
            attack();
        }
    }

    void attack()
    {
        timeToNextFire = Time.time + 0.01f;
        Collider[] hitColliders;
        if (!healing)
        {
            hitColliders = Physics.OverlapSphere(this.transform.position, towerStats.radius, LayerMask.GetMask("Enemy"));

        }
        else
        {
            hitColliders = Physics.OverlapSphere(this.transform.position, towerStats.radius, LayerMask.GetMask("Player"));
        }
        if (hitColliders.Length > 0)
        {
            int prevIdx = 0;
            Collider[] newPrev = new Collider[this.towerStats.numbTargets];
            maxHeap<Collider> targetHeap;
            if (healing)
            {
                targetHeap = new maxHeap<Collider>(hitColliders, comparePlayer);
            }
            else
            {
                targetHeap = new maxHeap<Collider>(hitColliders, compareTargets);
            }
            for (int i = 0; i < this.towerStats.numbTargets; i++)
            {
                if (targetHeap.isEmpty())
                {
                    break;
                }
                newPrev[prevIdx] = targetHeap.pop();
                StartCoroutine(FireProj(newPrev[prevIdx], towerStats.damage));
                prevIdx++;
            }
            this.previous = newPrev;
            timeToNextFire += towerStats.fireRate;
        }
    }
    IEnumerator FireProj(Collider target, float damage)
    {
        for (int i = 0; i < towerStats.numbProjectiles; i++)
        {
            yield return new WaitForSeconds(.1f);
            soundSource.PlayOneShot(fireSound, UnityEngine.Random.Range(0.08f, 0.12f));
            GameObject proj = Instantiate(projectile, this.transform.position, this.transform.rotation);
            proj.GetComponent<Projectile>().target = target;
            proj.GetComponent<Projectile>().damage = damage;
        }
    }
    public TowerStats GetTowerStats()
    {
        return this.towerStats;
    }
    public UpgradeBranch[] GetUpgradeTree()
    {
        return this.upgradeTree;
    }
    public void setTowerStats(TowerStats stats)
    {
        this.towerStats = stats;
    }
    public void TakeDamage(float damage)
    {
        parentContainer.takeDamage(damage);
    }
    private int compareTargets(Collider C1, Collider C2)
    {
        for (int i = 0; i < previous.Length; i++)
        {
            if (C1 == previous[i])
            {
                return 1;
            }
        }
        Enemy C1Stats = C1.GetComponent<Enemy>();
        Enemy C2Stats = C2.GetComponent<Enemy>();
        if (C1Stats.maxHealth > C2Stats.maxHealth)
        {
            return 1;
        }
        else if (C1Stats.maxHealth == C2Stats.maxHealth && C1Stats.health < C2Stats.health)
        {
            return 1;
        }
        return -1;
    }
    private int comparePlayer(Collider C1, Collider C2)
    {
        return 1;
    }
}
