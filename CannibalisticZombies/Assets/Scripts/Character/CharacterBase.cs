using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public class CharacterBase : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] protected float currentHealth = 10f;
        [SerializeField] protected float maxHealth = 10f;
        [SerializeField] protected float speed = 5f;
    }

}