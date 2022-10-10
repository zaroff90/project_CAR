using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace RGSK
{
    public class PhotonCustomFields : MonoBehaviourPunCallbacks
    {
        public RaceUI raceui;
        public PlayerCamera playcam;
        public OrbitAroundCamera orbitcam;
        public CinematicCamera cinecam;
        public MinimapFollowTarget minicam;

        public string OT;
        private void Start()
        {
            playcam = GameObject.Find("Player Camera").GetComponent<PlayerCamera>();
            orbitcam = GameObject.Find("Starting Grid Camera").GetComponent<OrbitAroundCamera>();
            cinecam = GameObject.Find("Cinematic Camera").GetComponent<CinematicCamera>();
            //minicam = GameObject.Find("Minimap Camera").GetComponent<MinimapFollowTarget>();

            raceui = GameObject.Find("Race UI").GetComponent<RaceUI>();
            StartCoroutine(OnRefresh());
        }
        public IEnumerator OnRefresh()
        {
            if (PhotonNetwork.IsConnected == false)
            {
                yield break;
            }
            yield return new WaitForSeconds(.5f);
            this.gameObject.tag = (string)PhotonNetwork.CurrentRoom.GetPlayer(this.GetComponent<PhotonView>().OwnerActorNr).CustomProperties["Role"];
            this.GetComponent<Statistics>().racerDetails.racerName = PhotonNetwork.CurrentRoom.GetPlayer(this.GetComponent<PhotonView>().OwnerActorNr).NickName;
            this.GetComponent<Statistics>().racerDetails.vehicleName = (string)PhotonNetwork.CurrentRoom.GetPlayer(this.GetComponent<PhotonView>().OwnerActorNr).CustomProperties["Car"];
            if (this.photonView.IsMine == true && PhotonNetwork.IsConnected == true)
            {
                raceui.player = this.GetComponent<Statistics>();


                playcam.target = this.transform;
                playcam.rigid = this.GetComponent<Rigidbody>();
                playcam.GetFixedCameraPositions();
                orbitcam.target = this.transform;
                cinecam.target = this.transform;
                //minicam.target = this.transform;
                
            }
            OT = this.GetComponent<PhotonView>().OwnerActorNr.ToString();
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                
            }
            if(OT == other.ActorNumber.ToString())
            {
                Debug.Log("we quit");
                this.GetComponent<PlayerControl>().enabled = false;
                this.GetComponent<OpponentControl>().enabled = true;
                this.GetComponent<Car_Controller>().controllable = true;
            }           
        }
    }
}