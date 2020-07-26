import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';

class LoginPage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return new LoginPageState();
  }
}

class LoginPageState extends State<LoginPage> {
  TextEditingController username = new TextEditingController();
  TextEditingController password = new TextEditingController();
  String errorMsg = "";

  @override
  Widget build(BuildContext context) => new Scaffold(
    appBar: new AppBar(
      title: new Text('Login'),
    ),
    body: new Container(
      child: new Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          new Text(errorMsg),
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
          SizedBox(height: 20),
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
              if (username.text.isEmpty || password.text.isEmpty) {
                setState(() {
                  errorMsg = "not all fields have been filled in";
                });
                return;
              }
              RequestManager.web
              .loginRequest(username.text, password.text)
              .then((value) {
                if (value) {
                  Navigator.of(context).pushReplacementNamed('/home');
                } else {
                  setState(() {
                    errorMsg = "username or password incorrect";
                  });
                }
              });
            }
          ),
          new FlatButton(
            child: new Text("register"),
            onPressed: () {
              Navigator.of(context).pushReplacementNamed("/register");
            },
          )
        ]
      ),
    ),
  );
}