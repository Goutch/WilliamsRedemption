using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
using Game.Puzzle;
using Harmony;
using UnityEngine;

namespace Game.Puzzle
{
    public class MovingPlatformPickupZone : MonoBehaviour
    {
        [SerializeField] private PlatformMover[] MovingPlatforms;
        [SerializeField] private bool MoveUp;
        [SerializeField] private bool MoveRight;

        private BoxCollider2D zone;

        private void Awake()
        {
            zone = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player))
            {
                foreach (var platform in MovingPlatforms)
                {
                    platform.SetVerticalDirection(MoveUp);
                    platform.SetHorizontalDirection(MoveRight);
                }
            }
        }
    }
}