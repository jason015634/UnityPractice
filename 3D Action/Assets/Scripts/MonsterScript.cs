using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterScript : MonoBehaviour
{
    enum State {spawn, move, attack, dead };

    State monsterState;

    public int monsterHP;
    public int attack;
    public int defense;
    public int attackSpeed;
    public int attackRange;

    public int moveSpeed;

    private Transform target;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();

        monsterHP = 10;
        monsterState = State.spawn;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Player") return;

        target = other.transform;
    }

    // 현재 재생되는 애니메이션 찾기
    bool IsAnimationPlaying(string animName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            return true;
        }
        return false;
    }

    void ChangeState(State newState)
    {
        if (monsterState == newState) return;

        monsterState = newState;
    }

    // Update is called once per frame
    void Update()
    {
        switch (monsterState)
        {
            case State.spawn:
                UpdateSpawn();
                break;
            case State.move:
                UpdateMove();
                break;
            case State.attack:
                break;

        }
    }

    void UpdateSpawn()
    {
        anim.Play("DieRecover");

        ChangeState(State.move);
    }

    void UpdateMove()
    {
        if (target != null)
        {
            anim.Play("WalkForwardBattle");

            Vector3 direction = (target.position - transform.position).normalized;

            direction.y = 0f;

            transform.rotation = Quaternion.LookRotation(direction);

            float distance = (target.position - transform.position).magnitude;

            transform.position += direction * moveSpeed * Time.deltaTime;

            if (distance <= attackRange)
            {
                ChangeState(State.attack);
                anim.SetBool("isAttack", true);
            }
        }
    }

    void UpdateAttack()
    {

    }
}
