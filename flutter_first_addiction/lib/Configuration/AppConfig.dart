import 'dart:convert';

import 'package:flutter/services.dart';

class AppConfig {
  final String serverEndpoint;

  AppConfig({this.serverEndpoint});

  static Future<AppConfig> loadConfig() async {

    final contents = await rootBundle.loadString(
      'assets/config/dev.json',
    );

    final json = jsonDecode(contents);

    return AppConfig(serverEndpoint: json['server']);
  }
}