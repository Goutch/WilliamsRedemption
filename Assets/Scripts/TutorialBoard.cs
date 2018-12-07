using Game.Entity.Player;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class TutorialBoard : MonoBehaviour
    {
        [SerializeField] private Sprite controllerSprite;
        [SerializeField] private Sprite KeyboardSprite;
        private PlayerInputs inputs;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            inputs = GameObject.FindGameObjectWithTag(Game.Values.Tags.Player).GetComponent<PlayerInputs>();
            spriteRenderer = GetComponent<SpriteRenderer>();

        }

        private void Update()
        {
            if (inputs.ControllerState.IsConnected)
            {
                spriteRenderer.sprite = controllerSprite;
            }
            else
            {
                spriteRenderer.sprite = KeyboardSprite;
            }
        }
    }
}