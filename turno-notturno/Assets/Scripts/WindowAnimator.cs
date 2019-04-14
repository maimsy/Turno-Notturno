using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowAnimator : MonoBehaviour
{

    public List<Transform> windows;
    public float closeDelayPerWindow = 2f;
    public float closeDurationPerWindow = 5f;
    public float minY;
    private float maxY;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        maxY = windows[0].localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForceOpen()
    {
        foreach (Transform window in windows)
        {
            Vector3 newPos = window.localPosition;
            newPos.y = minY;
            window.localPosition = newPos;
        }
    }

    public void ForceClose()
    {
        foreach (Transform window in windows)
        {
            Vector3 newPos = window.localPosition;
            newPos.y = maxY;
            window.localPosition = newPos;
        }
    }

    
    
    public void SmoothClose()
    {
        for (int i = 0; i < windows.Count; i++)
        {
            IEnumerator delayedCoroutine = SmoothMove(windows[i].gameObject, maxY, closeDurationPerWindow, closeDelayPerWindow * i);
            StartCoroutine(delayedCoroutine);
        }
    }
    
    IEnumerator SmoothMove(GameObject window, float targetY, float time, float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);
        Vector3 start = window.transform.localPosition;
        Vector3 end = new Vector3(start.x, targetY, start.z);
        for (float t = 0; t < 1; t += Time.smoothDeltaTime / time)
        {
            window.transform.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }
        window.transform.localPosition = Vector3.Lerp(start, end, 1);
    }
}
