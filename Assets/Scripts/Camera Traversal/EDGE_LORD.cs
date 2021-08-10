using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDGE_LORD : MonoBehaviour {
    [SerializeField]
    Vector2[] points;
    int i = 0;// 0 to 3 for 4 lines
   private void Awake() {
    PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
    if (poly == null)
    {
        poly = gameObject.AddComponent<PolygonCollider2D>();
    }
        for (int i = 0; i < poly.pathCount; i++) {
            points = poly.GetPath(i);
            EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D>();
            edge.points = points;
            edge.edgeRadius = 0.2f;
            edge.isTrigger = true;
           
        }
    Destroy(poly);
}
}