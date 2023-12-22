# Rigid-Buddies

A fun little physics-based video game, made as a project for the course Physically Based Simulation for Computer Graphics 2023.


## Features

- Triple A graphics
- Cutting edge physics simulations
- Extreme performance
- Exiting and action-packed gameplay

## Info

Unity version `LTS 2022.3.14f1`

## Physics

The physical basis has been custom created.
The developed scripts can be split in:
- `Colliders`: My Circle Collider, My Box Collider
- `Rigid body`: My RigidBody
- `Physics Manager`: PhysicsManager

## Scenes

- `Game3D`: This scene showcases the final product, featuring a 2D game with 3D graphics. It contains various object types: Bumpers (static circle colliders), Balls (circle colliders that can move), Borders(static box colliders that limit the scene scenario) and Flippers (user-controlled elements consisting of a central circle collider set as kinematic, and two static child colliders whose linear and angular velocity dynamically  update based on their relative positions to the center, ensuring appropriate collisions with the balls).

- `Game`: This scene contains the same pysical dynamics as Game3D but with simplified visuals.

- `Boxed and balls colliders`: In this scene, various types of colliders (boxes and circles) are presented. The velocities and angular velocities of each collider can be observed in the inspector through the myRigidBody component. Additionally, velocity and angular velocity parameters are displayed on top of one box and one sphere, providing a visual representation of how these properties evolve..

- `Polygon Collision`: This scene shows collision involving several polygons.

- `Shatter Effect`: This scene showcases the visual shattering effect employed in the Game3D scene

## Installation
The whole project can be inspected and run inside the Unity game engine:

1. Clone this repository.
2. Open the project in Unity version `LTS 2022.3.14f1`.
3. Open the `Game3D` scene.
4. Click the `Play` button.

Alternatively, an already built executable can be run. Depending on your hardware, you can choose from three variants:
- `Fullscreen` scales to your screen size. Requires a somewhat performant graphics card to run. The perspective can be distorted if your screen aspect ratio differs from 16:9 by a large amount.
- `Low Resolution Windowed` runs the simulation at a lower resolution. Even though the physics timestep is not bound to the framerate, the simulation can ofter glitch out at lower framerates. This variant should run on most devices.
- `Low Resolution Fullscreen` is the same as the other low resolution, but upscaled to fit the whole screen. It should also run on most devices.

All three variants showcase exactly the same rigid body simulation. Only the graphics/rendering settings differ.

## Usage

Once the game is running, and you find yourself in the game window, you can use the keys X-M, A-D or left-right arrow keys to control the flippers.
The objective is to eliminate the bricks using the provided balls. Each brick requires three hits to be destroyed, at which point it will visually shatter, and a new ball will be generated.
When space key is pressed all proyectiles suffer an upward force. And finally, the game can be restarted by pressing the R key.

