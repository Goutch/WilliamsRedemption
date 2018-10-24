using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, ITriggerable
{
    [SerializeField] private bool IsLocked;
    [SerializeField] private bool IsOpen;

    void Awake()
    {
        if (IsOpen)
        {
            Open();
        }
    }
    
    public void Open()
    {
        if (!IsLocked)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled =false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            IsOpen = true;
        }
    }

    public void Close()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled =true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled =true;
        IsOpen = false;
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

    public bool IsOpened()
    {
        return IsOpen;
    }
    


    
}