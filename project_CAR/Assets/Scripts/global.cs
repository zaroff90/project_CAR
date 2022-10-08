using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RGSK
{
    public class global : MonoBehaviour
    {
        public Button btnSingle;
        public Button btnMulti;

        public static string playerCar = "Camera_Ai 1";
        public static List<string> opponentCars = new List<string>();



        void Start()
        {
            //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
            btnSingle.onClick.AddListener(OnSingle);
            btnMulti.onClick.AddListener(OnMulti);
        }

        public void OnSingle()
        {
            RaceManager.onlineMode = false;
        }
        public void OnMulti()
        {
            RaceManager.onlineMode = true;
        }
    }
}
