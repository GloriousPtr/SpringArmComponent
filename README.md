<h1 align="left" style="border-bottom: none;">
  <a href="https://github.com/MohitSethi99/SpringArmComponent/">Spring Arm Component for Unity</a>
</h1>

[![Build Status](https://github.com/MohitSethi99/SpringArmComponent/workflows/build/badge.svg)](https://github.com/MohitSethi99/SpringArmComponent/actions?workflow=build)
[![CodeFactor](https://www.codefactor.io/repository/github/mohitsethi99/springarmcomponent/badge)](https://www.codefactor.io/repository/github/mohitsethi99/springarmcomponent)

<p align="left">
  <img alt="platforms" src="https://img.shields.io/badge/platform-Unity-blue?style=flat-square"/>
  <img alt="GitHub" src="https://img.shields.io/github/license/MohitSethi99/SpringArmComponent?color=blue&style=flat-square">
  <img alt="size" src="https://img.shields.io/github/repo-size/MohitSethi99/SpringArmComponent?style=flat-square"/>
  <br/>
</p>

Recreation of Unreal's Spring Arm Component in Unity.

Spring Arm Component is a solution for cameras to expand/retract based on gameplay situations. Typically when you add a Camera to a character for the purposes of creating a third person perspective. Spring Arm automatically controls how the camera handles situations where it becomes obstructed by level geometry or other objects in the level.


## Features

- Multiple collision detection using Raycasts
- Collision test resolution for different levels of preciseness
- TPS camera movement integrated
- Takes a minute to setup
- Fully commented code for better understanding


## Getting Started

Set the “Tool Handle Position” to Pivot mode for this to work correctly.

![Image](https://github.com/MohitSethi99/SpringArmComponent/blob/main/Documentation/Pivot.PNG)

Just drag and drop the SpringArm prefab from Prefabs folder to Hierarchy and delete your existing Main Camera or alternatively create a new GameObject, add SpringArm Script to it and make your Main Camera the child of this gameobject.

![Image](https://github.com/MohitSethi99/SpringArmComponent/blob/main/Documentation/Hierarchy.PNG)

![Image](https://github.com/MohitSethi99/SpringArmComponent/blob/main/Documentation/Inspector.PNG)


## Usage

- Target : The gameobject (Transform) which SpringArm has to follow.

- Movement Smooth Time : The camera lag time while following the target.

- Target Arm Length : Maximum distance to which the camera can move to behind the target. (Red line)

![Image](https://github.com/MohitSethi99/SpringArmComponent/blob/main/Documentation/Length.PNG)

- Socket Offset : Offset for the camera from the SpringArm (Socket is the position where camera will be).

- Target Offset : Offset of the SpringArm from the target.

- Do Collision Test : Turn On or Off collision tests.

- Collision Test Resolution : The count of rays raycasting from the SpringArm position to Camera position also referred as probe.

![Image](https://github.com/MohitSethi99/SpringArmComponent/blob/main/Documentation/4Res.PNG)

Resolution - 4

![Image](https://github.com/MohitSethi99/SpringArmComponent/blob/main/Documentation/20Res.PNG)

Resolution - 20

- Collision Probe Size : Radius of collision (Green sphere).

![Image](https://github.com/MohitSethi99/SpringArmComponent/blob/main/Documentation/Probe.PNG)

- Collision Smooth Time : The camera lag time while moving for collision tests.

- Collision Layer Mask : The layer mask for collision tests.

- Use Control Rotation : To turn on/off the mouse input for rotation of the spring arm.

```
Note : All the gizmos shown in above images can be turned on/off for debugging
purposes under Debugging Header.
```

