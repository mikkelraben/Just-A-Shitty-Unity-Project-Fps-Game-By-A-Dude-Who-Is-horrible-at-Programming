using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnPlayerAndSetup : NetworkBehaviour{
    public GameObject Player;
    public override void OnStartLocalPlayer()
    {
        CmdSpawnPlayer();

    }
    [Command]
    void CmdSpawnPlayer(){
        GameObject temp =Instantiate(Player);
        NetworkServer.SpawnWithClientAuthority(temp,gameObject);

    }
}
