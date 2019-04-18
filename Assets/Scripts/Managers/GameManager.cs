using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
   public static GameManager singleton;

   #region Delegates and Events
   public delegate void EmptyDelegate();
   public static event EmptyDelegate StartGameEvent;

   public delegate void IntDelegate(int num);
   
   public delegate void StringDelegate(string str);
   public static event StringDelegate DisplaySlideTitleEvent;


   public static event EmptyDelegate UpdateArtAndAnimationsEvent;
   public static event IntDelegate UpdateEnemyHealthEvent;
   public static event EmptyDelegate RandomEnemyColorsEvent;
   public static event EmptyDelegate AddEnemyEffectsEvent;
   public static event EmptyDelegate BulletTimeEvent;
   public static event EmptyDelegate ScreenShakeEvent;
   public static event EmptyDelegate EnableCameraPlayerTracking;
   public static event EmptyDelegate EnableAdvancedPlayerTracking;
   public static event EmptyDelegate ActivateSuperMachineGunEvent;
   public static event EmptyDelegate ActivateBetterExplosionsEvent;
   #endregion

   #region Editor Variables
   [SerializeField]
   [Tooltip("The slide to start at. For testing purposes.")]
   private int m_SlideToStartAt;
   
   public SceneTransition sceneTransitionHandler;
   #endregion

   #region Private Variables
   private int p_CurrentSlide;

   private string p_CurrentSlideTitle;
   #endregion

   #region Cached References
   private GameObject cr_Player;
   private EnemySpawner cr_Spawner;
   #endregion

   #region Initialization Methods
   private void Awake()
   {
      DontDestroyOnLoad(this);

      if (singleton == null)
         singleton = gameObject.GetComponent<GameManager>();
      else
         Destroy(gameObject);

      p_CurrentSlide = m_SlideToStartAt;

      if (p_CurrentSlide != 0)
         Debug.LogWarning("Not starting at the first slide. Make sure to " +
             "update before building.");

   }
   #endregion

   #region OnEnable and OnDisable
   private void OnEnable()
   {
      SceneManager.sceneLoaded += GoToCurrentSlide;
   }

   private void OnDisable()
   {
      SceneManager.sceneLoaded -= GoToCurrentSlide;
   }
   #endregion

   #region Main Updates
   private void Update()
   {
      if (Input.GetButton("Start"))
      {
         if (StartGameEvent != null){
            StartGameEvent();
            StartCoroutine(sceneTransitionHandler.SlideOut());
         }
      }

      if (Input.GetButtonDown("Restart")){
         SceneManager.LoadScene(0);
      }

      if (Input.GetButtonDown("Next"))
         GoToNextSlide();

      if (Input.GetButtonDown("Previous"))
         GoToPreviousSlide();
   }
   #endregion

   #region Time Modification Methods
   public void ChangeTimeScaleForSeconds(float newScale, float seconds)
   {
      Time.timeScale = newScale;
      Invoke("RestoreTimeScale", seconds);
   }

   private void RestoreTimeScale()
   {
      Time.timeScale = 1;
   }
   #endregion

   #region Moving Between Slides Methods
   private void GoToNextSlide()
   {
      if (p_CurrentSlide == 26)
         p_CurrentSlide = -1;

      p_CurrentSlide++;

      SceneManager.LoadScene(0);
   }

   private void GoToPreviousSlide()
   {
      if (p_CurrentSlide == 0)
         p_CurrentSlide = 27;

      p_CurrentSlide--;

      SceneManager.LoadScene(0);
   }

   private void GoToCurrentSlide(Scene scene, LoadSceneMode mode)
   {
      if (cr_Player == null)
         cr_Player = GameObject.FindGameObjectWithTag("Player");
      if (cr_Spawner == null)
         cr_Spawner = FindObjectOfType<EnemySpawner>();

      if (p_CurrentSlide == 0)
         PerformSlideAction(0);

      for (int slide = 1; slide <= p_CurrentSlide; slide++)
      {
         PerformSlideAction(slide);
      }

      sceneTransitionHandler.NextSceneUI(p_CurrentSlide, p_CurrentSlideTitle);
   }

   private void PerformSlideAction(int slide)
   {
      switch (slide)
      {
         case 0:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Start with an idea";
            Camera.main.cullingMask = 0;
            break;
         case 1:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Actually make a game";
            cr_Spawner.SpawnEnemies(1);
            break;
         case 2:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Add Art, Animations, and Music!";
            if (UpdateArtAndAnimationsEvent != null)
               UpdateArtAndAnimationsEvent();
            break;
         case 3:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Weaker Enemies";
            if (UpdateEnemyHealthEvent != null)
               UpdateEnemyHealthEvent(5);
            break;
         case 4:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Increased Fire Rate";
            cr_Player.GetComponent<PlayerController>().IncreaseFireRate();
            break;
         case 5:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "More Enemies";
            cr_Spawner.SpawnEnemies(5);
            if (UpdateEnemyHealthEvent != null)
               UpdateEnemyHealthEvent(5);
            if (UpdateArtAndAnimationsEvent != null)
               UpdateArtAndAnimationsEvent();
            break;
         case 6:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Strafing!";
            cr_Player.GetComponent<PlayerMotor>().EnableStrafing();
            break;
         case 7:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Different Enemies";
            if (RandomEnemyColorsEvent != null)
               RandomEnemyColorsEvent();
            break;
         case 8:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Enemy Effects";
            if (AddEnemyEffectsEvent != null)
               AddEnemyEffectsEvent();
            break;
         case 9:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Bigger Bullets!";
            cr_Player.GetComponent<PlayerAttack>().UseBigBullets();
            break;
         case 10:
            cr_Spawner.SpawnEnemies(6);
            if (UpdateEnemyHealthEvent != null)
               UpdateEnemyHealthEvent(5);
            if (UpdateArtAndAnimationsEvent != null)
               UpdateArtAndAnimationsEvent();
            if (RandomEnemyColorsEvent != null)
               RandomEnemyColorsEvent();
            if (AddEnemyEffectsEvent != null)
               AddEnemyEffectsEvent();
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Faster Bullets";
            cr_Player.GetComponent<PlayerAttack>().IncreaseBulletSpeed();
            break;
         case 11:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Muzzle Flash";
            cr_Player.GetComponent<PlayerAttack>().EnableMuzzleFlash();
            break;
         case 12:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "More Spread";
            cr_Player.GetComponent<PlayerAttack>().LessAccuracy();
            break;
         case 13:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Impact Effects!";
            cr_Player.GetComponent<PlayerAttack>().EnableImpactFX();
            break;
         case 14:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Bullet Knockback";
            cr_Player.GetComponent<PlayerAttack>().EnableBulletKnockback();
            break;
         case 15:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Player Knockback";
            cr_Player.GetComponent<PlayerMotor>().EnablePlayerKnockback();
            break;
         case 16:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Bullet Time";
            if (BulletTimeEvent != null)
               BulletTimeEvent();
            break;
         case 17:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Add some screenshake";
            if (ScreenShakeEvent != null)
               ScreenShakeEvent();
            break;
         case 18:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Tracer Rounds";
            cr_Player.GetComponent<PlayerAttack>().EnableTracerRounds();
            break;
         case 19:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Permanence";
            cr_Player.GetComponent<PlayerAttack>().EnableLeaveBulletCase();
            break;
         case 20:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Camera movement!";
            if (EnableCameraPlayerTracking != null)
               EnableCameraPlayerTracking();
            break;
         case 21:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "More BASS!";
            cr_Player.GetComponent<PlayerAttack>().EnableBassierSounds();
            break;
         case 22:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Even better camera movement!";
            if (EnableAdvancedPlayerTracking != null)
               EnableAdvancedPlayerTracking();
            break;
         case 23:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Super Machine Gun";
            if (ActivateSuperMachineGunEvent != null)
               ActivateSuperMachineGunEvent();
            break;
         case 24:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "More Challenge";
            cr_Spawner.SpawnEnemies(20);
            if (UpdateEnemyHealthEvent != null)
               UpdateEnemyHealthEvent(6);
            if (UpdateArtAndAnimationsEvent != null)
               UpdateArtAndAnimationsEvent();
            if (RandomEnemyColorsEvent != null)
               RandomEnemyColorsEvent();
            if (AddEnemyEffectsEvent != null)
               AddEnemyEffectsEvent();
            if (BulletTimeEvent != null)
               BulletTimeEvent();
            break;
         case 25:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Can We Lose?";
            cr_Spawner.SpawnEnemies(100);
            if (UpdateEnemyHealthEvent != null)
               UpdateEnemyHealthEvent(7);
            if (UpdateArtAndAnimationsEvent != null)
               UpdateArtAndAnimationsEvent();
            if (RandomEnemyColorsEvent != null)
               RandomEnemyColorsEvent();
            if (AddEnemyEffectsEvent != null)
               AddEnemyEffectsEvent();
            if (BulletTimeEvent != null)
               BulletTimeEvent();
            break;
         case 26:
            if (p_CurrentSlide == slide)
               p_CurrentSlideTitle = "Explosions are Our Friends";
            if (ActivateBetterExplosionsEvent != null)
               ActivateBetterExplosionsEvent();
            break;
      }

   }

   public string GetCurrentSlideTitle()
   {
      return p_CurrentSlideTitle;
   }

   public int GetSlideNumber(){
      
      return p_CurrentSlide;
      
   }
   #endregion
}
