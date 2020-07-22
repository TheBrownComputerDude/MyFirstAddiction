import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';
import 'package:flutter_first_addiction/Widgets/loginPage.dart';

import 'Widgets/homePage.dart';

void main() async {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  bool _result;
  MyApp();

  Future doLogin() async {
    print("hello");
    await RequestManager.web.autoLogin();
    print("pt2");
    _result = await RequestManager.web.checkLogin();
    print(_result);
  }


  @override
  Widget build(BuildContext context) {
    // Widget _defaultHome = new LoginPage();
    // if (_result) {
    //   _defaultHome = new HomePage();
    // }

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
        '/login': (BuildContext context) => new LoginPage()
      },
    );
  }
}

// class MyHomePage extends StatefulWidget {
//   MyHomePage({Key key, this.title}) : super(key: key);

//   final String title;

//   @override
//   _MyHomePageState createState() => _MyHomePageState();
// }

// class _MyHomePageState extends State<MyHomePage> {
//   int _counter = 0;

//   void _incrementCounter() {
//     setState(() {
//       _counter++;
//     });
//   }

//   @override
//   Widget build(BuildContext context) {
//     return Scaffold(
//       appBar: AppBar(
//         title: Text(widget.title),
//       ),
//       body: Center(
//         child: Column(
//           mainAxisAlignment: MainAxisAlignment.center,
//           children: <Widget>[
//             Text(
//               'You have pushed the button this many times:',
//             ),
//             Text(
//               '$_counter',
//               style: Theme.of(context).textTheme.headline4,
//             ),
//           ],
//         ),
//       ),
//       floatingActionButton: FloatingActionButton(
//         onPressed: _incrementCounter,
//         tooltip: 'Increment',
//         child: Icon(Icons.add),
//       ), // This trailing comma makes auto-formatting nicer for build methods.
//     );
//   }
// }
