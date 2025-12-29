# SPACE TRAVEL – GAME DESIGN DOCUMENT

## Version
v0.2 (Living Document)

---

## 1. High Concept

**Working title:** Space Travel  
**Platform:** Mobile (Android / iOS)  
**Perspective:** Top-down, 2D  
**Visual style:** Pixel art  
**Game model:** Free-to-play  
**Modes:** Singleplayer + asynchronous PvP  

**Core fantasy:**  
You design, optimize, and expand your own spaceship that acts as your only base, factory, and preparation ground for drone combat.

---

## 2. Design Pillars

### Ship as the Only Base
- No separate bases or colonies.
- The entire game happens on a single ship.
- Progression is tied to ship expansion and layout optimization.

### Layout & Synergies
- The ship is built on a tile grid.
- Module placement matters.
- Adjacent modules provide bonuses or penalties.
- Environmental factors affect crew and production.

### Economy-Driven Gameplay
- Resource acquisition, processing, and optimization are the core loop.
- PvP validates progression but is not the primary driver.

### Idle + Active Play
- Production continues offline.
- Active play enhances efficiency through overclock and minigames.

### Constant Resource Sinks
- Resources are always spent on upgrades, maintenance, crew, and layout changes.

---

## 3. Core Gameplay Loop

1. Design and optimize ship layout.
2. Travel to locations to acquire raw resources.
3. Process resources via ship modules (idle).
4. Upgrade modules and expand ship capacity.
5. Maintain ship systems and crew.
6. Build and upgrade a combat drone.
7. Participate in asynchronous PvP battles.
8. Unlock new blueprints.
9. Repeat from step 1.

---

## 4. Ship & Tile System

- The ship is a tile grid (starting at 6x6, expandable).
- Each tile has properties:
  - Structural type (normal, engine mount, reactor mount, habitat, etc.)
  - Oxygen level
  - Radiation level
  - Temperature
- Not every module can be placed on every tile.
- Adjacency rules create bonuses and penalties.

**Examples:**
- Reactor next to production modules increases output but raises radiation.
- Cooling next to smelters reduces failure risk.
- Habitat near unshielded reactor causes crew sickness.

---

## 5. Ship Modules

### Categories
- **Production** (Recycler, Smelter)
- **Energy** (Reactor, Battery)
- **Logistics** (Warehouse, Conveyors)
- **Functional** (Drone Hangar, Scanner, Engine)

### Module Properties
- Energy consumption
- Resource input/output
- Heat and/or radiation emission
- Wear level
- Upgrade level
- Placement restrictions (tile types)

---

## 6. Resources & Economy

### Resource Groups
- **Raw** – scrap, ore, gas
- **Refined** – metal, electronics, fuel
- **Components** – drone parts, advanced components

### Currencies
- **Gold** – main soft currency
- **PvP Credits** – arena currency

---

## 7. Storage System

- Storage capacity is defined per resource group.
- Example:
  - RawCapacity
  - RefinedCapacity
  - ComponentsCapacity
- Production halts when group capacity is full.

---

## 8. Offline Production

- Production always runs, including offline.
- First 8 hours offline: 100% efficiency.
- After 8 hours: production drops to ~20%.
- Long offline sessions result in a full warehouse, not infinite overflow.

---

## 9. Overclock (Online Only)

- Overclock works only while the game is active.
- Activated via a one-tap minigame:
  - Successful tap: +5% overclock
  - Missed tap: 2s input lock
- Overclock decays at 1% per second.
- Max overclock: 100%.

**Production multiplier:**
- 0% → x1.0
- 100% → x2.0

- Overclock resets to 0% on exit.

---

## 10. Maintenance

- After 8 hours of full-efficiency production:
  - Ship enters *Needs Maintenance* state
  - Overclock may be blocked
  - Some modules work with penalties
- Maintenance consumes resources and time.

---

## 11. Crew

- Start with one main astronaut.
- Long-term goal: semi-independent crew (Fallout Shelter–like).
- Crew stats:
  - HP
  - Radiation resistance
  - Temperature resistance
  - Specialization
- Crew requires:
  - Food
  - Medical care
  - Environmental protection

---

## 12. Combat Drone & PvP

- Drone is built and maintained on the ship.
- Requires components and repairs after each fight.
- PvP is asynchronous:
  - Players fight AI-controlled versions of other players’ drones.

**PvP Rewards:**
- PvP credits
- Unique components
- Blueprints

---

## 13. Resource Sinks

- Module upgrades
- Layout rearrangement
- Ship repairs
- Crew upkeep
- Drone construction and repair
- Temporary boosts
- Overclock usage
- Maintenance

---

## 14. Narrative Outline

- The protagonist lives a quiet life aboard their ship.
- A sudden attack destroys the ship’s systems and drone.
- The antagonist is a dominant drone arena champion.
- The player rebuilds their ship to challenge and defeat the antagonist in an official duel.

---

## Notes

This document is a living design reference.  
Systems may evolve during development, but all changes should respect the core pillars defined above.
