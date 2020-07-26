import 'package:camera/camera.dart';
import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';
import 'package:flutter_first_addiction/Widgets/loginPage.dart';

import 'Widgets/homePage.dart';
import 'Widgets/registerPage.dart';


void main() async {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  bool _result;

  Future doLogin() async {
    await RequestManager.web.autoLogin();
    _result = await RequestManager.web.checkLogin();
  }


  @override
  Widget build(BuildContext context) {

    return MaterialApp(
      title: 'First Addiction',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: new FutureBuilder(
        future: doLogin(),
        builder: (_, userData) {
          switch (userData.connectionState) {
            case ConnectionState.active:
            case ConnectionState.done:
              return _result ? new HomePage() : new LoginPage();
              break;
            default:
              return new Container(child: new Text("waiting"),);
          }
        }
      ),
      routes: <String, WidgetBuilder>{
      // Set routes for using the Navigator.
        '/home': (BuildContext context) => new HomePage(),
        '/login': (BuildContext context) => new LoginPage(),
        '/register': (BuildContext context) => new RegisterPage(),
      },
    );
  }
}