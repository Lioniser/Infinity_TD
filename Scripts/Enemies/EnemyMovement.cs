using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool isFrozen = false;

    private PathFinding pathfinder;
    private EnemyDamage damage;
    public float currentSpeed;
    public float basicSpeed;
    private float freezeTime = 0f;

    castle castle;
    Vector3 startPos;
    Vector3 targetPos;
    List<Waypoint> path;
    private void Awake() 
    {
        castle = FindObjectOfType<castle>();
        pathfinder = FindObjectOfType<PathFinding>();
        damage = GetComponent<EnemyDamage>();
    }
    void Start()
    {
        basicSpeed = damage.enemy.speed;
        currentSpeed = basicSpeed;

        path = pathfinder.setPath();
        StartCoroutine(enemyMove(path));
    }
    void Update()
    {
        FreezeMovement();
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / (currentSpeed / 2));
    }
    
    IEnumerator enemyMove(List<Waypoint> path)
    {
        foreach (Waypoint waypoint in path)
        { 
            transform.LookAt(waypoint.transform);
            targetPos = waypoint.transform.position;
            yield return new WaitForSeconds(currentSpeed);
        }
        damage.DestroyEnemy(false);
        castle.CastleDamage(GetComponent<Enemy>().pointsCost);
    }

    public void Freeze(float _frozenMultiplayer, float _freezeTime)
    {
        freezeTime = _freezeTime;
        currentSpeed = Mathf.Round(basicSpeed * (_frozenMultiplayer + 1f) * 10) / 10;
        isFrozen = true;
    }

    private void FreezeMovement()
    {
        if (freezeTime > 0)
        {
            freezeTime -= Time.deltaTime;
        }
        else if (freezeTime < 0)
        {
            isFrozen = false;
            freezeTime = 0f;
            currentSpeed = basicSpeed;
        }
    }
}
