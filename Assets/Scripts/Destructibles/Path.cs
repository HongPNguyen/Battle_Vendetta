using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    double p1 = 0.3;
    double p2 = 0.4;
    double p3 = 0.3;

    double Rp1 = 0.4;
    double Rp2 = 0.6;

    double select;

    int start = 1;  /// keep an eye on the start position switching
    int path;          // could add starting point related code to Airos

    // Random path selector
    public int Randomize(){
        select = Random.Range(0.0f, 1.0f);
        if (start == 1){

            if (select <= p1){
                path = 1;
            
            }else if (select < p1+p2){
                path = 2;
                start = 2;

            }else{
                path = 3;
                start = 2;
            }
        }else{
            if (select <= Rp1){
                path = 4;
            
            }else{
                path = 5;
                start = 1;
            }
        }
        return path;
    }

}