# Unity Inventory & Item Management System

A **modular inventory and item management system for Unity** designed for RPGs, adventure games, and narrative-driven experiences.

The system includes **multi-page inventories, an item database, player stat integration, runtime debug console, and save/load support**, all written in **C#** with an extensible architecture.

---

# Table of Contents

* [Overview](#overview)
* [Features](#features)
* [Architecture](#architecture)
* [Controls](#controls)
* [Example Usage](#example-usage)
* [Item IDs](#item-ids)
* [Project Structure](#project-structure)
* [Running The Test Build](#running-the-test-build)
* [Credits](#credits)
* [License](#license)

---

# Overview

This project implements a **fully functional inventory framework for Unity games**.

It is designed with **modularity and scalability in mind**, making it easy to add:

* new item types
* additional inventory pages
* custom gameplay mechanics
* new UI features

The system integrates directly with:

* **TextMeshPro** for UI
* **Ink (inkle)** for dialogue and narrative systems

---

# Features

## Multi-Page Inventory

Inventory is split into logical pages:

* Consumables
* Weapons
* Armor
* Key Items

Each page contains its own:

* item grid
* UI panel
* equipped slots

---

## Item Database

`ItemDB` serves as the **central registry for all item data**, including:

* item names
* item icons
* stack sizes
* stat modifiers
* destination inventory page

---

## Player Stat Integration

Equipping items modifies player attributes:

* Health
* Speed
* Jump
* Attack
* Defense

Armor equipment can also update the **player's visual sprite**.

---

## Inventory Operations

Supported operations include:

* Add items
* Remove items
* Set item counts
* Flood inventory
* Drop items
* Reset inventory
* Stack validation

---

## Debug Console

An **in-game developer console** allows runtime commands.

Example uses:

* spawning items
* editing inventory
* debugging gameplay

Type `.help` in the console to see commands.

---

## Save & Load System

The `SaveManager` stores:

* Player position
* Entire inventory state

The system serializes inventory pages and restores them during load.

---

## Dialogue Awareness

Inventory input is automatically disabled when:

* dialogue is active
* the game is paused

Dialogue integration uses **Ink**.

---

# Architecture

The project follows a **component-based Unity architecture**.

## Core Systems

### InventoryManager

Central controller for inventory logic.

Responsibilities:

* managing inventory pages
* handling item operations
* updating UI
* applying stat modifiers

Most methods are exposed through a **static API**.

---

### InventoryPage

Represents a single inventory category.

Responsibilities:

* managing grid slots
* tracking equipped items
* updating item sprites
* handling UI selectors

---

### ItemDB

The global **item registry**.

Stores:

* item definitions
* stack sizes
* metadata
* stat modifiers
* icon references

---

### Player

Stores default player stats and runtime stat values.

Equipped items modify player stats through:

```

InventoryManager.UpdateStats()

```

---

### SaveManager

Handles persistence.

Process:

```

Inventory Pages
v
Flattened Arrays
v
Encoded Save Data (Binary -> Base64 -> Binary)
v
Stored File

````

On load, the process is reversed to reconstruct inventory pages.

---

### DebugManager

Handles:

* debug logging
* runtime commands
* console interaction

---

# Controls

| Action | Key |
|------|------|
| Move / Menu Navigation | Arrow Keys |
| Confirm Menu Option | Z |
| Toggle Inventory | E |
| Toggle Debug Console | Tab |
| Pause Game | Escape |

---

# Example Usage

## Adding an Item

```csharp
InventoryManager.Add(1, 3);
````

Adds **3 items with ID 1** to the inventory.

---

## Removing an Item

```csharp
InventoryManager.Remove(2, 1);
```

Removes **1 item with ID 2**.

---

## Dropping an Item

```csharp
InventoryManager.DropItem(0, InventoryPage.PageTypes.weapons);
```

Drops the **first weapon slot item** into the world.

---

# Item IDs

Items are referenced by **unique integer IDs**.

| ID     | Item           |
| ------ | -------------- |
| 0      | Empty          |
| 1      | Apple          |
| 2      | Banana         |
| 3      | Grapes         |
| 4      | Lemon          |
| 5      | Lime           |
| 6      | Orange         |
| 7      | Pear           |
| 8      | Pineapple      |
| 12001  | Iron Helmet    |
| 12002  | Gold Helmet    |
| 12003  | Diamond Helmet |
| 12004  | Ultra Helmet   |
| 192001 | Iron Sword     |
| 192002 | Gold Sword     |
| 192003 | Diamond Sword  |
| 192004 | Ultra Sword    |

Each item also has a **maximum stack size** enforced by the system.

---

# Project Structure

```
Scripts

|-- Inventory
|   |-- InventoryManager.cs
|   |-- InventoryPage.cs
|   `-- ItemDB.cs
|
|-- Player
|   `-- Player.cs
|
|-- Save
|   `-- SaveManager.cs
|
|-- Debug
|   `-- DebugManager.cs
|
|-- UI
|   `-- Inventory UI
|
`-- Dialogue
    `-- Ink Integration
```

---

# Running The Test Build

1. Clone the repository

2. Navigate to:

```
Builds/
```

3. Run

```
Inventory System Test Version 2.exe
```

---

## Save Format

The save system uses **base64 + bitstring encoding**.

This is to ensure a fragile and obfuscated save file structure, which helps to prevent any unauthorized modifications to the player's save file outside the program.

---

# Credits

* Unity Engine
* TextMeshPro
* Ink (inkle Studios)

---

# License

This project is provided **as-is for educational and prototyping purposes.**

For commercial use, please ensure compliance with all third-party asset licenses.
