using System;
using Prefabs.Framework;
using Prefabs.Framework.AbilitySystem;
using Prefabs.Framework.AI;
using Prefabs.Framework.Damage;
using Prefabs.Framework.Health;
using Prefabs.Framework.ShopSystem;
using Prefabs.Ui.Health;
using Prefabs.Ui.InGameUI;
using Prefabs.Weapons;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prefabs.Player
{
    public class Player : MonoBehaviour, ITeamInterface
    {
        [SerializeField] JoyStick moveStick;
        [SerializeField] JoyStick aimStick;
        [SerializeField] CharacterController characterController;
        [SerializeField] float moveSpeed = 20f;
        [SerializeField] float maxMoveSpeed = 80f;
        [SerializeField] private float minMoveSpeed = 5f;
        //[SerializeField] float turnSpeed = 30f;
        [SerializeField] float animTurnSpeed = 30f;
        [SerializeField] private MovementComponent movementComponent;
        [SerializeField] private int teamID = 1;

        [Header("Inventory")]
        [SerializeField] Inventory inventoryComponent;

        [Header("Health & Damage")]
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private PlayerValueGauge healthBar;

        [Header("Ability & Stamina")]
        [SerializeField] private AbilityComponent abilityComponent;
        [SerializeField] private PlayerValueGauge staminaBar;

        [Header("UI")]
        [SerializeField] private UIManager uiManager;

        [SerializeField] private ShopItem testShopItem;
        [SerializeField] private ShopSystem testShopSystem;

        public void TestPurchase()
        {
            testShopSystem.TryPurchase(testShopItem, GetComponent<CreditComponent>());
        }
        
    
        Camera mainCam;
        private CameraController cameraController;
        [SerializeField] Animator animator;

        private float animatorTurnSpeed;

        private Vector2 moveInput;
        private Vector2 aimInput;

        private void OnDisable()
        {
            moveStick.oStickValueUpdated -= MoveStickUpdated;
            aimStick.oStickValueUpdated -= AimStickUpdated;
            aimStick.onStickTaped -= StartSwitchWeapon;

            healthComponent.onHealthChange -= HealthChanged;
            healthComponent.onDeath -= StartDeathSequence;

            abilityComponent.onStaminaChange -= StaminaChanged;
        }

        private void Start()
        {
            moveStick.oStickValueUpdated += MoveStickUpdated;
            aimStick.oStickValueUpdated += AimStickUpdated;
            aimStick.onStickTaped += StartSwitchWeapon;
        
            mainCam = Camera.main;
            cameraController = FindObjectOfType<CameraController>();
            animator = GetComponent<Animator>();
            healthComponent.onHealthChange += HealthChanged;
            healthComponent.onDeath += StartDeathSequence;
            healthComponent.BroadcastHealthValueImmediately();

            abilityComponent.onStaminaChange += StaminaChanged;
            abilityComponent.BroadcastStaminaChangeImmediately();
            
            GamePlayStatics.GameStarted();

            //Invoke(nameof(TestPurchase), 3f);
        }
        
        private void StartDeathSequence(GameObject killer)
        {
            animator.SetLayerWeight(2, 1);
            animator.SetTrigger("Death");
            
            uiManager.SetGameplayControlEnabled(false);
        }

        private void HealthChanged(float health, float delta, float maxHealth)
        {
            
            healthBar.UpdateValue(health, delta, maxHealth);
        }
        
        private void StaminaChanged(float newAmount, float maxAmount)
        {
            staminaBar.UpdateValue(newAmount, 0, maxAmount);
        }

        private void Update()
        {
            PreformMoveAndAim();
            UpdateCamera();
        }

        private void MoveStickUpdated(Vector2 inputValue)
        {
            moveInput = inputValue;
        }
    
        private void AimStickUpdated(Vector2 inputValue)
        {
            aimInput = inputValue;
            if (inventoryComponent.hasWeapon())
            {
                if (aimInput.magnitude > 0)
                {
                    animator.SetBool("Attacking", true);
                }
                else
                {
                    animator.SetBool("Attacking", false);
                }
            }
        }
    
        private void StartSwitchWeapon()
        {
            if (inventoryComponent.hasWeapon())
            {
                animator.SetTrigger("SwitchWeapon");
            }
        }

        #region AnimEventFuncs
        public void SwitchWeapon() 
        {
            inventoryComponent.NextWeapon();
        }

        public void AttackPoint()
        {
            if (inventoryComponent.hasWeapon())
            {
                inventoryComponent.GetActiveWeapon().Attack();
            }
            
        }
        #endregion
    
        Vector3 StickInputToWorldDir(Vector2 inputVal)
        {
            //we need to find the vector of the dir from the camera:
            Vector3 rightDir = mainCam.transform.right;
            Vector3 upDir = Vector3.Cross(rightDir, Vector3.up);
            return rightDir * inputVal.x + upDir * inputVal.y;
        }
    
        private void PreformMoveAndAim()
        {
            Vector3 moveDir = StickInputToWorldDir(moveInput);
            //characterController.Move(new Vector3(moveInput.x, 0 , moveInput.y) * (moveSpeed * Time.deltaTime));
            characterController.Move(moveDir * (moveSpeed * Time.deltaTime)); //func of CharacterController
        
            //Debug.Log(moveDir);
            UpdateAim(moveDir);
        
            //calc the bled tree val:
            float forward = Vector3.Dot(moveDir, transform.forward);
            float right = Vector3.Dot(moveDir, transform.right);
        
            animator.SetFloat("ForwardSpeed", forward);
            animator.SetFloat("RightSpeed", right);
        
            characterController.Move(Vector3.down * (10 * Time.deltaTime)); //so we wont push up by enemies that touch us
        }

        private void UpdateAim(Vector3 moveDir)
        {
            Vector3 aimDir = moveDir;
            if (aimInput.magnitude != 0)
            {
                aimDir = StickInputToWorldDir(aimInput);
            }
            RotateTowards(aimDir);
        }

        private void RotateTowards(Vector3 aimDir)
        {
            float currentTurnSpeed = 0; //so we will be in idle if the is no movement

            #region BeforeRefactoring
            // if (aimDir.magnitude != 0)
            // {
            //     Quaternion prevRot = transform.rotation;
            //     
            //     float turnLerpAlpha = turnSpeed * Time.deltaTime;
            //     //transform.LookAt(rightDir * moveInput.x + upDir * moveInput.y); //another way to rotate the obj to the walking dir
            //     //lerp for smoother rotation when we start to move
            //     transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimDir, Vector3.up), turnLerpAlpha); //up makes us look up with the head
            //     
            //     Quaternion currentRot = transform.rotation;
            //     float dir = Vector3.Dot(aimDir, transform.right) > 0 ? 1 : -1;
            //     float rotationDelta = Quaternion.Angle(prevRot, currentRot) * dir; 
            //     currentTurnSpeed = rotationDelta / Time.deltaTime; //normalized it with the time
            // }
            #endregion

            //after:
            movementComponent.RotateTowards(aimDir);
        
            animatorTurnSpeed = Mathf.Lerp(animatorTurnSpeed, currentTurnSpeed, Time.deltaTime * animTurnSpeed);
            animator.SetFloat("TurningSpeed", animatorTurnSpeed);

        }
        private void UpdateCamera()
        {
            if (moveInput.magnitude != 0 && aimInput.magnitude == 0 && cameraController != null) //if we are not moving, magnitude returns the length of the vector
            {
                cameraController.AddYawInput(moveInput.x);
            }
        }

        public int GetTeamID()
        {
            return teamID;
        }

        public void AddMoveSpeed(float boostAmount)
        {
            moveSpeed += boostAmount;
            moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, maxMoveSpeed);
        }

        public void DeathFinished()
        {
            uiManager.SwitchToDeathMenu();
        }
        
        
    }
}
