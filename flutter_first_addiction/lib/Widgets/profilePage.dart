import 'dart:io';
import 'dart:typed_data';

import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';
import 'package:flutter_first_addiction/Widgets/getHandleNamePage.dart';
import 'package:flutter_first_addiction/Widgets/videoPreview.dart';
import 'package:path_provider/path_provider.dart';
import 'package:thumbnails/thumbnails.dart';
import 'package:video_player/video_player.dart';
import 'package:video_thumbnail/video_thumbnail.dart';

class ProfilePage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return new ProfilePageState();
  }
}

class ProfilePageState extends State<ProfilePage> {

  String handle = '';

  Future<List<FileSystemEntity>> getAllVideos() async {
    handle = await RequestManager.web.getInfo();
    if(handle.isEmpty){
      Navigator.push(context, 
       new MaterialPageRoute(
              builder: (context) => new GetHandleNamePage()));
        
    }
    final Directory extDir = await getApplicationDocumentsDirectory();
    final String dirPath = '${extDir.path}/media';
    final myDir = Directory(dirPath);
    List<FileSystemEntity> _images;
    if( await myDir.exists()){
      _images = myDir.listSync(recursive: true, followLinks: false);
      _images.sort((a, b) {
        return b.path.compareTo(a.path);
      });
    }

    return _images;

  }

  Future<Uint8List> getThumbnail(String path) async {
    return await VideoThumbnail.thumbnailData(video: path);
  }
  @override
  Widget build(BuildContext context) {
    return new FutureBuilder(
      future: getAllVideos(),
      builder: (context, AsyncSnapshot<List<FileSystemEntity>> snapshot) {
        if (!snapshot.hasData || snapshot.data.isEmpty) {
          return Container();
        }
        print('${snapshot.data.length} ${snapshot.data}');
        if (snapshot.data.length == 0) {
          return Center(
            child: Text('No images found.'),
          );
        }

        return GridView.count(
          crossAxisCount: 3,
          children: List<Widget>.generate(
            snapshot.data.length,
            (index) {
              return new Card(
                child: new InkWell(
                  child: new Text(snapshot.data[index].path),
                  onTap: () async {
                    await Navigator.push(
                      context,
                      new MaterialPageRoute(
                        builder: (context) => new NewVideoPreview(snapshot.data[index].path)));
                    setState(() {});
                  },
                ),
              );
            }),
        );

        return PageView.builder(
          itemCount: snapshot.data.length,
          itemBuilder: (context, index) {
            var currentFile = snapshot.data[index].path;
            return Container(
              child: new Text(currentFile),
            );
          }
        );
      }
    );
  }
}