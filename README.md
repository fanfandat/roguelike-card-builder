# Roguelike Card Builder - Co-op RPG

A scalable co-op roguelike card builder game built in Unity.

## Project Overview

**Genre**: Roguelike Card Builder + Co-op RPG  
**Players**: 1-4 (online multiplayer)  
**Platform**: Steam/PC  
**Engine**: Unity

### Core Features (MVP)
- **Solo/Co-op Combat**: Turn-based card-based combat scaled by player count
- **Individual Decks**: Each player builds their own deck throughout the run
- **Gear System**: Equipment that customizes character appearance and stats
- **Run-Based Progression**: Win runs to earn base resources
- **Base Building**: Meta-progression between runs
- **Online Multiplayer**: 1-4 player co-op matches

## Development Philosophy

1. **Playability First**: Core loop (combat → cards → rewards) before polish
2. **Scalable Architecture**: Easy to add classes, skills, cards, enemies post-launch
3. **Data-Driven Design**: Cards, enemies, gear defined as scriptable objects
4. **Modular Systems**: Combat, UI, networking all independent

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/           # GameManager, NetworkManager, GameState
│   ├── Combat/         # Combat, Cards, Enemies
│   ├── Player/         # PlayerController, DeckManager, GearManager
│   ├── Base/           # BaseManager, Structure progression
│   ├── UI/             # UI managers and displays
│   └── Data/           # Scriptable objects (Cards, Enemies, Gear, Structures)
├── Prefabs/            # Reusable game objects
├── Scenes/             # MainMenu, CombatScene, BaseScene
├── Resources/          # Data files (Cards, Enemies, Gear, Structures)
└── UI/                 # UI assets and layouts
```

## Getting Started

1. Clone the repository
2. Open in Unity (2022 LTS or newer recommended)
3. Open `Assets/Scenes/MainMenu.unity`
4. Press Play

## Current Status

- [x] Project structure
- [x] Core systems scaffolding
- [ ] Combat system implementation
- [ ] Card system implementation
- [ ] Player controller & deck manager
- [ ] UI framework
- [ ] Base building system
- [ ] Online multiplayer
- [ ] Enemy system
- [ ] Gear system

## Next Steps

1. Implement basic combat loop
2. Create first 20 cards (data-driven)
3. Build card UI display
4. Implement turn-based combat
5. Add simple enemy encounters

## Architecture Notes

### Expansion Points
- **Cards**: Add new card types by creating new CardData scriptable objects
- **Enemies**: Procedurally generated, scale by player count
- **Classes**: Implement class system with unique abilities post-MVP
- **Gear**: Expand with more pieces, rarity tiers, stats
- **Structures**: Add more base buildings for meta-progression
- **Multiplayer**: Leverage NetworkManager for seamless scaling

## Tech Stack

- **Engine**: Unity 2022 LTS
- **Networking**: Netcode for GameObjects (or Mirror)
- **Data Format**: ScriptableObjects (JSON export ready)
- **UI**: Unity UI Toolkit (or UGUI for MVP)

## License

TBD
