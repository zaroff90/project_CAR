using System;
using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


namespace Com.MyCompany.MyGame
{
    public class OnlineGameManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerEntry;
        private GameObject entry;
        public GameObject timePanel;
        public int time=10;
        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
            int arena = (int)PhotonNetwork.CurrentRoom.CustomProperties["Arena"];
            if (arena == 1) PhotonNetwork.LoadLevel("City");
            if (arena == 2) PhotonNetwork.LoadLevel("Greens");
            if (arena == 3) PhotonNetwork.LoadLevel("Downtown");

        }


        #endregion

        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion

        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                if (time == 10)
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("Time", time);
                    PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
                    timePanel.SetActive(true);
                    StartCoroutine(timeStart());
                }
            }
            GameObject[] tags = GameObject.FindGameObjectsWithTag("GamerTag");
            for (int i = 0; i < tags.Length; i++)
            {
                Destroy(tags[i]);
            }

            for (int i = 1; i <= PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                GameObject entry = Instantiate(playerEntry);
                entry.transform.parent = playerEntry.transform.parent;
                entry.transform.GetChild(1).GetComponent<Text>().text = (string)PhotonNetwork.CurrentRoom.GetPlayer(int.Parse("0" + i)).NickName;
                entry.SetActive(true);
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        public IEnumerator timeStart()
        {
            if (time == 0)
            {
                LoadArena();
                yield break;
            }
            timePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Race Starts In :" + time;
            yield return new WaitForSeconds(1);
            time--;
            Hashtable hash = new Hashtable();
            hash.Add("Time", time); PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            StartCoroutine(timeStart());
        }
        #endregion
    }
}