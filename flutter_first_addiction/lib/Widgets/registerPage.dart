import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';

class RegisterPage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return new RegisterPageState();
  }
}

class RegisterPageState extends State<RegisterPage> {
  TextEditingController username = new TextEditingController();
  TextEditingController email = new TextEditingController();
  TextEditingController password = new TextEditingController();
  TextEditingController passwordConfirm = new TextEditingController();
  String warningMsg = "";

  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      appBar: new AppBar(
        title: new Text('Register')
      ),
      body: new Container(
        child: new Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              warningMsg
            ),
            TextField(
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
            TextField(
              controller: email,
              decoration: InputDecoration(
                contentPadding: EdgeInsets.fromLTRB(20.0, 15.0, 20.0, 15.0),
                hintText: "Email",
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(32.0)
                )
              ),
            ),
            SizedBox(height: 20),
            TextField(
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
            SizedBox(height: 20),
            TextField(
              controller: passwordConfirm,
              obscureText: true,
              decoration: InputDecoration(
                contentPadding: EdgeInsets.fromLTRB(20.0, 15.0, 20.0, 15.0),
                hintText: "Confirm Password",
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(32.0)
                )
              ),
            ),
            RaisedButton(
              child: new Text("Submit"),
              onPressed: () {
                if (username.text.isEmpty || email.text.isEmpty || password.text.isEmpty || passwordConfirm.text.isEmpty) {
                  setState(() {
                    warningMsg = "not all fields have been filled in";
                  });
                  return;
                }

                RequestManager.web.registerRequest(username.text, email.text, password.text)
                  .then((value) {
                    if (value) {
                      Navigator.of(context).pushReplacementNamed("/login");
                    } else {
                      setState(() {
                        warningMsg = "Unable to Register";
                      });
                    }
                  });
              },
            ),
            FlatButton(
              child: new Text("back to login"),
              onPressed: () {
                Navigator.of(context).pushReplacementNamed("/login");
              }
            )
          ],
        ),
      ),
    );
  }

}