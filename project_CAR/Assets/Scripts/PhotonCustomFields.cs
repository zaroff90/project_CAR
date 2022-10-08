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
        private void Start()
        {
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
        }

    }
}