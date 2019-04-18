using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyHealth : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("The amount of health this enemy starts with.")]
   private int m_StartingHealth;

   [SerializeField]
   [Tooltip("The percent of time being slown down when enemy gets hit. A 0.5" +
       "implies that time will be half speed.")]
   private float m_BulletTimeFactor;

   [SerializeField]
   [Tooltip("The duration of the bullet time in seconds.")]
   private float m_BulletTimeDuration;

   [SerializeField]
   [Tooltip("The special effects to use when an enemy dies.")]
   private ParticleSystem m_EnemyDeathFX;
   #endregion

   #region Private Variables
   private int p_CurrentHealth;

   private bool p_EnableBulletTime;

   private bool p_UseEnemyDeathFX;

   private bool p_FlashOnHit;
   #endregion

   #region Cached Components
   private EnemyGraphics cc_Graphics;
   #endregion

   #region Initialization
   private void Awake()
   {
      p_CurrentHealth = m_StartingHealth;
      p_EnableBulletTime = false;
      p_UseEnemyDeathFX = false;
      p_FlashOnHit = false;

      cc_Graphics = GetComponent<EnemyGraphics>();

      if (p_CurrentHealth <= 0)
         Debug.LogWarning("Current health was initialized to <= 0. Did " +
             "you mean to do that?");

      if (!CompareTag("Enemy"))
         Debug.LogWarning("Enemy does not have tag set to Enemy");
   }
   #endregion

   #region OnEnable and OnDisable
   private void OnEnable()
   {
      GameManager.UpdateEnemyHealthEvent += SetHealth;
      GameManager.BulletTimeEvent += SetBulletTimeTrue;
      GameManager.AddEnemyEffectsEvent += AddEnemyFX;
   }

   private void OnDisable()
   {
      GameManager.UpdateEnemyHealthEvent -= SetHealth;
      GameManager.BulletTimeEvent -= SetBulletTimeTrue;
      GameManager.AddEnemyEffectsEvent -= AddEnemyFX;
   }
   #endregion

   #region Health Methods
   public void DecreaseHealth(int amount)
   {
      p_CurrentHealth -= amount;
      if (p_FlashOnHit)
         cc_Graphics.Flash();
      if (p_CurrentHealth <= 0)
         Die();
   }
   #endregion

   #region Death Methods
   private void Die()
   {
      if (p_EnableBulletTime)
         ActivateBulletTime();
      if (Random.Range(0f, 1f) < .33f && p_UseEnemyDeathFX)
      {
         ParticleSystem ps = Instantiate(m_EnemyDeathFX, transform.position, Quaternion.identity);
         var main = ps.main;
         main.startColor = cc_Graphics.GetColor();
         Destroy(ps, 5);
      }

      Destroy(gameObject);
   }
   #endregion

   #region Bullet Time Methods
   private void ActivateBulletTime()
   {
      GameManager.singleton.ChangeTimeScaleForSeconds(m_BulletTimeFactor, m_BulletTimeDuration);
   }
   #endregion

   #region Methods Used When Updating Slides
   private void SetHealth(int newHealth)
   {
      p_CurrentHealth = newHealth;
   }

   private void SetBulletTimeTrue()
   {
      p_EnableBulletTime = true;
   }

   private void AddEnemyFX()
   {
      p_UseEnemyDeathFX = true;
      p_FlashOnHit = true;
   }
   #endregion
}
