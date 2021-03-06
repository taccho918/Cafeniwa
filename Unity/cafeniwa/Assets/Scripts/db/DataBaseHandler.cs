using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataBaseHandler : MonoBehaviour {
	private static AndroidJavaObject db;

	public DataBaseHandler(){
		#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			db = new AndroidJavaObject("com.example.databasehandlerlibrary.DatabaseHandler", activity);

			/*
			// 以下テストデータ
			// User "user_name"
			db.Call("addFriend", "ikeda");
			db.Call("addFriend", "takahashi");
			db.Call("addFriend", "okabe");

			// Cafe "cafe_name", "cafe_url", "user_id"
			db.Call("addCafe", "geekcafe", "geelcafe.com", 1);
			db.Call("addCafe", "coffeehouse", "coffeehouse.com", 1);
			db.Call("addCafe", "moyau", "moyau.com", 2);
			db.Call("addCafe", "pione", "pione.com", 3);
			db.Call("addCafe", "masuka", "masuka.com", 4);

			// Coupon "coupon_name", "level", "cafe_id"
			db.Call("addCoupon", "coupon1", 1, 1);
			db.Call("addCoupon", "coupon2", 4, 2);
			db.Call("addCoupon", "coupon3", 1, 3);
			db.Call("addCoupon", "coupon4", 1, 4);
			db.Call("addCoupon", "coupon5", 1, 5);
			db.Call("addCoupon", "coupon6", 1, 5);
			*/
		#endif
	}

	// id のユーザを1人取得して User を返却
	// 引数: id
	// 戻値: User
	public User getUser(int id){
		string str = db.Call<string>("getUser", id);
		List<string> ar = single_parser(str);
		User user = new User(int.Parse(ar[0]), // id
												 ar[1], // name
												 ar[2], // role
												 int.Parse(ar[3]) //auth
												);
		return user;
	}

	// id のカフェを1つ取得して Cafe を返却
	// 引数: id
	// 戻値: Cafe
	public Cafe getCafe(int id){
		string str = db.Call<string>("getCafe", id);
		List<string> ar = single_parser(str);
		Cafe cafe = new Cafe(int.Parse(ar[0]), // id
											   ar[1], // name
												 ar[2], // url
												 int.Parse(ar[3]), // count
												 int.Parse(ar[4]), // user_id
												 int.Parse(ar[5]), // auth
												 int.Parse(ar[6]), // image_number
												 ar[7], // color
												 int.Parse(ar[8]), // object_number
												 int.Parse(ar[9]) //block_number
											   );
		return cafe;
	}

	// id のクーポンを1つ取得して Coupon を返却
	// 引数: id
	// 戻値: Coupon
	public Coupon getCoupon(int id){
		string str = db.Call<string>("getCoupon", id);
		List<string> ar = single_parser(str);
		Coupon coupon = new Coupon(int.Parse(ar[0]), // id
															 ar[1], // name
															 int.Parse(ar[2]), // level
															 int.Parse(ar[3]), // cafe_id
															 int.Parse(ar[4]) // used
															 );
		return coupon;
	}

	// 未認証のフレンドを取得して User の List を返却
	// 引数: なし
	// 戻値: List<User>
	public List<User> getUnAuthFriends(){
		string str = db.Call<string>("getUnAuthFriends");

		List<List<string>> list = multi_parser(str);
		var result = new List<User>();
		foreach(List<string> data in list){
			User user = new User(int.Parse(data[0]), // id
													 data[1], // name
													 data[2], // role
													 int.Parse(data[3]) //auth
													);
			result.Add(user);
		}
		return result;
	}

	// 認証済みのフレンドを取得して User の List を返却
	// 引数: なし
	// 戻値: List<User>
	public List<User> getFriends() {
		string str = db.Call<string>("getFriends");

		List<List<string>> list = multi_parser(str);
		var result = new List<User>();
		foreach(List<string> data in list){
			User user = new User(int.Parse(data[0]), // id
													 data[1], // name
													 data[2], // role
													 int.Parse(data[3]) //auth
													);
			result.Add(user);
		}
		return result;
	}

	// 未認証カフェを取得して Cafe の List を返却
	// 引数: なし
	// 戻値: List<Cafe>
	public List<Cafe> getUnAuthCafes(){
		string str = db.Call<string>("getUnAuthCafes");

		List<List<string>> list = multi_parser(str);
		var result = new List<Cafe>();
		foreach (List<string> data in list) {
		 	Cafe cafe = new Cafe(int.Parse(data[0]), // id
													 data[1], // name
													 data[2], // url
													 int.Parse(data[3]), // count
													 int.Parse(data[4]), // user_id
													 int.Parse(data[5]), // auth
													 int.Parse(data[6]), // image_number
													 data[7], // color
													 int.Parse(data[8]), // object_number
													 int.Parse(data[9]) //block_number
													);
			result.Add(cafe);
		}
	 	return result;
	}

	// 認証済みの user_id をもつカフェを取得して Cafe の List を返却
	// 引数: id(ユーザの id，owner の場合は 1 を渡す)
	// 戻値: List<Cafe>
	public List<Cafe> getCafes(int user_id) {
		string str = db.Call<string>("getCafes", user_id);

		List<List<string>> list = multi_parser(str);
		var result = new List<Cafe>();
		foreach (List<string> data in list) {
			Cafe cafe = new Cafe(int.Parse(data[0]), // id
													 data[1], // name
													 data[2], // url
													 int.Parse(data[3]), // count
													 int.Parse(data[4]), // user_id
													 int.Parse(data[5]), // auth
													 int.Parse(data[6]), // image_number
													 data[7], // color
													 int.Parse(data[8]), // object_number
													 int.Parse(data[9]) //block_number
													);
			result.Add(cafe);
	 	}
	 	return result;
	}

	// cafe_id をもつクーポンを取得して Coupon の List を返却
	// 引数: cafe_id
	// 戻値: List<Coupon>
	public List<Coupon> getCoupons(int cafe_id){
		string str = db.Call<string>("getCoupons", cafe_id);
		List<List<string>> list = multi_parser(str);
		var result = new List<Coupon>();
		foreach (List<string> data in list) {
			Coupon coupon = new Coupon(int.Parse(data[0]), // id
																 data[1], // name
																 int.Parse(data[2]), // level
																 int.Parse(data[3]), // cafe_id
																 int.Parse(data[4]) // used
																 );
			result.Add(coupon);
		}
		return result;
	}

	// 未認証のカフェを認証する
	// 引数: id, color, object_number
	// 戻値: なし
	public void AuthenticateCafe(int id, string color, int object_number) {
		db.Call("AuthenticateCafe", id, color, object_number);
	}

	// 未認証のフレンドを認証する
	// 引数: id
	// 戻値: なし
	public void AuthenticateFriend(int id){
		db.Call("AuthenticateFriend", id);
	}

	// カフェの場所を移動
	// 引数: id, block_number
	// 戻値: なし
	public void MoveCafe(int id, int block_number) {
		db.Call("MoveCafe", id, block_number);
	}

	//////////////////////////////////////////////////////////////////////////////
	// 以下 parser
	//////////////////////////////////////////////////////////////////////////////

	// str example
	// {id:1,name:hoge}
	private List<string> single_parser(string str){
		List<string> result = new List<string>();
		string[] ar = str.Replace("{","").Replace("}","").Split(',');
		foreach(string a in ar){
				string[] tmp = a.Split(':');
				result.Add(tmp[1]);
			}
		return result;
	}

	// str example
	// {id:1,name:hoge}{id:2,name:fuga}
	private List<List<string>> multi_parser(string str){
		var result = new List<List<string>>();
		string[] datas = str.Replace("{","").Split('}');
		foreach(string data in datas){
			if (data != ""){
				List<string> data_list = new List<string>();
				string[] columns = data.Split(',');
				foreach(string column in columns){
					if (column != ""){
						string[] tmp = column.Split(':');
						data_list.Add(tmp[1]);
					}
				}
				result.Add(data_list);
			}
		}
		return result;
	}
}
