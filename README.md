# Kill Zombie
> Kill Zombie is my second project. I started this project on the first day of taking an online Unity class. I thought, "Hmm, of course, I should make an FPS project if I want to be a game programmer." But, as you know, the journey for a junior programmer is never easy. I hope you enjoy this simple game!

[Go to play KILL ZOMBIE](https://play.unity.com/en/games/2be07dfa-3e01-4246-a294-71c6adb7750a/killzombie)

![MainImage](https://github.com/user-attachments/assets/6dbe1243-90c7-4b7a-856b-d6412b9b62e0)

## 1. Description

Kill Zombie is an action-packed survival game where players must eliminate zombies and the fire woods they spawn from to progress through levels. Zombies spawn from fire woods, and the only way to stop the horde is by destroying the fire woods. Players must clear all zombies and fire woods in each level to advance, but as they level up, the zombie spawn rate increases, making it more challenging. There are 5 levels in total, each progressively harder.

The game features both first-person and third-person perspectives. Players can switch to the first-person mode by holding the right mouse button, allowing them to use firearms to destroy zombies and fire woods. However, shooting is only available in first-person mode.

## 2. Key Features

#### Camera
* **Third Person Camera and First Person Camera**<br>
The default mode is the third-person camera, but when the user clicks the right mouse button, it switches to the first-person camera. Both cameras share the same rotation and position. I used inheritance to minimize redundant code.

* **Death Camera**<br>
When the player collides with a zombie, the player falls to the left side. I implemented this feature using the DOTween asset.

#### Player
* **Player Movement**<br>
I focused more on third-person movement than first-person movement. In first-person view, the player's rotation simply follows the camera. However, the player’s rotation in third-person view is more complex. I made the player’s local coordinates point in the same direction as the third-person camera. For example, if the camera rotates to the right while the player is moving forward, the player will continue looking left. When the player starts moving forward again, they will follow the camera’s new direction.

* **Shooting**<br>
I used raycasting for shooting. If the player shoots a zombie or firewood, it gets removed from the scene. If the player shoots anywhere else, a bullet hole appears.

#### Zombie
Zombies spawn from firewood, so when the player removes the firewood, zombies will no longer spawn. The maximum number of zombies on the scene is 20. When a zombie collides with an object (except for the player), it rotates randomly.

## 3. Technologies Used

<img src="https://img.shields.io/badge/Unity-222324?style=for-the-badge&logo=unity&logoColor=white">
<img src="https://img.shields.io/badge/Visual%20Studio-007ACC.svg?&style=for-the-badge&logo=Visual%20Studio&logoColor=white"/>
<img src="https://img.shields.io/badge/C%23-4D53E8?style=for-the-badge&logo=c#&logoColor=white">

## 4. Demo
![MenuScene](https://github.com/user-attachments/assets/b1e987da-f36b-4110-8677-a953896744e7)
![playerDeath](https://github.com/user-attachments/assets/80d4b80a-ccce-4fb5-b0b9-358303bd71c4)

*****

_Development Period : Started on 2024-06-18, completed on 2024-10-06_ <br>
_Yehyun(Denise) Cho_
