import 'package:camera/camera.dart';
import 'package:flutter/material.dart';

class CameraPage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return new CameraPageState();
  }

}

class CameraPageState extends State<CameraPage> {
  CameraController controller;
  List<CameraDescription> cameras;

  @override
  void initState() {
    getCameras();
    super.initState();
  }
  Future getCameras() async {
    cameras = await availableCameras();
    controller = CameraController(cameras[0], ResolutionPreset.medium);
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

  @override
  Widget build(BuildContext context) {
    if (!controller.value.isInitialized) {
      return Container(
        child: new Text("can't access camera :("),
      );
    }
    return AspectRatio(
        aspectRatio:
        controller.value.aspectRatio,
        child: CameraPreview(controller));
  }
}