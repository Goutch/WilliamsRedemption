using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorScript : MonoBehaviour, ITriggerable
{
    [SerializeField] private bool IsLocked;
    
    public void Open()
    {
        if (!IsLocked)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled =false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void Close()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled =true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled =true;
    }

    public void LockDoor()
    {
        IsLocked = true;
    }

    public void UnlockDoor()
    {
        IsLocked = false;
    }
    
    public bool CanBeOpened()
    {
        return !IsLocked;
    }
    


    
}