using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class MeleeAttackController : MonoBehaviour
{
    [SerializeField] private float delayBeforeDestruction;

    public IEntityData EntityData { get; set; }

    private void Awake()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(delayBeforeDestruction);
        Destroy(this.gameObject);
    }
}

