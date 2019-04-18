using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMotor : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How quickly the enemy should be moving.")]
    private float m_Speed;
    #endregion

    #region Private Variables
    private bool p_CanMove;
    #endregion

    #region Cached Components
    private Rigidbody2D cc_Rb;
    #endregion

    #region Cached References
    private Transform cr_Player;
    #endregion

    #region Initialization Methods
    private void Awake()
    {
        p_CanMove = false;

        cc_Rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        cr_Player = GameObject.FindGameObjectWithTag("Player").transform;
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
        if (p_CanMove)
            FollowPlayer();
    }
    #endregion

    #region Follow Methods
    private void FollowPlayer()
    {
        if (cr_Player == null)
        {
            cc_Rb.angularVelocity = 0;
            cc_Rb.velocity = Vector2.zero;
            return;
        }

        transform.up = cr_Player.position - transform.position;
        
        Vector2 forward = transform.up;
        cc_Rb.MovePosition(
            cc_Rb.position + forward * m_Speed * Time.fixedDeltaTime);
    }
    #endregion

    #region Enabling/Disabling Movement
    public void EnableMovement()
    {
        p_CanMove = true;
    }
    #endregion
}
