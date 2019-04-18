using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("How fast the player moves around.")]
   private float m_Speed;

   [SerializeField]
   [Tooltip("How much to push the player backward when shooting.")]
   private float m_PlayerKnockbackForce;
   #endregion

   #region Private Variables
   private Vector2 p_MoveDirection;

   private bool p_KnockbackPlayer;

   private bool p_IsShooting;

   private bool p_IsStrafing;

   private bool p_CanMove;
   #endregion

   #region Cached Components
   private Rigidbody2D cc_Rb;
   #endregion

   #region Cached References
   private Camera cr_Camera;
   #endregion

   #region Initialization Methods
   private void Awake()
   {
      p_MoveDirection = Vector2.zero;
      p_KnockbackPlayer = false;
      p_IsShooting = false;
      p_IsStrafing = false;
      p_CanMove = false;

      cc_Rb = GetComponent<Rigidbody2D>();
   }

   private void Start()
   {
      cr_Camera = Camera.main;
   }
   #endregion

   #region OnEnable and OnDisable
   private void OnEnable()
   {
      GameManager.StartGameEvent += EnableMovement;
   }

   private void OnDisable()
   {
      GameManager.StartGameEvent -= EnableMovement;
   }
   #endregion

   #region Main Updates
   private void FixedUpdate()
   {
      Rotate();
      Move();
   }
   #endregion

   #region Movement methods
   public void UpdateMove(Vector2 dir)
   {
      p_MoveDirection = dir.normalized;
   }

   private void Rotate()
   {
      if (!p_CanMove)
         return;

      if (!p_IsStrafing)
      {
         float newRot;

         if (p_MoveDirection.y != 0)
            newRot = Mathf.Rad2Deg *
                Mathf.Atan2(-p_MoveDirection.x, p_MoveDirection.y);
         else
            newRot = -90 * p_MoveDirection.x;

         if (p_MoveDirection.magnitude != 0)
            cc_Rb.MoveRotation(newRot);
      }
      else
      {
         // Distance from camera to object.  We need this to get the proper calculation.
         float camDis = cr_Camera.transform.position.y - transform.position.y;

         // Get the mouse position in world space. Using camDis for the Z axis.
         Vector3 mouse = cr_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));

         float AngleRad = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x);
         float angle = (180 / Mathf.PI) * AngleRad;

         cc_Rb.rotation = angle - 90;
      }
   }

   private void Move()
   {
      if (!p_IsStrafing)
      {
         Vector2 forward = transform.up;
         if (p_MoveDirection.magnitude == 0)
            forward = Vector2.zero;
         cc_Rb.MovePosition(
             cc_Rb.position + forward * Time.fixedDeltaTime * m_Speed
             - (Vector2)transform.up * ((p_KnockbackPlayer && p_IsShooting) ? m_PlayerKnockbackForce : 0) * Time.fixedDeltaTime);
         if (p_KnockbackPlayer && p_IsShooting)
            p_IsShooting = false;
      }
      else
      {
         cc_Rb.MovePosition(
            cc_Rb.position + p_MoveDirection * Time.fixedDeltaTime * m_Speed
            - (Vector2)transform.up * ((p_KnockbackPlayer && p_IsShooting) ? m_PlayerKnockbackForce : 0) * Time.fixedDeltaTime);
         if (p_KnockbackPlayer && p_IsShooting)
            p_IsShooting = false;
      }
   }

   private void EnableMovement()
   {
      p_CanMove = true;
   }
   #endregion

   #region General Update Methods
   public void SetIsShootingTrue()
   {
      p_IsShooting = true;
   }
   #endregion

   #region Methods Used When Updating Slides
   public void EnablePlayerKnockback()
   {
      p_KnockbackPlayer = true;
   }

   public void EnableStrafing()
   {
      p_IsStrafing = true;
   }
   #endregion
}