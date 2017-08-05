using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//NetworkDiscoveryクラスを拡張する。
public class DiscoveryScript : NetworkDiscovery {//NetworkDiscoveryクラスを継承している

	//NetworkDiscoveryの初期化をする
	public void OnInitButton(){
		if (running) {//initializeで初期化し実行する場合、すでにNetworkDiscoveryが実行中であればエラーになるため、「running」というプロパティをチェックし、これがtrueであればStopBroadcastしてからInitializeするようにしている
			StopBroadcast ();//実行を停止
		}
		Initialize ();//初期化する
		Debug.Log ("Initialize.");
	}
	//サーバーとして実行する処理
	public void OnServerButton (){
		StartAsServer ();//サーバー側で実行
		Debug.Log ("Start As Server.");
	}
	//クライアントとして実行する処理
	public void OnClientButton(){
		StartAsClient ();//クライアントとして実行
		Debug.Log ("Start As Client.");
	}
	//メッセージを受信するコールバック定義
	////ブロードキャストされたメッセージがクライアントに送られると、OnReceivedBroadcastコールバックが呼び出され、
	/// アドレスとメッセージが引数として渡されます。ここで、メッセージを受け取った際の処理が用意できます。
	public override void OnReceivedBroadcast(string address,string msg){
		Debug.Log ("OnReceivedBroadcast address=["+address+"]message=["+msg+"]");
	}
	// Use this for initialization
	void Start () {
		if (isServer) {//サーバー側であれば処理
			CustomNetworkManager.singleton.StartHost ();
			Debug.Log ("StartHost");
		} else {//クライアント側であれば処理
			CustomNetworkManager.singleton.StartClient ();
			Debug.Log ("StartClient");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
