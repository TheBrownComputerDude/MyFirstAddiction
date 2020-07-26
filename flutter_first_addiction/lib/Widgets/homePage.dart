import 'package:camera/camera.dart';
import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/DBManager.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';
import 'package:flutter_first_addiction/Widgets/cameraPage.dart';
import 'package:flutter_first_addiction/Widgets/profilePage.dart';

import 'feedPage.dart';

class HomePage extends StatefulWidget {

  @override
  State<StatefulWidget> createState() {
    return new HomePageState();
  }
}

class HomePageState extends State<HomePage> {
  int tabIndex = 0;
  List<Widget> tabs;

  @override
  void initState() {
    super.initState();
    tabs = [
      new FeedPage(),
      new CameraPage(),
      new ProfilePage(),
    ];
  }

  @override
  Widget build(BuildContext context) {
    return new Scaffold(
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
      ),
      body: tabs[tabIndex],
      bottomNavigationBar: new BottomNavigationBar(
        currentIndex: tabIndex,
        onTap: (i) {
          setState(() {
            tabIndex = i;
          });
        },
        items: [
          new BottomNavigationBarItem(
            icon: new Icon(Icons.sentiment_very_satisfied),
            title: new Text("")
          ),
          new BottomNavigationBarItem(
            icon: new Icon(Icons.camera_alt),
            title: new Text("")
          ),
          new BottomNavigationBarItem(
            icon: new Icon(Icons.person),
            title: new Text("")
          ),
        ],
        selectedItemColor: Colors.white,
        unselectedItemColor: Colors.black,
        backgroundColor: Theme.of(context).primaryColor,
      ),
    );
  }
}