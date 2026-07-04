# Combat Test Scene Setup Guide

## Steps to Create CombatTestScene.unity

### 1. Create a New Scene
- In Unity, go to **File > New Scene**
- Save as `Assets/Scenes/CombatTestScene.unity`

### 2. Create Scene Hierarchy

Create this hierarchy in the scene:

```
CombatTestScene
├── Canvas (UI Root)
│   ├── HUD (Panel)
│   │   ├── PlayerHP (Text)
│   │   ├── EnemyHP (Text)
│   │   └── Energy (Text)
│   ├── HandContainer (Panel)
│   │   └── (Cards spawn here dynamically)
│   ├── EndTurnButton (Button)
│   └── Log (Text) [optional, for debugging]
├── GameManager (GameObject with GameManager script)
├── CombatTestSceneManager (GameObject)
├── CombatSystem (GameObject)
├── CardSystem (GameObject)
├── EnemySystem (GameObject)
└── EventSystem (auto-created)
```

### 3. Configure GameObjects

**GameManager** (if not already in scene):
- Add `GameManager.cs` script
- Add `GameState.cs` script

**CombatTestSceneManager**:
- Add `CombatTestSceneManager.cs` script
- Assign:
  - CombatSystem reference
  - DeckManager reference (if needed)
  - EnemySystem reference
  - End Turn Button reference
  - Player Class: Fighter (or Mage/Healer)
  - Player Starting HP: 50
  - Enemy Encounter Level: 1

**CombatSystem**:
- Add `CombatSystem.cs` script
- Assign:
  - Enemy Container: (empty GameObject for enemy UI)
  - Player Hand Container: HandContainer panel
  - Energy Display: Energy Text
  - Player HP Display: PlayerHP Text
  - Enemy HP Display: EnemyHP Text
  - Card Display Prefab: (see step 4)

**CardSystem**:
- Add `CardSystem.cs` script

**EnemySystem**:
- Add `EnemySystem.cs` script

### 4. Create Card Display Prefab

Create a prefab at `Assets/Prefabs/CardDisplay.prefab`:

1. Create new GameObject: `CardDisplay`
2. Add Image component (set color to dark)
3. Add Button component
4. Add `CardDisplayButton.cs` script
5. Create child TextObjects:
   - `CardName` (Text component)
   - `CardCost` (Text component)
   - `CardDescription` (Text component)
6. Drag into Prefabs folder to create prefab
7. Assign in CombatSystem's `cardDisplayPrefab` field

### 5. Configure Canvas & UI

**HandContainer (Panel)**:
- Set Layout Group to Horizontal Layout Group
- Set Child Force Expand: Width & Height
- Set Spacing: 10

**HUD Panel**:
- Position at top-left
- Add background image

**EndTurnButton**:
- Add Text child: "End Turn"
- Position at bottom-right

### 6. Test It!

1. Hit Play
2. You should see:
   - Player HP display
   - Enemy HP display
   - 5 cards in your hand
   - Click a card to play it
   - Enemy takes damage
   - Click "End Turn" to end your turn

## Troubleshooting

**Cards not showing up?**
- Check CardSystem is loading cards from Resources/Cards
- Verify CardDisplay prefab is assigned
- Check console for errors

**Can't click cards?**
- Verify CardDisplayButton script is on card prefab
- Check Button component is configured
- Check EventSystem exists in scene

**No enemies spawning?**
- Check EnemySystem is loading from Resources/Enemies
- Verify enemy assets exist

**Combat not ending?**
- Check HP calculations in TakeDamage()
- Verify enemy removal logic
