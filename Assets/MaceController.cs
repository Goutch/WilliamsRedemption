using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceController : MonoBehaviour {

    [SerializeField] GameObject plasmaGround;
    private float plasmaGroundHeigth;
    private Transform maceEnd;

    private void Awake()
    {
        maceEnd = transform.GetChild(0);
        plasmaGroundHeigth = plasmaGround.GetComponent<SpriteRenderer>().size.y;
    }

    public void AttackWithPlasma(float delayBeforeDestructionOfPlatforms)
    {
        GameObject plasmaGroundObject = Instantiate(plasmaGround, maceEnd.position, Quaternion.identity);
        plasmaGroundObject.GetComponent<PlasmaGroundController>().Init(delayBeforeDestructionOfPlatforms);
        plasmaGroundObject.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Ennemy);
    }
}
