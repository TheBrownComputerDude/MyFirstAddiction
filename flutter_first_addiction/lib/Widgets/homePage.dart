import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/DBManager.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';

class HomePage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return new HomePageState();
  }
}

class HomePageState extends State<HomePage> {
  @override
  Widget build(BuildContext context) {
    return new Scaffold(
    appBar: new AppBar(
      title: new Text('Home'),
    ),
    body: new Container(
      margin: new EdgeInsets.only(
        top: 50.0
      ),
      alignment: Alignment.center,
      child: new Column(
        children: <Widget>[
          new Text('Welcome to App!'),
          new FlatButton(
            child: new Text(
              'Logout'
            ),
            onPressed: () async {
              RequestManager.web.clearToken();

              DBManager.db.deleteLoginInfo().then((_) => {
                Navigator.of(context).pushReplacementNamed('/login')
              });
            }
          )
        ],
      ),
    ),
  );
  }

}