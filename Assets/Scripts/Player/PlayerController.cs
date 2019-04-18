using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("How quickly the player fires while the fire button is held.")]
   private float m_FireRate;

   [SerializeField]
   [Tooltip("Whenever increasing the fire rate, it is multiplied by this value.")]
   private float m_FireRateMultiplier; // = 15

   [SerializeField]
   [Tooltip("ugly base game sprite")]
   private Sprite m_UglySprite;
   #endregion

   #region Private Variables
   private bool p_CanMove;
   private bool p_CanAttack;

   // How much time is left before the next shot can be fired
   private float p_TimeToNextShot;

   private Sprite p_DefaultSprite;

   private bool p_IsUsingSuperMachineGun;

   private bool p_On;
   #endregion

   #region Cached Components
   private PlayerMotor cc_Motor;
   private PlayerAttack cc_Attack;
   private SpriteRenderer cc_Renderer;
   #endregion

   #region Initialization Methods
   private void Awake()
   {
      DisableComponents();

      p_TimeToNextShot = 0;
      p_IsUsingSuperMachineGun = false;

      cc_Motor = GetComponent<PlayerMotor>();
      cc_Attack = GetComponent<PlayerAttack>();

      if (m_FireRate == 0)
         Debug.LogWarning("Fire rate is set to 0...are you sure?");

      if (!CompareTag("Player"))
         Debug.LogWarning("Tag of player is NOT player.");

      cc_Renderer = GetComponent<SpriteRenderer>();
      p_DefaultSprite = cc_Renderer.sprite;
      cc_Renderer.sprite = m_UglySprite;
      p_On = false;
   }
   #endregion

   #region OnEnable and OnDisable
   private void OnEnable()
   {
      GameManager.StartGameEvent += EnableComponents;
      GameManager.UpdateArtAndAnimationsEvent += FixSprite;
      GameManager.ActivateSuperMachineGunEvent += ActivatingSuperMachineGun;
   }

   private void OnDisable()
   {
      GameManager.StartGameEvent -= EnableComponents;
      GameManager.UpdateArtAndAnimationsEvent -= FixSprite;
      GameManager.ActivateSuperMachineGunEvent -= ActivatingSuperMachineGun;
   }
   #endregion

   #region Main Updates
   private void Update()
   {
      Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"),
          Input.GetAxisRaw("Vertical"));
      if (p_CanMove)
         cc_Motor.UpdateMove(moveDir);

      if (Input.GetButton("Fire1") && p_TimeToNextShot == 0 && p_CanAttack)
      {
         p_TimeToNextShot = 1 / m_FireRate;
         cc_Attack.Shoot();
         if (p_IsUsingSuperMachineGun)
            cc_Attack.Shoot();
         cc_Motor.SetIsShootingTrue();
      }
      if (p_TimeToNextShot > 0)
         p_TimeToNextShot -= Time.deltaTime;
      else
         p_TimeToNextShot = 0;
   }
   #endregion

   #region Enabling/Disabling Components
   private void EnableComponents()
   {
      p_CanMove = true;
      p_CanAttack = true;
   }

   private void DisableComponents()
   {
      p_CanMove = false;
      p_CanAttack = false;
   }
   #endregion

   #region Collision Methods
   private void OnCollisionEnter2D(Collision2D collision)
   {
      GameObject other = collision.gameObject;
      if (other.CompareTag("Enemy"))
      {
         Destroy(gameObject);
      }
   }
   #endregion

   #region Methods Used When Updating Slides
   public void IncreaseFireRate()
   {
      m_FireRate *= m_FireRateMultiplier;
   }

   private void FixSprite()
   {
      if (p_On)
         return;
      p_On = true;

      cc_Renderer.sprite = p_DefaultSprite;
   }

   private void ActivatingSuperMachineGun()
   {
      p_IsUsingSuperMachineGun = true;
      IncreaseFireRate();
   }
   #endregion
}
