using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CannibalisticZombies
{
    ///-/////////////////////////////////////////////////////////////////////
    ///
    public class GameManager : MonoBehaviour
    {
        public static UnityAction onResetNight;
        public static UnityAction onResetNightCallback;

        private bool isResetting = false;

        ///-/////////////////////////////////////////////////////////////////////
        ///
        public void Awake()
        {
            onResetNightCallback += ResetNightLoopCallback;
            ResetNightLoop();

        }

        ///-/////////////////////////////////////////////////////////////////////
        ///
        public void ResetNightLoop()
        {
            isResetting = true;

            PlayerCharacterController.instance.gameObject.SetActive(false);
        }

        ///-/////////////////////////////////////////////////////////////////////
        ///
        public void ResetNightLoopCallback()
        {
            isResetting = false;

            PlayerCharacterController.instance.gameObject.SetActive(true);
        }
    }
}