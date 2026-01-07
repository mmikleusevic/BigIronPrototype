Big Iron Prototype 🌵

[Gameplay video](https://youtu.be/pe4lbsTePas)

This is a technical prototype for a Western-themed roguelike. I built this to test a specific gameplay loop: map-based exploration, high-stakes gambling, and combo-driven target combat.

This project is currently a showcase of an idea that I believe has the potential to grow into something much larger. With more development, the combat would be significantly more challenging, featuring expanded mechanics and a player-stat system where every attribute directly influences the gameplay and outcomes.</br></br>
🕹 Gameplay Mechanics</br></br>
Map & Progression

The flow is inspired by Slay the Spire. You navigate a node-based map where everything is randomly generated. There is no "weighted" luck here, it’s all random.

Event Rooms

  - These aren't just text boxes; they feature branching logic.

  - Choices can have conditions (e.g., requiring a certain amount of gold).

  - Some events are "chained," leading to multiple sub-events based on your decisions.

The Shop Room

  - Spend your gold on items that fundamentally change the combat.

  - Available upgrades include slowing down targets or physically increasing their size to make them easier to hit.
    
**I developed a custom State Machine to handle player and opponent interactions in Poker and in Combat.**

The Poker Room

  - AI: The AI evaluates your dice and its own hand to decide whether to hold or re-roll.

The Combat Room

  - Combat is split into three tiers to provide a natural difficulty curve for aim training:

    - Normal: Features static targets to help you find your rhythm.

    - Epic: Introduces rotating targets that test your timing.

    - Boss: The ultimate test with targets that both move and rotate.

    - Damage System: Your total damage isn't flat; it is calculated based on the highest combo you achieve during the round, rewarding precision and speed.

💻 Technical Architecture
Data-Driven Design (Scriptable Objects)

- I used Scriptable Objects to keep the project modular and easy to balance. This allowed me to separate data from logic for:

  - Item stats and shop inventories.

  - Event choices, requirements, and outcomes.

  - Enemy and target configurations.

  - Benefit: I can swap out stats or add new content without touching the core code.

- Performance & Optimization

  - Addressables: I use the Addressables system for scenes and prefabs. This keeps memory usage low and ensures assets are only loaded when needed.

  - Normalized Audio: Every sound in the game has been normalized. I implemented a system where volume levels duck and swell during transitions to keep the atmosphere consistent.

✨ Polish & UX

  - DOTween Pro

    - Used for all UI "magic" and juice.

    - UI elements react physically when you take damage or receive healing.

  - Input & Accessibility

    - Full support for both Controller and Mouse/Keyboard.

    - Includes a custom in-game sensitivity slider so you don't have to change your hardware settings to play.

  - Settings Persistence

    - Options for Volume, VFX, and Sensitivity are saved via PlayerPrefs.

    - Settings are automatically loaded the moment the game starts.

📜 License & Usage

Copyright © 2026 https://github.com/mmikleusevic - All Rights Reserved.

This project and its source code are proprietary. You may not:

  - Copy, modify, or distribute the code.

  - Replicate the project for commercial or personal use.

  - Use the scripts or logic in other projects.

This repository is strictly for demonstration and portfolio purposes.
