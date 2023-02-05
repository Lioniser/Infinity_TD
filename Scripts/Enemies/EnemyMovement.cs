using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool isFrozen = false;

    private PathFinding pathfinder;
    private EnemyDamage damage;
    public float speed;

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
        speed = damage.enemy.speed;
        path = pathfinder.setPath();
        StartCoroutine(enemyMove(path));
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / (speed / 2));
    }
    
    IEnumerator enemyMove(List<Waypoint> path)
    {
        foreach (Waypoint waypoint in path)
        { 
            transform.LookAt(waypoint.transform);
            targetPos = waypoint.transform.position;
            yield return new WaitForSeconds(speed);
        }
        damage.DestroyEnemy(false);
        castle.CastleDamage(GetComponent<Enemy>().pointsCost);
    }

    public void Freeze(float _frozenMultiplayer, float _FrozenTime)
    {
        isFrozen = true;
        StartCoroutine(FrozenMovement(_frozenMultiplayer, _FrozenTime));
    }

    private IEnumerator FrozenMovement(float _frozenMultiplayer, float _FrozenTime)
    {
        float _speed = speed;
        speed = Mathf.Round(speed * _frozenMultiplayer * 10)/10;

        yield return new WaitForSeconds(_FrozenTime);
        
        isFrozen = false;
        speed = _speed;
    }
}
