import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';

class LoginPage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return new LoginPageState();
  }
}

class LoginPageState extends State<LoginPage> {
  String _status = 'no-action';
  TextEditingController username = new TextEditingController();
  TextEditingController password = new TextEditingController();

  @override
  Widget build(BuildContext context) => new Scaffold(
    appBar: new AppBar(
      title: new Text('Login'),
    ),
    body: new Container(
      child: new Center(
        child: new Column(
          children: <Widget>[
            new TextField(
              controller: username,
              decoration: InputDecoration(
                contentPadding: EdgeInsets.fromLTRB(20.0, 15.0, 20.0, 15.0),
                hintText: "Username",
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(32.0)
                )
              ),
            ),
            new TextField(
              controller: password,
              obscureText: true,
              decoration: InputDecoration(
                contentPadding: EdgeInsets.fromLTRB(20.0, 15.0, 20.0, 15.0),
                hintText: "Password",
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(32.0)
                )
              ),
            ),
            new RaisedButton(
              child: new Text(
                'Login'
              ),
              onPressed: () {
                // setState(() => this._status = 'loading');
                RequestManager.web
                .loginRequest(username.text, password.text)
                .then((value) => {
                  if (value) {
                    Navigator.of(context).pushReplacementNamed('/home')
                  } else {
                    setState(() => this._status = 'rejected'),
                    print("didnt work")
                  }
                });
              }
            ),
          ]
        ),
      ),
    ),
  );
}