using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerAttack : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("Bullet sound")]
   private AudioClip m_BulletSFX;

   [SerializeField]
   [Tooltip("The prefab for the bullet.")]
   private Bullet m_BulletPrefab;

   [SerializeField]
   [Tooltip("The speed of the bullets.")]
   private float m_BulletSpeed;

   [SerializeField]
   [Tooltip("When increasing bullet speed, this is the multiplier to use.")]
   private float m_BulletSpeedMultiplier;

   [SerializeField]
   [Tooltip("When increasing the size of the bullet, this is the new size.")]
   private Vector3 m_NewBulletSize;

   [SerializeField]
   [Tooltip("How far to the left/right the spread should be. Larger " +
       "numbers will yield a larger spread. A one will yield a spread of " +
       "90 degrees total.")]
   private float m_BulletSpread;

   [SerializeField]
   [Tooltip("How much in front and to the sides the bullet should spawn. " +
       "Setting this to (0, 0) implies the bullet spawns at the center.")]
   private Vector2 m_Offset;

   [SerializeField]
   [Tooltip("The particle system to use for muzzle flash.")]
   private ParticleSystem m_MuzzleFlashPS;

   [SerializeField]
   [Tooltip("The case of the bullet that drops after a shot.")]
   private GameObject m_BulletCase;

   [SerializeField]
   [Tooltip("The amount of random force the bullet receives when exiting the gun. 0 = No randomness.")]
   private float spentAmmoCasingSpread = 1f;

   [SerializeField]
   [Tooltip("Duration screen is shaked when firing gun.")]
   private float bulletScreenShakeDuration = 0.05f;

   [SerializeField]
   [Tooltip("Duration screen is shaked when firing gun.")]
   private float bulletScreenShakeStrength = 0.2f;

   #endregion

   #region Private Variables
   private bool p_PlayBulletSound;

   private bool p_UseBigBullets;

   private bool p_UseLessAccuracy;

   private bool p_UseKnockback;

   private bool p_UseMuzzleFlash;

   private bool p_UseTracersRounds;

   private bool p_ImpactFX;

   private bool p_MegaExplosionFX;

   private bool p_LeaveBulletCase;

   private bool p_UseBassMixer;

   private bool p_UseScreenShake;
   #endregion

   #region Public Variables
   [Tooltip("The mixer to create bassier sounds for bullet SFX.")]
   public AudioMixerGroup cc_BassMixer;
   #endregion

   #region Cached Components
   private Rigidbody2D cc_Rb;
   private AudioSource cc_AudioSource;
   #endregion

   #region Initialization
   private void Awake()
   {
      p_PlayBulletSound = false;
      p_UseBigBullets = false;
      p_UseLessAccuracy = false;
      p_UseKnockback = false;
      p_UseMuzzleFlash = false;
      p_UseTracersRounds = false;
      p_ImpactFX = false;
      p_MegaExplosionFX = false;
      p_LeaveBulletCase = false;
      p_UseBassMixer = false;
      p_UseScreenShake = false;

      cc_Rb = GetComponent<Rigidbody2D>();
      cc_AudioSource = GetComponent<AudioSource>();
   }
   #endregion

   #region OnEnable and OnDisable
   private void OnEnable()
   {
      GameManager.UpdateArtAndAnimationsEvent += PlayBulletShotSFX;
      GameManager.ActivateSuperMachineGunEvent += ActivatingSuperMachineGun;
      GameManager.ActivateBetterExplosionsEvent += ActivateMegaExplosion;
   }

   private void OnDisable()
   {
      GameManager.UpdateArtAndAnimationsEvent -= PlayBulletShotSFX;
      GameManager.ActivateSuperMachineGunEvent -= ActivatingSuperMachineGun;
      GameManager.ActivateBetterExplosionsEvent -= ActivateMegaExplosion;
   }
   #endregion

   #region Shooting Methods
   public void Shoot()
   {
      Bullet b = Instantiate(m_BulletPrefab,
          transform.position +
              transform.up * m_Offset.y + transform.right * m_Offset.x,
          transform.rotation);
      Vector3 dir = transform.up;

      StartCoroutine(CameraShaker.Instance.ShakeCoroutinue(new ShakeParameters(bulletScreenShakeDuration, -dir, 0, bulletScreenShakeStrength)));
      
      if (p_LeaveBulletCase)
      {
         GameObject bulletCase = Instantiate(m_BulletCase, transform.position +
              transform.up * m_Offset.y + transform.right * m_Offset.x, transform.rotation);
         Rigidbody2D caseRb = bulletCase.GetComponent<Rigidbody2D>();
         Vector2 random = transform.right * 5 + new Vector3(Random.Range(-spentAmmoCasingSpread, spentAmmoCasingSpread), Random.Range(-spentAmmoCasingSpread, spentAmmoCasingSpread), 0);

         caseRb.AddForce(random, ForceMode2D.Impulse);
         caseRb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);
      }

      if (p_PlayBulletSound)
         cc_AudioSource.PlayOneShot(m_BulletSFX);
      if (p_UseLessAccuracy)
         dir += transform.right * Random.Range(-m_BulletSpread, m_BulletSpread);
      b.UpdateVelocity(dir * m_BulletSpeed);
      if (p_UseBigBullets)
         b.transform.localScale = m_NewBulletSize;//new Vector3(0.07f, 0.10f, 1);
      if (p_UseKnockback)
         b.DontUseTriggerCollision();
      if (p_UseMuzzleFlash)
         m_MuzzleFlashPS.Play();
      if (p_UseTracersRounds){
         //Randomly enable tracers for some bullets. 
         if (Random.Range(0, 1f) > .75f)
            b.EnableTracerRounds();
      }
      if (p_ImpactFX)
         b.EnableImpactFX();
      if (p_MegaExplosionFX)
         b.EnableMegaExplosionFX();
      if (p_UseBassMixer)
         cc_AudioSource.outputAudioMixerGroup = cc_BassMixer;
      Destroy(b.gameObject, 3);
   }
   #endregion

   #region Methods Used When Updating Slides
   public void PlayBulletShotSFX()
   {
      p_PlayBulletSound = true;
   }

   public void UseBigBullets()
   {
      p_UseBigBullets = true;
   }

   public void IncreaseBulletSpeed()
   {
      m_BulletSpeed *= m_BulletSpeedMultiplier;
   }

   public void LessAccuracy()
   {
      p_UseLessAccuracy = true;
   }

   public void EnableBulletKnockback()
   {
      p_UseKnockback = true;
   }

   public void EnableMuzzleFlash()
   {
      p_UseMuzzleFlash = true;
   }

   public void EnableTracerRounds()
   {
      p_UseTracersRounds = true;
   }

   public void EnableImpactFX()
   {
      p_ImpactFX = true;
   }

   public void EnableLeaveBulletCase()
   {
      p_LeaveBulletCase = true;
   }

   public void EnableBassierSounds()
   {
      p_UseBassMixer = true;
   }

   private void ActivatingSuperMachineGun()
   {
      IncreaseBulletSpeed();
   }

   private void ActivateMegaExplosion()
   {
      p_MegaExplosionFX = true;
   }
   #endregion
}
