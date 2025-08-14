# PlatformSwap

## Itch: https://rekonver.itch.io/platformswap

## Used Patterns and Architectures

### **Observer Pattern**
`GameEvents` with `OnLevelComplete` and `OnLevelRestart` events — a classic *Observer* implementation where subscribers react to changes without tight coupling.

### **Command / Action Trigger**
`ChangeRole` calls `SetActive`, `ApplyState`, and `DeactivateOthers` when a user clicks — similar to *Command*, where an action is encapsulated in an event handler.

### **Strategy Pattern**
`IDestructible` + `DestructibleInstant` + `DestructibleStepMover` — different destruction strategies that can be swapped without changing core logic.

### **Singleton-like Registry**
`SpriteActivityManager` keeps a static list of `allInstances` and interacts with other instances — a form of global access similar to a *Singleton Registry*.

### **Component-based Architecture**
Logic is split into independent `MonoBehaviour` components (Unity’s component-based architecture).

### **Data-Driven Settings**
Using `SerializeField` and `Header` for Inspector configuration allows behavior changes without modifying the code.

### **State Pattern**
`SpriteActivityManager.Active` + `ApplyState` modify the sprite’s appearance depending on its current state — a basic *State* implementation.

### **Template Method (partial)**
In `ScamWay.MoveOutAndBack`, the movement structure (out → pause → back) is fixed, but movement parameters are adjustable via the Inspector.
