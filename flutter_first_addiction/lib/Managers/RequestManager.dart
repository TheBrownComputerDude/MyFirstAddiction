import 'dart:convert';
import 'dart:io';

import 'package:dio/dio.dart';
import 'package:flutter_first_addiction/Configuration/AppConfig.dart';
import 'package:flutter_first_addiction/Managers/DBManager.dart';
import 'package:http/http.dart' as http;

class RequestManager {
  RequestManager._();
  static final RequestManager web = RequestManager._();
  Dio dio = new Dio();
  // static final String baseUrl = "http://192.168.0.41/";
  // static final String baseUrl = config.s;
  // static final String userUrl = baseUrl + "user/";
  static String token = "";

  Future<String> getUserEndpoint() async {
    var config = await AppConfig.loadConfig();
    return config.serverEndpoint;
  }

  Future<bool> loginRequest(String username, String password, {bool save = true}) async {
    var response = await http.get(await getUserEndpoint() + "user/login/", headers: {"username":username, "password":password});
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
    var response = await http.get(await getUserEndpoint() + "user/test/", headers: {HttpHeaders.authorizationHeader:"Bearer " + token});
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
      await getUserEndpoint() + "user/create/?username=${username}&email=${email}&password=${password}");
      if (response.statusCode == 200) {
        return true;
      }
      return false;
  }

  Future<String> getInfo() async {
    var uri = await getUserEndpoint() + "user/getInfo";

    var op = new Options(headers: {
      HttpHeaders.authorizationHeader:"Bearer " + token
    });
    var response = await dio.get(uri, options: op);
    if(response.statusCode == 204){
      return '';
    }
    return response.data["handleName"];
  }

  Future<bool> uploadVideo(File file) async {
    var uri = await getUserEndpoint() + "media/upload";
    var op = new Options(headers: {
      HttpHeaders.authorizationHeader:"Bearer " + token
    });
    var size = await file.length();
    var form = new FormData.fromMap({
      "video": await MultipartFile.fromFile(file.path)
    });

    var response = await dio.post(uri, data: form, options: op);
    if (response.statusCode == 200) {
      return true;
    }
    return false;
  }

  Future<List<int>> getVideoIds() async {
    var uri = await getUserEndpoint() + "user/videos";
    var op = new Options(headers: {
      HttpHeaders.authorizationHeader:"Bearer " + token
    });
    var response = await dio.get(uri,  options: op);

    if (response.statusCode == 200)
    {
      List<int> result = response.data.cast<int>();
      return result;
    }
    return new List<int>();


  }
}