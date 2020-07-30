import 'package:flutter/material.dart';
import 'package:video_player/video_player.dart';

class NewVideoPreview2 extends StatefulWidget {
  final int id;
  final String path;
  NewVideoPreview2(this.id, this.path);

  @override
  State<StatefulWidget> createState() {
    return new NewVideoPreview2State();
  }
}

class NewVideoPreview2State extends State<NewVideoPreview2> {
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

  void createController() async {
    // controller = VideoPlayerController.network(path + "media/video?videoId=${widget.id}");
    // await controller.initialize();
    controller = VideoPlayerController.network(
        widget.path + 'media/video?videoId=${widget.id}');

    await controller.initialize();
    controller.setLooping(true);
    print("hello");
    setState(() {});
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
            : Container(child: new Text("loading"),),
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
          // new Row(
          //   children: <Widget>[
          //     new IconButton(
          //       icon: new Icon(Icons.save),
          //       onPressed: () async {
          //         await RequestManager.web.uploadVideo(new File(widget.path));
          //         await Navigator.pushReplacement(
          //           context,
          //           new MaterialPageRoute(
          //             builder: (context) => new HomePage()));
          //       }
          //     ),
          //     new IconButton(
          //       icon: new Icon(Icons.delete_forever),
          //       onPressed: () async {
          //         var f = new File(widget.path);
          //         f.deleteSync();
          //         await Navigator.pushReplacement(
          //           context,
          //           new MaterialPageRoute(
          //             builder: (context) => new HomePage()));
          //       }
          //     )
          //   ],
          // )
        ]
      ),
    );
  }
}