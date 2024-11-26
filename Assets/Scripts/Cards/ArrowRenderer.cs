using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Video;

public class ArrowRenderer : MonoBehaviour
{
public GameObject arrowPrefab;
public GameObject dotPrefab;
// using a pool of dots is more efficient than generating and destroying a bunch every time we move
public  List<GameObject> dotPool = new List<GameObject>();
public int poolSize = 50;
public float dotSpacing = 10;
private GameObject arrow;
public float arrowAngleCorrection = 0;
public int dotsToSkip = 1;
private Vector3 arrowDirection;

private Canvas canvas;
  
    void Awake() {
      canvas = GetComponentInParent<Canvas>();
    }
    // Start is called before the first frame update
    void Start()
    {
        arrow = Instantiate(arrowPrefab, transform);
        arrow.transform.localPosition = Vector3.zero;
        InitializeDotPool(poolSize);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;
        Vector3 startPosition = transform.position; //mark
        Vector3 midPoint = CalcMidPoint(startPosition, mousePosition);
        UpdateArc(startPosition, midPoint, mousePosition);
        UpdateArrowhead(mousePosition);
    }

  private void UpdateArc(Vector3 start, Vector3 midPoint, Vector3 end) {
    int numDots = Mathf.CeilToInt(Vector3.Distance(start, end) / dotSpacing);
    for (int i = 0; i < numDots && i < dotPool.Count; i++) {
      // bezier curve calculation
      float t = i / (float)numDots;
      // t is required to be in [0,1]
      t = Mathf.Clamp(t, 0f, 1f);
      Vector3 position = CalcBezierPoint(start, midPoint, end, t);

      if (i != numDots - dotsToSkip) {
        dotPool[i].transform.position = position; //mark
        dotPool[i].SetActive(true);
      }
      // use final dot positions to determine arrow position
      if (i == numDots - (dotsToSkip + 1) && i - dotsToSkip + 1 >= 0) {
        arrowDirection = dotPool[i].transform.position; //mark
      }
    }
    // deactivate dots??
    for (int i = numDots - dotsToSkip; i < dotPool.Count; i++) {
      if (i > 0) {
        dotPool[i].SetActive(false);
      }
    }
  }

  // specifically for a quadratic bezier curve
  private Vector3 CalcBezierPoint(Vector3 startPos, Vector3 midPoint, Vector3 endPos, float t)
  {
    //return (1 - t) * (1 - t) * startPos + 2 * (1-t) * t * t * midPoint + t * t * t * t * endPos;
    float u = 1 - t;
    float tt = t*t;
    float uu = u*u;

    Vector3 point = uu * startPos;
    point += 2 * u * t * midPoint;
    point += tt * endPos;
    return point;
  }

  void UpdateArrowhead(Vector3 position) {
    arrow.transform.position = position;  //mark
    Vector3 direction = arrowDirection - position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    angle += arrowAngleCorrection;
    // rotates on z axis
    arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
  }

  Vector3 CalcMidPoint(Vector3 startPos, Vector3 endPos) {
      Vector3 midPoint = (startPos + endPos) / 2;
      // pushes midpoint up so we get an actual arc, not a line
      float arcHeight = Vector3.Distance(startPos, endPos) / 3f;
      midPoint.y += arcHeight;
      return midPoint;
    } 

    void InitializeDotPool(int count) {
      for (int i = 0; i < count; i++) {
        GameObject dot = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity, transform);
        dot.SetActive(false);
        dotPool.Add(dot);
      }
    }
}
