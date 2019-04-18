using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{

   public Text sceneText;
   public Text slideNumText;
   public GameObject topPanel;
   public GameObject bottomPanel;

   Vector3 bottomEndPos;
   Vector3 topEndPos;

   Vector3 bottomStartPos;
   Vector3 topStartPos;

   public AudioSource nextSlideSFX;

   public float animationTime = 0.7f;
   public float exitAnimationTime = 0.2f;

   private int p_CurTipNum;

   private void Awake()
   {
      p_CurTipNum = 0;

      topEndPos = topPanel.transform.localPosition;
      bottomEndPos = bottomPanel.transform.localPosition;

      topStartPos = topEndPos + Vector3.up * 160;
      bottomStartPos = bottomEndPos + Vector3.up * -170;

      topPanel.transform.localPosition = topStartPos;
      bottomPanel.transform.localPosition = bottomStartPos;
   }
   
   public void NextSceneUI(int sceneNumber, string title)
   {
      p_CurTipNum = sceneNumber;
      sceneText.text = title;
      slideNumText.text = "Tip " + p_CurTipNum.ToString() + ":";
      StartCoroutine(StartScene());
   }

   private IEnumerator StartScene()
   {

      if (!nextSlideSFX.isPlaying)
      {
         nextSlideSFX.Play();
      }
      
      float progress = 0;
      while (progress < animationTime)
      {
         topPanel.transform.localPosition = Vector3.Lerp(topStartPos, topEndPos, progress / animationTime);
         bottomPanel.transform.localPosition = Vector3.Lerp(bottomStartPos, bottomEndPos, progress / animationTime);
         progress += Time.deltaTime;
         yield return null;
      }
   }

   public IEnumerator SlideOut()
   {
      
      float progress = 0;
      while (progress < exitAnimationTime)
      {
         topPanel.transform.localPosition = Vector3.Lerp(topEndPos, topStartPos, progress / exitAnimationTime);
         bottomPanel.transform.localPosition = Vector3.Lerp(bottomEndPos, bottomStartPos, progress / exitAnimationTime);
         progress += Time.deltaTime;
         yield return null;
      }
   }
}
