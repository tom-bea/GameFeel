using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundOn : MonoBehaviour
{
   [SerializeField]
   [Tooltip("shit to turn on")]
   private GameObject[] m_Objects;

   private bool p_On;

   private void Awake()
   {
      p_On = false;
      foreach (var obj in m_Objects)
      {
         obj.SetActive(false);
      }
   }

   private void OnEnable()
   {
      GameManager.UpdateArtAndAnimationsEvent += ShowBackground;
   }

   private void OnDisable()
   {
      GameManager.UpdateArtAndAnimationsEvent -= ShowBackground;
   }

   private void ShowBackground()
   {
      if (p_On)
         return;

      p_On = true;
      foreach (var obj in m_Objects)
      {
         obj.SetActive(true);
      }
   }
}
