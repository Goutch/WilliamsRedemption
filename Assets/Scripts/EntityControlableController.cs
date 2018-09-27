using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityControlableController : MonoBehaviour{

    public Animator animator { get; private set; }
    public SpriteRenderer sprite { get; private set; }
    public bool Attacking { get; protected set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public abstract void UseCapacity1(PlayerController player);
    public abstract bool Capacity1Usable(IPlayerDataReadOnly playerData);

    public abstract bool CanUseBasicAttack(IPlayerDataReadOnly playerData);
    public abstract void UseBasicAttack(IPlayerData playerData);

    public void OnAttackFinish()
    {
        Attacking = false;
    }
}
