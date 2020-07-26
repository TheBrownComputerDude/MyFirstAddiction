import 'dart:convert';
import 'dart:io';

import 'package:flutter_first_addiction/Managers/DBManager.dart';
import 'package:http/http.dart' as http;

class RequestManager {
  RequestManager._();
  static final RequestManager web = RequestManager._();
  static final String baseUrl = "http://192.168.0.41/";
  static final String userUrl = baseUrl + "user/";
  String token = "";

  Future<bool> loginRequest(String username, String password, {bool save = true}) async {
    var response = await http.get(userUrl+ "login/", headers: {"username":username, "password":password});
    print(response);
    if (response.statusCode == 200) {
      var jsonResult = jsonDecode(response.body);
      token = jsonResult["token"];
      if (save) await DBManager.db.setLoginInfo(username, password);
      return true;
    }
    token = "";
    return false;
  }

  Future autoLogin() async {
    var userInfo = await DBManager.db.getLoginInfo();
    if (userInfo == null) return;
    var username = userInfo["username"];
    var password = userInfo["password"];
    await loginRequest(username, password, save: false);
  }

  Future<bool> checkLogin() async {
    if (token.isEmpty) return false;
    var response = await http.get(userUrl+ "test/", headers: {HttpHeaders.authorizationHeader:"Bearer " + token});
    if (response.statusCode == 200)
    {
      return true;
    }
    return false;
  }

  void clearToken() {
    token = "";
  }

  Future<bool> registerRequest(String username, String email, String password) async {
    var response = await http.post(
      userUrl + "create/?username=${username}&email=${email}&password=${password}");
      if (response.statusCode == 200) {
        return true;
      }
      return false;
  }
}