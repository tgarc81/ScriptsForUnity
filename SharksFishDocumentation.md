# Project 4 - Sharks vs Fish

### Developer Info

-   Name: Tyler Garcia

### Description

In this 3D project, I created a plane populated by Fishes and Sharks, which extends from the Vehicle class. The fishes and sharks are contained within this plane. The sharks will try to catch the closest fish to them, while the fishes will flee from any sharks within 5 Unity units from them, or randomly wander if they are not in range. The sharks and fishes are modelled from prefabs utilizing talented creators on the Unity asset store. Every single entity in the game utilizes obstacle avoidance and will not get close to the obstacles in the game, modeled as rocks. In addition, there is a custom purple shark that the player controls using WASD. Whenever the player or another shark consumes a fish, they get bigger and faster. When there are no fish left, the shark with the most fish eaten wins. This will be obviously shown because the winner shark will be scaled to a gargantuan size to make their dominance known! This project utilizes seeking, fleeing, pursuit, evasion, separation, boundary checking, collision detection (specifically AABB), wandering, player input, obstacle avoidance, and Reynold's concepts for weighting forces.

### User Responsibilities

-   The responsibility of the user is to locate their purple shark on the screen and use WASD in order to try and eat as much fish as possible. The user needs to consume the most fish to win so it is up to them to stay on the hunt and keep consuming to get faster and stronger!

### Known Issues

There are no known errors.


### Sources

My assets used are as follows:
* Shark and Fish Model - Alstra Infinite: [Unity Asset Store](https://assetstore.unity.com/packages/3d/characters/animals/fish/fish-polypack-202232)
* Rock Models for Obstacles - Bit Glacier: [Unity Asset Store](https://assetstore.unity.com/packages/3d/environments/low-poly-rock-models-119245)

