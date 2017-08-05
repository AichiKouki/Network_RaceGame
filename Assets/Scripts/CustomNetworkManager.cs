using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager//NetworkManagerクラスを拡張したいので、継承して機能を追加する 
{
	public NetworkDiscovery discovery;

	void Start() { Debug.Log("CustomNetworkManager start."); }

	// NetworkDsicovery
	public void OnReceivedBroadcast(string address, string msg)
	{
		Debug.Log("OnReceivedBroadcast " + address + ":" + msg);
	}


	public override NetworkClient StartHost()
	{
		Debug.Log("StartHost");
		return base.StartHost();//baseのメソッドを呼び出すことで、基底クラス(親クラス)に送られるようにしている。
	}
	//サーバーにクライアントが接続した
	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect(conn);
		Debug.Log ("OnServerConnect " + conn.connectionId);
	}
	//クライアントがサーバーに接続した
	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
		Debug.Log("OnClientConnect " + conn.connectionId);
	}
	//クライアントがサーバーとの接続を切断した
	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);
		Debug.Log("OnClientDisconnect " + conn.connectionId);
	}
	//クライアントが起動した
	public override void OnStartClient(NetworkClient client)
	{
		base.OnStartClient(client);
		Debug.Log("OnStartClient");
	}
	//サーバーにプレイヤが追加された
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("OnServerAddPlayer " + conn.connectionId + ":" + playerControllerId);
		base.OnServerAddPlayer(conn, playerControllerId);
	}
	//サーバー側のシーンの読み込みが完了した
	public override void OnServerSceneChanged(string sceneName)
	{
		Debug.Log("OnServerSceneChanged " + sceneName);
		base.OnServerSceneChanged(sceneName);
	}
	//クライアント側のシーンの読み込みが完了した
	public override void OnClientSceneChanged(NetworkConnection conn)
	{
		Debug.Log("OnClientSceneChanged " + conn.connectionId);
		base.OnClientSceneChanged(conn);
	}

}
