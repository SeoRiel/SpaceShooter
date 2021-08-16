using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    private AudioSource audio;                                      // 
    private Animator animator;                                      //
    private Transform playerTransform;                              //
    private Transform enemyTransform;                               //

    private readonly int hashFire = Animator.StringToHash("Fire");  //

    private float nextFire = 0.0f;                                  //
    private readonly float fireRate = 0.1f;                         //
    private readonly float damping = 10.0f;                         //

    public bool isFire = false;                                     //
    public AudioClip fireSFX;                                       //

}
