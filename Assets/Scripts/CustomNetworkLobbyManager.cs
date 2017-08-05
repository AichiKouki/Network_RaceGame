using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkLobbyManager : NetworkLobbyManager 
{
	//ホストとして実行
	public override void OnLobbyStartHost()
	{
		Debug.Log("OnLobbyStartHost");
	}
	//ホストを停止
	public override void OnLobbyStopHost()
	{
		Debug.Log("OnLobbyStopHost");
	}
	//サーバーとして実行
	public override void OnLobbyStartServer()
	{
		Debug.Log("OnLobbyStartServer");
	}
	//サーバー接続
	public override void OnLobbyServerConnect(NetworkConnection conn)
	{
		Debug.Log("OnLobbyServerConnect " + conn.connectionId);
	}
	//サーバーの接続を解除
	public override void OnLobbyServerDisconnect(NetworkConnection conn)
	{
		Debug.Log("OnLobbyServerDisonnect " + conn.connectionId);
	}
	//サーバー側でシーンが変更された
	public override void OnLobbyServerSceneChanged(string sceneName)
	{
		Debug.Log("OnLobbyServerSceneChanged " + sceneName);
	}
	//サーバー側でLobbyPlayerが生成された
	public override GameObject OnLobbyServerCreateLobbyPlayer
	(NetworkConnection conn, short playerController)
	{
		Debug.Log("OnLobbyServerCreateLobbyPlayer " + 
			conn.connectionId + ":" + playerController);
		return base.OnLobbyServerCreateLobbyPlayer(conn, playerController);
	}
	//サーバー側でGamePlayerが生成された
	public override GameObject OnLobbyServerCreateGamePlayer
	(NetworkConnection conn, short playerController)
	{
		Debug.Log("OnLobbyServerCreateGamePlayer " + 
			conn.connectionId + ":" + playerController);
		return base.OnLobbyServerCreateGamePlayer(conn, playerController);
	}
	//サーバー側でプレイヤーが取り除かれた
	public override void OnLobbyServerPlayerRemoved
	(NetworkConnection conn, short playerController)
	{
		Debug.Log("OnLobbyServerPlayerRemoved " + 
			conn.connectionId + ":" + playerController);
	}
	//サーバー側でプレイヤー用にシーンがロードされた
	public override bool OnLobbyServerSceneLoadedForPlayer
	(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		Debug.Log("OnLobbyServerSceneLoadedForPlayer " + 
			lobbyPlayer.name + ":" + gamePlayer.name);
		return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
	}
	//プレイヤーたちの準備が整った
	public override void OnLobbyServerPlayersReady()
	{
		Debug.Log("OnLobbyServerPlayersReady");
		base.OnLobbyServerPlayersReady();
	}

	// ●以下はクライアント側
	//クライアントがロビーに入室した
	public override void OnLobbyClientEnter()
	{
		Debug.Log("OnLobbyClientEnter");
		base.OnLobbyClientEnter();
	}
	//　クライアントがロビーを退出した
	public override void OnLobbyClientExit()
	{
		Debug.Log("OnLobbyClientExit");
		base.OnLobbyClientExit();
	}
	//クライアントが接続した
	public override void OnLobbyClientConnect(NetworkConnection conn)
	{
		Debug.Log("OnLobbyClientConnect " + conn.connectionId);
		base.OnLobbyClientConnect(conn);
	}
	//クライアントの接続が解除された
	public override void OnLobbyClientDisconnect(NetworkConnection conn)
	{
		Debug.Log("OnLobbyClientDisconnect " + conn.connectionId);
		base.OnLobbyClientDisconnect(conn);
	}
	//クライアントがスタートした
	public override void OnLobbyStartClient(NetworkClient client)
	{
		Debug.Log("OnLobbyStartClient");
		base.OnLobbyStartClient(client);
	}
	//クライアントが停止した
	public override void OnLobbyStopClient()
	{
		Debug.Log("OnLobbyStopClient");
		base.OnLobbyStopClient();
	}
	//クライアント側でシーンが変更された
	public override void OnLobbyClientSceneChanged(NetworkConnection conn)
	{
		base.OnLobbyClientSceneChanged(conn);
		Debug.Log("OnLobbyClientSceneChanged " + conn.connectionId);
	}
	//クライアント側でプレイヤーの追加に失敗した
	public override void OnLobbyClientAddPlayerFailed()
	{
		Debug.Log("OnLobbyClientAddPlayerFailed");
	}
}

