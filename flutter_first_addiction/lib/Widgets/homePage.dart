import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/DBManager.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';
import 'package:flutter_first_addiction/Widgets/profilePage.dart';

import 'feedPage.dart';

class HomePage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return new HomePageState();
  }
}

class HomePageState extends State<HomePage> {
  @override
  Widget build(BuildContext context) {
    return new DefaultTabController(
      length: 2,
      child: new Scaffold(
        appBar: new AppBar(
          title: new Text('Home'),
          actions: <Widget>[
            new PopupMenuButton(
              itemBuilder: (context) => [
                new PopupMenuItem(
                  child: new Text("logout"),
                  value: 1,
                )
              ],
              onSelected: (i) {
                  RequestManager.web.clearToken();

                  DBManager.db.deleteLoginInfo().then((_) =>
                    Navigator.of(context).pushReplacementNamed('/login')
                  );
              },
              icon: new Icon(Icons.more_horiz),
            )
          ],
          bottom: new TabBar(
            tabs: [
              new Tab(
                text: "profile"
              ),
              new Tab(
                text: "feed"
              )
            ]
          ),
        ),
        body: new Container(
          margin: new EdgeInsets.only(
            top: 50.0
          ),
          alignment: Alignment.center,
          child: new TabBarView(
            children: <Widget>[
              new ProfilePage(),
              new FeedPage()
            ]
          )
        ),
      )
    );
  }
}