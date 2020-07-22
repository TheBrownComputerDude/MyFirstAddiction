import 'package:sqflite/sqflite.dart';
import 'package:sqflite/sqlite_api.dart';
import 'package:path/path.dart';

class DBManager
{
  DBManager._();
  static final DBManager db = DBManager._();
  static Database _database;

  Future<Database> get database async {
    if (_database != null) return _database;

    _database = await initDB();

    return _database;
  }


  initDB() async {
    return await openDatabase(
      join(await getDatabasesPath(), 'firstAddiction.db'),
      onCreate: (db, version) async {
        await db.execute("create table loginInfo (username text not null unique, password text not null)");
      },
      version: 1
     );
  }

  Future<Map<String, dynamic>> getLoginInfo() async {
    final db = await database;
    var result = await db.query("loginInfo");
    return result.isEmpty ? null : result.first;
  }

  Future deleteLoginInfo() async {
    final db = await database;
    await db.delete("loginInfo");
  }

  Future setLoginInfo(String username, String password) async {
    final db = await database;
    if (username.isNotEmpty && password.isNotEmpty)
    {
      try {
        await db.insert("loginInfo", {"username":username, "password":password});
        print("Updated login info");
      } catch(e)
      {
        print(e);
      }
    }
  }

  // Future<List<Map<String, dynamic>>> getIngredients() async {
  //   final db = await database;
  //   var result = await db.query("ingredients");
  //   if (result.length == 0) {
  //     return null;
  //   }
  //   return result;
  // }

  addIngredient(String value) async {
    final db = await database;
    if (value.isNotEmpty){
      // var response = await db.rawQuery("select * from ingredients where name = tomatoes");
      // print(response);
      try {
        await db.insert("ingredients", {"name":value});
        print("added " + value + " to db.");
      } catch(e)
      {}
    }
  }

}