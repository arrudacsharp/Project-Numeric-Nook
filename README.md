# PROJECT: Numeric Nook


## Summary
This project was developed for early elementary school children, with a focus on fundamental mathematical operations. Through casual gamification, the game aims to stimulate logical thinking, abstraction, and mathematical processing, namely the ability to calculate and associate operations with their results. It also promotes selective attention, which involves identifying the correct answer, and visuomotor coordination, expressed through the act of picking up and transporting in-game elements. In this way, reasoning and action are integrated into an active and engaging learning experience.



## Challenges
During the development process of this project, several challenges arose. The first was determining where to begin. I needed to conduct extensive research in order to establish a comprehensive step-by-step plan that would ensure timely completion.
This was my first 2D game. Throughout my experience with Unity, I had previously worked exclusively with 3D games. Although 2D development is simpler in certain respects, there are significant conceptual differences that I had to learn.
Challenges also emerged in programming, such as creating an automated system capable of evaluating when a level had been completed and saving the result to enable basic level progression. The system I implemented uses a static class that evaluates a LevelIndex whenever a level is completed, incrementing its value by one. This approach effectively solved progression for the current scope; however, it is only viable because the game contains just two levels. If additional levels were introduced, a different approach would be required, preferably implementing direct control within LevelData to determine whether a level has previously been completed. Given the limited scope, the static class serves as a temporary solution.
The most significant challenge, however, was the Tilemap system. It took several hours to fully understand its rendering order behavior, particularly since such issues do not arise in 3D environments.



## Code Structure
From the outset, the project architecture was organized using two assembly definitions (asmdefs), with the objective of maintaining clean, modular, and decoupled code from Unity’s default assembly:

#### NumericNook.Core.Runtime
#### NumericNook.Core.Editor

The NumericNook.Core.Editor assembly was created to reserve space for future custom editor tools. Although it is currently empty, it is part of the planned project structure, particularly with future automation in mind, such as mathematical expression generation or level design support tools.

The NumericNook.Core.Runtime assembly contains all core gameplay logic.

## General Runtime Organization
The runtime assembly consolidates the project’s core systems, organized into:

  - Audio management
  - Gameplay and level progression
  - User interface
  - Configurable data via ScriptableObjects
  - Player interaction with the game world
  - Generation and distribution of numerical tokens

The overall system design is straightforward: each level receives a set of mathematical expressions, these expressions are distributed among houses, correct and incorrect numerical values are generated across the map, and the player must collect and deliver the correct value to each house in order to complete the level.


## Audio System

#### AudioManager.cs
 The central audio manager of the game. It is structured as a Singleton, allowing global access without requiring direct references between scripts. Its responsibilities include centralized control of music and sound effects, such as:
 
  - Playing and stopping music
  - Crossfade transitions
  - Sound effect playback via an AudioSource pool
  - The pooling system prevents constant instantiation and destruction of audio objects during gameplay.

#### AudioController.cs
 An intermediary script between the UI and the audio system. It modifies runtime audio data and forwards these changes to the AudioManager, allowing adjustment of music and SFX volume, muting/unmuting, and triggering the saving of settings.
 
#### AudioControllerUI.cs
 The UI layer of the audio system. This script connects sliders, buttons, and text elements to the AudioController, enabling the player to control volume and mute settings directly through the interface, with persistence across sessions.
 
#### AudioSettings.cs
 A ScriptableObject responsible for storing audio settings. It holds music and SFX values and provides persistence methods:
 
  - Save() — saves current settings
  - Load() — retrieves previously saved settings
    
This asset functions as the runtime data backbone of the audio system.



## Gameplay System

#### HouseController.cs
 One of the core scripts of the main gameplay loop. Each house in the scene has a HouseController, responsible for receiving a mathematical expression, displaying it visually, validating the delivery of a number, and triggering visual and audio feedback based on the result. It represents the final validation point of player interaction.

#### LevelController.cs
 The overall level controller. It aggregates essential information defining level behavior, including the next scene name, tutorial text, highlighted operator, and final level identification. It also tracks completed houses and player errors, triggering level completion when all objectives are met.

#### LevelControllerUI.cs
 The UI controller for the level. It bridges the UI and the current state of the level, handling:
 
  - Display of the completion panel
  - Final score and performance summary
  - Tutorial flow
  - Pause menu visibility
  - Navigation to the next level or main menu
 
 It relies on data exposed by the LevelController.

#### LevelSetup.cs
 Responsible for configuring the level at runtime. It prepares the scene based on predefined data by collecting houses, retrieving expressions, assigning them, and generating the token pool. It effectively connects configurable data to the playable scene.

#### LevelResult.cs
 A simple class representing the final outcome of a level. It stores data such as completed houses, correct answers, errors, and the player’s final score.

#### LevelRemember.cs
 A static class used to store simple progression data during execution. It tracks the last completed level and unlocks the next one, ensuring sequential progression.

## Data System

#### MathExpression.cs
 A ScriptableObject representing a mathematical expression. Each asset contains an operator and two values (A and B), forming the logical basis used during gameplay.

#### MathExpressionRegistry.cs
 A ScriptableObject that acts as an expression catalog. It stores all available expressions for a given context and allows runtime selection for level usage. It decouples mathematical content from scene logic.

#### LevelData.cs
 A ScriptableObject defining the logical configuration of each level. It references a MathExpressionRegistry and specifies:
 
  - Number of houses
  - Number of decoys
  - Numerical range for decoys

This asset is essential for level construction. Each level must have a properly configured LevelData referencing a compatible registry.
Token and Interaction System

#### NumberToken.cs
 The collectible numerical object. Attached to the token prefab, it stores the numerical value, displays it visually, allows player interaction, follows the player while carried, and resets when dropped. It represents the physical answer delivered to a house.

#### TokenSpawner.cs
 Responsible for spawning tokens in the scene. It uses the number pool generated by LevelSetup and attempts to place tokens within valid areas using Physics2D.OverlapCircle, respecting spawn zones, obstacle masks, minimum distances, and spawn attempt limits.

#### PlayerInteractor.cs
 Handles direct player interaction with the game world. It manages interaction input and defines the main interaction flow, including collecting, carrying, delivering, and dropping tokens, as well as opening the pause menu.

#### PlayerControl.cs
 Controls player movement. It reads input via the Input System and applies movement through Rigidbody2D, forming the basis of character locomotion.

#### MainMenuController.cs
 The main menu controller. It manages:
 
  - Panel navigation
  - Level selection
  - Locked level restrictions
  - Scene loading
  - Credits display
  - Menu audio

It also uses LevelRemember to manage level access.

## Overall System Flow
The main gameplay loop operates as follows:

  - The level has a configured LevelData.
  - LevelData references a MathExpressionRegistry.
  - LevelSetup selects expressions and assigns them to houses.
  - LevelSetup generates the number pool.
  - TokenSpawner instantiates NumberTokens.
  - The player collects numbers via PlayerInteractor.
  - Each HouseController validates the delivered number.
  - LevelController tracks progress and errors.
  - Upon completion, a LevelResult is generated.
  - LevelControllerUI displays results and enables progression.

This separation ensures the system remains clear, extensible, and reasonably decoupled, despite the project’s compact scope.


