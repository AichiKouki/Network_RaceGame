using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;//ネットワーク関連のパッケージをusingしています。ネットワーク利用のプログラム作成にはこのパッケージをusingします。
using UnityEngine.UI;
public class SphereScript : NetworkBehaviour {//ネットワークを利用するクラスは、Behaviourではなく、NetworkBehaviourというクラスを継承します。これはネットワーク関連の機能が実装されたクラスで、これを継承することで、HLAPIのネットワーク機能を利用したプログラムを作成することができます。

	private int CubeCount = 0;//フィールドを追加

	Text text;//ネットワークを使って途中から生成するので、publicを使わない方法を使っている。
	private static System.DateTime startTime = System.DateTime.Now;//ここにDateTimeインスタンスを保管しています。この値を元に経過秒数を計算することで、どのクライアントでも同じ値が表示されるようになっている。

	public GameObject cube;//ミサイル発射のためのオブジェクト

	//Cube発射する位置が低くて発射した瞬間に消えてしまうのでその修正
	Vector3 cubePos;

	//起動時ではなく、サーバーが起動するタイミングで呼び出してくれるメソッドでDateTimeを初期化する。このメソッドはNetworkBehaviourが用意しているメソッド。
	public override void OnStartServer(){
		startTime = System.DateTime.Now;//ここの関数を呼び出した時の時間を格納する。
	}
	void Start () {
		if (isLocalPlayer) {//ローカルプレイヤーかどうかを調べる
			text = GameObject.Find ("MsgText").GetComponent<Text> ();//オンラインで途中からスポーンされるので、publicから直接指定できない
		}
		//Debug.Log ("5÷10の余りは"+(5%10));
		//Debug.Log ("10÷3の余りは"+(10%3));
	}

	void Update () {//毎フレームごとに呼ばれる。間隔は処理能力に依存するため、一定ではない
		//ここでの処理は、サーバー側でCount()を呼び出してcountの値を更新し、クライアント側でUpdateCountしてMsgTextのテキストを更新すると、countの値がカウントされる
		if (isServer) {//サーバーにあるかどうかをチェックします。自分が見ているゲームの中で自分以外のキャラのこと
			Count();//countの値を更新する処理
		}
		if (isLocalPlayer) {//ローカルプレイヤーかどうかをチェックします。
			Move ();//カメラ追随処理
		}

		if (Input.GetKeyDown (KeyCode.Space)) {CmdSpawnIt ();}//単純にスペースキー押したら、オブジェクト発射する関数呼び出すだけ
	}

	//直接的な値の変更をする場合は、サーバー側で処理する。
	[ServerCallback]//メソッドをサーバー側で実行する。
	void Count(){
		if (!isServer) {return;}
		int count = (int)((System.DateTime.Now - startTime).TotalSeconds);//サーバー側でstaticフィールドの値を取り出して、下の処理ではクライアント側でテキスト処理をする。
		RpcSetCount (count);//サーバー側で
	}

	//サーバー側でこの関数を呼び出しているので、サーバー側からクライアント側で実行させるためにこの属性を追加している。
	[ClientRpc]//サーバーからクライアントへコマンドを送るためのものです。サーバーからstaticフィールドの値を取り出し処理を行っていた。
	void RpcSetCount(int n){
		if (text != null) {//Start関数でFind処理で、テキストを見つけられていたら処理
			text.text = "Client:" + n;//取りだいた値を使ってテキストを更新する。
		}
	}

	//Update関数で、ローカルプレイヤーの場合にはずっと呼ばれているんの
	[ClientCallback]//メソッドをクライアント側で実行
	void Move()//カメラを追随する処理。これはプレイヤーではなく他のクライアントと共有しているオブジェクトでもありませんから普通に操作しています。
	{
		if (!isLocalPlayer) { return; }
		Vector3 v = transform.position;
		//カメラの位置をボールの位置から、z軸5後ろで、高さがボールの3上の位置に常に追従させる処理をする。
		v.z -= 5;
		v.y += 3;
		Camera.main.transform.position = v;//追従処理
	}

