using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGSK
{
    public class DayNight : MonoBehaviour
    {
        public Material matDay;
        public Material matNight;

        public GameObject trackDay;
        public GameObject trackNight;
        // Start is called before the first frame update
        void Start()
        {
            if (global.time == "Day")
            {
                RenderSettings.skybox = matDay;
                GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
                trackDay.SetActive(true);
                trackNight.SetActive(false);
            }
            if (global.time == "Night")
            {
                RenderSettings.skybox = matNight;
                GameObject.Find("Directional Light").GetComponent<Light>().intensity = 0;
                trackDay.SetActive(false);
                trackNight.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}