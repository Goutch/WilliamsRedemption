﻿using System;
using Game.Puzzle.Light;
using Math;
using UnityEngine;

namespace Game.Puzzle
{
    public class Lamp : MonoBehaviour, ITriggerable
    {
        [SerializeField] private Sprite openSprite;

        [SerializeField] private Sprite closeSprite;

        [SerializeField] private bool isOpen;

        [SerializeField] private CircleLight linkedLight;

        private SpriteRenderer spriteRenderer;

        private void Update()
        {
            if (linkedLight != null)
                transform.root.rotation = Quaternion.EulerAngles(0, 0, Math2D.DegreeToRadian(linkedLight.FaceAngle+90));
        }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (linkedLight != null)
            {
                transform.root.rotation = Quaternion.EulerAngles(0, 0, linkedLight.FaceAngle);
            }

            if (isOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public void Open()
        {
            isOpen
                = true;
            spriteRenderer.sprite = openSprite;
        }

        public void Close()
        {
            isOpen = false;
            spriteRenderer.sprite = closeSprite;
        }

        public void Unlock()
        {
        }

        public void Lock()
        {
        }

        public bool IsLocked()
        {
            return false;
        }

        public bool IsOpened()
        {
            return isOpen;
        }

        public bool StateIsPermanentlyLocked()
        {
            return false;
        }

        public void PermanentlyLock()
        {
        }
    }
}