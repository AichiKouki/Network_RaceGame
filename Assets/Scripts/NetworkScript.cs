using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;//ネットワーク機能を使うので、usignディレクティブを使う

//NetworkManager HUDをつかなくてもNetworkManagerのクラスの機能を呼び出すことによってネットワーク接続ができる。
public class NetworkScript : NetworkBehaviour {
	public Canvas canvas;//3つのボタンの表示、非表示にしようする。

	public NetworkDiscovery discovery;//フィールド追加

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//ホストを起動
	public void OnHostButton(){
		canvas.gameObject.SetActive (false);//何かしらのボタンを押したら、canas自体を全部非表示にする。
		NetworkManager.singleton.StartHost ();//ホストとして起動。Networkingパッケージに用意されているクラス。このクラスは、シングルトンのため、勝手にインスタンス化してはいけない。
	}

	//クライアントとして起動
	public void OnClientButton(){
		canvas.gameObject.SetActive (false);
		NetworkManager.singleton.StartClient ();//クライアントとして起動
		//接続状況を知りたい時は、これらのメソッドの返値を利用できます。
		NetworkClient client=NetworkManager.singleton.StartClient();
		Debug.Log(client.serverIp);//アクセスするサーバーのIPアドレス
		Debug.Log (client.serverPort);//ポート番号
		Debug.Log (client.GetType());//クライアントをタイプを表す値(NetworkClientかLocalClient)が表示されます
	}

	//サーバーを起動する処理
	public void OnServerButton(){
		canvas.gameObject.SetActive (false);//ボタンが押されたらcanvasぜんたを非表示にする。
		NetworkManager.singleton.StartServer ();//サーバーとして起動
	}
	//Awake はゲームが始まる前に変数やゲーム状態を初期化するために使用します。なので、Startより前に呼ばれる
	void Awake(){
		Debug.Log ("NetworkScript Start");
		discovery.Initialize ();
		if (!discovery.StartAsServer ()) {//StartAsServerメソッドはサーバーとして実行に失敗するとfalseを返します。これを利用し、結果がfalseならばすでに他でサーバーが起動しているとみなし、StartAsClientメソッドでクライアントとして起動します。
			discovery.StartAsClient ();
			Debug.Log ("NetworkDiscovery StartAsClient");
		} else {//if文の条件式にStartAsServerでサーバー側で実行処理をやっているので、ここはDebugだけ
			Debug.Log ("NetworkDiscovery StartAsServer");
		}
	}
}
