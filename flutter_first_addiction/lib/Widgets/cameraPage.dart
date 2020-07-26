import 'dart:async';
import 'dart:io';

import 'package:camera/camera.dart';
import 'package:flutter/material.dart';
import 'package:flutter_first_addiction/Widgets/videoPreview.dart';
import 'package:path_provider/path_provider.dart';

class CameraPage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return new CameraPageState();
  }

}

class CameraPageState extends State<CameraPage> {
  CameraController controller;
  List<CameraDescription> cameras;
  int cameraIndex = 0;
  bool recording = false;
  bool paused = false;
  static final int videoTime = 10;
  int videoDuration = videoTime;
  Timer timer;
  String filePath;
  String timestamp() => DateTime.now().millisecondsSinceEpoch.toString();

  @override
  void initState() {
    getCameras();
    super.initState();
  }
  Future getCameras() async {
    cameras = await availableCameras();
    controller = CameraController(cameras[cameraIndex], ResolutionPreset.medium);
    controller.initialize().then((_) {
      if (!mounted) {
        return;
      }
      setState(() {});
    });
  }

  @override
  void dispose() {
    controller?.dispose();
    super.dispose();
  }

  Future swapCamera() async {
    setState(() {
      cameraIndex = cameraIndex == 0 ? 1 : 0;
    });
    final CameraDescription cameraDescription =
      cameras[cameraIndex];
    if (controller != null) {
      await controller.dispose();
    }
    controller = CameraController(cameraDescription, ResolutionPreset.medium);
    controller.initialize().then((_) {
      if (!mounted) {
        return;
      }
      setState(() {});
    });
  }

  Future record() async {
    final Directory extDir = await getApplicationDocumentsDirectory();
    final String dirPath = '${extDir.path}/media';
    await Directory(dirPath).create(recursive: true);
    final String filePath = '$dirPath/${timestamp()}.mp4';

    if (controller.value.isRecordingVideo) {
      // A recording is already started, do nothing.
      return null;
    }

    startTimer();
    setState(() {
      recording = true;
    });

    try {
      await controller.startVideoRecording(filePath);
    } on CameraException catch (e) {
      return null;
    }
    this.filePath = filePath;
  }

  Future pauseRecord() async {
    setState(() {
      paused = true;
    });
    await controller.pauseVideoRecording();
    timer.cancel();
  }

  Future unpauseRecord() async {
    setState(() {
      paused = false;
    });
    await controller.resumeVideoRecording();
    startTimer();
  }
  
  void startTimer() {
    timer = new Timer.periodic(
      new Duration(seconds: 1),
      (timer) async {
        if (videoDuration < 1) {
          await controller.stopVideoRecording();
          setState(() {
            videoDuration = videoTime;
          });
          timer.cancel();
          await Navigator.push(
            context,
            new MaterialPageRoute(
              builder: (context) => new NewVideoPreview(filePath)));
        } else {
          setState(() {
            videoDuration -= 1;
          });
          print(videoDuration);
        }
      });
  }

  @override
  Widget build(BuildContext context) {
    if (!controller.value.isInitialized) {
      return Container(
        child: new Text("can't access camera :("),
      );
    }
    return new Column(
      children: <Widget>[ 
        new AspectRatio(
          aspectRatio:
          controller.value.aspectRatio,
          child: CameraPreview(controller)
        ),
        new LinearProgressIndicator(
          value: 1 - videoDuration / videoTime,
        ),
        new IconButton(
          icon: new Icon(recording && !paused ? Icons.pause : Icons.play_arrow),
          onPressed: () {
            if (!recording) {
              record();

            } else if (!paused) {
              pauseRecord();
            } else {
              unpauseRecord();
            }
          },
        ),
        new IconButton(
          icon: new Icon(
            cameraIndex == 0 ? Icons.camera_front : Icons.camera_rear
          ),
          onPressed: swapCamera
        )
      ]
    );
  }
}