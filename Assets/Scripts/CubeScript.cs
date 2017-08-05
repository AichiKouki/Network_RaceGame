using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CubeScript : NetworkBehaviour {

	const short DestroyMsg = 12345;//メッセージを区別するための番号
	[SyncVar]//ステータスの同期で使われる。
	public int Number;//数値や文字列だけではなく、Vector3などのデータを送信するための処理に必要な変数の宣言
	[SyncVar]
	public string Name;

	void Start () {
		//DestroyMsgメッセージの登録
		foreach(NetworkClient client in NetworkClient.allClients){//NetworkClient.allClientsは現在接続されている全てのNetworkClientをListにまとめたものが得られます。これを利用することで、全クライアントに対し処理を行えるようになる
			//NetworkServer.RegisterHandler(DestroyMsg,OnDestroyMsg);//これだとホストしかメッセージが受け取れない(foreachを作る前)
			//OnTriggerEnterメソッドでSend(SendToAll)関数を呼び出した時、12345の番号を使った送信をされた場合、以下の関数が呼ばれる。
			client.RegisterHandler(DestroyMsg,OnDestroyMsg);//DestroyMsgのメッセージタイプの登録ができました。以後、OnDestroyMsgの番号が設定されたネットワークメッセージが送られてきたら、DestroyMsgを呼び出すようになります。
		}
	}

	//DestroyMsg用ハンドラ
	void OnDestroyMsg(NetworkMessage msg){//返値はvoidであり、引数にはNetworkMessageインスタンスが渡されています。このインスタンスを使い、StringMessageを取り出して、送信されたメッセージを出力しています。
		Debug.Log(msg.ReadMessage<CubeMessage>().getMessage());//<>ではメッセージクラスを指定する。今回は自分で作ったMessageクラスを指定している
	}

	void Update () {
		//発射したオブジェクトの高度が100より低くなったら自分のオブジェクト削除
		if (transform.position.y < -100) {//発車したオブジェクトがTerrainの外に行ったとしてもcubeオブジェクトを削除できるにする処理
			GameObject.Destroy (gameObject);
		}
	}

	//下のTerrainに触れたら自分自身を削除
	void OnTriggerEnter(Collider collider){
		Debug.Log ("Trigger!"+collider.name);//Terrainにぶつかったオブジェクトの名前取得する処理
		if (collider.name == "Terrain") {//衝突しているオブジェクトがTerrainだった場合。つまり、地面に触れている場合
			//int number=new System.Random().Next(1000);//1000までの乱数を作成する処理
			//全NetworkClientにメッセージをを送信する
			//cubeオブジェクトが地面に触れた時にメッセージをサーバーに送信する処理
			//下の一行が、メッセージの作成。StringMessageインスタンスを作成しています。
			CubeMessage msg=new CubeMessage(Name,Number,transform.position);//NameとかNumberとかは、SphereScriptで代入している。
			//NetworkClientに送信します
			NetworkServer.SendToAll(DestroyMsg,msg);//SendToAllは、全てのクライアントにメッセージを一斉送信するもの。たったこれだけで全クライアントにメッセージを送ることができる

			GameObject.Destroy (gameObject);
		}
	}
}
//オリジナルメッセージクラス
public class CubeMessage:MessageBase{//メッセージを読み取る時に使う。
	public string Name;
	public int Number;
	public Vector3 LostPosition;

	//デフォルトコンストラクタ。必須
	public CubeMessage(){}

	//必要な情報を引数で渡すコンストラクタ
	public CubeMessage(string name,int number,Vector3 pos){
		//このクラスのフィールド変数に代入している。sphereScriptクラスで、cubeを生成したと同時にコンポーネント(cubeScript)を取得して直接このスクリプトの変数に代入している。
		this.Name = name;
		this.Number = number;
		this.LostPosition = pos;
	}

	//保管データをStringで出力する
	public string getMessage(){
		return "Destroy "+Name+"("+Number+",[x:"+LostPosition.x+",y:"+LostPosition.y+",z"+LostPosition.z+"])";
	}
}