using Game.Puzzle.Light;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Player
{
    public delegate void PlayerDeathEventHandler();
    //BEN_REVIEW : Pouvez-vous éviter de tout appeller "Controller". "Player", c'est pas suffisant ?
    //
    //             Ce commentaire s'applique aussi pour toutes les autres classes "Controller". Les seuls "Controleurs"
    //             que j'accepte, c'est les contrôleur pour le MVC.
    //
    //             Je dois vous avouer que c'est un peu ma faute : j'ai eu la mauvaise idée de créer un "GameController"
    //             et un "EnemyController" dans le Tp1 et je le regrette désormais...
    public class PlayerController : MonoBehaviour
    {
        private Dictionary<string, bool> buttonsPressed; //BEN_CORRECTION : Jamais utilisé. À quoi cela devait-il servir ?
        private Dictionary<string, bool> buttonsReleased; //BEN_CORRECTION : Jamais utilisé. À quoi cela devait-il servir ?
        private Dictionary<string, bool> buttonsHeld; //BEN_CORRECTION : Jamais utilisé. À quoi cela devait-il servir ?

        [SerializeField]
        [Tooltip("Layers William collides with.")]
        private LayerMask williamLayerMask;

        [SerializeField]
        [Tooltip("Layers Reaper collides with.")]
        private LayerMask reaperLayerMask;

        [SerializeField] private float invincibilitySeconds;
        private Health health;
        //BEN_REVIEW : Ici, vous utilisez un "Singleton". Personnellement, je pense que les singletons, c'est le mal incarné.
        //             Éventuellement, tout devient un singleton et, rapidement, tout ce qui est utile dans les langages
        //             orientés objets disparait (héritage, polymorphisme, etc...). Au final, on se rammasse avec que du fonctionnel
        //             (bref, du C).
        //
        //             Malgré cela, laissez ça comme ça. Vous êtes rendu trop creux pour retourner en arrière (et oui, à ce point là).
        //             On va vivre avec...
        public static PlayerController instance;   

        //BEN_REVIEW : Pouvez-vous essayer de regroupez les propriétés ensemble, les attributs ensemble et les
        //             "SerializedFields" ensemble ? Cela nous aiderais à nous retrouver.
        private WilliamController williamController;
        private ReaperController reaperController;
        public EntityController CurrentController { get; private set; }
        private EntityController currentController;

        private LightSensor lightSensor;
        public KinematicRigidbody2D kRigidBody { get; private set; } //BEN_CORRECTION : Standard de nommage.
        private LayerMask layerMask; //BEN_CORRECTION : Inutilisé.


        //BEN_CORRECTION : Pourquoi les "Vies" ne sont-elles pas gérées dans leur propre composant ? Je pense même que cela pourrait
        //                 directement aller dans le composant "Health".
        private int nbPlayerLivesLeft;

        //BEN_CORRECTION : C'est pas le genre d'information qui devrait se retrouver dans le "GameController" ?
        //                 De toute façon, semble inutilisé.
        private int currentLevel;
        private int numbOfLocks = 0;

        public LayerMask WilliamLayerMask
        {
            get { return williamLayerMask; }
            set { williamLayerMask = value; }
        }

        public LayerMask ReaperLayerMask
        {
            get { return reaperLayerMask; }
            set { reaperLayerMask = value; }
        }


        //BEN_REVIEW : IsOnGround, IsGrounded...pouvez-vous utiliser une seule façon de dire la même chose ?
        public bool IsOnGround => kRigidBody.IsGrounded;
        public bool IsDashing { get; set; }
        //BEN_REVIEW : Ce genre d'information pourrait être obtenue autrement en utilisant le "KinematicRigidBody".
        //             En fait, le joueur bouge pas quand sa vélocité est à 0 non ?
        //             Alors pourquoi pas le calculer ainsi :
        //
        //               public bool IsMoving => kRigidBody.Velocity.sqrMagnitude > 0; //Où 0 serait un certain threshold...À remplacer 
        //
        //             Ainsi, pas besoin de s'assurer que la valeur est bonne à chaque frame, car lorsque l'on en a besoin,
        //             il suffit tout simplement de la calculer.
        public bool IsMoving { get; set; }
        private bool isInvincible = false;

        public bool IsInvincible => isInvincible;

        public FacingSideUpDown DirectionFacingUpDown { get; set; }
        public FacingSideLeftRight DirectionFacingLeftRight { get; set; }

        private void Awake()
        {
            currentLevel = 1;
            if (instance == null)
                instance = this;

            kRigidBody = GetComponent<KinematicRigidbody2D>();
            layerMask = kRigidBody.LayerMask;


            health = GetComponent<Health>();
            williamController = GetComponentInChildren<WilliamController>();
            reaperController = GetComponentInChildren<ReaperController>();
            GetComponent<HitSensor>().OnHit += HandleCollision;

            lightSensor = GetComponent<LightSensor>();
            lightSensor.OnLightExpositionChange += OnLightExpositionChanged;
            IsMoving = false;
        }

        private void Start()
        {
            OnLightExpositionChanged(true);
        }

        public void DamagePlayer()
        {
            if (!isInvincible)
            {
                health.Hit();
                StartCoroutine(InvincibleRoutine());
            }
        }

        private IEnumerator InvincibleRoutine()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibilitySeconds);
            isInvincible = false;
        }

        private void HandleCollision(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Enemy ||
                other.DamageSource == HitStimulus.DamageSourceType.Obstacle)
            {
                DamagePlayer();
            }
        }


        private void OnLightExpositionChanged(bool exposed)
        {
            if (exposed)
                OnLightEnter();
            else
            {
                OnLightExit();
            }
        }

        public void OnLightEnter()
        {
            //BEN_REVIEW : Au lieu de ça, si vous extrayez la condition dans une fonction, vous auriez :
            //
            //             if (IsTransformationUnlocked()) { ... }
            //
            //             Ou
            //
            //             if (!IsTransformationLocked()) { ... }
            if (numbOfLocks == 0)
            {
                williamController.sprite.flipX = reaperController.sprite.flipX;
                williamController.gameObject.SetActive(true);
                reaperController.OnAttackFinish();
                CurrentController = williamController;
                reaperController.gameObject.SetActive(false);
                kRigidBody.LayerMask = williamLayerMask;

            }
        }

        public void OnLightExit()
        {
            //BEN_REVIEW : Voir commentaire ci-dessus.
            if (numbOfLocks == 0)
            {
                reaperController.sprite.flipX = williamController.sprite.flipX;
                reaperController.gameObject.SetActive(true);
                williamController.OnAttackFinish();
                CurrentController = reaperController;
                williamController.gameObject.SetActive(false);
                kRigidBody.LayerMask = reaperLayerMask;

            }
        }

        public void LockTransformation()
        {
            numbOfLocks += 1;
        }

        public void UnlockTransformation()
        {
            if (numbOfLocks > 0)
                numbOfLocks -= 1;

            if (numbOfLocks == 0)
                OnLightExpositionChanged(lightSensor.InLight);
        }
    }
}