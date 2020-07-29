import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Managers/RequestManager.dart';
import 'package:video_player/video_player.dart';

class NewVideoPreview extends StatefulWidget {
  final String path;
  NewVideoPreview(this.path);

  @override
  State<StatefulWidget> createState() {
    return new NewVideoPreviewState();
  }

}

class NewVideoPreviewState extends State<NewVideoPreview> {
  VideoPlayerController controller;
  bool playing = false;


  @override
  void initState() {
    super.initState();
    createController();
  }

  @override
  void dispose() {
    super.dispose();
    controller.dispose();
  }

  void createController() {
    var f = new File(widget.path);
    controller = new VideoPlayerController.file(f);
    controller.setLooping(true);
    controller.initialize().then((value) => setState(() {}));
  }

  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      appBar: new AppBar(),
      body: new Column(
        children: <Widget>[controller.value.initialized
            ? AspectRatio(
                aspectRatio: controller.value.aspectRatio,
                child: VideoPlayer(controller),
              )
            : Container(),
          new IconButton(
            icon: new Icon(playing ? Icons.pause : Icons.play_arrow),
            onPressed: () {
              if (!playing) {
                controller.play();
                setState(() {
                  playing = true;
                });
              } else {
                controller.pause();
                setState(() {
                  playing = false;
                });
              }
            }
          ),
          new Row(
            children: <Widget>[
              new IconButton(
                icon: new Icon(Icons.save),
                onPressed: () async {
                  await RequestManager.web.uploadVideo(new File(widget.path));
                }
              ),
              new IconButton(
                icon: new Icon(Icons.delete_forever),
                onPressed: () {
                  var f = new File(widget.path);
                  f.deleteSync();
                  Navigator.pop(context);
                }
              )
            ],
          )

        ]
      ),
    );
  }
}