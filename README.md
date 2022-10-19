# XCOM-Like-Prototype
 
This Game was made in Unity version 2021.3.10f1 in 6 days.

You can play it in one of two ways:
- Go to its [itch.io](https://boquimpani.itch.io/xcom-like-prototype) page download and unpack the .rar file from there and run the "XCOM-Like Prototype.exe" file.
- Download this repository, the Unity Hub and Unity Editor version 2021.3.10f1 and open the repository folder with Unity.

Game Instructions:
- The W, A, S and D keys move the camera.
- On your turn you can click one of your units to select it.
- With a unit selected you can click an enemy unit to shoot it or an empty space on the map to move there.
- When hovering the mouse over an enemy unit or the map, the tips on the bottom of the screen show how many action points (AP) are required to shoot or move and the hit chance of the attack.
- If an unit doesn't have enough AP, it cannot perform any actions until the players next turn.
- The game ends when there is only one player left.

Game Features:
- Phoenix Point-like aim system. The hit chance of units is calculated based on the units actual line of sight.
- Procedural world with different kinds of terrains.
- Terrains use scriptable objects for ease of implementing new terrain types.
- A* pathfinding with detection for blocked spaces and different movement costs for different terrain types.
- Variable number of players. From 2 up to 8. Can be easily expanded by just adding more colors to the playerColors pool.
- Variable number of units per player. Allows for user-determined handicap and game length.
- Customizable world. World size and tree density can be easily set before match.