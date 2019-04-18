using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrailRenderer))]
public class Bullet : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("The baby explosion to use when bullet hits an enemy.")]
   private ParticleSystem m_ImpactFX;

   [SerializeField]
   [Tooltip("The explosion to use for mega explosion.")]
   private ParticleSystem m_SuperExplosionFX;
   #endregion

   #region Private Variables
   private bool p_PlayImpactFX;

   private bool p_PlayMegaExplosionFX;
   #endregion

   #region Cached Components
   private Rigidbody2D cc_Rb;

   private TrailRenderer cc_Trail;
   #endregion

   #region Initialization Methods
    private void Awake()
   {
      cc_Rb = GetComponent<Rigidbody2D>();
      cc_Trail = GetComponent<TrailRenderer>();

      cc_Trail.enabled = false;
      p_PlayImpactFX = false;
      p_PlayMegaExplosionFX = false;
   }
   #endregion

   #region Velocity Updates
   public void UpdateVelocity(Vector2 newVel)
   {
      cc_Rb.velocity = newVel;
   }
   #endregion

   #region Collision Methods
   public void DontUseTriggerCollision()
   {
      GetComponent<Collider2D>().isTrigger = false;
   }

   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (!collision.collider.CompareTag("Enemy"))
      {
         if (p_PlayImpactFX)
         {
            if (Random.Range(0f, 1f) < 0.25f && p_PlayMegaExplosionFX)
            {
               ParticleSystem megaPS = Instantiate(m_SuperExplosionFX, transform.position, Quaternion.identity);
               Destroy(megaPS.gameObject, 0.3f);
            }
            ParticleSystem ps = Instantiate(m_ImpactFX, transform.position, Quaternion.identity);
            Destroy(ps.gameObject, 0.3f);
         }
         Destroy(gameObject);
         return;
      }

      EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
      enemy.DecreaseHealth(1);
      if (p_PlayImpactFX)
      {
         if (Random.Range(0f, 1f) < 0.25f && p_PlayMegaExplosionFX)
         {
            ParticleSystem megaPS = Instantiate(m_SuperExplosionFX, transform.position, Quaternion.identity);
            Destroy(megaPS.gameObject, 0.3f);
         }
         ParticleSystem ps = Instantiate(m_ImpactFX, transform.position, Quaternion.identity);
         Destroy(ps.gameObject, 0.3f);
      }
      Destroy(gameObject, 0.01f);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (!other.CompareTag("Enemy"))
      {
         if (p_PlayImpactFX)
         {
            if (Random.Range(0f, 1f) < 0.25f && p_PlayMegaExplosionFX)
            {
               ParticleSystem megaPS = Instantiate(m_SuperExplosionFX, transform.position, Quaternion.identity);
               Destroy(megaPS.gameObject, 0.3f);
            }
            ParticleSystem ps = Instantiate(m_ImpactFX, transform.position, Quaternion.identity);
            Destroy(ps.gameObject, 0.3f);
         }
         Destroy(gameObject);
         return;
      }

      EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
      enemy.DecreaseHealth(1);
      if (p_PlayImpactFX)
      {
         if (Random.Range(0f, 1f) < 0.25f && p_PlayMegaExplosionFX)
         {
            ParticleSystem megaPS = Instantiate(m_SuperExplosionFX, transform.position, Quaternion.identity);
            Destroy(megaPS.gameObject, 0.3f);
         }
         ParticleSystem ps = Instantiate(m_ImpactFX, transform.position, Quaternion.identity);
         Destroy(ps.gameObject, 0.3f);
      }
      Destroy(gameObject);
   }
   #endregion

   #region Graphics Methods
   public void EnableTracerRounds()
   {
      cc_Trail.enabled = true;
   }

   public void EnableImpactFX()
   {
      p_PlayImpactFX = true;
   }

   public void EnableMegaExplosionFX()
   {
      p_PlayMegaExplosionFX = true;
   }
   #endregion
}