	[ClientCallback]//メソッドをクライアント側で実行させる
	void FixedUpdate(){//固定のインターバルで呼ばれる。よって、物理演算に関わる処理は、ここに記述する必要がある。
		//移動関連の処理だが、数値の代入しかやっていないので、クライアントで大丈夫
		if (!isLocalPlayer) {return;}//ローカルプレイヤーでなければ処理をしない
		float x = Input.GetAxis ("Horizontal");
		float z = Input.GetAxis ("Vertical");
		CmdMoveSphere (x,z);//実際にボールにAddForceする処理がある関数を引数付けで呼び出す
	}

	//Sphereの移動
	[Command]//プレイヤーを操作するためのコマンドとして認識する。[Command]を設定されたメソッドは、クライアントから呼び出され、サーバー側で実行されます。サーバー側で処理する理由は、サーバー側でオブジェクトを移動しないと、他のクライアントに反映されないから。
	public void CmdMoveSphere(float x,float z){
		Vector3 v = new Vector3 (x,0,z)*10f;
		GetComponent<Rigidbody> ().AddForce (v);//自分自身のコンポーネントを取得して、AddFordceする。
	}

	//プレイヤーオブジェクトの位置以外の各種設定を行う場合は、「OnStartLocalPlayer」というコールバックを使う
	public override void OnStartLocalPlayer(){//いくつかのクライアントでアクセスすると、自分のプレイヤー(ローカルプレイヤー)だけが、赤く変わり、それ以外のリモートプレイヤーは、全てデフォルトの色のままになっている
		Debug.Log ("SphereScript::OnStartLocalPlayer");
		base.OnStartLocalPlayer ();
		//単純にRendererを呼び出して、colorを変更しているだけ
		Renderer r = GetComponent<Renderer> ();//自分自身のRendererコンポーネントを取得する
		Color c = Color.red;//Colorクラスをインスタンス化して、赤色を作成
		r.material.color = c;//自分のcolorの部分を赤色に設定
	}

	//ブロックの名前一覧
	string[] names=new string[]{"zero","one","two","three","four","five","six","seven","eight","nine"};

	//ミサイル発射処理。Spawnするインスタンスを生成できるのは、サーバー側のみです。だから、[Command]属性をつけてサーバー側で実行した。
	[Command]//プレイヤーを操作するためのコマンドとして実行する。このメソッドはクライアント側から呼び出され、サーバー側で実行されます。
	void CmdSpawnIt(){
		Debug.Log ("spawned.");//すぽーんんしたことをデバッグする。
		//ミサイル生成
		cubePos=new Vector3(transform.position.x,transform.position.y+1,transform.position.z);
		GameObject obj = Instantiate<GameObject> (cube,cubePos,Quaternion.Euler(new Vector3(0,0,0)));//生成するプレファブと位置と角度を指定

		CubeScript cubescript=(CubeScript)obj.GetComponent("CubeScript");//CubeScript型の変数宣言しておいて、生成するcubeからCubeScript読み取って代入する
		CubeCount++;
		cubescript.Name=names[CubeCount%10];//CubeScriptの同期させている変数に値を代入している
		cubescript.Number = CubeCount;//同期してる変数に数値をだいにゅしている。
		//obj.GetComponent("CubeScript").name="Cube"+CubeCount++;//Cubeオブジェクトの名前を変更している。CubeScriptを使って名前を変更している

		//UnityEngine.NetworkパッケージにあるNetworkServerクラスにstaticメソッドとして用意されています。
		NetworkServer.Spawn (obj);//Spawnとはネットワークで共有するオブジェクトを生成すること。引数にSpawnするGameObjectを渡す
		Rigidbody r = obj.GetComponent<Rigidbody> ();//Rigidbodyコンポーネントを取得したobjオブジェクトをrに代入
		Vector3 v = Camera.main.transform.forward;//カメラのz軸方向をvを代入
		v.y += 1f;//カメラのy軸方向に1をたす
		r.AddForce (v*1000);//objオブジェクトに100の力を入れる
		r.AddTorque (new Vector3(10f,0f,10)*100);//10,0,10の方向で回転させる
	}
}
