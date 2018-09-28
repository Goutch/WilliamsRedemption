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

    public void AttackWithPlasma()
    {
        Instantiate(plasmaGround, new Vector2(transform.position.x, transform.position.y ), Quaternion.identity);
    }

}
