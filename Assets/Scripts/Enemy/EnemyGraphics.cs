using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Animator))]
public class EnemyGraphics : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("Base game sprite")]
   private Sprite m_UglySprite;

   [SerializeField]
   [Tooltip("How long the enemy should be flashing for")]
   private float m_FlashingLength;

   [SerializeField]
   [Tooltip("The color to switch to when flashing")]
   private Color m_FlashColor;
   #endregion

   #region Private Variables
   private Sprite p_DefaultSprite;

   private bool p_On;

   private Color p_DefaultColor;
   #endregion

   #region Cached Components
   private SpriteRenderer cc_Renderer;
    private ParticleSystem cc_Trail;
   private Animator cc_Animator;
    #endregion

    #region Initialization
    private void Awake()
    {
        cc_Renderer = GetComponent<SpriteRenderer>();
        cc_Trail = GetComponent<ParticleSystem>();
      cc_Animator = GetComponent<Animator>();
      cc_Animator.enabled = false;

      p_DefaultSprite = cc_Renderer.sprite;
      p_DefaultColor = cc_Renderer.color;
      cc_Renderer.sprite = m_UglySprite;
      p_On = false;
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
      GameManager.UpdateArtAndAnimationsEvent += PlayAnimation;
        GameManager.RandomEnemyColorsEvent += ChangeColor;
        GameManager.AddEnemyEffectsEvent += AddFX;
    }

    private void OnDisable()
    {
      GameManager.UpdateArtAndAnimationsEvent -= PlayAnimation;
      GameManager.RandomEnemyColorsEvent -= ChangeColor;
        GameManager.AddEnemyEffectsEvent -= AddFX;
    }
   #endregion

   #region Accessors and Mutators
   public Color GetColor()
   {
      return p_DefaultColor;
   }
   #endregion

   #region Sprite Manipulation Methods
   public void Flash()
   {
      StartCoroutine(FlashRoutine());
   }

   private IEnumerator FlashRoutine()
   {
      cc_Renderer.color = m_FlashColor;
      yield return new WaitForSecondsRealtime(m_FlashingLength);
      cc_Renderer.color = p_DefaultColor;
   }
   #endregion

   #region Methods Used When Updating Slides
   private void PlayAnimation()
   {
      if (p_On)
         return;
      p_On = true;

      cc_Renderer.sprite = p_DefaultSprite;
      cc_Animator.enabled = true;
   }

    private void ChangeColor()
    {
      p_DefaultColor = cc_Renderer.color = Random.ColorHSV();
    }

    private void AddFX()
    {
        var main = cc_Trail.main;
        main.startColor = cc_Renderer.color;
        cc_Trail.Play();
    }
    #endregion
}
