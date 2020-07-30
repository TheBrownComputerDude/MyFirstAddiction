
import 'package:flutter/material.dart';

class GetHandleNamePage extends StatefulWidget{
  @override
  State<StatefulWidget> createState() {
    return GetHandleNamePageState();
  }
  
}

class GetHandleNamePageState extends State<GetHandleNamePage> {

  TextEditingController nickname = new TextEditingController();
  TextEditingController nickname2 = new TextEditingController();
  String errorMsg = "";

  @override
  Widget build(BuildContext context) {

    return new Scaffold(
      appBar: AppBar(
        title: const Text('Whatever I guess'),
      ),
      body: new Container(
        child: new Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            new Text(errorMsg),
            new TextField(
              controller: nickname,
              decoration: InputDecoration(
                contentPadding: EdgeInsets.fromLTRB(20.0, 15.0, 20.0, 15.0),
                hintText: "Nickname",
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(32.0)
                )
              ),
            ),
          SizedBox(height: 20),
          new TextField(
            controller: nickname2,
            obscureText: true,
            decoration: InputDecoration(
              contentPadding: EdgeInsets.fromLTRB(20.0, 15.0, 20.0, 15.0),
              hintText: "Nickname",
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(32.0)
              )
            ),
          ),
          ]
        )
      )
    );
  }

}