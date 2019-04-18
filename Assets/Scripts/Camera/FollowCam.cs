using System.Collections;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public float lerpSpeed = 7.5f;
    public float maxSpeed = 5f;
    public Vector2 desiredPos;

    private GameObject player;
    private bool p_CameraFollowPlayer = false;
    private bool p_AdvancedFollowPlayer = false;
    private Camera mainCam;

    [Tooltip("How far the camera can look away from the player centre. Warning: A higher number increases disorientation.")]
    public float lookaheadRadius = 3;

    [Tooltip("Object (wall/cliff/edge) to set the bounds of the camera tracker.")]
    public GameObject leftBounds;
    [Tooltip("Object (wall/cliff/edge) to set the bounds of the camera tracker.")]
    public GameObject rightBounds;
    [Tooltip("Object (wall/cliff/edge) to set the bounds of the camera tracker.")]
    public GameObject upperBounds;
    [Tooltip("Object (wall/cliff/edge) to set the bounds of the camera tracker.")]
    public GameObject lowerBounds;

    //Position of centre of starting screen to set the bounds of the camera tracker
    private Vector3 centre;

    [Tooltip("Used to set the bounds of the camera tracker. Assumes horizontally symmetrical map.")]
    public float horizontalOffset;
    [Tooltip("Used to set the bounds of the camera tracker. Assumes vertically symmetrical map.")]
    public float verticalOffset;

 
    void OnEnable()
    {
        GameManager.EnableCameraPlayerTracking += EnableCameraTracking;
        GameManager.EnableAdvancedPlayerTracking += EnableAdvancedTracking;
    }


    void OnDisable()
    {
        GameManager.EnableCameraPlayerTracking -= EnableCameraTracking;
        GameManager.EnableAdvancedPlayerTracking -= EnableAdvancedTracking;
    }
    
    void Start()
    {
        centre = this.transform.position;
        player = GameObject.FindGameObjectWithTag("Player"); 
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
      if (player == null)
         return;

      if (p_AdvancedFollowPlayer){
          Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, player.transform.position.z);
          Vector3 cursorPos = mainCam.ScreenToWorldPoint(pos);
          Vector3 cursorVector = cursorPos - player.transform.position;
          float length = cursorVector.magnitude;
          Vector3 normalized = cursorVector.normalized;
          Vector3 midPoint = player.transform.position + 0.5f*Mathf.Clamp(length, 0, lookaheadRadius)*normalized;
          FollowPosition(midPoint);
        } 
        else if (p_CameraFollowPlayer)
          FollowPosition(player.transform.position);

        float width = rightBounds.transform.position.x - leftBounds.transform.position.x;
        float height = upperBounds.transform.position.y - lowerBounds.transform.position.y;
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(transform.position.x,  centre.x - width/2 + horizontalOffset, centre.x + width/2 - horizontalOffset);
        clampedPos.y = Mathf.Clamp(transform.position.y, centre.y - height/2 + verticalOffset, centre.y + height/2 - verticalOffset);

        // print(clampedPos.x + " " + clampedPos.y);

        transform.SetPositionAndRotation(clampedPos, Quaternion.identity);
    }

    void FollowPosition(Vector3 desiredPos){
      if (player != null)
      {
        // Vector2 targetPosition = Vector2.Lerp(transform.position, desiredPos, lerpSpeed * Time.deltaTime);
        // targetPosition = Vector2.MoveTowards(targetPosition, desiredPos, Time.deltaTime);

        // transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * maxSpeed);

        transform.position = desiredPos;

      }
    }

    void EnableCameraTracking(){
      p_CameraFollowPlayer = true;
    }

    void EnableAdvancedTracking(){
      p_AdvancedFollowPlayer = true;
    }
}
