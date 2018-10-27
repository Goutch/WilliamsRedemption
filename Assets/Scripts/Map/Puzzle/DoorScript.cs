using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, ITriggerable
{
    [Tooltip("Locks the door.")]
    [SerializeField] private bool isLocked;
    [Tooltip("Check this box if you want the door to start opened.")]
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
        this.gameObject.GetComponent<SpriteRenderer>().enabled =false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        IsOpen = true;   
    }

    public void Close()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled =true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled =true;
        IsOpen = false;
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }
    
    public bool IsOpened()
    {
        return IsOpen;
    }

    public bool IsLocked()
    {
        return isLocked;
    }
}