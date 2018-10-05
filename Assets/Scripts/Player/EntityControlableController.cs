using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityControlableController : MonoBehaviour{

    public Animator animator { get; private set; }
    public Collider2D Collider { get; private set; }
    public SpriteRenderer sprite { get; private set; }
    public bool Attacking { get; protected set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();
    }

    public abstract void UseCapacity(PlayerController player ,Vector2 direction);
    public abstract bool CapacityUsable(PlayerController player);

    public abstract bool CanUseBasicAttack(PlayerController player);
    public abstract void UseBasicAttack(PlayerController player ,Vector2 direction);

    public void OnAttackFinish()
    {
        Attacking = false;
    }
}
