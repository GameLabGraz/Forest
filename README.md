# Forest - An Immersive Analytics Framework for Virtual Reality Experiences

<img src="/images/logo/logo.png" width="200"/>


This project focuses on the identification of user experience flows, interaction characteristics, challenges, and advantages of fully immersive VR applications by using an immersive analytics tool. The goal is to use Immersive Analytics (IA) to gain a better understanding of user behavior and to support developers during their design decisions. For this purpose, an IA framework will be developed, implemented, and applied in existing VR environments to analyze the user behavior in different application scenarios such as (1) learning, (2) industry, and (3) medical. The outcomes should help developers to improve their design concepts and to adapt the environments to the users' needs.

The following figure shows the conceptual overview of the IA framework. An interface between the VR environment and the IA tool will provide the data visualization and allows the developer to observe the operations from the users' perspective in an overlapping scene. An intelligent agent should support the developer in the design process by making suggestions and creating computer-based VR content.

<img src="/images/architecture/architecture.png" width="500"/>

## Import packages into your Unity project
To import a package into a Unity project, add the following line to your /Packages/manifest.json file:

    "com.gamelabgraz.immersiveanalytics": "https://github.com/GameLabGraz/Forest.git#package/immersiveanalytics"

## Update package in your Unity project
Remove the "lock" block for the package in your /Packages/manifest.json file.
Unity will then automatically import the latest version of the package.

    "com.gamelabgraz.immersiveanalytics": {
      "revision": "package/immersiveanalytics",
      "hash": "<hash value>"
    }
 
 ## Supported SDK's
| SDK | Download Link |
|---------------|---------------|
| Unity 2019.4 and higher  | [Unity] |
| HTC Vive | [HTC Vive] |
| Oculus | [Oculus Integration] |

[Unity]: https://unity3d.com
[HTC Vive]: https://www.vive.com
[Oculus Integration]: https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022
