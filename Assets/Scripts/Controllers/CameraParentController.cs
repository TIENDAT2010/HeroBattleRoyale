using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraParentController : MonoBehaviour
{
    [Header("Camera Configurations")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeAmount = 0.25f;
    [SerializeField] private float decreaseFactor = 1.5f;

    [Header("Camera References")]
    [SerializeField] private Transform cameraTrans = null;

    public Transform CameraTrans => cameraTrans;
    private Vector3 dragOrigin = Vector3.zero;
    private float dragSpeed = 40f;
    private bool isStopped = false;
    private bool isSwitchView = false;
    private ViewMode viewMode = ViewMode.Vertical;
    private Vector3 verticalViewPos = new Vector3(0, 0, -30);
    private Vector3 horizontalViewPos = new Vector3(15, 0, 0);
    private Vector3 verticalEulerAngles = new Vector3(50, 0, 0);
    private Vector3 horizontalEulerAngles = new Vector3(30, 0, 0);


    private void Start()
    {
        viewMode = ViewMode.Vertical;
        transform.position = verticalViewPos;
    }


    private void Update()
    {
        if (!isStopped && !isSwitchView)
        {
            if(viewMode == ViewMode.Vertical)
            {
                if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
                {
                    dragOrigin = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0) && dragOrigin.sqrMagnitude != 0)
                {
                    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                    Vector3 move = new Vector3(-pos.x * dragSpeed, 0, -pos.y * dragSpeed);
                    transform.Translate(move, Space.World);
                    dragOrigin = Input.mousePosition;

                    //Clamp the positions
                    Vector3 currentPos = transform.position;
                    currentPos.x = Mathf.Clamp(currentPos.x, -20f, 20f);
                    currentPos.z = Mathf.Clamp(currentPos.z, -30f, 30f);
                    transform.position = currentPos;

                }

                if (Input.GetMouseButtonUp(0))
                {
                    dragOrigin = Vector3.zero;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
                {
                    dragOrigin = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0) && dragOrigin.sqrMagnitude != 0)
                {
                    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                    Vector3 move = new Vector3(pos.y * dragSpeed, 0 ,  -pos.x * dragSpeed);
                    transform.Translate(move, Space.World);
                    dragOrigin = Input.mousePosition;

                    //Clamp the positions
                    Vector3 currentPos = transform.position;
                    currentPos.x = Mathf.Clamp(currentPos.x, -10f, 25f);
                    currentPos.z = Mathf.Clamp(currentPos.z, -25f, 25f);
                    transform.position = currentPos;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    dragOrigin = Vector3.zero;
                }
            }    
        }
    }


    /// <summary>
    /// Check if user is touching an UI game object.
    /// </summary>
    /// <returns></returns>
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }




    /// <summary>
    /// Coroutine shake this camera.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRShake()
    {
        yield return new WaitForSeconds(0.15f);
        Vector3 originalPos = cameraTrans.localPosition;
        float shakeDurationTemp = shakeDuration;
        while (shakeDurationTemp > 0)
        {
            Vector3 newPos = originalPos + Random.insideUnitSphere * shakeAmount;
            newPos.z = originalPos.z;
            cameraTrans.localPosition = newPos;
            shakeDurationTemp -= Time.deltaTime * decreaseFactor;
            yield return null;
        }
        cameraTrans.localPosition = originalPos;
    }





    /// <summary>
    /// Move the camera parent to the enemy tower position.
    /// </summary>
    public void MoveToEnemyTower()
    {
        isStopped = true;
        StartCoroutine(CRMoveToTargetPos(new Vector3(0f, 0f, 45f)));
    }


    /// <summary>
    /// Move the camera parent to the player tower position.
    /// </summary>
    public void MoveToPlayerTower()
    {
        isStopped = true;
        StartCoroutine(CRMoveToTargetPos(new Vector3(0f, 0f, -15f)));
    }


    public void RotateCamera(Transform pos)
    {
        StartCoroutine(CRRotateCameraAroundTower(pos));
    }    



    /// <summary>
    /// Coroutine move the camera parent to the target position.
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    private IEnumerator CRMoveToTargetPos(Vector3 targetPos)
    {
        float t = 0;
        float moveTime = 0.5f;
        Vector3 startPos = transform.position;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float factor = t / moveTime;
            transform.position = Vector3.Lerp(startPos, targetPos, factor);
            yield return null;
        }
        StartCoroutine(CRShake());
    }



    /// <summary>
    /// Switch view mode.
    /// </summary>
    public void SwitchViewMode()
    {
        if(!isSwitchView)
        {
            if (viewMode == ViewMode.Vertical)
            {
                viewMode = ViewMode.Horizontal;
                StartCoroutine(CRSwitchToHorizontalView());
            }
            else
            {
                viewMode = ViewMode.Vertical;
                StartCoroutine(CRSwitchToVerticalView());
            }
        }    
    }    


    /// <summary>
    /// Coroutine switch to horizontal view.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRSwitchToHorizontalView()
    {
        isSwitchView = true;
        float t = 0;
        float moveTime = 1f;
        Vector3 currentForward = transform.forward;
        Vector3 startPos = transform.position;
        Vector3 endPos = horizontalViewPos;
        Vector3 currentEulerAngles = cameraTrans.localEulerAngles;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float factor = t / moveTime;
            transform.position = Vector3.Lerp(startPos, endPos, factor);
            transform.forward = Vector3.Lerp(currentForward, Vector3.left, factor);
            cameraTrans.localEulerAngles = Vector3.Lerp(currentEulerAngles, horizontalEulerAngles, factor);
            yield return null;
        }
        isSwitchView = false;
    }


    /// <summary>
    /// Coroutine switch to vertical view.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRSwitchToVerticalView()
    {
        isSwitchView = true;
        float t = 0;
        float moveTime = 1f;
        Vector3 currentForward = transform.forward;
        Vector3 startPos = transform.position;
        Vector3 endPos = verticalViewPos;
        Vector3 currentEulerAngles = cameraTrans.localEulerAngles;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float factor = t / moveTime;
            transform.position = Vector3.Lerp(startPos, endPos, factor);
            transform.forward = Vector3.Lerp(currentForward, Vector3.forward, factor);
            cameraTrans.localEulerAngles = Vector3.Lerp(currentEulerAngles, verticalEulerAngles, factor);
            yield return null;
        }
        isSwitchView = false;
    }


    private IEnumerator CRRotateCameraAroundTower(Transform towerPos)
    {
        float t = 0;
        float moveTime = 1f;
        Vector3 currentForward = transform.forward;
        Vector3 startPos = transform.position;
        Vector3 endPos = towerPos.position;
        Vector3 currentEulerAngles = cameraTrans.localEulerAngles;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float factor = t / moveTime;
            transform.position = Vector3.Lerp(startPos, endPos, factor);
            transform.forward = Vector3.Lerp(currentForward, Vector3.forward, factor);
            cameraTrans.localEulerAngles = Vector3.Lerp(currentEulerAngles, verticalEulerAngles, factor);
            yield return null;
        }
        Vector3 vector3 = transform.eulerAngles;
        while(t < 6f)
        {
            t += Time.deltaTime;
            vector3.y += 50f * Time.deltaTime;
            transform.eulerAngles = vector3;
            yield return null;
        }
    }    
}