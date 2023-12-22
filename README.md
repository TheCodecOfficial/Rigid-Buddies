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

1. Clone this repository.
2. Open the project in Unity.
3. Open the `Game3D` scene.
4. Click the `Play` button.

## Usage

Once the game is running, and you find yourself in the game window, you can use the keys X-M, A-D or left-right arrow keys to control the flippers.
The objective is to eliminate the bricks using the provided balls.Each brick requires three hits be destroyed, at which point it will visually shatter, and a new ball will be generated.

